using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModbusSpace;
using System.Windows;
using System.IO.Ports;
using System.Timers;
using System.Windows.Threading;

namespace TestPanuonUI
{
    public class ChargPCBATestModbus : ModbusRtuDrive
    {
        private          Options               odm;
        private          SerialPort            port;
        public           DataViewModel         dvm;

        private          byte                  SlaveId = 2;
        const            ushort                ReadyAddr = 0;
        const            ushort                CmdAddr = ReadyAddr + 1;  
        const            ushort                ResultAddr = ReadyAddr + 2;
        const            ushort                StatusAddr = ReadyAddr + 3;
        const            ushort                ResultValueAddr = ReadyAddr + 4;
        private          uint                  timeout = 10000;
        private          ushort                RegNum = 1;

        private          Timer                 t;
        private          bool                  recvTimeOut = false;
        public           string                statusTextInfo;
        public           bool                  abortTestFlag = false;
        private          uint                  errTimes = 0;
        private          uint                  passTimes = 0;
        public           bool                  isStopTestFlag = true;
        public           int                   selectedID = -1;
        public           double                testPrecent = 0;
        private          bool                  isGetResult = false;
        private          bool                  isTestDone = false;

        public ChargPCBATestModbus(Options _odm,DataViewModel _dvm)
        {
            this.odm = _odm;
            this.dvm = _dvm;
            //config serial port 
            port = new SerialPort(odm.ODM.ComPort);
            port.BaudRate = 9600;
            port.DataBits = 8;
            port.Parity = Parity.None;
            port.StopBits = StopBits.One;

            this.timeout =(uint) _odm.ODM.MaxNumOfBoard;
            this.SlaveId = _odm.ODM.MinNumOfSalve;

            t = new Timer(timeout);
            t.Elapsed += new System.Timers.ElapsedEventHandler(theout);
            t.AutoReset = false;
            t.Enabled = false;  
        }

        public void theout(object source, System.Timers.ElapsedEventArgs e)

        {
            recvTimeOut = true;
        }

        public bool closeSerialPort()
        {
            if (port.IsOpen)
                try
                {
                    port.Close();
                    if (!port.IsOpen) return false;
                }
                catch (Exception e)
                {
                    statusTextInfo = e.Message.ToString();
                }
            return true;
        }

        public bool openSerialPort()
        {
            if (!port.IsOpen)
                try
                {
                    port.Open();
                    if (port.IsOpen) return true;
                }
                catch (Exception e)
                {
                    statusTextInfo = e.Message.ToString();
                }
            return false;
        }

        public static void Delay(int milliSecond)
        {
            int start = Environment.TickCount;
            while (Math.Abs(Environment.TickCount - start) < milliSecond)//毫秒
            {
            }
        }
        public void StartTestOne()
        {
            try
            {
                TestCmd((uint)selectedID, new ushort[] { this.dvm.TestDataModelView[selectedID].cmd, 0x00 });
                Delay(1000);
                this.isStopTestFlag = true;
            }
            catch (Exception e)
            {
                statusTextInfo = e.Message.ToString();
            }
        }
        public void StartTestAll()
        {
            errTimes = 0;
            passTimes = 0;
            try
            {
                int count = this.dvm.TestDataModelView.Count;
                foreach (TestDataModel i in dvm.TestDataModelView)
                {
                    TestCmd((uint)i.id -1, new ushort[] { i.cmd, 0x00 });
                    testPrecent += 100 / count;
                }
                testPrecent = 100;                
                if (errTimes == 0) this.dvm.ResultViewModel = "通过";
                else this.dvm.ResultViewModel = "不通过";

                Delay(1000);                                            //等待UI界面更新
                this.isStopTestFlag = true;
            }
            catch (Exception e)
            {
                statusTextInfo = e.Message.ToString();
                this.dvm.ResultViewModel = "不通过";
            }
        }

        public void StartTestAll_stopToFail()
        {
            errTimes = 0;
            passTimes = 0;
            try
            {
                int count = this.dvm.TestDataModelView.Count;
                foreach (TestDataModel i in dvm.TestDataModelView)
                {
                    TestCmd(i.id - 1, new ushort[] { i.cmd, 0x00 });
                    testPrecent += 100 / count;
                    if (errTimes > 0) break;
                }
                testPrecent = 100;
                if (errTimes == 0) this.dvm.ResultViewModel = "通过";
                else this.dvm.ResultViewModel = "不通过";
                Delay(1000);                                           //等待UI界面更新
                this.isStopTestFlag = true;
            }
            catch (Exception e)
            {
                statusTextInfo = e.Message.ToString();
                this.dvm.ResultViewModel = "不通过";
            }
        }

        private ushort GetSlaveState()
        {
            Delay(200);
            ushort SlaveReadFlag = ModbusSerialRtuMasterReadRegisters(port, SlaveId, ReadyAddr, RegNum)[0];
            return SlaveReadFlag;
        }
        private void SendCmd(ushort addr, ushort[] Cmd)
        {
            Delay(200);
            ModbusSerialRtuMasterWriteRegisters(port, SlaveId, addr, Cmd);
        }
        private bool ResultDataHandler(uint id)
        {
            uint times = 0;
            byte[] b = new byte[4];
            ushort[] d;
            float top = this.dvm.TestDataModelView[(int)id].testTopLimit;
            float floor = dvm.TestDataModelView[(int)id].testFloor;
            while (times < 5)
            {
                d = ModbusSerialRtuMasterReadRegisters(port, SlaveId, ResultValueAddr, (ushort)(RegNum + 3));
                b[0] = (byte)(d[0] & 0xFF);
                b[1] = (byte)(d[1] & 0xFF);
                b[2] = (byte)(d[2] & 0xFF);
                b[3] = (byte)(d[3] & 0xFF);
                float v = BitConverter.ToSingle(b, 0);
                dvm.TestDataModelView[(int)id].resultData = v;
                if (top != floor)
                {
                    if (v >= floor && v <= top)
                    {
                        return true;
                    }
                }
                else
                {
                    dvm.TestDataModelView[(int)id].remarks = "上下限不能相等";
                    return false;
                }
                times++;
            }
            return false;
        }
        private void UpdataResult(uint id)
        {
            ushort[] r;
            if (!isGetResult)
            {
                r = ModbusSerialRtuMasterReadRegisters(port, SlaveId, ResultAddr, (ushort)(RegNum + 1));
                switch (r[0])
                {
                    case (ushort)ResultState.RESULT_PASS_FLAG:
                        t.Enabled = false;
                        recvTimeOut = false;
                        passTimes++;
                        dvm.TestDataModelView[(int)id].testResult = "通过";
                        isGetResult = true;
                        break;
                    case (ushort)ResultState.RESULT_FAIL_FLAG:
                        t.Enabled = false;
                        recvTimeOut = false;
                        errTimes++;
                        dvm.TestDataModelView[(int)id].testResult = "不通过";
                        isGetResult = true;
                        break;
                    case (ushort)ResultState.RESULT_WAIT_FLAG:
                        string strshow = "未识别状态码。";
                        string  strtemp = this.dvm.TestDataModelView[(int)id].pointtitle;
                        if (strtemp != "")
                        {
                            strshow = strtemp;
                        }
                        t.Enabled = false;
                        recvTimeOut = false;
                        if (MessageBox.Show(strshow , "提示", MessageBoxButton.YesNo, MessageBoxImage.Question,
                            MessageBoxResult.Yes) == MessageBoxResult.Yes)
                        {
                            passTimes++;
                            dvm.TestDataModelView[(int)id].testResult = "通过";
                        }
                        else
                        {
                            errTimes++;
                            dvm.TestDataModelView[(int)id].testResult = "不通过";
                        }
                        isGetResult = true;
                        break;
                    case (ushort)ResultState.RESULT_DATA_FLAG:
                        t.Enabled = false;
                        recvTimeOut = false;
                        if (ResultDataHandler(id))
                        {
                            passTimes++;
                            dvm.TestDataModelView[(int)id].testResult = "通过";
                        }
                        else
                        {
                            errTimes++;
                            dvm.TestDataModelView[(int)id].testResult = "不通过";
                        }
                        isGetResult = true;
                        break;
                }
            }
            else SendCmd(ReadyAddr, new ushort[] { (ushort)SlaveState.SLAVE_STATE_UPDATE }); 
        }
        private void xTCStateHandlers(uint id,ushort[] Cmd)
        {
            ushort state = GetSlaveState();
            switch (state)
            {
                case (ushort)SlaveState.SLAVE_STATE_READY:
                    if (!isGetResult)
                        SendCmd(CmdAddr, Cmd);
                    else isTestDone = true;
                    break;
                case (ushort)SlaveState.SLAVE_STATE_GETCMD:
                    break;
                case (ushort)SlaveState.SLAVE_STATE_FINSH:
                    UpdataResult(id);
                    break;
                case (ushort)SlaveState.SLAVE_STATE_UPDATE:
                    break;
                case (ushort)SlaveState.SLAVE_STATE_TIMEOUT:
                    break;
            }
        }

        public void TestCmd(uint id,ushort[] Cmd)
        {
            isTestDone = false;
            isGetResult = false;
            recvTimeOut = false;
            try
            {
                if (!abortTestFlag && dvm.TestDataModelView[(int)id].isCheckedBoxTrue && dvm.TestDataModelView[(int)id].cmd != 0)
                {
                    dvm.TestDataModelView[(int)id].testResult = "正在测试...";
                    t.Enabled = true;
                    while (!isTestDone)
                    {
                        xTCStateHandlers(id, Cmd);
                        if (recvTimeOut)
                        {
                            recvTimeOut = false;
                            errTimes++;
                            SendCmd(ReadyAddr, new ushort[] { (ushort)SlaveState.SLAVE_STATE_TIMEOUT });
                            dvm.TestDataModelView[(int)id].testResult = "不通过";
                            dvm.TestDataModelView[(int)id].remarks = "响应超时";
                            break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                //statusTextInfo = e.Message.ToString();
                this.dvm.TestDataModelView[(int)id].testResult = "不通过";
                this.dvm.TestDataModelView[(int)id].remarks = e.Message.ToString();
                errTimes++;
            }
        }

        /// <summary>
        /// the ushort[] data of revice plc translate to string
        /// </summary>
        /// <param name="recData"> Recive data type is ushort[]</param>
        /// <returns></returns>
        private string toString(ushort[] recData)
        {
            //string temp=string.Empty;
            List<byte> tl = new List<byte>();
            foreach (ushort s in recData)
            {
                if (s != 0)
                    tl.Add((byte)s);
            }
            byte[] temp = tl.ToArray();
            return Encoding.Default.GetString(temp);
        }
    }

}

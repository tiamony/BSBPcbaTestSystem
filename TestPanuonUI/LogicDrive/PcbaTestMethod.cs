using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Panuon.UI.Silver;
using System.IO.Ports;
using System.Timers;
using ModbusSpace;

namespace BsbPcbaTestSpace
{
    public partial class MainWindow : WindowX
    {
        private Timer t;
        private SerialCom scEx;
        private ModbusRtuDrive mrd;
        //配置变量
        //从机地址
        private byte SlaveId = 2;
        //单项测试超时时间
        private int timeout = 100000;
        //串口名称
        private string ComNum = "Com1";
        //自定义寄存器地址
        const ushort ReadyAddr = 0;
        const ushort CmdAddr = ReadyAddr + 1;
        const ushort ResultAddr = ReadyAddr + 2;
        const ushort StatusAddr = ReadyAddr + 3;
        const ushort ResultValueAddr = ReadyAddr + 4;
        const ushort RegNum = 1;
        //测试进程控制标志位
        private bool isGetResult = false;
        private bool isTestDone = false;
        private bool recvTimeOut = false;
        //通过、不通过项计数
        private uint errTimes = 0;
        private uint passTimes = 0;
        /// <summary>
        /// 测试具体方法初始化
        /// </summary>
        private void PcbaTestMethodInit()
        {
            scEx = new SerialCom(ComNum);
            mrd = new ModbusRtuDrive();
            t = new Timer(timeout);
            t.Elapsed += new System.Timers.ElapsedEventHandler(TimeOutEvent);
            t.AutoReset = false;
            t.Enabled = false;
        }
        /// <summary>
        /// 超时执行函数
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        public void TimeOutEvent(object source, System.Timers.ElapsedEventArgs e)
        {
            //是否超时标志位
            recvTimeOut = true;
        }
        /// <summary>
        /// 自定义延时函数（等待硬件动作）
        /// </summary>
        /// <param name="milliSecond"></param>
        public static void Delay(int milliSecond)
        {
            int start = Environment.TickCount;
            while (Math.Abs(Environment.TickCount - start) < milliSecond)
            {
            }
        }
        /// <summary>
        /// 具体测试函数
        /// </summary>
        /// <param name="id">测试项目ID</param>
        /// <param name="Cmd">测试项目CMD</param>
       public void TestCmd(uint id, ushort[] Cmd)
       {
            isTestDone = false;
            isGetResult = false;
            recvTimeOut = false;
            try
            {
                if (dvmEx.TestDataModelView[(int)id].isCheckedBoxTrue && dvmEx.TestDataModelView[(int)id].cmd != 0)
                {
                    if (tsState != TSRunState.RUN_STATE_ABORT)
                    {
                        dvmEx.TestDataModelView[(int)id].testResult = "正在测试...";
                        t.Enabled = true;
                        while (true)
                        {
                            TCStateHandlers(id, Cmd);
                            if (isTestDone) break;
                            if (recvTimeOut)
                            {
                                recvTimeOut = false;
                                errTimes++;
                                SendCmd(ReadyAddr, new ushort[] { (ushort)SlaveState.SLAVE_STATE_TIMEOUT });
                                dvmEx.TestDataModelView[(int)id].testResult = "不通过";
                                dvmEx.TestDataModelView[(int)id].remarks = "响应超时";
                                break;
                            }
                        }
                    }
                    else
                    {
                        this.dvmEx.TestDataModelView[(int)id].testResult = "已终止";
                    }
                }

            }
            catch (Exception e)
            {
                this.dvmEx.TestDataModelView[(int)id].testResult = "不通过";
                this.dvmEx.TestDataModelView[(int)id].remarks = e.Message.ToString();
                errTimes++;
            }
       }
       /// <summary>
       /// 测试状态机
       /// </summary>
       /// <param name="id">测试项目ID</param>
       /// <param name="Cmd">测试项目CMD</param>
       private void TCStateHandlers(uint id, ushort[] Cmd)
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
        /// <summary>
        /// 获取测试结果，判断是否通过
        /// </summary>
        /// <param name="id">项目ID</param>
        private void UpdataResult(uint id)
        {
            ushort[] r;
            if (!isGetResult)
            {
                r = mrd.ModbusSerialRtuMasterReadRegisters(this.scEx.port, SlaveId, ResultAddr, (ushort)(RegNum + 1));
                switch (r[0])
                {
                    case (ushort)ResultState.RESULT_PASS_FLAG:
                        t.Enabled = false;
                        recvTimeOut = false;
                        passTimes++;
                        dvmEx.TestDataModelView[(int)id].testResult = "通过";
                        isGetResult = true;
                        break;
                    case (ushort)ResultState.RESULT_FAIL_FLAG:
                        t.Enabled = false;
                        recvTimeOut = false;
                        errTimes++;
                        dvmEx.TestDataModelView[(int)id].testResult = "不通过";
                        isGetResult = true;
                        break;
                    case (ushort)ResultState.RESULT_WAIT_FLAG:
                        string strshow = "未识别状态码。";
                        string strtemp = this.dvmEx.TestDataModelView[(int)id].pointtitle;
                        if (strtemp != "")
                        {
                            strshow = strtemp;
                        }
                        t.Enabled = false;
                        recvTimeOut = false;
                        if (MessageBox.Show(strshow, "提示", MessageBoxButton.YesNo, MessageBoxImage.Question,
                            MessageBoxResult.Yes) == MessageBoxResult.Yes)
                        {
                            passTimes++;
                            dvmEx.TestDataModelView[(int)id].testResult = "通过";
                        }
                        else
                        {
                            errTimes++;
                            dvmEx.TestDataModelView[(int)id].testResult = "不通过";
                        }
                        isGetResult = true;
                        break;
                    case (ushort)ResultState.RESULT_DATA_FLAG:
                        t.Enabled = false;
                        recvTimeOut = false;
                        if (ResultDataHandler(id))
                        {
                            passTimes++;
                            dvmEx.TestDataModelView[(int)id].testResult = "通过";
                        }
                        else
                        {
                            errTimes++;
                            dvmEx.TestDataModelView[(int)id].testResult = "不通过";
                        }
                        isGetResult = true;
                        break;
                }
            }
            else SendCmd(ReadyAddr, new ushort[] { (ushort)SlaveState.SLAVE_STATE_UPDATE });
        }
        /// <summary>
        /// 获取测试结果返回值
        /// </summary>
        /// <param name="id">测试项目ID</param>
        /// <returns></returns>
      private bool ResultDataHandler(uint id)
      {
            uint times = 0;
            byte[] b = new byte[4];
            ushort[] d;
            float top = this.dvmEx.TestDataModelView[(int)id].testTopLimit;
            float floor = dvmEx.TestDataModelView[(int)id].testFloor;
            while (times < 5)
            {
                d = mrd.ModbusSerialRtuMasterReadRegisters(this.scEx.port, SlaveId, ResultValueAddr, (ushort)(RegNum + 3));
                b[0] = (byte)(d[0] & 0xFF);
                b[1] = (byte)(d[1] & 0xFF);
                b[2] = (byte)(d[2] & 0xFF);
                b[3] = (byte)(d[3] & 0xFF);
                float v = BitConverter.ToSingle(b, 0);
                dvmEx.TestDataModelView[(int)id].resultData = v;
                if (top != floor)
                {
                    if (v >= floor && v <= top)
                    {
                        return true;
                    }
                }
                else
                {
                    dvmEx.TestDataModelView[(int)id].remarks = "上下限不能相等";
                    return false;
                }
                times++;
            }
            return false;
      }
        /// <summary>
        /// 获取从机状态
        /// </summary>
        /// <returns></returns>
        private ushort GetSlaveState()
        {
            Delay(200);
            ushort SlaveReadFlag = mrd.ModbusSerialRtuMasterReadRegisters(this.scEx.port, SlaveId, ReadyAddr, RegNum)[0];
            return SlaveReadFlag;
        }
        /// <summary>
        /// 发送从机测试指令
        /// </summary>
        /// <param name="addr">从机地址</param>
        /// <param name="Cmd">指令</param>
        private void SendCmd(ushort addr, ushort[] Cmd)
        {
            Delay(200);
            mrd.ModbusSerialRtuMasterWriteRegisters(this.scEx.port, SlaveId, addr, Cmd);
        }

    }
}

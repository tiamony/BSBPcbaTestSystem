using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModbusSpace;
using System.IO.Ports;
using System.Windows;

namespace TestPanuonUI
{
    public class KubotaBoardModbus:ModbusRtuDrive
    {
        private ushort _productNumRegAddress = 400;
        private ushort _registers = 1;
        private ushort _standarHoursTotalRegAddress = 235;
        private ushort _actualHoursTotalRegAddress = 230;
        private ushort _productIDRegAddress = 300;
        private ushort _productIDRegNum = 11;
        private ushort _timeOver1RegAddress = 220;
        private ushort _countTime1RegAddress = 210;
        private ushort _placementNum1RegAddress = 208;
        private ushort _timeSeting1RegAddress = 200;
        private ushort _timeOver2RegAddress = 222;
        private ushort _countTime2RegAddress = 212;
        private ushort _placementNum2RegAddress = 203;
        private ushort _timeSeting2RegAddress = 202;
        private ushort _timeOver3RegAddress = 224;
        private ushort _countTime3RegAddress = 214;
        private ushort _placementNum3RegAddress = 205;
        private ushort _timeSeting3RegAddress = 204;
        private ushort _timeOver4RegAddress = 226;
        private ushort _countTime4RegAddress = 216;
        private ushort _placementNum4RegAddress = 207;
        private ushort _timeSeting4RegAddress = 206;

        private Options odm;
        private SerialPort port;
        public DataViewModel dvm = new DataViewModel();

        public KubotaBoardModbus(Options _odm)
        {
            this.odm = _odm;
            //config serial port 
            port = new SerialPort(odm.ODM.ComPort);
            port.BaudRate = 9600;
            port.DataBits = 8;
            port.Parity = Parity.None;
            port.StopBits = StopBits.One;
            /*
            dvm.DataModelView.Add(new DataModel()
            {
                LineName = "3321",
                ProductID = "SN23967929UJ",
                ProductNum = 2000,
                StandardHoursTotal = 40,
                ActualHoursTotal = 33,
                TimeOver1 = 0,
                CountTime1 = 33,
                PlacementNum1 = 1,
                TimeSeting1 = 10,
                TimeOver2 = 0,
                CountTime2 = 33,
                PlacementNum2 = 1,
                TimeSeting2 = 10,
                TimeOver3 = 0,
                CountTime3 = 33,
                PlacementNum3 = 1,
                TimeSeting3 = 10,
                TimeOver4 = 0,
                CountTime4 = 33,
                PlacementNum4 = 1,
                TimeSeting4 = 10
            });*/
        }

        public void closeSerialPort()
        {
            if (port.IsOpen)
                try
                {
                    port.Close();
                }
                catch (Exception e)
                {
                    throw e;
                }
        }
        public void OnlineList()
        {
            try
            {
                dvm.DataModelView.Clear();
                for (byte b = odm.ODM.MinNumOfSalve; b <= odm.ODM.MaxNumOfBoard; b++)
                {
                    ushort StandarHTotal = ModbusSerialRtuMasterReadRegisters(port, b, _standarHoursTotalRegAddress, _registers)[0];
                    string LN = "00" + b.ToString();
                    string PID = toString(ModbusSerialRtuMasterReadRegisters(port, b, _productIDRegAddress, _productIDRegNum));
                    ushort PlNum1 = ModbusSerialRtuMasterReadRegisters(port, b, _placementNum1RegAddress, _registers)[0];
                    ushort TSeting1 = ModbusSerialRtuMasterReadRegisters(port, b, _timeSeting1RegAddress, _registers)[0];
                    ushort PlNum2 = ModbusSerialRtuMasterReadRegisters(port, b, _placementNum2RegAddress, _registers)[0];
                    ushort TSeting2 = ModbusSerialRtuMasterReadRegisters(port, b, _timeSeting2RegAddress, _registers)[0];
                    ushort PlNum3 = ModbusSerialRtuMasterReadRegisters(port, b, _placementNum3RegAddress, _registers)[0];
                    ushort TSeting3 = ModbusSerialRtuMasterReadRegisters(port, b, _timeSeting3RegAddress, _registers)[0];
                    ushort PlNum4 = ModbusSerialRtuMasterReadRegisters(port, b, _placementNum4RegAddress, _registers)[0];
                    ushort TSeting4 = ModbusSerialRtuMasterReadRegisters(port, b, _timeSeting4RegAddress, _registers)[0];

                    dvm.DataModelView.Add(new DataModel() {
                        SlaveID = b,
                        StandardHoursTotal =StandarHTotal,
                        LineName =LN,
                        ProductID =PID,
                        PlacementNum1 =PlNum1,
                        PlacementNum2=PlNum2,
                        PlacementNum3=PlNum3,
                        PlacementNum4=PlNum4,
                        TimeSeting1=TSeting1,
                        TimeSeting2=TSeting2,
                        TimeSeting3=TSeting3,
                        TimeSeting4=TSeting4
                     });
                }
            }
            catch (Exception e)
            {
                if (e.GetType() != typeof(TimeoutException))
                {
                    MessageBox.Show(e.Message.ToString(), "错误窗口", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public void KubotaModusProtocol()
        {
            try
            {
                foreach (DataModel b in dvm.DataModelView)
                {
                    b.ProductNum = ModbusSerialRtuMasterReadRegisters(port, (byte)b.SlaveID, _productNumRegAddress, _registers)[0];
                    //b.StandardHoursTotal = ModbusSerialRtuMasterReadRegisters(port, (byte)b.SlaveID, _standarHoursTotalRegAddress, _registers)[0];
                    b.ActualHoursTotal = ModbusSerialRtuMasterReadRegisters(port, (byte)b.SlaveID, _actualHoursTotalRegAddress, _registers)[0];
                    b.TimeOver1 = ModbusSerialRtuMasterReadRegisters(port, (byte)b.SlaveID, _timeOver1RegAddress, _registers)[0];
                    b.CountTime1 = ModbusSerialRtuMasterReadRegisters(port, (byte)b.SlaveID, _countTime1RegAddress, _registers)[0];                  
                    b.TimeOver2 = ModbusSerialRtuMasterReadRegisters(port, (byte)b.SlaveID, _timeOver2RegAddress, _registers)[0];
                    b.CountTime2 = ModbusSerialRtuMasterReadRegisters(port, (byte)b.SlaveID, _countTime2RegAddress, _registers)[0];
                    //b.PlacementNum2 = ModbusSerialRtuMasterReadRegisters(port, (byte)b.SlaveID, _placementNum2RegAddress, _registers)[0];
                    //b.TimeSeting2 = ModbusSerialRtuMasterReadRegisters(port, (byte)b.SlaveID, _timeSeting2RegAddress, _registers)[0];
                    b.TimeOver3 = ModbusSerialRtuMasterReadRegisters(port, (byte)b.SlaveID, _timeOver3RegAddress, _registers)[0];
                    b.CountTime3 = ModbusSerialRtuMasterReadRegisters(port, (byte)b.SlaveID, _countTime3RegAddress, _registers)[0];
                    //b.PlacementNum3 = ModbusSerialRtuMasterReadRegisters(port, (byte)b.SlaveID, _placementNum3RegAddress, _registers)[0];
                    //b.TimeSeting3 = ModbusSerialRtuMasterReadRegisters(port, (byte)b.SlaveID, _timeSeting3RegAddress, _registers)[0];
                    b.TimeOver4 = ModbusSerialRtuMasterReadRegisters(port, (byte)b.SlaveID, _timeOver4RegAddress, _registers)[0];
                    b.CountTime4 = ModbusSerialRtuMasterReadRegisters(port, (byte)b.SlaveID, _countTime4RegAddress, _registers)[0];
                    //b.PlacementNum4 = ModbusSerialRtuMasterReadRegisters(port, (byte)b.SlaveID, _placementNum4RegAddress, _registers)[0];
                    //b.TimeSeting4 = ModbusSerialRtuMasterReadRegisters(port, (byte)b.SlaveID, _timeSeting4RegAddress, _registers)[0];
                }
            }
            catch (Exception e)
            {
                if (e.GetType() != typeof(TimeoutException))
                {
                    MessageBox.Show(e.Message.ToString(), "错误窗口", MessageBoxButton.OK, MessageBoxImage.Error);
                }
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
                if (s!=0)
                tl.Add((byte)s);
            }
            byte[] temp = tl.ToArray();
            return Encoding.Default.GetString(temp);
        }

    }
}

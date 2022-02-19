using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace BsbPcbaTestSpace
{
    public class SerialCom
    {
        //串口
        public SerialPort port;
        /// <summary>
        /// 串口初始化
        /// </summary>
        /// <param name="ComPort"></param>
        public SerialCom(string ComPort)
        {
            port = new SerialPort(ComPort);
            port.BaudRate = 9600;
            port.DataBits = 8;
            port.Parity = Parity.None;
            port.StopBits = StopBits.One;
        }
        /// <summary>
        /// 关闭串口
        /// </summary>
        /// <returns></returns>
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
                    throw e;
                }
            return true;
        }
        /// <summary>
        /// 打开串口
        /// </summary>
        /// <returns></returns>
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
                    throw e;
                }
            return false;
        }
    }
}

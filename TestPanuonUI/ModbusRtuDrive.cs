using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Modbus.Data;
using Modbus.Device;
using Modbus.Utility;
using System.IO.Ports;
using System.Windows;

namespace ModbusSpace
{
    public class ModbusRtuDrive
    {
       /// <summary>
       /// the master pc write slave holding registers
       /// </summary>
       /// <param name="_portname"></param>
       /// <param name="_slaveid"></param>
       /// <param name="_startAddress"></param>
       /// <param name="_registers"></param>
        public void ModbusSerialRtuMasterWriteRegisters(SerialPort _port, byte _slaveid, ushort _startAddress, ushort[] _registers)
        {
            using (IModbusSerialMaster master = ModbusSerialMaster.CreateRtu(_port))
            {
                if (!_port.IsOpen)
                {
                    try
                    {
                        _port.Open();
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message.ToString(), "错误窗口", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                // create modbus master
                master.Transport.ReadTimeout = 2000;
                master.Transport.WriteTimeout = 2000;
                master.Transport.Retries = 0;
                master.Transport.WaitToRetryMilliseconds = 1;
                byte slaveId = _slaveid;
                ushort startAddress = _startAddress;
                ushort[] registers = _registers;

                // write three registers
                master.WriteMultipleRegisters(slaveId, startAddress, registers);
            }
        }
       /// <summary>
        /// the master pc read slave holding registers
       /// </summary>
       /// <param name="_portname"></param>
       /// <param name="_slaveid"></param>
       /// <param name="_startAddress"></param>
       /// <param name="_registersnum"></param>
       /// <returns></returns>
        public ushort[] ModbusSerialRtuMasterReadRegisters(SerialPort _port, byte _slaveid, ushort _startAddress, ushort _registersnum)
        {
            using (IModbusSerialMaster master = ModbusSerialMaster.CreateRtu(_port))
            {
                if (!_port.IsOpen)
                {
                    try
                    {
                        _port.Open();
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message.ToString(), "错误窗口", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                // create modbus master
                master.Transport.ReadTimeout = 1000;
                master.Transport.WriteTimeout = 1000;
                master.Transport.Retries = 0;
                master.Transport.WaitToRetryMilliseconds = 1;
                byte _slaveId = _slaveid;
                // write three registers
                ushort[] temp={};
                temp = master.ReadHoldingRegisters(_slaveId, _startAddress, _registersnum);
                return temp;
            }
        }
        /// <summary>
        /// the master pc write and read slave holding registers
        /// </summary>
        /// <param name="_portname"></param>
        /// <param name="_slaveid"></param>
        /// <param name="_startAddress"></param>
        /// <param name="_numberOfPointsToRead"></param>
        /// <param name="_startWriteAddress"></param>
        /// <param name="_writeData"></param>
        /// <returns></returns>
        public ushort[] ModbusSerialRtuMasterReadWriteRegisters(SerialPort _port, byte _slaveid, ushort _startAddress, ushort _numberOfPointsToRead, ushort _startWriteAddress, ushort[] _writeData)
        {
            using (IModbusSerialMaster master = ModbusSerialMaster.CreateRtu(_port))
            {
                if (!_port.IsOpen)
                {
                    try
                    {
                        _port.Open();
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message.ToString(), "错误窗口", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                // create modbus master
                master.Transport.ReadTimeout = 1000;
                master.Transport.WriteTimeout = 1000;
                master.Transport.Retries = 0;
                master.Transport.WaitToRetryMilliseconds = 1;
                byte _slaveId = _slaveid;
                // write three registers
                ushort[] temp = master.ReadWriteMultipleRegisters(_slaveId, _startAddress, _numberOfPointsToRead, _startWriteAddress, _writeData);
                return temp;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using INotifyPropertyChangedSpace;

namespace TestPanuonUI
{
    public class OptionsDataModel:NotificationObject
    {
        private string _comport = "COM1";
        public string ComPort
        {
            get { return _comport; }
            set { _comport = value; RaisePropertyChanged("ComPort"); }
        }

        private int _maxnumofboard = 1;
        public int MaxNumOfBoard
        {
            get { return _maxnumofboard; }
            set { _maxnumofboard = value; RaisePropertyChanged("MaxNumOfBoard"); }
        }
        private string _boarddataPath = @"d:\OptionsData\";
        public string BoardDataPath
        {
            get { return _boarddataPath; }
            set { _boarddataPath = value; RaisePropertyChanged("BoardDataPath"); }
        }

        private byte _minnumOfSalve = 1;
        public byte MinNumOfSalve
        {
            get { return _minnumOfSalve; }
            set { _minnumOfSalve = value; RaisePropertyChanged("MinNumOfSalve"); }
        }
    }

    public class ComPortCombo
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}

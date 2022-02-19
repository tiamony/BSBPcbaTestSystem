using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using INotifyPropertyChangedSpace;

namespace BsbPcbaTestSpace
{
    public class OptionsDataModel:NotificationObject
    {
        //串口名称
        private string _comport = "COM1";
        public string ComPort
        {
            get { return _comport; }
            set { _comport = value; RaisePropertyChanged("ComPort"); }
        }
        //测试超时时间
        private int _maxnumofboard = 1;
        public int MaxNumOfBoard
        {
            get { return _maxnumofboard; }
            set { _maxnumofboard = value; RaisePropertyChanged("MaxNumOfBoard"); }
        }
        //从机地址
        private byte _minnumOfSalve = 1;
        public byte MinNumOfSalve
        {
            get { return _minnumOfSalve; }
            set { _minnumOfSalve = value; RaisePropertyChanged("MinNumOfSalve"); }
        }
        //上次打开时测试文件路径
        private string _strfilepath;
        public string StrFilePath
        {
            get { return _strfilepath; }
            set { _strfilepath = value;RaisePropertyChanged("StrFilePath"); }
        }
        //测试治具编号
        private uint _testFixtureID = 1;
        public uint TestFixtureID
        {
            get { return _testFixtureID; }
            set { _testFixtureID = value; RaisePropertyChanged("TestFixtureID"); }
        }
        //公司名称
        private string _companyName = "BSB";
        public string CompanyName
        {
            get { return _companyName; }
            set { _companyName = value; RaisePropertyChanged("CompanyName"); }
        }
        //测试数据是否存入数据库
        private bool _isSaveDataToSQL = false;
        public bool IsSaveDataToSQL
        {
            get { return _isSaveDataToSQL; }
            set { _isSaveDataToSQL = value; RaisePropertyChanged("IsSaveDataToSQL"); }
        }
        //测试所在公司
        ObservableCollection<ComanpyName> comanpyNamesList = new ObservableCollection<ComanpyName>();
        public ObservableCollection<ComanpyName> ComanpyNamesList
        {
            get { return comanpyNamesList; }
            set
            {
                comanpyNamesList = value;
                RaisePropertyChanged("ComanpyNamesList");
            }
        }
        //产品名称
        ObservableCollection<ProductName> productNamesList = new ObservableCollection<ProductName>();
        public ObservableCollection<ProductName> ProductNamesList
        {
            get { return productNamesList; }
            set
            {
                productNamesList = value;
                RaisePropertyChanged("ProductNamesList");
            }
        }
        /// <summary>
        /// 获取产品名称（从测试文件获取的名称）
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string GetProductName(string path)
        {
            int i = 0;
            if (path != "" && path != null)
            {
                string[] temp = path.Split(new char[] { '\\', '.' });
                if (temp.Length - 2 > 0)
                  i = temp.Length - 2;
                return temp[i];
            }
           return null;
        }

    }
    /// <summary>
    /// 串口类型
    /// </summary>
    public class ComPortCombo
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}

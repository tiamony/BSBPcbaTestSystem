using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BsbPcbaTestSpace
{
    public class DataWhereSQL:BindableObject
    {
        //测试治具编号
        ObservableCollection<TestFixtureID> testFixtureIDs = new ObservableCollection<TestFixtureID>();
        public ObservableCollection<TestFixtureID> TestFixtureIDs
        {
            get { return testFixtureIDs; }
            set
            {
                testFixtureIDs = value;
                SetValue(testFixtureIDs);
            }
        }
        //测试所在公司
        ObservableCollection<ComanpyName> comanpyNames = new ObservableCollection<ComanpyName>();
        public ObservableCollection<ComanpyName> ComanpyNames
        {
            get { return comanpyNames; }
            set
            {
                comanpyNames = value;
                SetValue(comanpyNames);
            }
        }
        //产品名称
        ObservableCollection<ProductName> productNames = new ObservableCollection<ProductName>();
        public ObservableCollection<ProductName> ProductNames
        {
            get { return productNames; }
            set
            {
                productNames = value;
                SetValue(productNames);
            }
        }
        //测试结果
        ObservableCollection<ResultType> resultTypes= new ObservableCollection<ResultType>();
        public ObservableCollection<ResultType> ResultTypes
        {
            get { return resultTypes; }
            set
            {
                resultTypes = value;
                SetValue(resultTypes);
            }
        }
        //查询结果
        ObservableCollection<SqlTestData> sqlTestDatas = new ObservableCollection<SqlTestData>();
        public ObservableCollection<SqlTestData> SqlTestDatas
        {
            get { return sqlTestDatas; }
            set
            {
                sqlTestDatas = value;
                SetValue(sqlTestDatas);
            }
        }
        //加载标志位
        private bool isSerachLoading;
        public bool IsSerachLoading
        {
            get { return isSerachLoading; }
            set
            {
                isSerachLoading = value;
                SetValue(isSerachLoading);
            }
        }
        //加载导出数据loading标志位
        private bool isPopDataLoading;
        public bool IsPopDataLoading
        {
            get { return isPopDataLoading; }
            set
            {
                isPopDataLoading = value;
                SetValue(isPopDataLoading);
            }
        }
    }
    /// <summary>
    /// 测试治具编号类
    /// </summary>
    public class TestFixtureID
    {
        //测试治具编号
        public uint id { get; set; }
        public string describe { get; set; }
    }
    /// <summary>
    /// 测试所在公司
    /// </summary>
    public class ComanpyName
    {
        //测试所在公司
        public string company { get; set; }
        public string local { get; set; }
    }
    /// <summary>
    /// 产品名称
    /// </summary>
    public class ProductName
    {
        //名称
        public string name { get; set; }
        //显示名
        public string namedescribe { get; set; }
    }
    /// <summary>
    /// 测试结果
    /// </summary>
    public class ResultType
    {
        //测试结果状态
        public ResultEnum rem { get; set; }
        //测试结果描述
        public string resultDescribe { get; set; }
    }
    /// <summary>
    /// 测试结果类型
    /// </summary>
    public enum ResultEnum
    {
        success,
        fail,
    }
    /// <summary>
    /// 搜索选择类型
    /// </summary>
    public enum SqlItemType
    {
        测试员,
        产品条码,
        测试名称,
        测试内容,
        测试治具编号,
    }
    public class SqlTestData
    {
        //数据库项目编号
        public int sqlID { get; set; }
        //是否被选中
        public bool isSelected { get; set; }
        //序号
        public uint id { get; set; }
        //治具编号
        public uint testfixtureid { get; set; }
        //公司名称
        public string companyName { get; set; }
        //产品名称
        public string testProductName { get; set; }
        //测试员
        public string conner { get; set; }
        //条码
        public string barcode { get; set; }
        //项目序号
        public uint itemID { get; set; }
        //是否测试
        public bool isCheckedBoxTrue { get; set; }
        //项目名称
        public string testName { get; set; }
        //测试内容
        public string testBody { get; set; }
        //测试上限
        public float testTopLimit { get; set; }
        //测试下限
        public float testFloor { get; set; }
        //测试值
        public float testValue { get; set; }
        //测试结果
        public string testResult { get; set; }
        //测试备注
        public string remarks { get; set; }
        //测试日期
        public string testdate { get; set; }
    }

}

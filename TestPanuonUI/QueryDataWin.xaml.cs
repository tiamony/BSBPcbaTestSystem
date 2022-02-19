using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ListToDatatable;
using Microsoft.Win32;
using Panuon.UI.Silver;

namespace BsbPcbaTestSpace
{
    /// <summary>
    /// QueryDataWin.xaml 的交互逻辑
    /// </summary>
    public partial class QueryDataWin : WindowX
    {
        //默认搜索选择
        //private SqlItemType sqlType = SqlItemType.产品条码;
        //查询条件数据类
        public DataWhereSQL dws;
        //当前登录用户信息
        private UserInfo pUser = new UserInfo();
        //配置信息
        private OptionsDataModel ODM;
        /// <summary>
        /// 查询窗口构造体
        /// </summary>
        public QueryDataWin(UserInfo presentUser,OptionsDataModel odm)
        {
            InitializeComponent();
            pUser = presentUser;
            ODM = odm;
        }
        /// <summary>
        /// 根据当前登录用户的等级，来开放对应权限
        /// </summary>
        private void PowerMangement()
        {
            switch (pUser.PositionLevel)
            {
                case PositionLevelEnum.Administrator:
                    break;
                case PositionLevelEnum.Engineer:
                    deleteDataBtn.IsEnabled = false;
                    selectAllBtn.IsEnabled = false;
                    cancelSelectAllBtn.IsEnabled = false;
                    break;
                default:
                    deleteDataBtn.IsEnabled = false;
                    selectAllBtn.IsEnabled = false;
                    cancelSelectAllBtn.IsEnabled = false;
                    break;
            }
        }
        /// <summary>
        /// 警告信息提示（一直提示）
        /// </summary>
        /// <param name="message">提示信息</param>
        private void Alert(string message)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                // #FF68217A
                statusBar.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0x21, 0x2A));
                statusInfoTextBlock.Text = message;
            }));
        }
        /// <summary>
        /// 普通状态信息提示
        /// </summary>
        /// <param name="message">提示信息</param>
        private void Information(string message)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                // #FF007ACC
                statusBar.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0x7A, 0xCC));
                statusInfoTextBlock.Text = message;
            }));
        }
        //搜素类型值
        private SqlItemType sqlItemSelect;
        /// <summary>
        /// 搜索类型选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WhereTypeCombox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (whereTypeCombox.SelectedIndex > -1)
            {
                sqlItemSelect = (SqlItemType)whereTypeCombox.SelectedValue;
            }
        }
        /*
        /// <summary>
        /// 加载测试编号列表数据
        /// </summary>
        private void LoadTestFixtureID()
        {
            this.whereTypeCombox.SelectedValue = (SqlItemType)sqlType;
            dws.TestFixtureIDs.Add(new TestFixtureID()
            {
                describe = "不选择",
                id = 0,
            });
            for (int i = 1; i < 50; i++)
                dws.TestFixtureIDs.Add(new TestFixtureID()
                {
                    describe = "BSB-" + i,
                    id = (uint)i,
                });

            this.fixtureIDList.ItemsSource = dws.TestFixtureIDs;
        }
        */
        /// <summary>
        /// 加载公司名称列表数据
        /// </summary>
        private void LoadComanpyNames()
        {
            /*
            dws.ComanpyNames.Add(new ComanpyName()
            {
                company = "不选择",
                local = "未知",
            });
            dws.ComanpyNames.Add(new ComanpyName()
            {
                company = "巴斯巴",
                local = "深圳",
            });
            dws.ComanpyNames.Add(new ComanpyName()
            {
                company = "巴斯巴（遵义）",
                local = "遵义",
            });
            dws.ComanpyNames.Add(new ComanpyName()
            {
                company = "拓邦",
                local = "深圳",
            });
            */
            this.comanpyNameList.ItemsSource = ODM.ComanpyNamesList;
        }
        /// <summary>
        /// 加载产品名称列表数据
        /// </summary>
        private void LoadProductNames()
        {
            /*
            dws.ProductNames.Add(new ProductName()
            {
                name = "不选择",
                namedescribe = "不选择",
            });
            dws.ProductNames.Add(new ProductName()
            {
                name = "ACS32-BSB-LX-TEST",
                namedescribe = "理想2.0交流充电桩",
            });
            dws.ProductNames.Add(new ProductName()
            {
                name = "ACS32-BSB-CC-TEST",
                namedescribe = "长城3合1交流充电桩",
            });
            */
            this.productNameList.ItemsSource = ODM.ProductNamesList;
        }
        /// <summary>
        /// 加载测试结果类型列表
        /// </summary>
        private void LoadResultType()
        {
            dws.ResultTypes.Add(new ResultType()
            {
                rem = ResultEnum.success,
                resultDescribe = "不选择",
            });
            dws.ResultTypes.Add(new ResultType()
            {
                rem = ResultEnum.success,
                resultDescribe = "通过",
            });
            dws.ResultTypes.Add(new ResultType()
            {
                rem = ResultEnum.fail,
                resultDescribe = "不通过",
            });
            this.resultTypeList.ItemsSource = dws.ResultTypes;
        }
        /// <summary>
        /// 窗口加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WindowX_Loaded(object sender, RoutedEventArgs e)
        {
            dws = new DataWhereSQL();
            this.showSqlData.ItemsSource = this.dws.SqlTestDatas;
            this.toolDock.DataContext = this.dws;
            //LoadTestFixtureID();
            LoadComanpyNames();
            LoadProductNames();
            LoadResultType();
            startTime.DateTime = DateTime.Now;
            endTime.DateTime = startTime.DateTime;
            PowerMangement();
        }
        /*
        //治具列表值
        private TestFixtureID fixtureIDselect;
        /// <summary>
        /// 治具列表选择改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FixtureIDList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.fixtureIDList.SelectedIndex > -1)
            {
                fixtureIDselect =(TestFixtureID) this.fixtureIDList.SelectedValue;
            }
        }
        */
        //公司名称值
        private ComanpyName companyNameSelect;
        /// <summary>
        /// 公司名称列表选择改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComanpyNameList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comanpyNameList.SelectedIndex > -1)
            {
                companyNameSelect =(ComanpyName) comanpyNameList.SelectedValue;
            }
        }
        //产品名称值
        private ProductName productNameSelect;
        /// <summary>
        /// 产品名称列表选择改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProductNameList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (productNameList.SelectedIndex > -1)
            {
                productNameSelect =(ProductName) productNameList.SelectedValue;
            }
        }
        //测试结果值
        private ResultType resultTypeSelect;
        /// <summary>
        /// 测试结果类型列表选择改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResultTypeList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (resultTypeList.SelectedIndex > -1)
            {
                resultTypeSelect =(ResultType) resultTypeList.SelectedValue;
            }
        }
        /// <summary>
        /// 搜素框获取焦点事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WhereTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            whereTextBox.BorderBrush = new SolidColorBrush(Colors.Green);
        }
        //搜素框文本值
        private string serachTextBox;
        /// <summary>
        /// 搜素文本框失去焦点事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WhereTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (whereTextBox.Text != "" && whereTextBox.Text != null)
            {
                serachTextBox = whereTextBox.Text;
            }
            else
                serachTextBox = "";
        }
        /// <summary>
        /// 获取数据库查询字符串
        /// </summary>
        /// <returns>返回拼接的字符串</returns>
        private string GetSqlString()
        {
            string sqlStr = "";
            if (serachTextBox != null && serachTextBox != "")
            {
                sqlStr = serachTextBox;
                switch (sqlItemSelect)
                {
                    case SqlItemType.产品条码:
                        sqlStr = "barcode = '"+sqlStr+"'";
                        break;
                    case SqlItemType.测试内容:
                        sqlStr = "%" + sqlStr + "%";
                        sqlStr = "testbody like '" + sqlStr + "'";
                        break;
                    case SqlItemType.测试名称:
                        sqlStr = "%" + sqlStr + "%";
                        sqlStr = "testname like '" + sqlStr + "'";
                        break;
                    case SqlItemType.测试员:
                        sqlStr = "%" + sqlStr + "%";
                        sqlStr = "conner like '" + sqlStr + "'";
                        break;
                    case SqlItemType.测试治具编号:
                        sqlStr = "testfixtureid = '" + sqlStr + "'";
                        break;
                }
            }
            if (companyNameSelect.company != "不选择")
            {
                string s = "%" + companyNameSelect.company + "%";
                if (sqlStr != "")
                    sqlStr += " and localname like '"+s+"'";
                else
                    sqlStr = "localname like '" + s + "'";
            }
            if (productNameSelect.namedescribe != "不选择")
            {
                string s = "%" + productNameSelect.name + "%";
                if (sqlStr != "")
                    sqlStr += " and testproductname like '" + s + "'";
                else
                    sqlStr = "testproductname like '" + s + "'";
            }
            if (resultTypeSelect.resultDescribe != "不选择")
            {
                if (sqlStr != "")
                    sqlStr += " and testresult = '" + resultTypeSelect.resultDescribe + "'";
                else
                    sqlStr = "testresult = '" + resultTypeSelect.resultDescribe + "'";
            }
            int i = DateTime.Compare(startTime.DateTime, endTime.DateTime);
            if (i<0 && startTime.DateTime.ToString() != endTime.DateTime.ToString())
            {
                if (sqlStr != "")
                    sqlStr += " and testdate between '" + startTime.DateTime + "' and '" + endTime.DateTime + "'";
                else
                    sqlStr = " testdate between '" + startTime.DateTime + "' and '" + endTime.DateTime + "'";
            }
            return sqlStr;
        }
        /// <summary>
        /// 切换搜索LOADING
        /// </summary>
        /// <param name="onoff"></param>
        private void SwitchSreachLoading(bool onoff)
        {
            if (onoff)
            {
                this.Dispatcher.Invoke(new Action(() =>
                {
                    dws.IsSerachLoading = true;
                   
                }));
            }else
            {
                this.Dispatcher.Invoke(new Action(() =>
                {
                    dws.IsSerachLoading = false;
                    //this.toolDock.DataContext = this.dws;
                }));
            }
        }
       /// <summary>
       /// 切换导出按钮LOADING状态
       /// </summary>
       /// <param name="onoff"></param>
        private void SwitchPopDataLoading(bool onoff)
        {
            if (onoff)
            {
                this.Dispatcher.Invoke(new Action(() =>
                {
                    dws.IsPopDataLoading = true;

                }));
            }
            else
            {
                this.Dispatcher.Invoke(new Action(() =>
                {
                    dws.IsPopDataLoading = false;
                }));
            }
        }
        /// <summary>
        /// 查询按钮单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Thread startQuerySql = new Thread(() =>
            {
                try
                {
                    string temp = "";
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        temp = GetSqlString();
                    }));
                    if (temp != "")
                    {
                        Information("正在查询，请稍后...");
                        SwitchSreachLoading(true);
                        MySqlHandler msh = new MySqlHandler();
                        dws.SqlTestDatas = new System.Collections.ObjectModel.ObservableCollection<SqlTestData>(msh.GetSqlTestDatas(temp));
                        if (dws.SqlTestDatas != null)
                        {
                            this.Dispatcher.Invoke(new Action(() =>
                            {
                                this.showSqlData.ItemsSource = this.dws.SqlTestDatas;
                            }));
                        }
                        Information("查询已完成,共计：(" + dws.SqlTestDatas.Count + ")条数据。");
                    }
                    else
                        Alert(" 请先选择查询条件。");
                }
                catch (Exception ex)
                {
                    Alert(ex.Message.ToString());
                }
                finally
                {
                    SwitchSreachLoading(false);
                }
            });
            startQuerySql.Start();
        }
        /// <summary>
        /// 取消全选按钮单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (dws.SqlTestDatas.Count > 0)
            {
                foreach (SqlTestData s in dws.SqlTestDatas)
                {
                    s.isSelected = false;
                   
                }
                dws.SqlTestDatas = new System.Collections.ObjectModel.ObservableCollection<SqlTestData>(dws.SqlTestDatas);
                this.showSqlData.ItemsSource = this.dws.SqlTestDatas;
                Information(" 已全部取消全选。");
            }
            else
                Alert(" 没有任何数据可以取消选择。");
        }
        /// <summary>
        /// 全选按钮单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (dws.SqlTestDatas.Count > 0)
            {
                foreach (SqlTestData s in dws.SqlTestDatas)
                {
                    s.isSelected = true;
                }
                dws.SqlTestDatas = new System.Collections.ObjectModel.ObservableCollection<SqlTestData>(dws.SqlTestDatas);
                this.showSqlData.ItemsSource = this.dws.SqlTestDatas;
                Information(" 已选中全部数据。");
            }
            else
                Alert(" 没有任何数据可供选择。");
        }
        /// <summary>
        /// 删除按钮单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Thread deleteSqlDataSelectThread = new Thread(() =>
            {
                int i = 0;
                try
                {
                    if (dws.SqlTestDatas.Count > 0)
                    {
                        foreach (SqlTestData s in dws.SqlTestDatas)
                        {
                            if (s.isSelected)
                            {
                                MySqlHandler msh = new MySqlHandler();
                                if (msh.DeleteSqlDataBySelect(s.sqlID))
                                {
                                    i++;
                                }
                            }
                        }
                        Information("已删除数据：（" + i + ")条。");
                    }
                    else
                        Information(" 没有任何数据。");
                }
                catch (Exception ex)
                {
                    Alert(ex.Message.ToString());
                }

            });
            deleteSqlDataSelectThread.Start();
        }
        /// <summary>
        /// 导出按钮单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            Thread saveSqlDataThread = new Thread(() =>
            {
                try
                {
                    if (dws.SqlTestDatas.Count > 0)
                    {
                        SwitchPopDataLoading(true);
                        Information("正在写入数据...");
                        DataTable dt = ListToDatatableHelper.ToDataTable(dws.SqlTestDatas.ToList());
                        SaveFileDialog dialog = new SaveFileDialog();
                        dialog.Filter = "Office 2007 File|*.xlsx|Office 2000-2003 File|*.xls|所有文件|*.*";
                        dialog.DefaultExt = ".xlsx";
                        dialog.Title = "保存测试文件";
                        if (dialog.ShowDialog() == true)
                        {
                            string filename0 = dialog.FileName;
                            DataTableExcelHelper.TableToExcel(dt, filename0);
                            Information("测试数据已保存到" + filename0);
                        }
                        else
                            Information("已取消数据保存。");
                        /*
                        SwitchPopDataLoading(true);
                        Information("正在写入数据...");
                        DataTable dt = ListToDatatableHelper.ToDataTable(dws.SqlTestDatas.ToList());
                        EcportExcelHelper eeh = new EcportExcelHelper();
                        //eeh.Export(this.showSqlData, "测试数据");
                        string filePath = eeh.ExcelExport(dt, "测试数据");
                        if (filePath != null && filePath != "")
                            Information("测试数据已保存到" + filePath);
                        else
                        {
                            if (filePath == "") Information("已取消数据保存。");
                            else Alert("保存数据失败。");
                        }
                        */
                    }
                }
                catch (Exception ex)
                {
                    Alert(ex.Message.ToString());
                }
                finally
                {
                    SwitchPopDataLoading(false);
                }
            });
            saveSqlDataThread.Start();
        }

    } 

}

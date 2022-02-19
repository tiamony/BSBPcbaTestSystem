using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Panuon.UI.Silver;
using System.IO;
using XmlDeserializeSpace;

namespace BsbPcbaTestSpace
{
    /// <summary>
    /// OptionsWin.xaml 的交互逻辑
    /// </summary>
    public partial class OptionsWin : WindowX
    {
        //配置文件数据
        public OptionsDataModel ODM = new OptionsDataModel();
        //配置文件路径
        private string OptionsDataPath = Directory.GetCurrentDirectory() + @"\config.xml";
        List<ComPortCombo> comboxConent = new List<ComPortCombo>();
        //加载配置文件委托
        public delegate void LoadConfigHandler();
        /// <summary>
        /// 配置参数窗口结构体
        /// </summary>
        /// <param name="odm"></param>
        /// <param name="path"></param>
        public OptionsWin(OptionsDataModel odm,string path)
        {
            InitializeComponent();
            for (int i = 1; i < 20; i++)
                comboxConent.Add(new ComPortCombo { ID = i, Name = "COM" + i });
            PortSetComboBox.ItemsSource = comboxConent;
            this.ODM = odm;
            this.OptionsDataPath = path;
            //this.OptionInit();
            foreach (ComPortCombo c in comboxConent)
            {
                if (c.Name == ODM.ComPort) this.PortSetComboBox.SelectedIndex = c.ID - 1;
            }
            this.DataContext = this.ODM;
            this.companyNameList.ItemsSource = this.ODM.ComanpyNamesList;
            this.productNameList.ItemsSource = this.ODM.ProductNamesList;
            ShowTreeView();
        }
        /// <summary>
        /// 显示配置目录
        /// </summary>
        private void ShowTreeView()
        {
            List<DataTreeViewNodeItem> itemList = new List<DataTreeViewNodeItem>();
            DataTreeViewNodeItem node1 = new DataTreeViewNodeItem()
            {
                DisplayName = "基本设置",
                Name = "基本设置",
                //Icon = @"Icon/baseset.png"
            };
            itemList.Add(node1);
            DataTreeViewNodeItem node1tag1 = new DataTreeViewNodeItem()
            {
                DisplayName = "测试配置",
                Name = "测试配置",
                Icon = @"Icon/testseting.png",
            };
            node1.Children.Add(node1tag1);
            DataTreeViewNodeItem node1tag2 = new DataTreeViewNodeItem()
            {
                DisplayName = "数据库设置",
                Name = "数据库设置",
                Icon = @"Icon/dbset.png",
            };
            node1.Children.Add(node1tag2);
            
            DataTreeViewNodeItem node2 = new DataTreeViewNodeItem()
            {
                DisplayName = "查询条件设置",
                Name = "数据库查询条件设置",
                //Icon = @"Icon/dbset.png",
            };
            itemList.Add(node2);
            DataTreeViewNodeItem node2tag1 = new DataTreeViewNodeItem()
            {
                DisplayName = "公司名称列表设置",
                Name = "公司名称列表设置",
                Icon = @"Icon/companyName.png",
            };
            node2.Children.Add(node2tag1);
            DataTreeViewNodeItem node2tag2 = new DataTreeViewNodeItem()
            {
                DisplayName = "产品名称列表设置",
                Name = "产品名称列表设置",
                Icon = @"Icon/productName.png",
            };
            node2.Children.Add(node2tag2);
            ConfigItemTreeView.ItemsSource = itemList;

        }
        /// <summary>
        /// 配置项目按下释放事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConfigItemTreeView_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (this.ConfigItemTreeView.SelectedItem != null)
                {
                    DataTreeViewNodeItem dvn = ConfigItemTreeView.SelectedItem as DataTreeViewNodeItem;
                    if (dvn.Name == "基本设置" || dvn.Name == "测试配置")
                    {
                        DBSetingGb.Visibility = Visibility.Hidden;
                        companyNameSetGb.Visibility = Visibility.Hidden;
                        productameSetGb.Visibility = Visibility.Hidden;
                        TestSetingGb.Visibility = Visibility.Visible;
                    }
                    if (dvn.Name == "数据库设置")
                    {
                        DBSetingGb.Visibility = Visibility.Visible;
                        TestSetingGb.Visibility = Visibility.Hidden;
                        companyNameSetGb.Visibility = Visibility.Hidden;
                        productameSetGb.Visibility = Visibility.Hidden;
                    }
                    if (dvn.Name == "数据库查询条件设置" || dvn.Name == "公司名称列表设置")
                    {
                        DBSetingGb.Visibility = Visibility.Hidden;
                        TestSetingGb.Visibility = Visibility.Hidden;
                        companyNameSetGb.Visibility = Visibility.Visible;
                        productameSetGb.Visibility = Visibility.Hidden;
                    }
                    if (dvn.Name == "产品名称列表设置")
                    {
                        DBSetingGb.Visibility = Visibility.Hidden;
                        TestSetingGb.Visibility = Visibility.Hidden;
                        companyNameSetGb.Visibility = Visibility.Hidden;
                        productameSetGb.Visibility = Visibility.Visible;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        //加载配置文件事件
        public event LoadConfigHandler LoadConfigEvent;
        /// <summary>
        /// 确定（单击事件）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            XmlSerializa.CreateXML(ODM, OptionsDataPath);
            LoadConfigEvent?.Invoke();
            this.Close();
        }
        /// <summary>
        /// 取消（单击事件）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 窗口列表选择改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PortSetComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.PortSetComboBox.SelectedIndex > -1)
            {
                ODM.ComPort = comboxConent[this.PortSetComboBox.SelectedIndex].Name;
            }
        }
        /// <summary>
        /// 添加公司名称列表按钮单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddItemBtn_Click(object sender, RoutedEventArgs e)
        {
            this.ODM.ComanpyNamesList.Add(new ComanpyName()
            {
                company = "巴斯巴",
                local = "深圳",
            });
        }
        /// <summary>
        /// 移除公司名称列表按钮单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteItemBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.ODM.ComanpyNamesList.Count > 0)
                    this.ODM.ComanpyNamesList.RemoveAt(companySelectID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //选中公司名称列表序号
        private int companySelectID;
        /// <summary>
        /// 公司名称列表选择改变时触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CompanyNameList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.companyNameList.SelectedIndex > -1)
            {
                companySelectID = this.companyNameList.SelectedIndex;
            }
        }
        /// <summary>
        /// 添加产品名称按钮单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddProductBtn_Click(object sender, RoutedEventArgs e)
        {
            this.ODM.ProductNamesList.Add(new ProductName()
            {
                name = "ACS32-BSB-CC-TEST",
                namedescribe = "理想2.0",
            });
        }
        //选中产品名称列表序号
        private int productSelectID;
        /// <summary>
        /// 产品名称列表选择改变时事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProductNameList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.productNameList.SelectedIndex > -1)
            {
                productSelectID = this.productNameList.SelectedIndex;
            }
        }
        /// <summary>
        /// 移除产品名称单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteProductBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.ODM.ProductNamesList.Count > 0)
                {
                    this.ODM.ProductNamesList.RemoveAt(productSelectID);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

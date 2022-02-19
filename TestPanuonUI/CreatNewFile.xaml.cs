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
using Microsoft.Win32;
using XmlDeserializeSpace;
using System.Threading;
using System.Collections.ObjectModel;

namespace BsbPcbaTestSpace
{
    /// <summary>
    /// CreatNewFile.xaml 的交互逻辑
    /// </summary>
    public partial class CreatNewFile : WindowX
    {
        private DataViewModel testItemData = new DataViewModel();
        private DataViewModel testItemSource = new DataViewModel();
        private uint i                     = 1;
        public CreatNewFile()
        {
            InitializeComponent();
            this.NewTestFile.ItemsSource = this.testItemData.TestDataModelView;
            this.DataContext = this;
        }

        private void Newbtn_Click(object sender, RoutedEventArgs e)
        {
            this.testItemData.TestDataModelView.Add(new TestDataModel() { id = i++ ,
                isCheckedBoxTrue = true});
        }

        private void Deletebtn_Click(object sender, RoutedEventArgs e)
        {
           
                try
                {
                    if (this.NewTestFile.SelectedIndex > -1)
                    {
                        this.testItemData.TestDataModelView.RemoveAt(this.NewTestFile.SelectedIndex);
                        i--;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "错误窗口", MessageBoxButton.OK, MessageBoxImage.Error);
                }
        }

        private void Savebtn_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "测试文件 (*.xml)|*.xml";
            dialog.DefaultExt = ".xml";
            dialog.Title = "保存测试文件";

            if (dialog.ShowDialog() == true)
            {
                string filename0 = dialog.FileName;
                SaveFile(filename0);
            }
           
        }
        private void SaveFile(string filestr)
        {
            
            XmlSerializa.CreateXML(this.testItemData.TestDataModelView, filestr);
        }
        private void OpenFile(string filestr)
        {
            this.testItemSource.TestDataModelView = (XmlSerializa.Deserialize(typeof(ObservableCollection<TestDataModel>),
                filestr) as ObservableCollection<TestDataModel>);
        }
        private void RefreshData()
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                try
                {
                    if (this.testItemSource.TestDataModelView.Count > 0)
                    {
                        this.testItemData.TestDataModelView.Clear();
                        foreach (TestDataModel t in testItemSource.TestDataModelView)
                        {
                            this.testItemData.TestDataModelView.Add(t);
                        }
                    }
                }
                catch (Exception ex)
                {
                    //throw ex;
                    MessageBox.Show(ex.Message.ToString(), "错误窗口", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }));
        }

        private void Openbtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "测试文件 (*.xml)|*.xml";
            dialog.DefaultExt = ".xml";
            dialog.Title = "打开测试文件";

            if (dialog.ShowDialog() == true)
            {
                string filename0 = dialog.FileName;
                OpenFile(filename0);
                RefreshData();
            }
        }

        private void Exitbtn_Click(object sender, RoutedEventArgs e)
        {
            //Environment.Exit(0);
            this.Close();
        }

        private void Sortbtn_Click(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                try
                {
                    if (this.testItemData.TestDataModelView.Count > 0)
                    {
                        for (uint i = 0; i < this.testItemData.TestDataModelView.Count; i++)
                        {
                            TestDataModel t = this.testItemData.TestDataModelView[(int)i];
                            this.testItemData.TestDataModelView[(int)i] = (new TestDataModel()
                            {
                                id = i + 1,
                                isCheckedBoxTrue = t.isCheckedBoxTrue,
                                testBody = t.testBody,
                                testFloor = t.testFloor,
                                testName = t.testName,
                                testTopLimit = t.testTopLimit,
                                pointtitle = t.pointtitle,
                                cmd= t.cmd
                            });
                        }

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "错误窗口", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }));
        }


        private void CreatFileWindow_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
            }
            else
                this.WindowState = WindowState.Maximized;
        }
    }
}

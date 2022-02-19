using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Panuon.UI.Silver;
using System.Threading;
using Microsoft.Win32;
using XmlDeserializeSpace;
using System.Collections.ObjectModel;
using System.IO;

namespace BsbPcbaTestSpace
{
    public partial class MainWindow : WindowX
    {
       //测试线程
       private Thread startProtocolThread;
        /// <summary>
        /// 测试线程初始化
        /// </summary>
        private void TestCoreInit()
        {           
            startProtocolThread = new Thread(StartTestThread);
            startProtocolThread.IsBackground = true;
            startProtocolThread.Start();
        }
        /// <summary>
        /// 向数据库写入测试数据
        /// </summary>
        private bool WriteTestDataToSQL()
        {
            MySqlHandler msh = new MySqlHandler();
            List<TestDataModel> sqlListTestData = new List<TestDataModel>(); ;
            bool res = false;
            try
            {
                foreach (TestDataModel t in dvmEx.TestDataModelView)
                {
                    t.testfixtureid = opdmEx.TestFixtureID;
                    t.localname = opdmEx.CompanyName;
                    t.testProductName = opdmEx.GetProductName(opdmEx.StrFilePath);
                    t.conner = duvmEx.PresentUserInfo.User;
                    t.barcode = "";
                    t.testdate = DateTime.Now;
                    sqlListTestData.Add(t);
                }
                msh.InsertintoDB(sqlListTestData);
                res = true;
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 测试线程事件执行函数
        /// </summary>
        private void StartTestThread()
        {
        try
        {
            while (!isStopThread)
            {
                    switch (tsState)
                    {
                        case TSRunState.RUN_STATE_START:
                            if (comState == SerialState.SERIAL_STATE_OPEN)
                            {
                                if (uiState == UIState.UI_STATE_CLEANED)
                                {
                                    Information(" 正在测试...");
                                    tsState = TSRunState.RUN_STATE_RUNNING;
                                    if (this.DVM.isSelectTestAll && this.DVM.isStopWhenFinsh)
                                        StartTestAll();
                                    if (this.DVM.isSelectTestOne && this.selectedID > -1)
                                        StartTestOne();
                                    if (this.DVM.isStopWhenFail && this.DVM.isSelectTestAll)
                                        StartTestAll_stopToFail();
                                    tsState = TSRunState.RUN_STATE_STOP;                                   
                                }
                            }
                            break;
                        case TSRunState.RUN_STATE_STOP:
                            Information(" 测试已停止");
                            if (uiState == UIState.UI_STATE_STOPUP)
                            {
                                tsState = TSRunState.RUN_STATE_READY;
                                if (opdmEx.IsSaveDataToSQL)
                                {
                                    try
                                    {
                                        if (WriteTestDataToSQL())
                                            Information(" 写入测试数据到数据库成功。");
                                        else Information(" 写入测试数据到数据库失败。");
                                    }
                                    catch (Exception ex)
                                    {
                                        Alert(ex.Message.ToString());
                                    }
                                }
                                uiState = UIState.UI_STATE_LOADED;
                            }
        
                            break;
                        case TSRunState.RUN_STATE_READY:
                            if (comState == SerialState.SERIAL_STATE_CLOSE)
                                tsState = TSRunState.RUN_STATE_NOREADY;
                            break;
                        case TSRunState.RUN_STATE_NOREADY:
                            if (uiState == UIState.UI_STATE_LOADED && comState == SerialState.SERIAL_STATE_OPEN)
                                tsState = TSRunState.RUN_STATE_READY;
                            break;
                        case TSRunState.RUN_STATE_INIT:
                            if (uiState == UIState.UI_STATE_LOADED && tfState == TestFileState.TF_STATE_LOADED
                                && comState == SerialState.SERIAL_STATE_OPEN)
                                tsState = TSRunState.RUN_STATE_READY;
                            break;
                    }
                    switch (tfState)
                    {
                        case TestFileState.TF_STATE_NOFILE:
                            if (this.opdmEx.StrFilePath != null)
                            {
                                if (File.Exists(opdmEx.StrFilePath))
                                {
                                    this.dvmEx.TestDataModelView = XmlSerializa.Deserialize(typeof(ObservableCollection<TestDataModel>),
                                        this.opdmEx.StrFilePath) as ObservableCollection<TestDataModel>;
                                    tfState = TestFileState.TF_STATE_LOADING;
                                }
                                else Alert("测试文件不存在,请重新选择。");
                            }
                            break;
                        case TestFileState.TF_STATE_LOADING:
                            if (uiState == UIState.UI_STATE_LOADED)
                            tfState = TestFileState.TF_STATE_LOADED;
                            break;
                        case TestFileState.TF_STATE_OPENFILE:
                            OpenFileDialog dialog = new OpenFileDialog();
                            dialog.Filter = "测试文件 (*.xml)|*.xml";
                            dialog.DefaultExt = ".xml";
                            dialog.Title = "打开测试文件";
                            if (dialog.ShowDialog() == true)
                            {
                                string filename0 = dialog.FileName;
                                this.opdmEx.StrFilePath = filename0;
                                
                                this.dvmEx.TestDataModelView = (XmlSerializa.Deserialize(typeof(ObservableCollection<TestDataModel>),
                                 filename0) as ObservableCollection<TestDataModel>);
                               
                            }
                            tfState = TestFileState.TF_STATE_LOADING;
                            uiState = UIState.UI_STATE_INI;
                            break;
                    }

                Thread.Sleep(100);
            }
        }
        catch (Exception ex)
        {
            if (!ex.Message.ToString().Contains("正在中止线程"))
                    //MessageBox.Show(ex.Message.ToString(), "错误窗口", MessageBoxButton.OK, MessageBoxImage.Error);
                    Alert(ex.Message.ToString());
            }

    }

}
}


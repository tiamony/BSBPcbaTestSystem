using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Panuon.UI.Silver;
using System.Threading;
using System.Drawing;
using System.Windows.Media.Imaging;

namespace BsbPcbaTestSpace
{
    public partial class MainWindow : WindowX
    {
        private Thread refreshDataThread;
        #region 初始化
        /// <summary>
        /// UI线程初始化
        /// </summary>
        private void UICoreInit()
        {
            refreshDataThread = new Thread(UpdataUIThread);
            refreshDataThread.IsBackground = true;
            refreshDataThread.Start();
        }
        #endregion
        /// <summary>
        /// 获取用户图像
        /// </summary>
        /// <param name="pe">用户等级</param>
        /// <returns></returns>
        public static BitmapImage GetFaceImage(PositionLevelEnum pe)
        {
            BitmapImage icon = new BitmapImage();
            icon.BeginInit();
            switch (pe)
            {
                case PositionLevelEnum.Administrator:
                    icon.UriSource = new Uri(
                                    "Icon/adminstrator.png", UriKind.Relative);
                    break;
                case PositionLevelEnum.Engineer:
                    icon.UriSource = new Uri(

                                    "Icon/Engineer.png", UriKind.Relative);
                    break;
                case PositionLevelEnum.Conner:
                    icon.UriSource = new Uri(
                                    "Icon/user.png", UriKind.Relative);
                    break;
                default:
                    icon.UriSource = new Uri(
                                    "Icon/user.png", UriKind.Relative);
                    break;
            }
            icon.EndInit();
            return icon;
        }
        /// <summary>
        /// 用户头像更新
        /// </summary>
        private void UIuserImage()
        {
            BitmapImage icon = GetFaceImage(this.duvmEx.PresentUserInfo.PositionLevel);
            this.duvmEx.PresentUserInfo.ShowFaceImage = icon;
        }
        #region UI MainWindow Title
        /// <summary>
        /// MainWindow标题内容更新
        /// </summary>
        private void MainWindowTitleHandler()
        {
            this.titleWindow.Title = "充电桩PCBA测试" + " " + this.opdmEx.StrFilePath;
        }
        #endregion
        #region UI Result TextBlock
        /// <summary>
        /// 结果显示文本内容更新
        /// </summary>
        private void ResultTextblockHandler()
        {
            switch (tsState)
            {
                case TSRunState.RUN_STATE_LOADFILE:
                    this.dvmEx.ResultViewModel = "待测试";
                    break;
                case TSRunState.RUN_STATE_RUNNING:
                    this.dvmEx.ResultViewModel = "测试中...";
                    break;
                case TSRunState.RUN_STATE_ABORT:
                    this.dvmEx.ResultViewModel = "终止中...";
                    break;

            }
            if (this.dvmEx.ResultViewModel != null)
              this.DVM.ResultViewModel = dvmEx.ResultViewModel;
        }
        #endregion
        #region UI Start、Stop And conn connnot Btn
        private void ConnBtnHandler()
        {
            if (comState == SerialState.SERIAL_STATE_OPEN)
            {
                ConnStatuePush("已连接", MessageType.MSG_TYPE_OPEN);
              
                if (this.openCom.IsEnabled)
                {
                    this.openCom.IsEnabled = false;
                }
            }
            else openCom.IsEnabled = true;
        }
        private void ConnnotBtnHandler()
        {
            if (comState == SerialState.SERIAL_STATE_CLOSE || tsState == TSRunState.RUN_STATE_RUNNING)
            {
                if (comState == SerialState.SERIAL_STATE_CLOSE)
                {
                    ConnStatuePush("已断连接", MessageType.MSG_TYPE_NORMAL);
                  
                }
                if (this.closeCom.IsEnabled)
                {
                    this.closeCom.IsEnabled = false;
                }
            }
            else closeCom.IsEnabled = true;
        }
        /// <summary>
        /// 终止测试按钮更新
        /// </summary>
        private void AbortBtnHandler()
        {
            if (tsState == TSRunState.RUN_STATE_ABORT)
                this.DVM.isAboutTestFlag = true;
            if (tsState == TSRunState.RUN_STATE_STOP)
            {
                this.DVM.isAboutTestFlag = false;
                if (this.stopTestBtn.IsEnabled)
                {
                    this.stopTestBtn.IsEnabled = false;
                }
            }
            if (tsState == TSRunState.RUN_STATE_START)
            {
                if (!this.stopTestBtn.IsEnabled)
                {
                    this.stopTestBtn.IsEnabled = true;
                }
            }

        }
        /// <summary>
        /// 启动测试按钮更新
        /// </summary>
        private void StartBtnHandler()
        {
            if (tsState == TSRunState.RUN_STATE_READY)
            {
                if (!this.startTestBtn.IsEnabled)
                    this.startTestBtn.IsEnabled = true;
            }
            if (tsState == TSRunState.RUN_STATE_NOREADY)
            {
                if (startTestBtn.IsEnabled)
                    this.startTestBtn.IsEnabled = false;
            }
            if (tsState == TSRunState.RUN_STATE_RUNNING)
            {
                if (startTestBtn.IsEnabled)
                    this.startTestBtn.IsEnabled = false;
            }
        }
        #endregion
        #region UI 表格
        /// <summary>
        /// 表格内容更新
        /// </summary>
        private void DataGridHandler()
        {
            if (tfState == TestFileState.TF_STATE_LOADING)
            {
                if (uiState != UIState.UI_STATE_LOADED)
                {
                    UpdataTestFileToDataGrid();
                    uiState = UIState.UI_STATE_LOADED;
                }
            }
            switch (tsState)
            {
                case TSRunState.RUN_STATE_LOADFILE:
                    if (uiState != UIState.UI_STATE_LOADED)
                    {
                        UpdataTestFileToDataGrid();
                        uiState = UIState.UI_STATE_LOADED;
                    }
                    break;
                case TSRunState.RUN_STATE_START:
                    if (this.dvmEx.TestDataModelView.Count > 0)
                        for (ushort i = 0; i < this.dvmEx.TestDataModelView.Count; i++)
                        {
                            this.dvmEx.TestDataModelView[i].testResult = null;
                            this.dvmEx.TestDataModelView[i].remarks = null;
                        }
                    uiState = UIState.UI_STATE_CLEANED;
                    break;
                case TSRunState.RUN_STATE_RUNNING:
                    UpdataDataGrid();
                    break;
                case TSRunState.RUN_STATE_STOP:
                    UpdataDataGrid();
                    uiState = UIState.UI_STATE_STOPUP;
                    break;
            }
        }
        private void UpdataDataGrid()
        {
            uint c = (uint)dvmEx.TestDataModelView.Count;
            for (ushort i = 0; i < c; i++)
            {
                //this.DVM.ResultViewModel = dvmEx.ResultViewModel;
                this.DVM.progressPrecent = testPrecent;
                TestDataModel d = dvmEx.TestDataModelView[i];
                if (DVM.TestDataModelView.Count < i + 1)
                {
                    DVM.TestDataModelView.Add(new TestDataModel()
                    {
                        isCheckedBoxTrue = d.isCheckedBoxTrue,
                        id = d.id,
                        testName = d.testName,
                        testBody = d.testBody,
                        testTopLimit = d.testTopLimit,
                        testFloor = d.testFloor,
                        testResult = d.testResult,
                        remarks = d.remarks,
                        resultData = d.resultData
                    });
                }
                else
                {
                    d.isCheckedBoxTrue = DVM.TestDataModelView[i].isCheckedBoxTrue;
                    DVM.TestDataModelView[i] = (new TestDataModel()
                    {
                        isCheckedBoxTrue = d.isCheckedBoxTrue,
                        id = d.id,
                        testName = d.testName,
                        testBody = d.testBody,
                        testTopLimit = d.testTopLimit,
                        testFloor = d.testFloor,
                        testResult = d.testResult,
                        remarks = d.remarks,
                        resultData = d.resultData
                    });

                }
            }
        }
        /// <summary>
        /// 更新测试文件到表格
        /// </summary>
        private void UpdataTestFileToDataGrid()
        {
            try
            {
                if (this.dvmEx.TestDataModelView.Count > 0)
                {
                    this.DVM.TestDataModelView.Clear();
                    foreach (TestDataModel t in dvmEx.TestDataModelView)
                    {
                        this.DVM.TestDataModelView.Add(t);
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message.ToString(), "错误窗口", MessageBoxButton.OK, MessageBoxImage.Error);
                Alert(ex.Message.ToString());
            }
        }
        #endregion
        #region UI Thread
        /// <summary>
        /// UI更新线程
        /// </summary>
        private void UpdataUIThread()
        {
            try
            {
                while (!isStopThread)
                {
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        UpdateTimeDate();
                        MainWindowTitleHandler();
                        ResultTextblockHandler();
                        DataGridHandler();
                        AbortBtnHandler();
                        StartBtnHandler();
                        ConnBtnHandler();
                        ConnnotBtnHandler();
                        //UIuserImage();
                    }
                    ));
                    Thread.Sleep(400);
                }
            }
            catch (Exception ex)
            {
                if (!ex.Message.ToString().Contains("正在中止线程"))
                    //MessageBox.Show(ex.Message.ToString(), "错误窗口", MessageBoxButton.OK, MessageBoxImage.Error);
                    Alert(ex.Message.ToString());
            }
        }
        #endregion

    }
}

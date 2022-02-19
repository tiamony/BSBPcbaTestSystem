using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Panuon.UI.Silver;
using System.Windows.Input;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using XmlDeserializeSpace;

namespace BsbPcbaTestSpace
{
    public partial class MainWindow : WindowX
    {
        /// <summary>
        /// 根据登录用户等级进行权限管理
        /// </summary>
        private void PowerMangement()
        {
            switch (duvmEx.PresentUserInfo.PositionLevel)
            {
                case PositionLevelEnum.Administrator:
                    break;
                case PositionLevelEnum.Engineer:
                    break;
                case PositionLevelEnum.Conner:
                    this.newFileMenu.IsEnabled = false;
                    if (this.duvmEx.PresentUserInfo.User != "Unknown")
                        this.setParmMenu.IsEnabled = false;
                    else
                        this.setParmMenu.IsEnabled = true;
                    this.reloadSetingMenu.IsEnabled = false;
                    this.sqlQueryMenu.IsEnabled = false;
                    break;
                default:
                    this.newFileMenu.IsEnabled = false;
                    this.setParmMenu.IsEnabled = false;
                    this.reloadSetingMenu.IsEnabled = false;
                    this.sqlQueryMenu.IsEnabled = false;
                    break;
            }
        }
        /// <summary>
        /// 窗口加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TitleWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //菜单命令绑定
            IcommdMenuInit();
            //数据绑定
            this.TestBody.ItemsSource = this.DVM.TestDataModelView;
            this.testSTP.DataContext = this.DVM;
            this.resultLabel.DataContext = this.DVM;
            this.TestingFlagProbar.DataContext = this.DVM;
            this.userInfoCanvas.DataContext = this.duvmEx.PresentUserInfo;
            this.DataContext = this;
            //测试线程初始化
            TestCoreInit();
            //UI更新线程初始化
            UICoreInit();
            //更新用户图像UI
            UIuserImage();
            //权限更新
            PowerMangement();
            //用户名提示信息更新
            UserNameToolTipUpdate();
        }
        /// <summary>
        /// 用户名提示信息更新
        /// </summary>
        private void UserNameToolTipUpdate()
        {
            if (this.duvmEx.PresentUserInfo.OnlineState == OnlineEnum.Online)
                this.userName.ToolTip = "已登陆";
            else
                this.userName.ToolTip = "未登陆";
        }
        /// <summary>
        /// 用户名UI效果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Mouse_MouseMove(object sender, MouseEventArgs e)
        {
            this.userName.Foreground = new SolidColorBrush(Colors.Blue);
        }
        /// <summary>
        /// 用户名UI效果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserName_MouseLeave(object sender, MouseEventArgs e)
        {
            this.userName.Foreground = new SolidColorBrush(Colors.Gray);
        }
        /// <summary>
        /// 用户名单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserName_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (this.duvmEx.PresentUserInfo.OnlineState == OnlineEnum.Online)
            {
                Usermanagement umw = new Usermanagement(duvmEx);
                umw.ShowDialog();
            }
        }
        /// <summary>
        /// 取消被选中的测试项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            this.TestBody.SelectedIndex = -1;
        }
        /// <summary>
        /// 窗口关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WindowX_Closing(object sender, CancelEventArgs e)
        {
            try
            {
                this.isStopThread = true;
                scEx.closeSerialPort();
                startProtocolThread.Abort();
                refreshDataThread.Abort();
                if (this.opdmEx.StrFilePath != "" && this.opdmEx.StrFilePath != null
                    && this.strFilePathed != this.opdmEx.StrFilePath)
                    XmlSerializa.CreateXML(opdmEx, OptionsDataPath);
                if (this.duvmEx.PresentUserInfo.User != "Unknown")
                {
                    MySqlHandler msh = new MySqlHandler();
                    msh.UpdateMysql(this.duvmEx.PresentUserInfo.User, "离线", OnlineEnum.Offline);
                }
                Environment.Exit(0);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBoxX.Show(ex.Message.ToString(), "错误", null, MessageBoxButton.OK, configKey: "ErrorTheme");
            }
            finally
            {
                this.isStopThread = true;
                scEx.closeSerialPort();
                startProtocolThread.Abort();
                refreshDataThread.Abort();
                Environment.Exit(0);
                this.Close();
            }
        }
        /// <summary>
        /// 测试表格被选中事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TestBody_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.TestBody.SelectedIndex > -1)
            {
                this.selectedID = this.TestBody.SelectedIndex;
            }
        }
        /// <summary>
        /// 窗口双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TitleWindow_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (this.titleWindow.ActualHeight >= SystemParameters.WorkArea.Size.Height &&
                this.titleWindow.ActualWidth >= SystemParameters.WorkArea.Size.Width)
            {
                this.WindowState = WindowState.Normal;
            }
            else
                this.WindowState = WindowState.Maximized;

        }
    }
}

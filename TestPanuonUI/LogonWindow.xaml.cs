using System;
using System.Collections.Generic;
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
using Panuon.UI.Silver;

namespace BsbPcbaTestSpace
{
    /// <summary>
    /// LogonWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LogonWindow : WindowX
    {
        private bool isUserNameOK = false;
        private bool isPswOK = false;
        private bool logonState = false;
        public LogonWindow()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WindowX_Loaded(object sender, RoutedEventArgs e)
        {
           
        }
        /// <summary>
        /// 状态栏信息提示
        /// </summary>
        /// <param name="msg">警告信息</param>
        private void Alert(string msg)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                // #FF68217A
                statusBar.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0x21, 0x2A));
                statusInfoTextBlock.Text = msg;
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
        /// <summary>
        /// 用户输入框文本改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserNameTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            Information("正在输入用户名...");
            this.userNameTB.BorderBrush = new SolidColorBrush(Colors.Black);
            if (this.userNameTB.Text != "" && this.pswTB.Password != "")
            {
                this.logonBtn.Content = "登陆";
                logonState = true;
            }
            else
            {
                this.logonBtn.Content = "离线登陆";
                logonState = false;
            }
        }
        /// <summary>
        /// 用户文本框失去焦点事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserNameTB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (userNameTB.Text == "" || userNameTB.Text == null)
            {
                this.userNameTB.BorderBrush = new SolidColorBrush(Colors.Red);
                Alert("用户名不能为空。");
                isUserNameOK = false;
            }
            else isUserNameOK = true;
        }
        /// <summary>
        /// 密码框失去焦点事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PswTB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (pswTB.Password == "" || pswTB.Password == null)
            {
                this.pswTB.BorderBrush = new SolidColorBrush(Colors.Red);
                Alert("密码不能为空。");
                isPswOK = false;
            }
            else isPswOK = true;
            if (pswTB.Password.Length < 6)
            {
                this.pswTB.BorderBrush = new SolidColorBrush(Colors.Red);
                Alert("密码长度不能小于6位。");
                isPswOK = false;
            }
            else isPswOK = true;
        }
        /// <summary>
        /// 密码框密码改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PswTB_PasswordChanged(object sender, RoutedEventArgs e)
        {
            Information("正在输入密码...");
            this.pswTB.BorderBrush = new SolidColorBrush(Colors.Black);
            if (this.userNameTB.Text != "" && this.pswTB.Password != "")
            {
                this.logonBtn.Content = "登陆";
                logonState = true;
            }
            else
            {
                this.logonBtn.Content = "离线登陆";
                logonState = false;
            }
        }
        /// <summary>
        /// 登录UI交互
        /// </summary>
        /// <param name="isOK">两种状态标志位</param>
        private void LognUI(bool isOK)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                if (isOK)
                {
                    this.logonLoad.IsRunning = true;
                    this.logonBtn.IsEnabled = false;
                    this.logonBtn.Opacity = 0.5;
                }
                else
                {
                    this.logonLoad.IsRunning = false;
                    this.logonBtn.IsEnabled = true;
                    this.logonBtn.Opacity = 1;
                }
            }));
        }
        /// <summary>
        /// 登录事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Thread logonThread = new Thread(() =>
            {
                if (logonState == true)
                {
                    string user = null;
                    string psw = null;
                    MySqlHandler msh = new MySqlHandler();
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        user = userNameTB.Text;
                        psw = pswTB.Password;
                    }));
                    try
                    {
                        Information(" 正在登录...");
                        if (isPswOK && isUserNameOK)
                        {
                            LognUI(true);
                            //从数据库中判断用户名和密码是否正确
                            UserInfo userTemp = msh.LogonUserInfo(user, psw);
                            if (userTemp != null)
                            {
                                if (msh.UpdateMysql(user, "在线", OnlineEnum.Online))
                                {
                                    this.Dispatcher.Invoke(new Action(() =>
                                    {
                                        MainWindow mw = new MainWindow(userTemp);
                                        Window window = Window.GetWindow(this);//关闭父窗体
                                        window.Close();
                                        mw.ShowDialog();
                                    }));
                                }
                            }
                            else
                            {
                                Alert("用户名或密码错误。");
                                LognUI(false);
                            }
                        }
                        else Alert("用户名或密码错误。");
                    }
                    catch (Exception ex)
                    {
                        Alert(ex.Message.ToString());
                    }
                    finally
                    {
                        LognUI(false);
                    }
                }
                else
                {
                    try
                    {
                        Information(" 正在登录...");
                        LognUI(true);
                        this.Dispatcher.Invoke(new Action(() =>
                        {
                            MainWindow mw = new MainWindow();
                            Window window = Window.GetWindow(this);//关闭父窗体
                            window.Close();
                            mw.ShowDialog();
                        }));
                    }
                    catch (Exception ex)
                    {
                        Alert(ex.Message.ToString());
                    }
                    finally
                    {
                        LognUI(false);
                    }
                }
            });
            logonThread.Start();
        }
    }
}

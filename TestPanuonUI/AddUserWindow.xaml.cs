using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// AddUserWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AddUserWindow : WindowX
    {
        private bool isUserOK = false;
        private bool isPswOK = false;
        private bool isPswAgainOK = false;
        private MySqlHandler msh;
        private DataUserInfoViewModel userInfoClass;
        //加载用户列表委托
        public delegate void LoadUserListHandler();
        //定义委托事件
        public event LoadUserListHandler LoadUserListEvent;
        /// <summary>
        /// 添加用户构造体函数
        /// </summary>
        /// <param name="duvm"></param>
        public AddUserWindow(DataUserInfoViewModel duvm)
        {
            InitializeComponent();
            this.userInfoClass = duvm;
        }
        /// <summary>
        /// 添加用户窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WindowX_Loaded(object sender, RoutedEventArgs e)
        {
            msh = new MySqlHandler();
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
        /// 用户名输入框失去焦点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                isUserOK = true;
                if (this.userTextBox.Text == "" || this.userTextBox.Text == null)
                {
                    Alert(" 用户名不能为空。");
                    isUserOK = false;
                    usererroIcon.Visibility = Visibility.Visible;
                    userIcon.Visibility = Visibility.Hidden;
                    userTextBox.BorderBrush = new SolidColorBrush(Colors.Red);
                }
                if (IsSpecialCharacters(userTextBox.Text))
                {
                    Alert(" 用户名不能包含特殊字符。");
                    isUserOK = false;
                    usererroIcon.Visibility = Visibility.Visible;
                    userIcon.Visibility = Visibility.Hidden;
                    userTextBox.BorderBrush = new SolidColorBrush(Colors.Red);
                }
                if (this.userTextBox.Text.Length < 3 || this.userTextBox.Text.Length > 20)
                {
                    Alert(" 用户名长度最小3位最大20位。");
                    isUserOK = false;
                    usererroIcon.Visibility = Visibility.Visible;
                    userIcon.Visibility = Visibility.Hidden;
                    userTextBox.BorderBrush = new SolidColorBrush(Colors.Red);
                }
                if (msh.UserNameisUsed(userTextBox.Text))
                {
                    Alert(" 该用户名已经被使用过。");
                    isUserOK = false;
                    usererroIcon.Visibility = Visibility.Visible;
                    userIcon.Visibility = Visibility.Hidden;
                    userTextBox.BorderBrush = new SolidColorBrush(Colors.Red);
                }
                if (isUserOK)
                {
                    //isUserOK = true;
                    Information(" 输入用户名成功。");
                    usererroIcon.Visibility = Visibility.Hidden;
                    userIcon.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                Alert(ex.Message.ToString());
                isUserOK = false;
                usererroIcon.Visibility = Visibility.Visible;
                userIcon.Visibility = Visibility.Hidden;
                userTextBox.BorderBrush = new SolidColorBrush(Colors.Red);
            }

        }

        /// <summary>
        /// 判断是否含有特殊符号
        /// </summary>
        /// <param name="str">待检测的字符串</param>
        /// <returns></returns>
        public static bool IsSpecialCharacters(string str)
        {
            Regex regExp = new Regex("[^0-9a-zA-Z\u4e00-\u9fa5]");
            if (regExp.IsMatch(str))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 密码输入框失去焦点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PswBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (this.pswBox.Password == "" || this.pswBox.Password == null)
            {
                Alert(" 密码不能为空。");
                isPswOK = false;
                pswerroIcon.Visibility = Visibility.Visible;
                pswIcon.Visibility = Visibility.Hidden;
                pswBox.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            if (pswBox.Password.Length < 6)
            {
                Alert(" 密码长度不能小于6位。");
                isPswOK = false;
                pswerroIcon.Visibility = Visibility.Visible;
                pswIcon.Visibility = Visibility.Hidden;
                pswBox.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            if (this.pswBox.Password != "" &&
                this.pswBox.Password != null &&
               pswBox.Password.Length >= 6)
            {
                isPswOK = true;
                Information(" 输入密码成功。");
                pswerroIcon.Visibility = Visibility.Hidden;
                pswIcon.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// 密码确认输入框失去焦点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PswboxAgain_LostFocus(object sender, RoutedEventArgs e)
        {
            if (this.pswboxAgain.Password == "" || this.pswboxAgain.Password == null)
            {
                Alert(" 密码不能为空。");
                isPswAgainOK = false;
                pswaginerroIcon.Visibility = Visibility.Visible;
                pswaginIcon.Visibility = Visibility.Hidden;
                pswboxAgain.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            if (pswBox.Password != pswboxAgain.Password)
            {
                Alert(" 密码与之前输入的密码不同。");
                isPswAgainOK = false;
                pswaginerroIcon.Visibility = Visibility.Visible;
                pswaginIcon.Visibility = Visibility.Hidden;
                pswboxAgain.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            if (this.pswboxAgain.Password != "" &&
                this.pswboxAgain.Password != null &&
               pswBox.Password == pswboxAgain.Password)
            {
                isPswAgainOK = true;
                Information(" 输入密码成功。");
                pswaginerroIcon.Visibility = Visibility.Hidden;
                pswaginIcon.Visibility = Visibility.Visible;
            }
        }
        /// <summary>
        /// UI初始化
        /// </summary>
        private void Init()
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                loadLD.IsRunning = false;
                logonBtn.IsEnabled = true;
                logonBtn.Opacity = 1;
                userTextBox.Text = null;
                pswBox.Password = null;
                pswboxAgain.Password = null;
                isUserOK = false;
                isPswAgainOK = false;
                isPswOK = false;
                pswerroIcon.Visibility = Visibility.Hidden;
                pswIcon.Visibility = Visibility.Hidden;
                userIcon.Visibility = Visibility.Hidden;
                usererroIcon.Visibility = Visibility.Hidden;
                pswaginerroIcon.Visibility = Visibility.Hidden;
                pswaginIcon.Visibility = Visibility.Hidden;
            }));
       
        }
        /// <summary>
        /// 将用户信息保存到数据库
        /// </summary>
        private void WriteUserInfoToSql()
        {
            string pos = "测试员";
            PositionLevelEnum pe = PositionLevelEnum.Conner;
            try
            {
                if (this.userInfoClass.PresentUserInfo != null)
                {
                    switch (userInfoClass.PresentUserInfo.PositionLevel)
                    {
                        case PositionLevelEnum.Administrator:
                            pos = "工程师";
                            pe = PositionLevelEnum.Engineer;
                            break;
                        case PositionLevelEnum.Engineer:
                            pos = "测试员";
                            pe = PositionLevelEnum.Conner;
                            break;
                    }
                }
                this.Dispatcher.Invoke(new Action(() =>
                {
                    UserInfo userInfo = new UserInfo()
                    {
                        User = this.userTextBox.Text,
                        Psw = this.pswBox.Password,
                        Position = pos,
                        PositionLevel = pe,
                        Online = "离线",
                        OnlineState = OnlineEnum.Offline,
                        Own = this.userInfoClass.PresentUserInfo.User,
                    };

                    msh.InsertintoDB(userInfo);
                }));
            }
            catch (Exception ex)
            {
                Alert(ex.Message.ToString());
            }
            
        }
       /// <summary>
       /// 注册按钮单击事件
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
        private void LogonBtn_Click(object sender, RoutedEventArgs e)
        {
            Thread regUserThread = new Thread(() => {
                try
                {
                    string user = null;
                    if (isUserOK && isPswAgainOK && isPswOK)
                    {
                        this.Dispatcher.Invoke(new Action(() =>
                        {
                            loadLD.IsRunning = false;
                            this.logonBtn.IsEnabled = false;
                            this.logonBtn.Opacity = 0.5;
                            user = userTextBox.Text;
                        }));

                        //将用户数据录入数据库
                        WriteUserInfoToSql();
                        Information(" 添加用户" + user + "成功，" + "你可以选择再次添加用户或退出。");
                        Init();
                        this.Dispatcher.Invoke(new Action(() =>
                        {
                            LoadUserListEvent?.Invoke();
                        }));

                    }
                    else
                        Alert("用户名或密码错误。");
                }
                catch (Exception ex)
                {
                    Alert(ex.Message.ToString());
                }
                finally
                {
                    Init();
                }
            });
            regUserThread.Start();
        }
        /// <summary>
        /// 按钮获取焦点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LogonBtn_MouseMove(object sender, MouseEventArgs e)
        {
            logonBtn.Focus();
        }
        /// <summary>
        /// 用户输入框文本改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (userTextBox.Text != "" && userTextBox.Text != null)
            {
                userTextBox.BorderBrush = new SolidColorBrush(Colors.Green);
                Information("正在输入用户名...");
            }
        }
        /// <summary>
        /// 密码输入框密码改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PswBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (pswBox.Password != "" && pswBox.Password != null)
            {
                pswBox.BorderBrush = new SolidColorBrush(Colors.Green);
                Information("正在输入密码...");
            }
        }
        /// <summary>
        /// 密码再次输入框密码改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PswboxAgain_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (pswboxAgain.Password != "" && pswboxAgain.Password != null)
            {
                pswboxAgain.BorderBrush = new SolidColorBrush(Colors.Green);
                Information("正在再次输入密码...");
            }
        }

    }
}

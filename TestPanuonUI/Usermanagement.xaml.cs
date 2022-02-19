using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Usermanagement.xaml 的交互逻辑
    /// </summary>
    public partial class Usermanagement : WindowX
    {
        private List<UserInfo> UserList { get; set; }
        public DataUserInfoViewModel UserInfoClass;
        public ObservableCollection<UserInfo> obserUserList;

        private string waitDeleteUserName = null;
        /// <summary>
        /// UI交互逻辑
        /// </summary>
        /// <param name="duvm"></param>
        public Usermanagement(DataUserInfoViewModel duvm)
        {
            InitializeComponent();
            this.UserInfoClass = duvm;
        }

        /// <summary>
        /// 加载用户列表
        /// </summary>
        private void LoadUserList()
        {
            //UserList.Clear();
            MySqlHandler msh = new MySqlHandler();
            if (UserInfoClass.PresentUserInfo.PositionLevel == PositionLevelEnum.Administrator)
                UserList = msh.GetUserListByOwn();
            else
                UserList = msh.GetUserListByOwn(this.UserInfoClass.PresentUserInfo.User);
            obserUserList = new ObservableCollection<UserInfo>(UserList);
            this.Dispatcher.Invoke(new Action(() =>
            {
                this.UserInfoList.ItemsSource = obserUserList;
            }));
           
        }
        /// <summary>
        /// 用户权限管理（登录用户是否具有添加用户权限）
        /// </summary>
        private void PowerManagement()
        {
            switch (UserInfoClass.PresentUserInfo.PositionLevel)
            {
                case PositionLevelEnum.Administrator:
                    this.addUserLable.IsEnabled = true;
                    break;
                case PositionLevelEnum.Engineer:
                    this.addUserLable.IsEnabled = true;
                    break;
                case PositionLevelEnum.Conner:
                    this.addUserLable.IsEnabled = false;
                    break;
                default:
                    this.addUserLable.IsEnabled = false;
                    break;
            }
        }
        /// <summary>
        /// 添加用户单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Label_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
             openAdduserWinLoad.IsRunning = true;
             AddUserWindow aw = new AddUserWindow(UserInfoClass);
             aw.LoadUserListEvent += new AddUserWindow.LoadUserListHandler(LoadUserList);
             aw.ShowDialog();
        }
        /// <summary>
        /// 用户管理窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WindowX_Loaded(object sender, RoutedEventArgs e)
        {
            this.userInfoCanvas.DataContext = this.UserInfoClass.PresentUserInfo;
            LoadUserList();
            PowerManagement();
        }
        /// <summary>
        /// 用户管理窗口激活事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WindowX_Activated(object sender, EventArgs e)
        {
            this.openAdduserWinLoad.IsRunning = false;
        }
        /// <summary>
        /// 用户列表左键单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserInfoList_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (UserInfoList.SelectedIndex > -1)
            {
               waitDeleteUserName = obserUserList[this.UserInfoList.SelectedIndex].User;
            }    
        }
        /// <summary>
        /// 移除用户左键单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Thread deleteUserThread = new Thread(() =>
            {
                try
                {
                    if (waitDeleteUserName != "" && waitDeleteUserName != null)
                    {
                        this.Dispatcher.Invoke(new Action(() =>
                        {
                            this.openAdduserWinLoad.IsRunning = true;
                            MySqlHandler msh = new MySqlHandler();
                            if (msh.DeleteUser(waitDeleteUserName))
                            {
                                LoadUserList();
                            }
                        }));
                    }
                }
                catch (Exception ex)
                {
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        MessageBoxX.Show(ex.Message.ToString(), "错误", null, MessageBoxButton.OK, configKey: "ErrorTheme");
                    }));
                }
                 finally
                {
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        this.openAdduserWinLoad.IsRunning = false;
                    }));  
                }
            });
            deleteUserThread.Start();
        }
    }
}

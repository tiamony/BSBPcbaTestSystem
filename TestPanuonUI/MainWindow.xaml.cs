using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Panuon.UI.Silver;

namespace BsbPcbaTestSpace
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : WindowX
    {
        public MainWindow(UserInfo userInfo)
        {
            InitializeComponent();
            //数据初始化
            DataInit();
            //登陆用户更新本地在线状态信息
            userInfo.Online = "在线";
            userInfo.OnlineState = OnlineEnum.Online;
            this.duvmEx.PresentUserInfo = userInfo;
            messageBoxXInit();
        }
        public MainWindow()
        {
            InitializeComponent();
            //数据初始化
            DataInit();
            UserInfo userInfo = new UserInfo()
            {
                User = "Unknown",
                Psw = "Unknown",
                Position = "测试员",
                PositionLevel = PositionLevelEnum.Conner,
                Online = "离线",
                OnlineState = OnlineEnum.Offline,
                Own = "Unknown",
            };
            this.duvmEx.PresentUserInfo = userInfo;
            messageBoxXInit();
        }

        /// <summary>
        /// MessageBoxX 风格初始化
        /// </summary>
        private void messageBoxXInit()
        {
            MessageBoxX.MessageBoxXConfigurations.Add("InfoTheme", new Panuon.UI.Silver.Core.MessageBoxXConfigurations()
            {
                MessageBoxIcon = MessageBoxIcon.Info,
                MessageBoxStyle = MessageBoxStyle.Classic,
            });
            MessageBoxX.MessageBoxXConfigurations.Add("ErrorTheme", new Panuon.UI.Silver.Core.MessageBoxXConfigurations()
            {
                MessageBoxIcon = MessageBoxIcon.Error,
                MessageBoxStyle = MessageBoxStyle.Classic,
            });
        }
    }

}

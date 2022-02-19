using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Panuon.UI.Silver;

namespace BsbPcbaTestSpace
{
    public partial class MainWindow : WindowX
    {
        #region 连接状态
        /// <summary>
        /// 推送消息
        /// </summary>
        /// <param name="msg">消息内容</param>
        /// <param name="mt">消息类型</param>
        /// <returns></returns>
        private bool ConnStatuePush(string msg,MessageType mt)
        {
            switch (mt)
            {
                case MessageType.MSG_TYPE_ERROR:
                    this.statusText.Text = msg;
                    this.statusText.Foreground = new SolidColorBrush(Colors.Red);
                    break;
                case MessageType.MSG_TYPE_NORMAL:
                    this.statusText.Text = msg;
                    this.statusText.Foreground = new SolidColorBrush(Colors.Black);
                    break;
                case MessageType.MSG_TYPE_WARNING:
                    this.statusText.Text = msg;
                    this.statusText.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0x21, 0x2A));
                    break;
                case MessageType.MSG_TYPE_OPEN:
                    this.statusText.Text = msg;
                    this.statusText.Foreground = new SolidColorBrush(Colors.Green);
                    break;
            }
            return true;
        }
        #endregion
        #region 底部状态栏
        /// <summary>
        /// 更新时间信息
        /// </summary>
        private void UpdateTimeDate()
        {
            string timeDateString = "";
            DateTime now = DateTime.Now;
            timeDateString = string.Format("{0}年{1}月{2}日 {3}:{4}:{5}",
                now.Year,
                now.Month.ToString("00"),
                now.Day.ToString("00"),
                now.Hour.ToString("00"),
                now.Minute.ToString("00"),
                now.Second.ToString("00"));

            timeDateTextBlock.Text = timeDateString;
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
                if (comState == SerialState.SERIAL_STATE_OPEN)
                {
                    // #FFCA5100
                    statusBar.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xCA, 0x51, 0x00));
                }
                else
                {
                    // #FF007ACC
                    statusBar.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0x7A, 0xCC));
                }
                statusInfoTextBlock.Text = message;
            }));
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
    /// Abort.xaml 的交互逻辑
    /// </summary>
    public partial class Abort : WindowX
    {
        /// <summary>
        /// 关于窗口构造体函数
        /// </summary>
        public Abort()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 关闭串口按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        //文件路径
        private String filePath = Directory.GetCurrentDirectory() + @"\programInfo.txt";
        /// <summary>
        /// 从文件中读出文本内容放入RICHBOX
        /// </summary>
        /// <param name="richTextBox"></param>
        /// <param name="filepath"></param>
        private void LoadText(RichTextBox richTextBox,string filepath)
        {
            //string textFile = @"\Win\WPFDemo\WPFDemo\Resource\1.txt";
            FileStream fs;
            if (File.Exists(filepath))
            {

                fs = new FileStream(filepath, FileMode.Open, FileAccess.Read);

                using (fs)
                {
                    TextRange text = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);

                    text.Load(fs, DataFormats.Text);

                }
            }
        }
        /// <summary>
        /// 窗口加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WindowX_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadText(this.programInfoRichTextBox, filePath);
            }
            catch (Exception ex)
            {
                //MessageBoxX.Show(ex.Message.ToString(), "错误", null, MessageBoxButton.OK, configKey: "ErrorTheme");
                this.programInfoRichTextBox.AppendText(ex.Message.ToString());
            }
        }
    }
}

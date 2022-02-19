using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Panuon.UI.Silver;
using CmdClass;
using System.Windows.Input;
using Microsoft.Win32;
using XmlDeserializeSpace;
using System.Collections.ObjectModel;

namespace BsbPcbaTestSpace
{
    public partial class MainWindow : WindowX
    {
        public ICommand SetParmMeun { get; set; }
        public ICommand ExitMeun { get; set; }
        public ICommand AbortMeun { get; set; }
        public ICommand NewFile { get; set; }
        public ICommand OpenFile { get; set; }
        public ICommand ConnDb { get; set; }
        public ICommand ReLoadParmMeun { get; set; }
        public ICommand SetParmNewMeun { get; set; }
        public ICommand QuerySqlMeun { get; set; }
        /// <summary>
        /// 菜单命令绑定初始化
        /// </summary>
        private void IcommdMenuInit()
        {
            SetParmMeun = new RelayCommand(SetParmTask);
            ExitMeun = new RelayCommand(CLK_ExitTask);
            NewFile = new RelayCommand(CLK_NewFile);
            AbortMeun = new RelayCommand(CLK_Abort);
            OpenFile = new RelayCommand(CLK_OpenFile);
            ConnDb = new RelayCommand(CLK_ConnDb);
            ReLoadParmMeun = new RelayCommand(CLK_ReLoadParm);
            SetParmNewMeun = new RelayCommand(CLK_SetParmNew);
            QuerySqlMeun = new RelayCommand(CLK_QuerySql);
        }
        /// <summary>
        /// 打开查询数据窗口
        /// </summary>
        /// <param name="obj"></param>
        private void CLK_QuerySql(Object obj)
        {
            QueryDataWin qdw = new QueryDataWin(this.duvmEx.PresentUserInfo,opdmEx);
            qdw.ShowDialog();
        }
        /// <summary>
        /// 设置窗口菜单命令
        /// </summary>
        /// <param name="obj"></param>
        private void CLK_SetParmNew(Object obj)
        {
            OptionsWin ow = new OptionsWin(opdmEx,OptionsDataPath);
            ow.LoadConfigEvent += new OptionsWin.LoadConfigHandler(LoadConfig);
            ow.ShowDialog();
        }
        /// <summary>
        /// 重新加载配置文件命令
        /// </summary>
        /// <param name="obj"></param>
        private void CLK_ReLoadParm(object obj)
        {
            LoadConfig();
            Information("重新加载配置文件成功。");
        }
        /// <summary>
        /// 数据库连接菜单命令
        /// </summary>
        /// <param name="obj"></param>
        private void CLK_ConnDb(object obj)
        {
            //MSH = new MySqlHandler();
            //MSH.InsertintoDB();
            //opdmEx.GetProductName(opdmEx.StrFilePath);
        }
        /// <summary>
        /// 打开文件菜单命令
        /// </summary>
        /// <param name="obj"></param>
        private void CLK_OpenFile(object obj)
        {
            tfState = TestFileState.TF_STATE_OPENFILE;
        }
        /// <summary>
        /// 新建窗口菜单命令
        /// </summary>
        /// <param name="obj"></param>
        private void CLK_NewFile(object obj)
        {
            CreatNewFile cnf = new CreatNewFile();
            cnf.ShowDialog();
        }
        /// <summary>
        /// 设置菜单命令
        /// </summary>
        /// <param name="obj"></param>
        private void SetParmTask(object obj)
        {
            Options opw = new Options(opdmEx);
            opw.LoadConfigEvent += new Options.LoadConfigHandler(LoadConfig);
            opw.ShowDialog();
        }
        /// <summary>
        /// 退出菜单命令
        /// </summary>
        /// <param name="obj"></param>
        private void CLK_ExitTask(object obj)
        {
            try
            {
                if (this.duvmEx.PresentUserInfo.User != "Unknown")
                {
                    MySqlHandler msh = new MySqlHandler();
                    msh.UpdateMysql(this.duvmEx.PresentUserInfo.User, "离线", OnlineEnum.Offline);
                }
                if (this.opdmEx.StrFilePath != "" && this.opdmEx.StrFilePath != null
                   && this.strFilePathed != this.opdmEx.StrFilePath)
                    XmlSerializa.CreateXML(opdmEx, OptionsDataPath);
                Environment.Exit(0);
                this.Close();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message.ToString(), "错误窗口", MessageBoxButton.OK, MessageBoxImage.Error);
                Alert(ex.Message.ToString());
            }
            finally
            {
                Environment.Exit(0);
                this.Close();
            }
        }
        /// <summary>
        /// 打开关于窗口菜单命令
        /// </summary>
        /// <param name="obj"></param>
        private void CLK_Abort(object obj)
        {
            Abort abw = new Abort();
            abw.ShowDialog();
        }

    }
}

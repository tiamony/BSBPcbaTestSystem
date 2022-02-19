using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Panuon.UI.Silver;
using XmlDeserializeSpace;

namespace BsbPcbaTestSpace
{
    public partial class MainWindow : WindowX
    {
        //测试数据
        private DataViewModel dvmEx;
        //配置数据
        private OptionsDataModel opdmEx;
        //显示数据
        private DataViewModel DVM;
        //程序运行状态
        private TSRunState tsState;
        //串口连接状态
        private SerialState comState;
        //UI更新状态
        private UIState uiState;
        //配置文件路径
        private string OptionsDataPath = Directory.GetCurrentDirectory() + @"\config.xml";
        //线程终止控制
        private bool isStopThread = false;
        //测试文件加载状态
        private TestFileState tfState;
        //登录用户信息
        private DataUserInfoViewModel duvmEx;
        //打开时测试文件路径
        private string strFilePathed = null;
        /// <summary>
        /// 数据初始化
        /// </summary>
        private void DataInit()
        {
            dvmEx = new DataViewModel();
            opdmEx = new OptionsDataModel();
            DVM = new DataViewModel();
            duvmEx = new DataUserInfoViewModel();
            comState = SerialState.SERIAL_STATE_CLOSE;
            tsState = TSRunState.RUN_STATE_INIT;
            uiState = UIState.UI_STATE_INI;
            tfState = TestFileState.TF_STATE_NOFILE;
            LoadConfig();
            strFilePathed = this.opdmEx.StrFilePath;
        }
        /// <summary>
        /// 为配置变量赋值
        /// </summary>
        private void LoadConfig()
        {
            this.OptionInit();
            SlaveId = this.opdmEx.MinNumOfSalve;
            timeout = this.opdmEx.MaxNumOfBoard;
            ComNum = this.opdmEx.ComPort;
            PcbaTestMethodInit();
        }
        
        /// <summary>
        /// 加载配置文件
        /// </summary>
        private void OptionInit()
        {
            try
            {
                if (File.Exists(OptionsDataPath))
                {
                    opdmEx = (XmlSerializa.Deserialize(typeof(OptionsDataModel), OptionsDataPath) as OptionsDataModel);
                }
                else XmlSerializa.CreateXML(opdmEx, OptionsDataPath);
            }
            catch (Exception e)
            {
                Alert(e.Message.ToString());
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BsbPcbaTestSpace
{
    public class TestDataModel
    {
        public uint id { get; set; }
        public string testName { get; set; }
        public string testBody { get; set; }
        public float testTopLimit { get; set; }
        public float testFloor { get; set; }
        public string testResult { get; set; }
        public string remarks { get; set; }
        public ushort cmd { get; set; }
        public bool  isCheckedBoxTrue { get; set; }
        public string pointtitle { get; set; }
        public float resultData { get; set; }
        public string barcode { get; set; }
        public DateTime testdate { get; set; }
        public uint testfixtureid { get; set; }
        public string localname { get; set; }
        public string testProductName { get; set; }
        public string conner { get; set; }
    }
    public enum SlaveState
    {
        SLAVE_STATE_READY      = 0XFF,
        SLAVE_STATE_GETCMD     = 0XFE,
        SLAVE_STATE_TESTING    = 0XFD,
        SLAVE_STATE_FINSH      = 0XFC,
        SLAVE_STATE_INIT       =0XFB,
        SLAVE_STATE_UPDATE     =0XFA,
        SLAVE_STATE_TIMEOUT    =0XF9
    }
    public enum ResultState
    {
        RESULT_PASS_FLAG   = 0x59,
        RESULT_FAIL_FLAG   = 0x55,
        RESULT_WAIT_FLAG   = 0x30,
        RESULT_DATA_FLAG   = 0X20
    }
    public enum StateRegState
    {
        RESULT_RED_LED_STATE        =  0x01,
        RESULT_GREEN_LED_STATE      =  0x02
    }
    public enum SerialState
    {
        SERIAL_STATE_OPEN,
        SERIAL_STATE_CLOSE
    }
    public enum MessageType
    {
        MSG_TYPE_NORMAL,
        MSG_TYPE_WARNING,
        MSG_TYPE_ERROR,
        MSG_TYPE_OPEN
    }
    public enum TSRunState
    {
        RUN_STATE_START,
        RUN_STATE_RUNNING,
        RUN_STATE_STOP,
        RUN_STATE_ABORT,
        RUN_STATE_NOFILE,
        RUN_STATE_READY,
        RUN_STATE_LOADFILE,
        RUN_STATE_NOREADY,
        RUN_STATE_NOLOAD,
        RUN_STATE_INIT
    }
    public enum UIState
    {
        UI_STATE_LOADED,
        UI_STATE_CLEANED,
        UI_STATE_UPDATAED,
        UI_STATE_STOPUP,
        UI_STATE_INI,
        UI_STATE_READY
    }
    public enum TestFileState
    {
        TF_STATE_NOFILE,
        TF_STATE_OPENFILE,
        TF_STATE_LOADING,
        TF_STATE_LOADED
    }
}

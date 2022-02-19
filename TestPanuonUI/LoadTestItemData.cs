using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TestPanuonUI
{
     public class LoadTestItemData
    {
        private  List<string> testItemName = new List<string>()
        { 
            "LED指示灯测试",
            "LED指示灯测试",
            "温度测试",
            "急停检测",
            "CP检测",
            "CP检测",
            "PE接地检测",
            "漏电流检测",
            "4G通信模块检测",
            "模式电阻识别",
            "枪头温度检测",
            "输出电压检测"
        };
        private List<string> testItemBody = new List<string>()
        { 
            "3个红灯检测",
            "13个绿灯检测",
            "内部温度NTC测试",
            "急停按钮输入检测",
            "CP检测，接2.73K电阻",
            "CP检测，接0.88K电阻",
            "PE接地检测",
            "漏电流检测",
            "4G通信模块检测",
            "模式电阻识别",
            "枪头温度检测",
            "输出电压检测"
        };
        private List<byte> testCmd = new List<byte>() { 0x01, 0x02, 0x04, 0x03, 0x05, 0x06, 0x07, 0x08 ,0x09,0x0A,0x0B,0x0C};
        private List<uint> testItemTopLimit = new List<uint>()
        {  0,0,0,0,0,0,0,0,0,0,0 ,0};

        private List<uint> testItemFloor = new List<uint>()
        {  0,0,0,0,0,0,0,0,0,0,0,0 };

        public DataViewModel dvm = new DataViewModel();
        public LoadTestItemData()
        {
            try
            {
                dvm.TestDataModelView.Clear();
                dvm.ResultViewModel = "待测试";
                if (testItemName.Count>0)

                for (int id = 0; id < testItemName.Count; id++)
                {
                    dvm.TestDataModelView.Add(new TestDataModel()
                    {
                        id = (uint)id + 1,
                        testName = testItemName[id],
                        testBody = testItemBody[id],
                        testTopLimit = testItemTopLimit[id],
                        testFloor = testItemFloor[id],
                        isCheckedBoxTrue = true,
                        cmd = testCmd[id]
                    }
                                                                    );
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message.ToString(), "错误窗口", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

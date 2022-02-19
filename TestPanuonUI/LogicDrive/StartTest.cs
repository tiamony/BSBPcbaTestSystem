using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO.Ports;
using Panuon.UI.Silver;

namespace BsbPcbaTestSpace
{
    public partial class MainWindow : WindowX
    {
        private           int                   selectedID = -1;
        private           double                testPrecent = 0;
        /// <summary>
        /// 单项测试
        /// </summary>
        public void StartTestOne()
        {
            try
            {
                TestCmd((uint)selectedID, new ushort[] { this.dvmEx.TestDataModelView[selectedID].cmd, 0x00 });
            }
            catch (Exception e)
            {
                Alert(e.Message.ToString());
            }
        }
        /// <summary>
        /// 测试所有项目
        /// </summary>
        public void StartTestAll()
        {
            errTimes = 0;
            passTimes = 0;
            try
            {
                int count = this.dvmEx.TestDataModelView.Count;
                foreach (TestDataModel i in dvmEx.TestDataModelView)
                {
                    TestCmd((uint)i.id - 1, new ushort[] { i.cmd, 0x00 });
                    testPrecent += 100 / count;
                }
                testPrecent = 100;                
                if (errTimes == 0) this.dvmEx.ResultViewModel = "通过";
                else this.dvmEx.ResultViewModel = "不通过";
            }
            catch (Exception e)
            {
                Alert(e.Message.ToString());
                this.dvmEx.ResultViewModel = "不通过";
            }
        }
        /// <summary>
        /// 测试所有项目，当遇到不通过的项目时停止
        /// </summary>
        public void StartTestAll_stopToFail()
        {
            errTimes = 0;
            passTimes = 0;
            try
            {
                int count = this.dvmEx.TestDataModelView.Count;
                foreach (TestDataModel i in dvmEx.TestDataModelView)
                {
                     TestCmd(i.id - 1, new ushort[] { i.cmd, 0x00 });
                     testPrecent += 100 / count;
                     if (errTimes > 0) break;
                }
                testPrecent = 100;
                if (errTimes == 0) this.dvmEx.ResultViewModel = "通过";
                else this.dvmEx.ResultViewModel = "不通过";
            }
            catch (Exception e)
            {
                Alert(e.Message.ToString());
                this.dvmEx.ResultViewModel = "不通过";
            }
        }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Panuon.UI.Silver;
using System.Windows.Input;

namespace BsbPcbaTestSpace
{
    public partial class MainWindow : WindowX
    {
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (scEx.openSerialPort())
                {
                    comState = SerialState.SERIAL_STATE_OPEN;
                    Information("端口" + this.opdmEx.ComPort + "已打开。");
                }
            }
            catch (Exception ex)
            {
                Alert(ex.Message.ToString());
            }

        }

        private void CloseCom_Click(object sender, RoutedEventArgs e)
        {
            if (tsState != TSRunState.RUN_STATE_RUNNING)
            {
                while (true)
                {
                    try
                    {
                        if (scEx.closeSerialPort())
                        {
                            comState = SerialState.SERIAL_STATE_CLOSE;
                            Information("端口" + this.opdmEx.ComPort + "已关闭。");
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Alert(ex.Message.ToString());
                    }
                }
            }
            else
            {
                Alert( "测试正在进行中,请测试完成后再执行操作。");
            }
        }
        private void Add_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Add_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (comState == SerialState.SERIAL_STATE_OPEN)
            {
                if (this.dvmEx.TestDataModelView.Count > 0 && tsState == TSRunState.RUN_STATE_READY)
                {
                    this.testPrecent = 0;  
                    tsState = TSRunState.RUN_STATE_START;
                }
                else
                {
                    Information("请先打开测试文件。");
                }
            }
            else Alert("请先进行连接。");
        }

        private void Sub_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Sub_Executed(object sender, ExecutedRoutedEventArgs e)
        {
           if (tsState == TSRunState.RUN_STATE_RUNNING)
              tsState = TSRunState.RUN_STATE_ABORT;
        }
    }
}

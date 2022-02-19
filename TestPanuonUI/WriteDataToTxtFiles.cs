using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;

namespace TestPanuonUI
{
    public class WriteDataToTxtFiles
    {
        public WriteDataToTxtFiles(string dir,DataViewModel dvm)
        {

            try
            {
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                string path = dir + DateTime.Now.ToString("yyyy-MM-dd-HH-mm") + ".txt";
                FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs);
                foreach (DataModel d in dvm.DataModelView)
                {
                    sw.WriteLine(d.LineName+"   "+DateTime.Now.ToString("yyyy/MM/dd HH:mm"));
                    sw.WriteLine("产品卡号：" + d.ProductID + "   " + "生产数量：" + d.ProductNum + "   " + "总标准工时：" + d.StandardHoursTotal+"   "
                        + "总实际工时：" + d.ActualHoursTotal);
                    sw.WriteLine("超时：" + d.TimeOver1 + "   " + "计时：" + d.CountTime1 + "   " + "配置人数：" + d.PlacementNum1 + "   " + "工时设置："
                        + d.TimeSeting1);
                    sw.WriteLine("超时：" + d.TimeOver2 + "   " + "计时：" + d.CountTime2 + "   " + "配置人数：" + d.PlacementNum2+"   " + "工时设置："
                        + d.TimeSeting2);
                    sw.WriteLine("超时：" + d.TimeOver3 + "   " + "计时：" + d.CountTime3 + "   " + "配置人数：" + d.PlacementNum3+"   " + "工时设置："
                        + d.TimeSeting3);
                    sw.WriteLine("超时：" + d.TimeOver4 + "   " + "计时：" + d.CountTime4 + "   " + "配置人数：" + d.PlacementNum4+"   " + "工时设置："
                        + d.TimeSeting4 +"\r\n");
                }
                sw.Close();
                //fs.Close();

                
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message.ToString(), "错误窗口", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

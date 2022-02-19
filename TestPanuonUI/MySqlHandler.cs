using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows;
using System.Data;
using Panuon.UI.Silver;

namespace TestPanuonUI
{
    public class MySqlHandler
    {
       String connetStr = "server=rm-wz9yx2rl44fe32c34zo.mysql.rds.aliyuncs.com;port=3306;user=tiamo_ny_test_1;" +
            "password=Nieyan594210; database=bsb_pcba_test_ystem;";
        // server=127.0.0.1/localhost 代表本机，端口号port默认是3306可以不写
        public MySqlHandler()
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

            MySqlConnection conn = new MySqlConnection(connetStr);
            try
            {
                conn.Open();//打开通道，建立连接，可能出现异常,使用try catch语句
                MessageBoxX.Show("已与数据库建立连接", "提示",null, MessageBoxButton.OK,configKey:"InfoTheme");
                //在这里使用代码对数据库进行增删查改
            }
            catch (MySqlException ex)
            {
                MessageBoxX.Show(ex.Message.ToString(), "错误", null, MessageBoxButton.OK, configKey: "ErrorTheme");
            }
            finally
            {
                conn.Close();
            }
            
        }

        public void InsertintoDB()
        {
            MySqlConnection conn = new MySqlConnection(connetStr);
            conn.Open();//必须打开通道之后才能开始事务
            MySqlTransaction transaction = conn.BeginTransaction();
            try
            {
               // foreach (TestDataModel t in dv.TestDataModelView)
                //{
                    uint fxid = 1;
                    string lname = "shengzhen";
                    uint ists = 1;
                    string pname = "LX";
                    string tbody = "led test";
                    string tsname = "led test";
                    float limup = 11;
                    float limdown = 12;
                    float tvalue = 22;
                    string tresult = "pass";
                    string restr = "no error";
                    DateTime dt = DateTime.Now;
                    string bc = "123";
                    uint titemid = 1;
                    string sql1 = "insert into testinfo(" +
                        "testfixtureid,localname,isteset,testname,testproductname,testbody,limitup,limitdown,testvalue,testresult,remark,testdate,barcode,testitemid) " +
                        "values('" + fxid + "','" + lname + "','" + ists + "','" + tsname + "','" + pname + "','" + tbody + "','" + limup + "','" + limdown + "'," +
                        "'" + tvalue + "','" + tresult + "','" + restr + "','" + dt + "','" + bc + "','" + titemid + "')";
                    MySqlCommand cmd1 = new MySqlCommand(sql1, conn);
                    cmd1.ExecuteNonQuery();
              //  }


            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message.ToString(), "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                transaction.Rollback();//事务ExecuteNonQuery()执行失败报错，username被设置unique
                conn.Close();
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                {
                    transaction.Commit();//事务要么回滚要么提交，即Rollback()与Commit()只能执行一个
                    conn.Close();
                }
            }
        }
    }
}

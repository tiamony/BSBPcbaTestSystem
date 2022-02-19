using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows;
using System.Data;
using Panuon.UI.Silver;

namespace BsbPcbaTestSpace
{
    public class MySqlHandler
    {
        private String connetTestSystemDB = "server=rm-wz9yx2rl44fe32c34zo.mysql.rds.aliyuncs.com;port=3306;user=tiamo_ny_test_1;" +
            "password=Nieyan594210; database=bsb_pcba_test_ystem;";
        // server=127.0.0.1/localhost 代表本机，端口号port默认是3306可以不写
        /// <summary>
        /// 打开数据库
        /// </summary>
        public MySqlHandler()
        {
            /*
            conn = new MySqlConnection(connetTestSystemDB);
            messageBoxXInit();
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
               // conn.Close();
            }
            */
        }
        /// <summary>
        /// 根据id列删除该列所有的数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteSqlDataBySelect(Int32 id)
        {
            MySqlConnection conn = new MySqlConnection(connetTestSystemDB);
            try
            {
                conn.Open();
                string sql = "delete from testinfo where id='" + id + "'";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                int result = cmd.ExecuteNonQuery();//返回值是数据库中受影响的数据行数
                if (result > -1)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                //MessageBoxX.Show(ex.Message.ToString(), "错误", null, MessageBoxButton.OK, configKey: "ErrorTheme");
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return false;
        }
        /// <summary>
        /// 根据查询条件返回测试数据
        /// </summary>
        /// <param name="sqlstr"></param>
        /// <returns>返回测试数据类型的列表</returns>
        public List<SqlTestData> GetSqlTestDatas(string sqlstr)
        {
            MySqlConnection conn = new MySqlConnection(connetTestSystemDB);
            List<SqlTestData> sqlTemp = new List<SqlTestData>();
            uint i= 0;
            conn.Open();
            try
            {
                string sql = "select * from testinfo where "+sqlstr+ " order by testdate";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    i++;
                    sqlTemp.Add(new SqlTestData()
                    {
                        isSelected = false,
                        id = i,
                        sqlID = reader.GetInt32("id"),
                        testfixtureid = reader.GetUInt16("testfixtureid"),
                        companyName = reader.GetString("localname"),
                        testProductName = reader.GetString("testproductname"),
                        conner = reader.GetString("conner"),
                        barcode = reader.GetString("barcode"),
                        itemID = reader.GetUInt16("testitemid"),
                        isCheckedBoxTrue = Convert.ToBoolean( reader.GetInt16("isteset")),
                        testName = reader.GetString("testname"),
                        testBody = reader.GetString("testbody"),
                        testTopLimit = reader.GetFloat("limitup"),
                        testFloor = reader.GetFloat("limitdown"),
                        testValue = reader.GetFloat("testvalue"),
                        testResult = reader.GetString("testresult"),
                        remarks = reader.GetString("remark"),
                        testdate = reader.GetString("testdate"),
                    });
                }
                return sqlTemp;
            }
            catch (Exception ex)
            {
                //MessageBoxX.Show(ex.Message.ToString(), "错误", null, MessageBoxButton.OK, configKey: "ErrorTheme");
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// 判断输入的用户名是否被占用
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool UserNameisUsed(string name)
        {
            MySqlConnection conn = new MySqlConnection(connetTestSystemDB);
            try
            {
                conn.Open();
                string sql = "select * from user where username ='" + name + "'";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    conn.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                //MessageBoxX.Show(ex.Message.ToString(), "错误", null, MessageBoxButton.OK, configKey: "ErrorTheme");
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return false;
        }
        /// <summary>
        /// 用户登录，并获取登录成功后用户信息
        /// </summary>
        /// <param name="name">用户名</param>
        /// <param name="psw">密码</param>
        /// <returns></returns>
        public UserInfo LogonUserInfo(string name)
        {
            MySqlConnection conn = new MySqlConnection(connetTestSystemDB);
            UserInfo userTemp = new UserInfo();
            conn.Open();
            try
            {
                string sql = "select * from user where username ='" + name + "'";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    userTemp.User = reader.GetString("username");
                    userTemp.Psw = reader.GetString("password");
                    userTemp.Position = reader.GetString("Position");
                    userTemp.Online = reader.GetString("online");
                    userTemp.PositionLevel = (PositionLevelEnum)reader.GetUInt16("positionLevel");
                    userTemp.OnlineState = (OnlineEnum)reader.GetUInt16("onlinestatus");
                    userTemp.Own = reader.GetString("own");
                    conn.Close();
                    return userTemp;
                }
            }
            catch (Exception ex)
            {
                MessageBoxX.Show(ex.Message.ToString(), "错误", null, MessageBoxButton.OK, configKey: "ErrorTheme");
            }
            finally
            {
                conn.Close();
            }
            return null;
        }
        /// <summary>
        /// 用户登录，并获取登录成功后用户信息
        /// </summary>
        /// <param name="name">用户名</param>
        /// <param name="psw">密码</param>
        /// <returns></returns>
        public UserInfo LogonUserInfo(string name, string psw)
        {
            MySqlConnection conn = new MySqlConnection(connetTestSystemDB);
            UserInfo userTemp = new UserInfo();
            conn.Open();
            try
            {
                string sql = "select * from user where username ='" + name + "' and password = '"+psw+"'";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    userTemp.User = reader.GetString("username");
                    userTemp.Psw = reader.GetString("password");
                    userTemp.Position = reader.GetString("Position"); 
                    userTemp.Online = reader.GetString("online");
                    userTemp.PositionLevel =(PositionLevelEnum) reader.GetUInt16("positionLevel");
                    userTemp.OnlineState = (OnlineEnum)reader.GetUInt16("onlinestatus");
                    userTemp.Own = reader.GetString("own");
                    conn.Close();
                    return userTemp;
                }
            }
            catch (Exception ex)
            {
                MessageBoxX.Show(ex.Message.ToString(), "错误", null, MessageBoxButton.OK, configKey: "ErrorTheme");
            }
            finally
            {
                conn.Close();
            }
            return null;
        }
        /// <summary>
        /// 获取所有用户所属的用户列表
        /// </summary>
        /// <param name="own">所属用户</param>
        /// <returns></returns>
        public List<UserInfo> GetUserListByOwn()
        {
            List<UserInfo> userInfoList = new List<UserInfo>();
            MySqlConnection conn = new MySqlConnection(connetTestSystemDB);
            UserInfo userTemp = new UserInfo();
            conn.Open();
            try
            {
                string sql = "select * from user";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    PositionLevelEnum pe = (PositionLevelEnum)reader.GetInt16("positionLevel");
                    userInfoList.Add(new UserInfo()
                    {
                        User = reader.GetString("username"),
                        Position = reader.GetString("position"),
                        Online = reader.GetString("online"),
                        PositionLevel = (PositionLevelEnum)reader.GetInt16("positionLevel"),
                        OnlineState = (OnlineEnum)reader.GetInt16("onlineStatus"),
                        Own = reader.GetString("own"),
                        ShowFaceImage = MainWindow.GetFaceImage(pe),
                    });
                }
                conn.Close();
                return userInfoList;
            }
            catch (Exception ex)
            {
                MessageBoxX.Show(ex.Message.ToString(), "错误", null, MessageBoxButton.OK, configKey: "ErrorTheme");
            }
            finally
            {
                conn.Close();
            }
            return null;
        }
        /// <summary>
        /// 获取登录用户所属的用户列表
        /// </summary>
        /// <param name="own">所属用户</param>
        /// <returns></returns>
        public List<UserInfo> GetUserListByOwn(string own)
        {
            List<UserInfo> userInfoList = new List<UserInfo>();
            MySqlConnection conn = new MySqlConnection(connetTestSystemDB);
            UserInfo userTemp = new UserInfo();
            conn.Open();
            try
            {
                string sql = "select * from user where own ='" + own + "'";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    PositionLevelEnum pe = (PositionLevelEnum)reader.GetInt16("positionLevel");
                    userInfoList.Add(new UserInfo()
                    {
                        User = reader.GetString("username"),
                        Position = reader.GetString("position"),
                        Online = reader.GetString("online"),
                        PositionLevel = (PositionLevelEnum)reader.GetInt16("positionLevel"),
                        OnlineState = (OnlineEnum)reader.GetInt16("onlineStatus"),
                        Own = reader.GetString("own"),
                        ShowFaceImage = MainWindow.GetFaceImage(pe),
                    });
                }
                conn.Close();
                return userInfoList;
            }
            catch (Exception ex)
            {
                MessageBoxX.Show(ex.Message.ToString(), "错误", null, MessageBoxButton.OK, configKey: "ErrorTheme");
            }
            finally
            {
                conn.Close();
            }
            return null;
        }
        /// <summary>
        /// 根据用户名删除用户
        /// </summary>
        /// <param name="username"></param>
        /// <returns>bool类型的值（TRUE-执行数据库成功，FALSE-执行数据库失败）</returns>
        public bool DeleteUser(string username)
        {
            MySqlConnection conn = new MySqlConnection(connetTestSystemDB);
            try
            {
                conn.Open();
                string sql = "delete from user where username='"+username+"'";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                int result = cmd.ExecuteNonQuery();//返回值是数据库中受影响的数据行数
                if (result > -1)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                //MessageBoxX.Show(ex.Message.ToString(), "错误", null, MessageBoxButton.OK, configKey: "ErrorTheme");
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return false; ;
        }
        /// <summary>
        /// 根据条件更新用户在线状态信息
        /// </summary>
        /// <param name="wherestr">查询条件</param>
        /// <param name="value">在线值</param>
        /// <param name="online">在线状态</param>
        /// <returns></returns>
        public bool UpdateMysql(string wherestr, string value,OnlineEnum online)
        {
            MySqlConnection conn = new MySqlConnection(connetTestSystemDB);
            conn.Open();//必须打开通道之后才能开始事务
            MySqlTransaction transaction = conn.BeginTransaction();
            try
            {
                string sql1 = "UPDATE `bsb_pcba_test_ystem`.`user` SET `" +
                    "online`='"+value+"',onlinestatus ='"+Convert.ToInt16(online)+"' WHERE `username`='"+wherestr+"';";
                MySqlCommand cmd1 = new MySqlCommand(sql1, conn);
                cmd1.ExecuteNonQuery();
                return true;
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
            return false;
        }
        /// <summary>
        /// 向数据库中插入用户数据
        /// </summary>
        public void InsertintoDB(UserInfo userinfo)
        {
            MySqlConnection conn = new MySqlConnection(connetTestSystemDB);
            conn.Open();//必须打开通道之后才能开始事务
            MySqlTransaction transaction = conn.BeginTransaction();
            try
            {
                string sql1 = "insert into user(username,password,position,online,positionlevel,onlinestatus,own)" +
                              "values('" + userinfo.User + "','" + userinfo.Psw + "','" + userinfo.Position + "'," +
                              "'" + userinfo.Online + "','" +Convert.ToInt16( userinfo.PositionLevel) + "'," +
                              "'" +Convert.ToInt16( userinfo.OnlineState) + "','"+userinfo.Own+"')";


                MySqlCommand cmd1 = new MySqlCommand(sql1, conn);
                cmd1.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                transaction.Rollback();//事务ExecuteNonQuery()执行失败报错，username被设置unique
                conn.Close();
                throw ex;
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

        /// <summary>
        /// 向数据库中插入测试数据
        /// </summary>
        public void InsertintoDB(List<TestDataModel> testData)
        {
            int i = 0;
            MySqlConnection conn = new MySqlConnection(connetTestSystemDB);
            conn.Open();//必须打开通道之后才能开始事务
            MySqlTransaction transaction = conn.BeginTransaction();
            try
            {
                foreach (TestDataModel t in testData)
                {
                    if (i < testData.Count)
                    {
                        string sql1 = "insert into testinfo(testfixtureid,localname,testproductname,conner,barcode,testitemid," +
                                      "isteset,testname,testbody,limitup,limitdown,testvalue,testresult,remark,testdate)" +
                                      "values('" + t.testfixtureid + "','" + t.localname + "','" + t.testProductName + "'," +
                                      "'" + t.conner + "','" + t.barcode + "','" + t.id + "','" + Convert.ToInt16(t.isCheckedBoxTrue) + "'," +
                                      "'" + t.testName + "','" + t.testBody + "','" + t.testTopLimit + "','" + t.testFloor + "'," +
                                      "'" + t.resultData + "','" + t.testResult + "','" + t.remarks + "','" + t.testdate + "')";


                        MySqlCommand cmd1 = new MySqlCommand(sql1, conn);
                        cmd1.ExecuteNonQuery();
                        i++;
                    }
                }
            }
            catch (MySqlException ex)
            {
                //MessageBox.Show(ex.Message.ToString(), "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                //transaction.Rollback();//事务ExecuteNonQuery()执行失败报错，username被设置unique
                throw ex;
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

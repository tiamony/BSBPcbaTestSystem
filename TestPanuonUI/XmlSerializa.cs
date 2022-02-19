using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using System.Xml;
using System.Windows;

namespace XmlDeserializeSpace
{
    public class XmlSerializa
    {
        /// <summary>  
        /// <remarks>Xml序列化与反序列化</remarks>  
        /// <creator>zhangdapeng</creator>  
        /// </summary>  
            #region 反序列化  
            /// <summary>  
            /// 反序列化  
            /// </summary>  
            /// <param name="type">类型</param>  
            /// <param name="xml">XML字符串</param>  
            /// <returns></returns>  
            public static object Deserialize(Type type, string xmlStr)
            {
            try
            {
                using (FileStream fs = new FileStream(xmlStr,FileMode.Open))
                {
                    XmlSerializer xs = new XmlSerializer(type); 
                    return xs.Deserialize(fs);
                }
            }
            catch (Exception ex)
            {
               //throw (ex);
                MessageBox.Show(ex.Message.ToString(), "错误窗口", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }


        public static bool CreateXML<T>(T obj, string filePath)
        {
            XmlWriter writer = null;    //声明一个xml编写器
            XmlWriterSettings writerSetting = new XmlWriterSettings //声明编写器设置
            {
                Indent = true,//定义xml格式，自动创建新的行
                Encoding = UTF8Encoding.UTF8,//编码格式
            };

            try
            {
                //创建一个保存数据到xml文档的流
                writer = XmlWriter.Create(filePath, writerSetting);
            }
            catch (Exception ex)
            {
                //throw ex;
                MessageBox.Show(ex.Message.ToString(), "错误窗口", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            XmlSerializer xser = new XmlSerializer(typeof(T));  //实例化序列化对象

            try
            {
                xser.Serialize(writer, obj);  //序列化对象到xml文档
            }
            catch (Exception ex)
            { 
                //throw ex;
                MessageBox.Show(ex.Message.ToString(), "错误窗口", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            finally
            {
                writer.Close();
            }
            return true;
        }
            #endregion
        }
    }


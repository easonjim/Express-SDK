using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Xml;
using System.Reflection;
/********************************************************************************
      ** 创建人：    Jim
      ** 创建时间:   2008
      ** 功能描述：  现在这个序列化帮助类 只封装了 二进制和XML方法 json的转换使用第三方
      **              可以使用
*********************************************************************************/
namespace ComLib
{
    /// <summary>
    /// 序列化帮助类 
    /// </summary>
   public class SerializeHelper
    {
        #region 序列化与反序列化二进制
        /// <summary>
        /// 序列化对象二进制
        /// </summary>
        /// <param name="obj">要序列化的对象</param>
        /// <returns>返回二进制</returns>
        public static byte[] SerializeModel(Object obj)
        {
            if (obj != null)
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                MemoryStream ms = new MemoryStream();
                byte[] b;
                binaryFormatter.Serialize(ms, obj);
                ms.Position = 0;
                b = new Byte[ms.Length];
                ms.Read(b, 0, b.Length);
                ms.Close();
                return b;
            }
            else
                return new byte[0];
        }

        /// <summary>
        /// 反序列化对象
        /// </summary>
        /// <param name="b">要反序列化的二进制</param>
        /// <returns>返回对象</returns>
        public static T DeserializeModel<T>(byte[] b)
        {
            if (b == null || b.Length == 0)
                return default(T);
            else
            {
                object result = new object();
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                MemoryStream ms = new MemoryStream();
                try
                {
                    ms.Write(b, 0, b.Length);
                    ms.Position = 0;
                    result = binaryFormatter.Deserialize(ms);
                    ms.Close();
                }
                catch { }
                return (T)result;
            }
        }
        #endregion

        #region Model与XML互相转换
        /// <summary>
        /// Model转化为XML的方法
        /// </summary>
        /// <param name="model">要转化的Model</param>
        /// <returns></returns>
        public static string ModelToXML(object model)
        {
            XmlDocument xmldoc = new XmlDocument();
            XmlElement ModelNode = xmldoc.CreateElement("Model");
            xmldoc.AppendChild(ModelNode);

            if (model != null)
            {
                foreach (PropertyInfo property in model.GetType().GetProperties())
                {
                    XmlElement attribute = xmldoc.CreateElement(property.Name);
                    if (property.GetValue(model, null) != null)
                        attribute.InnerText = property.GetValue(model, null).ToString();
                    else
                        attribute.InnerText = "[Null]";
                    ModelNode.AppendChild(attribute);
                }
            }

            return xmldoc.OuterXml;
        }

        /// <summary>
        /// XML转化为Model的方法
        /// </summary>
        /// <param name="xml">要转化的XML</param>
        /// <param name="SampleModel">Model的实体示例，New一个出来即可</param>
        /// <returns></returns>
        public static object XMLToModel(string xml, object SampleModel)
        {
            if (string.IsNullOrEmpty(xml))
                return SampleModel;
            else
            {
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.LoadXml(xml);

                XmlNodeList attributes = xmldoc.SelectSingleNode("Model").ChildNodes;
                foreach (XmlNode node in attributes)
                {
                    foreach (PropertyInfo property in SampleModel.GetType().GetProperties())
                    {
                        if (node.Name == property.Name)
                        {
                            if (node.InnerText != "[Null]")
                            {
                                if (property.PropertyType == typeof(System.Guid))
                                    property.SetValue(SampleModel, new Guid(node.InnerText), null);
                                else
                                    property.SetValue(SampleModel, Convert.ChangeType(node.InnerText, property.PropertyType), null);
                            }
                            else
                                property.SetValue(SampleModel, null, null);
                        }
                    }
                }
                return SampleModel;
            }
        }
        #endregion

        #region "对象base64序列化和放序列化"
         /// <summary>
        /// 获取对象的Base64字符串表示形式
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ToBase64String(object source)
        {
            return Convert.ToBase64String(ToByteArray(source));
        }

        /// <summary>
        /// 从Base64字符串获得一个对象
        /// </summary>
        /// <param name="base64String"></param>
        /// <returns></returns>
        public static object FromBase64String(string base64String)
        {
            if (string.IsNullOrEmpty(base64String)) return null;

            return FromByteArray(Convert.FromBase64String(base64String));
        }
        /// <summary>
        /// 从字节数组获得一个对象
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static object FromByteArray(byte[] bytes)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                //使用从缓冲区读取的数据将字节块写入当前流。
                stream.Write(bytes, 0, bytes.GetLength(0));

                BinaryFormatter formatter = new BinaryFormatter();
                stream.Position = 0;
                return formatter.Deserialize(stream);
            }
        }
        /// <summary>
        /// 获取对象的字节数组表示形式
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static byte[] ToByteArray(object source)
        {
            if (source == null)
                return null;
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, source);
                return stream.ToArray();
            }
        }
        #endregion
       
    }
}

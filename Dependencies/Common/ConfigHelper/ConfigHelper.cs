using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Configuration;
using System.Web;
using System.Xml;
namespace ComLib
{
    /// <summary>
    /// web.config操作类
    /// </summary>
    public  class ConfigHelper
    {

        #region 得到AppSettings中的配置字符串信息
        /// <summary>
        /// 得到AppSettings中的配置字符串信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetConfigString(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
        #endregion

        #region 得到AppSettings中的配置bool信息
        /// <summary>
        /// 得到AppSettings中的配置信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetConfigInfo<T>(string key)
        {
            T result = default(T);
            string cfgVal = GetConfigString(key);
            if (null != cfgVal && string.Empty != cfgVal)
            {
                try
                {
                    result = (T)Convert.ChangeType(cfgVal, typeof(T));
                }
                catch (FormatException)
                {
                    // Ignore format exceptions.
                }
            }

            return result;
        }
        #endregion


        #region "保存AppSettings中的节点配置"
        /// <summary>
        /// 保存config文件AppSettings节点设置
        /// </summary>
        /// <param name="xmlTargetElement">关键字</param>
        /// <param name="xmlText">value</param>
        /// <param name="configPath">config文件路径 传入相对路径 不传默认寻找根目录下Web.config</param>
        public static void SaveXmlElementValue(string xmlTargetElement, string xmlText,string configPath)
        {
            string returnInt = null;
            string filename = string.Empty;
            if(string.IsNullOrEmpty(configPath))
                filename = HttpContext.Current.Server.MapPath("~") + @"/Web.config";
            else
                filename = HttpContext.Current.Server.MapPath(configPath);
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(filename);
            XmlNodeList topM = xmldoc.DocumentElement.ChildNodes;
            foreach (XmlNode element in topM)
            {
                if (element.Name == "appSettings")
                {
                    XmlNodeList node = element.ChildNodes;
                    if (node.Count > 0)
                    {
                        foreach (XmlNode el in node)
                        {
                            if (el.Name == "add")
                            {
                                if (el.Attributes["key"].InnerXml == xmlTargetElement)
                                {
                                    //保存web.config数据
                                    el.Attributes["value"].Value = xmlText;
                                    xmldoc.Save(filename);
                                    return;
                                }
                            }
                        }
                    }
                    else
                    {
                        returnInt = "Web.Config配置文件未配置";
                    }
                    break;
                }
                else
                {
                    returnInt = "Web.Config配置文件未配置";
                }
            }

            if (returnInt != null)
                throw new Exception(returnInt);
        }
        #endregion

    }
}


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Collections.Specialized;
using ComLib.JavaScriptLib;
using ComLib.Exceptions;
/********************************************************************************
      ** 创建人：    Jim
      ** 创建时间:   2008/06/09
      ** 功能描述：  获取QueryString  如需过滤敏感SQL 请调用FilterSQL方法
      **              此类中包含一个 静态属性获取URL地址的方法
*********************************************************************************/
namespace ComLib
{
    /// <summary>
    /// 获取QueryString  如需过滤敏感SQL 请调用FilterSQL方法
    /// </summary>
    public class QueryStringHelper
    {
        #region "获取整型的参数"
        /// <summary>
        /// 获得指定表单参数的int类型值 
        /// </summary>
        /// <param name="queryName">表单参数</param>
        /// <returns></returns>
        public static int GetIntByQueryString(string queryName)
        {
            try
            {
                string sValue = HttpContext.Current.Request[queryName];
                if (RegexHelper.IsNumber(sValue))
                {
                    return int.Parse(sValue);
                }
                else
                {
                    return 0;
                }


            }
            catch
            {

            }
            return 0;
        }
        /// <summary>
        /// 获得指定表单参数的int类型值 
        /// </summary>
        /// <param name="queryName">表单参数</param>
        /// <param name="defaultVal">缺省值</param>
        /// <returns></returns>
        public static int GetIntByQueryString(string queryName, int defaultVal)
        {
            try
            {
                string sValue = HttpContext.Current.Request[queryName];
                if (RegexHelper.IsNumber(sValue))
                {
                    return int.Parse(sValue);
                }
                else
                {
                    return defaultVal;
                }


            }
            catch
            {

            }
            return defaultVal;
        }
        #endregion

        #region "获取string类型的参数"
        /// <summary>
        /// 获取string类型URL参数并且过滤SQL关键字
        /// </summary>
        /// <param name="queryName"></param>
        /// <returns></returns>
        public static string GetStringByQueryString(string queryName)
        {
            try
            {
                if (System.Web.HttpContext.Current.Request[queryName] != null)
                {
                    return FilterSQL(System.Web.HttpContext.Current.Request[queryName]);
                }
                else
                {
                    return "";
                }
            }
            catch
            {

            }
            return "";
        }

        #endregion

        #region 过滤SQL参数
        /// <summary>
        /// SQL字符过滤处理（防SQL注入攻击）
        /// </summary>
        /// <param name="text">字符</param>
        /// <returns></returns>
        public static string FilterSQL(string text)
        {
            string validSql = "";
            if (!string.IsNullOrEmpty(text))
            {
                text = text.Replace("\"", "&quot;");
                text = text.Replace(";", "");
                text = text.Replace("'", "''");
                text = text.Replace("--", "''--''");
                text = text.Replace("%25", "");
                text = text.Replace("%0a", "");
                text = text.Replace("%22", "");
                text = text.Replace("%27", "");
                text = text.Replace("%5c", "");
                text = text.Replace("%2f", "");
                text = text.Replace("%3c", "");
                text = text.Replace("%3e", "");
                text = text.Replace("%26", "");

                text = text.Replace("<", "&lt;");
                text = text.Replace(">", "&gt;");

                text = text.Replace("select ", "ＳＥＬＥＣＴ");
                text = text.Replace("insert ", "ＩＮＳＥＲＴ");
                text = text.Replace("delete ", "ＤＥＬＥＴＥ");
                text = text.Replace("count(", "ＣＯＵＮＴ");
                text = text.Replace("drop table ", "ＤＲＯＰ ＴＡＢＬＥ");
                text = text.Replace("drop  ", "ＤＲＯＰ ");
                text = text.Replace("create ", "ＣＲＥＡＴＥ");
                text = text.Replace("update ", "ＵＰＤＡＴＥ");
                text = text.Replace("truncate ", "ＴＲＵＮＣＡＴＥ");
                text = text.Replace("asc(", "ＡＳＣ");
                text = text.Replace("mid(", "ＭＩＤ");
                text = text.Replace("char(", "ＣＨＡＲ");
                text = text.Replace("xp_cmdshell", "ＸＰ_ＣＭＤＳＨＥＬＬ");
                text = text.Replace("xp_dirtree", "ＸＰ_dirtree");
                text = text.Replace("xp_regread", "ＸＰ_regread");
                text = text.Replace("alter ", "Ａlter");
                text = text.Replace("char(124)", "ＣＨＡＲ(124)");
                text = text.Replace("db_name()", "ＤＢ_name()");
                text = text.Replace("''", "");
                text = text.Replace("exec master", "ＥＸＥＣ　ＭＡＳＴＥＲ");
                text = text.Replace(" and ", "ＡＮＤ");
                text = text.Replace("net user", "ＮＥＴ　ＵＳＥＲ");
                text = text.Replace(" or ", "ＯＲ");
                validSql = text;
            }
            return validSql;
        }
        #endregion

        #region  静态属性获取URL地址
        /// <summary>
        /// 这个静态属性的调用必须用以下代码方法调用
        /// 代码调用:
        /// Response.Write(QueryStringHelper.BaseUrl);
        /// </summary>
        public static string BaseUrl
        {
            get
            {
                //strBaseUrl用于存储URL地址
                string strBaseUrl = "";
                //获取当前HttpContext下的地址
                strBaseUrl += "http://" + HttpContext.Current.Request.Url.Host;
                //如果端口不是80的话，那么加入特殊端?
                if (HttpContext.Current.Request.Url.Port.ToString() != "80")
                {
                    strBaseUrl += ":" + HttpContext.Current.Request.Url.Port;
                }
                strBaseUrl += HttpContext.Current.Request.ApplicationPath;

                return strBaseUrl + "/";
            }
        }
        #endregion

        #region "获取用户Form提交字段值"

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="strName">参数名</param>
        /// <param name="type">传递的话就要验证 不传递就不验证</param>
        /// <returns></returns>
        public static T GetFromString<T>(string strName, DataType type)
        {
            string obj = "";
            if ("".Equals(GetStringByQueryString(strName)))
            {
                obj = GetFormString(strName);
            }
            else
            {
                obj = GetStringByQueryString(strName);
            }
            if (obj != "" && type != null)
            {
                #region "判断类型"
                switch (type)
                {
                    case DataType.Int:
                        int IntTempValue = 0;
                        if (!int.TryParse(obj, out IntTempValue))
                            ErrorManager.Handle("输入数据格式验证失败" + string.Format("{0}字段值：{1}数据类型必需为Int型!", strName, obj), null);
                        return (T)Convert.ChangeType(IntTempValue, typeof(T));
                    case DataType.Dat:
                        DateTime DateTempValue = DateTime.MinValue;
                        if (!DateTime.TryParse(obj, out DateTempValue))
                            ErrorManager.Handle("输入数据格式验证失败" + string.Format("{0}字段值：{1}数据类型必需为时间型!", strName, obj), null);
                        return (T)Convert.ChangeType(DateTempValue, typeof(T));
                    case DataType.Long:
                        long LongTempValue = long.MinValue;
                        if (!long.TryParse(obj, out LongTempValue))
                            ErrorManager.Handle("输入数据格式验证失败:" + string.Format("{0}字段值：{1}数据类型必需为Long型!", strName, obj), null);
                        return (T)Convert.ChangeType(LongTempValue, typeof(T));
                    case DataType.Double:
                        double DoubleTempValue = double.MinValue;
                        if (!double.TryParse(obj, out DoubleTempValue))
                            ErrorManager.Handle("输入数据格式验证失败:" + string.Format("{0}字段值：{1}数据类型必需为Double型!", strName, obj), null);
                        return (T)Convert.ChangeType(DoubleTempValue, typeof(T));
                    case DataType.CharAndNum:
                        if (!CheckRegEx(obj, "^[A-Za-z0-9]+$"))
                            ErrorManager.Handle("输入数据格式验证失败:" + string.Format("{0}字段值：{1}数据类型必需为英文或数字!", strName, obj), null);
                        return (T)Convert.ChangeType(strName, typeof(T));
                    case DataType.CharAndNumAndChinese:
                        if (!CheckRegEx(obj, "^[A-Za-z0-9\u00A1-\u2999\u3001-\uFFFD]+$"))
                            ErrorManager.Handle("输入数据格式验证失败:" + string.Format("{0}字段值：{1}数据类型必需为英文或数字或中文!", strName, obj), null);
                        return (T)Convert.ChangeType(strName, typeof(T));
                    case DataType.Email:
                        if (!CheckRegEx(obj, "\\w+([-+.]\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*"))
                            ComLib.Exceptions.ErrorManager.Handle("输入数据格式验证失败", new Exception("googog"));
                        ErrorManager.Handle("输入数据格式验证失败:" + string.Format("{0}字段值：{1}数据类型必需为邮件地址!", strName, obj), null);
                        return (T)Convert.ChangeType(obj, typeof(T));
                    default:
                        return (T)Convert.ChangeType(obj, typeof(T)); ;
                }
                #endregion
                
            }

            if (obj == "")
            {
                return default(T);
            }
            return obj.ConvertTo<T>();
        }

        #region "正式表达式验证"
        /// <summary>
        /// 正式表达式验证
        /// </summary>
        /// <param name="C_Value">验证字符</param>
        /// <param name="C_Str">正式表达式</param>
        /// <returns>符合true不符合false</returns>
        public static bool CheckRegEx(string C_Value, string C_Str)
        {
            Regex objAlphaPatt;
            objAlphaPatt = new Regex(C_Str, RegexOptions.Compiled);


            return objAlphaPatt.Match(C_Value).Success;
        }
        #endregion


        /// <summary>
        /// 获得指定表单参数的值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <returns>表单参数的值</returns>
        public static string GetFormString(string strName)
        {
            if (HttpContext.Current.Request.Form[strName] == null)
            {
                return "";
            }
            return HttpContext.Current.Request.Form[strName];
        }

        #endregion

    }


    #region "通用枚举"
    /// <summary>
    /// 获取方式
    /// </summary>
    public enum MethodType
    {
        /// <summary>
        /// Post方式
        /// </summary>
        Post = 1,
        /// <summary>
        /// Get方式
        /// </summary>
        Get = 2
    }

    /// <summary>
    /// 获取数据类型
    /// </summary>
    public enum DataType
    {
        /// <summary>
        /// 字符
        /// </summary>
        Str = 1,
        /// <summary>
        /// 日期
        /// </summary>
        Dat = 2,
        /// <summary>
        /// 整型
        /// </summary>
        Int = 3,
        /// <summary>
        /// 长整型
        /// </summary>
        Long = 4,
        /// <summary>
        /// 双精度小数
        /// </summary>
        Double = 5,
        /// <summary>
        /// 只限字符和数字
        /// </summary>
        CharAndNum = 6,
        /// <summary>
        /// 只限邮件地址
        /// </summary>
        Email = 7,
        /// <summary>
        /// 只限字符和数字和中文
        /// </summary>
        CharAndNumAndChinese = 8

    }

    /// <summary>
    /// 表操作方法
    /// </summary>
    public enum DataTable_Action
    {
        /// <summary>
        /// 插入
        /// </summary>
        Insert = 0,
        /// <summary>
        /// 更新
        /// </summary>
        Update = 1,
        /// <summary>
        /// 删除
        /// </summary>
        Delete = 2
    }
    #endregion

    /// <summary>
    /// 获取HttpRequest 的泛型方法  
    /// </summary>
    [System.Obsolete("这个方法还不成熟 不要使用")]
    public static class QueryStringHelpers
    {
        /// <summary>
        /// 获取QueryString的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <param name="queryName"></param>
        /// <returns></returns>
        public static T GetQueryString<T>(this HttpRequest request, string queryName)
        {
            //if (request == null) {throw new Exception("HttpRequest Parameter is Null ");}
            //if (string.IsNullOrEmpty(queryName)) { throw new Exception("queryName is Null or Empty"); }
            try
            {
                var obj = request.QueryString[queryName];
                return (T)Convert.ChangeType(obj, typeof(T));
            }
            catch
            {

            }
            return default(T);
        }
    }
}

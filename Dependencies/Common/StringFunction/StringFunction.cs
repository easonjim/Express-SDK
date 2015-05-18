using System;
using System.Web;
using System.Text;
using System.Text.RegularExpressions;

namespace ComLib
{
    /// <summary>
    /// 常用函数基类
    /// </summary>
    public class StringHelper
    {
        #region 替换字符串

        /// <summary>
        /// 功能:替换字符
        /// </summary>
        /// <param name="strVAlue">字符串</param>
        /// <returns>替换掉'的字符串</returns>
        public static string FilterReplace(string strVAlue)
        {
            string str = "";
            str = strVAlue.Replace("'", "");
            return str;
        }

        #endregion

        #region 对表 表单内容进行转换HTML操作,

        /// <summary>
        /// 功能:对表 表单内容进行转换HTML操作,
        /// </summary>
        /// <param name="fString">html字符串</param>
        /// <returns></returns>
        public static string HtmlCode(string fString)
        {
            string str = "";
            str = fString.Replace(">", "&gt;");
            str = fString.Replace("<", "&lt;");
            str = fString.Replace(" ", "&nbsp;");
            str = fString.Replace("\n", "<br />");
            str = fString.Replace("\r", "<br />");
            str = fString.Replace("\r\n", "<br />");
            return str;
        }

        #endregion

        #region 判断是否:返回值：√ or ×

        /// <summary>
        /// 判断是否:返回值：√ or ×
        /// </summary>
        /// <param name="b">true 或false</param>
        /// <returns>√ or ×</returns>
        public static string Judgement(bool b)
        {
            string s = "";
            if (b == true)
                s = "<b><font color=#009900>√</font></b>";
            else
                s = "<b><font color=#FF0000>×</font></b>";
            return s;
        }

        #endregion

        #region 截取字符串

        /// <summary>
        /// 功能:截取字符串长度
        /// </summary>
        /// <param name="str">要截取的字符串</param>
        /// <param name="length">字符串长度</param>
        /// <param name="flg">true:加...,flase:不加</param>
        /// <returns></returns>
        public static string GetString(string str, int length, bool flg)
        {
            int i = 0, j = 0;
            foreach (char chr in str)
            {
                if ((int)chr > 127)
                {
                    i += 2;
                }
                else
                {
                    i++;
                }
                if (i > length)
                {
                    str = str.Substring(0, j);
                    if (flg)
                        str += "...";
                    break;
                }
                j++;
            }
            return str;
        }

        #endregion

        #region 截取字符串+…

        /// <summary>
        /// 截取字符串+…
        /// </summary>
        /// <param name="strInput"></param>
        /// <param name="intlen"></param>
        /// <returns></returns>
        public static string CutString(string strInput, int intlen) //截取字符串
        {
            ASCIIEncoding ascii = new ASCIIEncoding();
            int intLength = 0;
            string strString = "";
            byte[] s = ascii.GetBytes(strInput);
            for (int i = 0; i < s.Length; i++)
            {
                if ((int)s[i] == 63)
                {
                    intLength += 2;
                }
                else
                {
                    intLength += 1;
                }

                try
                {
                    strString += strInput.Substring(i, 1);
                }
                catch
                {
                    break;
                }

                if (intLength > intlen)
                {
                    break;
                }
            }
            //如果截过则加上半个省略号
            byte[] mybyte = System.Text.Encoding.Default.GetBytes(strInput);
            if (mybyte.Length > intlen)
            {
                strString += "…";
            }
            return strString;
        }

        #endregion

        #region 截取字符窜对字符串中的HTML进行过滤

        public static string CutString_FiterHtml(string strInput, int intlen) //截取字符串
        {
            ASCIIEncoding ascii = new ASCIIEncoding();
            int intLength = 0;
            string strString = "";
            strInput = StripHTML(strInput); //先过滤 后截取
            byte[] s = ascii.GetBytes(strInput);
            for (int i = 0; i < s.Length; i++)
            {
                if ((int)s[i] == 63)
                {
                    intLength += 2;
                }
                else
                {
                    intLength += 1;
                }

                try
                {
                    strString += strInput.Substring(i, 1);
                }
                catch
                {
                    break;
                }

                if (intLength > intlen)
                {
                    break;
                }
            }
            //如果截过则加上半个省略号
            //byte[] mybyte = System.Text.Encoding.Default.GetBytes( strInput );
            //if( mybyte.Length > intlen ) {
            //    strString += "…";
            //}
            return strString;
        }

        #endregion

        #region 去除HTML标记

        ///去除HTML标记C#函数
        /// <summary>
        /// 去除HTML标记
        /// </summary>
        /// <param name="strHtml">包括HTML的源码 </param>
        /// <returns>已经去除后的文字</returns> 
        public static string StripHTML(string Htmlstring)
        {
            //删除脚本
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "",
                                       RegexOptions.IgnoreCase);
            //删除HTML
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "",
                                       RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "",
                                       RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"",
                                       RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&",
                                       RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<",
                                       RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">",
                                       RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", "   ",
                                       RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1",
                                       RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2",
                                       RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3",
                                       RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9",
                                       RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "",
                                       RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"\</*font[^>]*\>", "  ",
                                       RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "([ ]+)", "",
                                       RegexOptions.IgnoreCase);
            Htmlstring.Replace(" ", "");
            Htmlstring.Replace("<", "");
            Htmlstring.Replace(">", "");
            Htmlstring.Replace("\r\n", "");
            // Htmlstring = HttpContext.Current.Server.HtmlEncode( Htmlstring ).Trim();

            return Htmlstring;

        }

        #endregion

        #region 字符串分函数

        /// <summary>
        /// 字符串分函数
        /// </summary>
        /// <param name="strings"></param>
        /// <param name="index"></param>
        /// <param name="Separ"></param>
        /// <returns></returns>
        public string StringSplit(string strings, int index, string Separ)
        {
            string[] s = strings.Split(char.Parse(Separ));
            return s[index];
        }

        #endregion

        #region 分解字符串为数组

        /// <summary>
        /// 字符串分函数
        /// </summary>
        /// <param name="str">要分解的字符串</param>
        /// <param name="splitstr">分割符,可以为string类型</param>
        /// <returns>字符数组</returns>
        public static string[] splitstr(string str, string splitstr)
        {
            if (splitstr != "")
            {
                System.Collections.ArrayList c = new System.Collections.ArrayList();
                while (true)
                {
                    int thissplitindex = str.IndexOf(splitstr);
                    if (thissplitindex >= 0)
                    {
                        c.Add(str.Substring(0, thissplitindex));
                        str = str.Substring(thissplitindex + splitstr.Length);
                    }
                    else
                    {
                        c.Add(str);
                        break;
                    }
                }
                string[] d = new string[c.Count];
                for (int i = 0; i < c.Count; i++)
                {
                    d[i] = c[i].ToString();
                }
                return d;
            }
            else
            {
                return new string[] { str };
            }
        }

        #endregion

        #region 检测一个字符符,是否在另一个字符中,存在,存在返回true,否则返回false

        /// <summary>
        /// 检测一个字符符,是否在另一个字符中,存在,存在返回true,否则返回false
        /// </summary>
        /// <param name="srcString">原始字符串</param>
        /// <param name="aimString">目标字符串</param>
        /// <returns></returns>
        public static bool IsEnglish(string srcString, string aimString)
        {
            bool Rev = true;
            string chr;
            if (aimString == "" || aimString == null) return false;
            for (int i = 0; i < aimString.Length; i++)
            {
                chr = aimString.Substring(i, 1);
                if (srcString.IndexOf(chr) < 0)
                {
                    return false;
                }

            }
            return Rev;
        }

        #endregion

        #region 检测字符串中是否含有中文及中文长度

        /// <summary>
        /// 检测字符串中是否含有中文及中文长度
        /// </summary>
        /// <param name="str">要检测的字符串</param>
        /// <returns>中文字符串长度</returns>
        public static int CnStringLength(string str)
        {
            ASCIIEncoding n = new ASCIIEncoding();
            byte[] b = n.GetBytes(str);
            int l = 0; // l 为字符串之实际长度 
            for (int i = 0; i <= b.Length - 1; i++)
            {
                if (b[i] == 63) //判断是否为汉字或全脚符号 
                {
                    l++;
                }
            }
            return l;

        }

        #endregion

        #region 处理NULL值为空字符显示

        /// <summary>
        /// 功能:处理NULL值为空字符显示
        /// </summary>
        /// <param name="strVAlue">字符串</param>
        /// <returns>空的字符串</returns>
        public static string ConvertNullToEmpty(string strVAlue)
        {
            if (strVAlue == null)
            {
                return String.Empty;
            }
            else
            {
                return strVAlue;
            }
        }

        #endregion

        /// <summary>
        /// 返回字符串真实长度, 1个汉字长度为2
        /// </summary>
        /// <returns>字符长度</returns>
        public static int GetStringLength(string str)
        {
            return Encoding.Default.GetBytes(str).Length;
        }
    }
}

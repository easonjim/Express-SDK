using System;
using System.Web;
using System.Text;
using System.Text.RegularExpressions;

namespace ComLib
{
    /// <summary>
    /// ���ú�������
    /// </summary>
    public class StringHelper
    {
        #region �滻�ַ���

        /// <summary>
        /// ����:�滻�ַ�
        /// </summary>
        /// <param name="strVAlue">�ַ���</param>
        /// <returns>�滻��'���ַ���</returns>
        public static string FilterReplace(string strVAlue)
        {
            string str = "";
            str = strVAlue.Replace("'", "");
            return str;
        }

        #endregion

        #region �Ա� �����ݽ���ת��HTML����,

        /// <summary>
        /// ����:�Ա� �����ݽ���ת��HTML����,
        /// </summary>
        /// <param name="fString">html�ַ���</param>
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

        #region �ж��Ƿ�:����ֵ���� or ��

        /// <summary>
        /// �ж��Ƿ�:����ֵ���� or ��
        /// </summary>
        /// <param name="b">true ��false</param>
        /// <returns>�� or ��</returns>
        public static string Judgement(bool b)
        {
            string s = "";
            if (b == true)
                s = "<b><font color=#009900>��</font></b>";
            else
                s = "<b><font color=#FF0000>��</font></b>";
            return s;
        }

        #endregion

        #region ��ȡ�ַ���

        /// <summary>
        /// ����:��ȡ�ַ�������
        /// </summary>
        /// <param name="str">Ҫ��ȡ���ַ���</param>
        /// <param name="length">�ַ�������</param>
        /// <param name="flg">true:��...,flase:����</param>
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

        #region ��ȡ�ַ���+��

        /// <summary>
        /// ��ȡ�ַ���+��
        /// </summary>
        /// <param name="strInput"></param>
        /// <param name="intlen"></param>
        /// <returns></returns>
        public static string CutString(string strInput, int intlen) //��ȡ�ַ���
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
            //����ع�����ϰ��ʡ�Ժ�
            byte[] mybyte = System.Text.Encoding.Default.GetBytes(strInput);
            if (mybyte.Length > intlen)
            {
                strString += "��";
            }
            return strString;
        }

        #endregion

        #region ��ȡ�ַ��ܶ��ַ����е�HTML���й���

        public static string CutString_FiterHtml(string strInput, int intlen) //��ȡ�ַ���
        {
            ASCIIEncoding ascii = new ASCIIEncoding();
            int intLength = 0;
            string strString = "";
            strInput = StripHTML(strInput); //�ȹ��� ���ȡ
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
            //����ع�����ϰ��ʡ�Ժ�
            //byte[] mybyte = System.Text.Encoding.Default.GetBytes( strInput );
            //if( mybyte.Length > intlen ) {
            //    strString += "��";
            //}
            return strString;
        }

        #endregion

        #region ȥ��HTML���

        ///ȥ��HTML���C#����
        /// <summary>
        /// ȥ��HTML���
        /// </summary>
        /// <param name="strHtml">����HTML��Դ�� </param>
        /// <returns>�Ѿ�ȥ���������</returns> 
        public static string StripHTML(string Htmlstring)
        {
            //ɾ���ű�
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "",
                                       RegexOptions.IgnoreCase);
            //ɾ��HTML
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

        #region �ַ����ֺ���

        /// <summary>
        /// �ַ����ֺ���
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

        #region �ֽ��ַ���Ϊ����

        /// <summary>
        /// �ַ����ֺ���
        /// </summary>
        /// <param name="str">Ҫ�ֽ���ַ���</param>
        /// <param name="splitstr">�ָ��,����Ϊstring����</param>
        /// <returns>�ַ�����</returns>
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

        #region ���һ���ַ���,�Ƿ�����һ���ַ���,����,���ڷ���true,���򷵻�false

        /// <summary>
        /// ���һ���ַ���,�Ƿ�����һ���ַ���,����,���ڷ���true,���򷵻�false
        /// </summary>
        /// <param name="srcString">ԭʼ�ַ���</param>
        /// <param name="aimString">Ŀ���ַ���</param>
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

        #region ����ַ������Ƿ������ļ����ĳ���

        /// <summary>
        /// ����ַ������Ƿ������ļ����ĳ���
        /// </summary>
        /// <param name="str">Ҫ�����ַ���</param>
        /// <returns>�����ַ�������</returns>
        public static int CnStringLength(string str)
        {
            ASCIIEncoding n = new ASCIIEncoding();
            byte[] b = n.GetBytes(str);
            int l = 0; // l Ϊ�ַ���֮ʵ�ʳ��� 
            for (int i = 0; i <= b.Length - 1; i++)
            {
                if (b[i] == 63) //�ж��Ƿ�Ϊ���ֻ�ȫ�ŷ��� 
                {
                    l++;
                }
            }
            return l;

        }

        #endregion

        #region ����NULLֵΪ���ַ���ʾ

        /// <summary>
        /// ����:����NULLֵΪ���ַ���ʾ
        /// </summary>
        /// <param name="strVAlue">�ַ���</param>
        /// <returns>�յ��ַ���</returns>
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
        /// �����ַ�����ʵ����, 1�����ֳ���Ϊ2
        /// </summary>
        /// <returns>�ַ�����</returns>
        public static int GetStringLength(string str)
        {
            return Encoding.Default.GetBytes(str).Length;
        }
    }
}

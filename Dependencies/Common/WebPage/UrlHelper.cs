using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;
using System.Web;
using System.Collections.Specialized;
using System.IO;

namespace ComLib
{
    /// <summary>
    /// URL�Ĳ�����
    /// </summary>
    public class UrlHelper
    {
        static System.Text.Encoding encoding = System.Text.Encoding.UTF8;

        #region URL��64λ����
        public static string Base64Encrypt(string sourthUrl)
        {
            string eurl = HttpUtility.UrlEncode(sourthUrl);
            eurl = Convert.ToBase64String(encoding.GetBytes(eurl));
            return eurl;
        }
        #endregion

        #region URL��64λ����
        public static string Base64Decrypt(string eStr)
        {
            if (!IsBase64(eStr))
            {
                return eStr;
            }
            byte[] buffer = Convert.FromBase64String(eStr);
            string sourthUrl = encoding.GetString(buffer);
            sourthUrl = HttpUtility.UrlDecode(sourthUrl);
            return sourthUrl;
        }
        /// <summary>
        /// �Ƿ���Base64�ַ���
        /// </summary>
        /// <param name="eStr"></param>
        /// <returns></returns>
        public static bool IsBase64(string eStr)
        {
            if ((eStr.Length % 4) != 0)
            {
                return false;
            }
            if (!Regex.IsMatch(eStr, "^[A-Z0-9/+=]*$", RegexOptions.IgnoreCase))
            {
                return false;
            }
            return true;
        }
        #endregion

        #region "���URL���� ���������ѭ�����"
        /// <summary>
        /// ���URL���� ���������ѭ�����
        /// </summary>
        public static string AddParam(string url, string paramName, string value)
        {
            Uri uri = new Uri(url);
            if (string.IsNullOrEmpty(uri.Query))
            {
                string eval = HttpContext.Current.Server.UrlEncode(value);
                return String.Concat(url, "?" + paramName + "=" + eval);
            }
            else
            {
                string eval = HttpContext.Current.Server.UrlEncode(value);
                return String.Concat(url, "&" + paramName + "=" + eval);
            }
        }
        #endregion

        #region "����URL����"
        /// <summary>
        /// ����URL����
        /// </summary>
        public static string UpdateParam(string url, string paramName, string value)
        {
            string keyWord = paramName + "=";
            int index = url.IndexOf(keyWord) + keyWord.Length;
            int index1 = url.IndexOf("&", index);
            if (index1 == -1)
            {
                url = url.Remove(index, url.Length - index);
                url = string.Concat(url, value);
                return url;
            }
            url = url.Remove(index, index1 - index);
            url = url.Insert(index, value);
            return url;
        }
        #endregion

        #region "��ȡӦ�ó����·��"
        /// <summary>
        /// ��ȡӦ�ó����·��
        /// </summary>
        /// <returns></returns>
        public static string AppPath()
        {
            string _ApplicationPath = HttpContext.Current.Request.ApplicationPath;
            if (_ApplicationPath != "/")
                _ApplicationPath += "/";
            return _ApplicationPath;
        }


        #endregion

        #region "�������ڳ��������web���ú��������� �����ļ��ľ���·��"

        /// <summary>
        /// ��õ�ǰ����·�� �������ڳ��������web���ú��������� �����ļ��ľ���·��
        /// </summary>
        /// <param name="strPath">ָ����·��</param>
        /// <returns>����·��</returns>
        public static string GetMapPath(string strPath)
        {
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Server.MapPath(strPath);
            }
            else //��web��������
            {
                strPath = strPath.Replace("/", "\\");
                if (strPath.StartsWith("\\"))
                {
                    strPath = strPath.Substring(strPath.IndexOf('\\', 1)).TrimStart('\\');
                }
                return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strPath);
            }
        }
        #endregion

        #region  Static Property Get BaseUrl(��̬���Ի�ȡURL��ַ)
        /// <summary>
        /// �����̬���Եĵ��ñ��������´��뷽����?
        /// �������:
        /// Response.Write(UIHelper.BaseUrl);
        /// </summary>
        public static string BaseUrl
        {
            get
            {
                //strBaseUrl���ڴ洢URL��ַ
                string strBaseUrl = "";
                //��ȡ��ǰHttpContext�µĵ�ַ
                strBaseUrl += "http://" + HttpContext.Current.Request.Url.Host;
                //����˿ڲ���80�Ļ�����ô���������?
                if (HttpContext.Current.Request.Url.Port.ToString() != "80")
                {
                    strBaseUrl += ":" + HttpContext.Current.Request.Url.Port;
                }
                strBaseUrl += HttpContext.Current.Request.ApplicationPath;

                return strBaseUrl + "/";
            }
        }
        #endregion

        #region ����URL��������
        public static void GetDomain(string fromUrl, out string domain, out string subDomain)
        {
            domain = "";
            subDomain = "";
            try
            {
                if (fromUrl.IndexOf("����Ƭ") > -1)
                {
                    subDomain = fromUrl;
                    domain = "��Ƭ";
                    return;
                }

                UriBuilder builder = new UriBuilder(fromUrl);
                fromUrl = builder.ToString();

                Uri u = new Uri(fromUrl);

                if (u.IsWellFormedOriginalString())
                {
                    if (u.IsFile)
                    {
                        subDomain = domain = "�ͻ��˱����ļ�·��";

                    }
                    else
                    {
                        string Authority = u.Authority;
                        string[] ss = u.Authority.Split('.');
                        if (ss.Length == 2)
                        {
                            Authority = "www." + Authority;
                        }
                        int index = Authority.IndexOf('.', 0);
                        domain = Authority.Substring(index + 1, Authority.Length - index - 1).Replace("comhttp", "com");
                        subDomain = Authority.Replace("comhttp", "com");
                        if (ss.Length < 2)
                        {
                            domain = "����·��";
                            subDomain = "����·��";
                        }
                    }
                }
                else
                {
                    if (u.IsFile)
                    {
                        subDomain = domain = "�ͻ��˱����ļ�·��";
                    }
                    else
                    {
                        subDomain = domain = "����·��";
                    }
                }
            }
            catch
            {
                subDomain = domain = "����·��";
            }
        }

        /// <summary>
        /// ���� url �ַ����еĲ�����Ϣ
        /// </summary>
        /// <param name="url">����� URL</param>
        /// <param name="baseUrl">��� URL �Ļ�������</param>
        /// <param name="nvc">���������õ��� (������,����ֵ) �ļ���</param>
        public static void ParseUrl(string url, out string baseUrl, out NameValueCollection nvc)
        {
            if (url == null)
                throw new ArgumentNullException("url");

            nvc = new NameValueCollection();
            baseUrl = "";

            if (url == "")
                return;

            int questionMarkIndex = url.IndexOf('?');

            if (questionMarkIndex == -1)
            {
                baseUrl = url;
                return;
            }
            baseUrl = url.Substring(0, questionMarkIndex);
            if (questionMarkIndex == url.Length - 1)
                return;
            string ps = url.Substring(questionMarkIndex + 1);

            // ��ʼ����������    
            Regex re = new Regex(@"(^|&)?(\w+)=([^&]+)(&|$)?", RegexOptions.Compiled);
            MatchCollection mc = re.Matches(ps);

            foreach (Match m in mc)
            {
                nvc.Add(m.Result("$2").ToLower(), m.Result("$3"));
            }
        }

        #endregion

        #region "����ҳ�治������"
        /// <summary>
        /// ����ҳ�治������
        /// </summary>
        public static void SetPageNoCache()
        {

            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ExpiresAbsolute = System.DateTime.Now.AddSeconds(-1);
            HttpContext.Current.Response.Expires = 0;
            HttpContext.Current.Response.CacheControl = "no-cache";
            HttpContext.Current.Response.AddHeader("Pragma", "No-Cache");
        }
        #endregion

        #region "��ȡҳ��url"
        /// <summary>
        /// ��ȡ��ǰ����ҳ���ַ
        /// </summary>
        public static string GetScriptName
        {
            get
            {
                return HttpContext.Current.Request.ServerVariables["SCRIPT_NAME"].ToString();
            }
        }

        /// <summary>
        /// ��ȡ��ǰ����ҳ���ַ����
        /// </summary>
        public static string GetScriptNameQueryString
        {
            get
            {
                return HttpContext.Current.Request.ServerVariables["QUERY_STRING"].ToString();
            }
        }

        /// <summary>
        /// ���ҳ���ļ����Ͳ�����
        /// </summary>
        public static string GetScriptNameUrl
        {
            get
            {
                string Script_Name = GetScriptName;
                Script_Name = Script_Name.Substring(Script_Name.LastIndexOf("/") + 1);
                Script_Name += "?" + GetScriptNameQueryString;
                return Script_Name;
            }
        }

        /// <summary>
        /// ��ȡ��ǰ����ҳ��Url
        /// </summary>
        public static string GetScriptUrl
        {
            get
            {
                return GetScriptNameQueryString == "" ? GetScriptName : string.Format("{0}?{1}", GetScriptName, GetScriptNameQueryString);
            }
        }

        /// <summary>
        /// ���ص�ǰҳ��Ŀ¼��url
        /// </summary>
        /// <param name="FileName">�ļ���</param>
        /// <returns></returns>
        public static string GetHomeBaseUrl(string FileName)
        {
            string Script_Name = GetScriptName;
            return string.Format("{0}/{1}", Script_Name.Remove(Script_Name.LastIndexOf("/")), FileName);
        }

        /// <summary>
        /// ���ص�ǰ��վ��ַ
        /// </summary>
        /// <returns></returns>
        public static string GetHomeUrl()
        {
            return HttpContext.Current.Request.Url.Authority;
        }

        /// <summary>
        /// ��ȡ��ǰ�����ļ�����Ŀ¼
        /// </summary>
        /// <returns>·��</returns>
        public static string GetScriptPath
        {
            get
            {
                string Paths = HttpContext.Current.Request.ServerVariables["PATH_TRANSLATED"].ToString();
                return Paths.Remove(Paths.LastIndexOf("\\"));
            }
        }
        #endregion
     
    }
}

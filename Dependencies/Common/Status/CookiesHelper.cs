using System;
using System.Web;

namespace ComLib
{

    /// <summary>
    /// 网站Cookie操作类
    /// </summary>
    public class CookieHelper : System.Web.UI.Page
    {

        public CookieHelper()
        {
        }
       
        /// <summary>
        /// 保存一个Cookie
        /// </summary>
        /// <param name="CookieName">Cookie名称</param>
        /// <param name="CookieValue">Cookie值</param>
        /// <param name="CookieTime">Cookie过期0为关闭页面失效,1为保存1天，2为保存1月，3为保存1年</param>
        /// <param name="CookieDomainName">CookieDomainName</param>
        public static void SaveCookie(string CookieName, string CookieValue, int CookieTime, string CookieDomainName)
        {
            SaveCookie(CookieName, CookieValue, CookieTime, CookieDomainName, true);


        }
        /// <summary>
        /// 保存一个Cookie
        /// </summary>
        /// <param name="CookieName">Cookie名称</param>
        /// <param name="CookieValue">Cookie值</param>
        /// <param name="CookieTime">Cookie过期0为关闭页面失效,1为保存1天，2为保存1月，3为保存1年</param>
        /// <param name="CookieDomainName">CookieDomainName</param>
        /// <param name="Endcode">是否编码</param>
        public static void SaveCookie(string CookieName, string CookieValue, int CookieTime, string CookieDomainName, bool Endcode)
        {
            HttpCookie myCookie = new HttpCookie(CookieName);
            if (Endcode)
            {
                myCookie.Value = HttpContext.Current.Server.UrlEncode(CookieValue);
            }
            else
            {
                myCookie.Value = CookieValue;
            }
            // myCookie.Name = CookieName;
            if (CookieDomainName != "")
                myCookie.Domain = CookieDomainName;
            TimeSpan ts = new TimeSpan(0, 0, 0, 0);
            DateTime tm = DateTime.Now;
            switch (CookieTime)
            {
                case 1:
                    ts = new TimeSpan(1, 0, 0, 0);
                    break;
                case 2:
                    ts = new TimeSpan(30, 0, 0, 0);
                    //保存一月
                    break;
                case 3:
                    ts = new TimeSpan(365, 0, 0, 0);
                    //保存一年
                    break;
                default:
                    if (CookieTime > 3)
                        ts = new TimeSpan(0, CookieTime, 0);
                    break;

            }
            if (CookieTime != 0)
            {
                myCookie.Expires = tm.Add(ts);

            }
            if (HttpContext.Current.Response.Cookies[CookieName] != null)
                HttpContext.Current.Response.Cookies.Remove(CookieName);

            HttpContext.Current.Response.Cookies.Add(myCookie);


        }
        /// <summary>
        /// 保存一个Cookie
        /// </summary>
        /// <param name="CookieName">Cookie名称</param>
        /// <param name="CookieChildName">Cookie子名称</param>
        /// <param name="CookieValue">Cookie子值</param>
        /// <param name="CookieTime">Cookie过期0为关闭页面失效,1为保存1天，2为保存1月，3为保存1年</param>
        /// <param name="CookieDomainName">CookieDomainName</param>
        public static void SaveCookie(string CookieName, string CookieChildName, string CookieValue, int CookieTime, string CookieDomainName)
        {
            HttpCookie myCookie;
            if (HttpContext.Current.Request[CookieName] != null)
            {
                myCookie = HttpContext.Current.Request.Cookies[CookieName];
                if (myCookie.Values[CookieChildName] != null && myCookie.Values[CookieChildName] != "")
                    myCookie.Values.Remove(CookieChildName);
            }
            else
            {

                myCookie = new HttpCookie(CookieName);

            }

            if (CookieDomainName != "")
                myCookie.Domain = CookieDomainName;
            TimeSpan ts = new TimeSpan(0, 0, 0, 0);
            DateTime tm = DateTime.Now;
            switch (CookieTime)
            {
                case 1:
                    ts = new TimeSpan(1, 0, 0, 0);
                    break;
                case 2:
                    ts = new TimeSpan(30, 0, 0, 0);
                    //保存一月
                    break;
                case 3:
                    ts = new TimeSpan(365, 0, 0, 0);
                    //保存一年
                    break;
                default:
                    if (CookieTime > 3)
                        ts = new TimeSpan(0, CookieTime, 0);
                    break;

            }
            if (CookieTime != 0)
            {
                myCookie.Expires = tm.Add(ts);

            }


            myCookie.Values.Add(CookieChildName, HttpContext.Current.Server.UrlEncode(CookieValue));
            HttpContext.Current.Response.Cookies.Add(myCookie);



        }




        /// <summary>
        /// 清除CookieValue
        /// </summary>
        /// <param name="CookieName">Cookie名称</param>
        /// <param name="CookieDomainName">CookieDomainName</param>
        public static void ClearCookie(string CookieName, string CookieDomainName)
        {
            HttpCookie myCookie = new HttpCookie(CookieName);
            DateTime now = DateTime.Now;
            if (CookieDomainName != "")
                myCookie.Domain = CookieDomainName;
            myCookie.Value = "";
            myCookie.Expires = now.AddYears(-2);
            HttpContext.Current.Response.Cookies.Add(myCookie);
        }

        /// <summary>
        /// 取得CookieValue
        /// </summary>
        /// <param name="CookieName">Cookie名称</param>
        /// <returns>Cookie的值</returns>
        public static string GetCookie(string CookieName)
        {
            HttpCookie myCookie = new HttpCookie(CookieName);
            myCookie = System.Web.HttpContext.Current.Request.Cookies[CookieName];

            if (myCookie != null)
                return HttpContext.Current.Server.UrlDecode(myCookie.Value);
            else
                return "";
        }
        /// <summary>
        /// 取得CookieValue
        /// </summary>
        /// <param name="CookieName">Cookie名称</param>
        /// <param name="CookieChildName">二维Cookie名称</param>
        /// <returns>Cookie的值</returns>
        public static string GetCookie(string CookieName, string CookieChildName)
        {
            HttpCookie myCookie = new HttpCookie(CookieName);
            myCookie = System.Web.HttpContext.Current.Request.Cookies[CookieName];

            if (myCookie != null)
            {
                return HttpContext.Current.Server.UrlDecode(myCookie.Values[CookieChildName]);
            }
            else
                return "";
        }


    }
}

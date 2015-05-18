/******************************************************************************
 *  公司：       Jsoft 
 *  作者：       Jim
 *  创建时间：   2012-09-10 20:58:09
 *  描述：
 *  更新：
 ******************************************************************************/

using System;
using System.Web;


namespace ComLib
{

    /// <summary>
    /// 系统的固定数据，比如网站根路径、app路径、主机名称等。
    /// 1) 可以在非web请求时候使用，比如定时器的新线程中使用;
    /// 2) 获取不同域名级别下的的统一主机值
    /// </summary>
    public class SystemInfo {

        public static SystemInfo Instance = loadSystemInfo();

        private SystemInfo() { }

        /// <summary>
        /// 比 ApplicationPath 多一个斜杠
        /// </summary>
        public static String RootPath { get { return SystemInfo.Instance.rootPath; } }

        /// <summary>
        /// 比如 /myapp 或者 /
        /// </summary>
        public static String ApplicationPath { get { return SystemInfo.Instance.applicationPath; } }

        /// <summary>
        /// 主机名 www.abc.com 或 localhost 或 127.0.0.1
        /// </summary>
        public static String Host { get { return SystemInfo.Instance.host; } }

        /// <summary>
        /// 网站首页，普通模式是"/"，二级域名下是http加主机名
        /// </summary>
        public static String SiteRoot { get { return SystemInfo.Instance.siteRoot; } }

        /// <summary>
        /// 不带二级域名的 Host 名称，比如 abc.com 或 baidu.com
        /// </summary>
        public static String HostNoSubdomain { get { return SystemInfo.Instance.hostNoSubdomain; } }

        /// <summary>
        /// Host 是否是ip地址
        /// </summary>
        public static Boolean HostIsIp {
            get { return SystemInfo.Instance.hostIsIp; }
        }

        /// <summary>
        /// Host 是否等于 localhost
        /// </summary>
        public static Boolean HostIsLocalhost {
            get { return SystemInfo.Instance.hostIsLocalhost; }
        }

        /// <summary>
        /// 主机名(或ip地址)+端口号
        /// </summary>
        public static String Authority { get { return SystemInfo.Instance.authority; } }

        private String rootPath;
        private String applicationPath;
        private String host;
        private String authority;

        private Boolean hostIsIp;
        private Boolean hostIsLocalhost;
        private String hostNoSubdomain;
        private String siteRoot;

        //-------------------------------------------------------------------------------------------------

        private static Boolean _initialized = false;
        private static Object _objLock = new Object();

     

        private static SystemInfo loadSystemInfo() {

            SystemInfo obj = new SystemInfo();

            if (IsWeb) {

                obj.applicationPath = HttpContext.Current.Request.ApplicationPath;
                obj.rootPath = addEndSlash( obj.applicationPath );
                obj.authority = HttpContext.Current.Request.Url.Authority;

                obj.host = HttpContext.Current.Request.Url.Host;

                obj.hostIsLocalhost = EqualsIgnoreCase( obj.host, "localhost" );
                obj.hostIsIp = RegexHelper.IsIPv4( obj.host );
                obj.hostNoSubdomain = getHostNoSubdomain( obj );

            }
            else {
                obj.applicationPath = "/";
                obj.rootPath = "/";
                obj.host = "localhost";
            }

            return obj;
        }

        public static Boolean EqualsIgnoreCase(String s1, String s2)
        {

            if (s1 == null && s2 == null) return true;
            if (s1 == null || s2 == null) return false;

            if (s2.Length != s1.Length) return false;

            return string.Compare(s1, 0, s2, 0, s2.Length, StringComparison.OrdinalIgnoreCase) == 0;
        } 

        private static String getHostNoSubdomain( SystemInfo obj ) {

            if (obj.hostIsIp || obj.hostIsLocalhost) {
                return obj.host;
            }
            else {
                return getHostNoSubdomain( obj.host );
            }

        }

        private static String getHostNoSubdomain( String host ) {
            int firstDotIndex = host.IndexOf( '.' );
            String result = host.Substring( firstDotIndex + 1, host.Length - firstDotIndex - 1 );
            if (result.IndexOf( '.' ) < 0) return host;
            return result;
        }

        private static String addEndSlash( String appPath ) {
            if (!appPath.EndsWith( "/" )) return appPath + "/";
            return appPath;
        }


        public static Boolean IsWeb { get { return HttpContext.Current != null; } }
        public static Boolean IsWindows {
            get { return Environment.OSVersion.VersionString.ToLower().IndexOf( "windows" ) >= 0; }
        }

        //-------------------------------------------------------------------------------------------------

        public static void UpdateSessionId() {
            String sessionId = getSessionId();
            if (sessionId != null) updateCookie( sessionId );
        }

        private static String getSessionId() {
            String sessionId = "ASPNET_SESSIONID";
            HttpRequest req = HttpContext.Current.Request;
            if (req.Form[sessionId] != null) return req.Form[sessionId];
            if (req.QueryString[sessionId] != null) return req.QueryString[sessionId];
            return null;
        }

        private static void updateCookie( String sessionId ) {

            String cookieName = "ASP.NET_SESSIONID";

            HttpRequest req = HttpContext.Current.Request;
            HttpResponse res = HttpContext.Current.Response;

            HttpCookie cookie = req.Cookies.Get( cookieName );

            if (cookie == null) {
                res.Cookies.Add( new HttpCookie( cookieName, sessionId ) );
            }
            else {
                cookie.Value = sessionId;
                req.Cookies.Set( cookie );
            }
        }


    }

}

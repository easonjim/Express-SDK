using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace kuaidi
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {

        }

        protected void Session_Start(object sender, EventArgs e)
        {
            Session.Timeout = 5;
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            HttpContext context = ((HttpApplication)sender).Context;
            Exception ex = context.Server.GetLastError();
            if (ex == null || !(ex is HttpException))
                return;

            ComLib.LogLib.Log4NetBase.Log(ex);

            StringBuilder sb = new StringBuilder();

            try
            {
                sb.Append("Host : " + context.Request.Url.Host);
                sb.Append(Environment.NewLine);
                sb.Append("Port : " + context.Request.Url.Port);
                sb.Append(Environment.NewLine);
                sb.Append("User Host Address : " + context.Request.UserHostAddress);
                sb.Append(Environment.NewLine);
                sb.Append("User Agent : " + context.Request.UserAgent);
                sb.Append(Environment.NewLine);
                sb.Append("Url : " + context.Request.Url);
                sb.Append(Environment.NewLine);
                sb.Append("Raw Url : " + context.Request.RawUrl);
                sb.Append(Environment.NewLine);


                while (ex != null)
                {
                    sb.Append("Message : " + ex.Message);
                    sb.Append(Environment.NewLine);
                    sb.Append("Source : " + ex.Source);
                    sb.Append(Environment.NewLine);
                    sb.Append("StackTrace : " + ex.StackTrace);
                    sb.Append(Environment.NewLine);
                    sb.Append("TargetSite : " + ex.TargetSite);
                    sb.Append(Environment.NewLine);
                    ex = ex.InnerException;
                }
            }
            catch (Exception ex2)
            {
                sb.Append("Error logging error : " + ex2.Message);
            }

            //TODO:写入日志
            ComLib.LogLib.Log4NetBase.Log(sb.ToString());



            context.Items["LastErrorDetails"] = sb.ToString();
            context.Response.StatusCode = 500;
            Server.ClearError();

            // Server.Transfer is prohibited during a page callback.
            System.Web.UI.Page currentPage = context.CurrentHandler as System.Web.UI.Page;
            if (currentPage != null && currentPage.IsCallback)
                return;


            ComLib.LogLib.Log4NetBase.Log("程序错误");
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}
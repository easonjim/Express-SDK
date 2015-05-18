using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ComLib;
using Newtonsoft.Json;

namespace kuaidi
{
    /// <summary>
    /// API 的摘要说明
    /// </summary>
    public class API : IHttpHandler, IRequiresSessionState
    {
        private string Type
        {
            get { return ComLib.QueryStringHelper.GetStringByQueryString("type"); }
        }

        private string Number
        {
            get { return ComLib.QueryStringHelper.GetStringByQueryString("number"); }
        }

        private string Method
        {
            get { return ComLib.QueryStringHelper.GetStringByQueryString("method"); }
        }

        private string Name
        {
            get { return ComLib.QueryStringHelper.GetStringByQueryString("name"); }
        }

        public void ProcessRequest(HttpContext context)
        {
            ComLib.LogLib.Log4NetBase.Log("API调用-时间：" + DateTime.Now.ToString() +" IP：" + context.Request.UserHostAddress + " UserAgent：" + context.Request.UserAgent);

            if (!Method.IsNullOrEmpty())
            {
                switch (Method)
                {
                    case "get.name":
                        ResponseName(context);
                        break;
                    default:
                        context.Response.Write("{ErrorCode:\"-1\",ErrorMessage:\"参数错误\"}");
                        break;
                }
            }
            else
            {
                switch (Type)
                {
                    case "html":
                        ResponseHTML(context);
                        break;
                    case "json":
                        ResponseJSON(context);
                        break;
                    default:
                        context.Response.Write("{ErrorCode:\"-1\",ErrorMessage:\"参数错误\"}");
                        break;
                }
            }
            ComLib.LogLib.Log4NetBase.Log("API响应-时间：" + DateTime.Now.ToString());
            context.Response.End();
        }

        private void ResponseHTML(HttpContext context)
        {
            if (SessionHelper.Get<string>("NumberHTML").IsNullOrEmpty() ||
                ((!SessionHelper.Get<string>("NumberHTML").IsNullOrEmpty()) &&
                 !SessionHelper.Get<string>("NumberHTML").Equals(Number)))
            {
                ComLib.LogLib.Log4NetBase.Log("调用ResponseHTML-重新获取-时间："+DateTime.Now.ToString());
                SessionHelper.Set("GetHtml", kuaidi100.GetHtml(Number));
                SessionHelper.Set("NumberHTML", Number);
                context.Response.Clear();
                context.Response.Write(SessionHelper.Get<string>("GetHtml"));
            }
            else
            {
                ComLib.LogLib.Log4NetBase.Log("调用ResponseHTML-缓存数据-时间：" + DateTime.Now.ToString());
                context.Response.Clear();
                context.Response.Write(SessionHelper.Get<string>("GetHtml"));
            }
        }

        private void ResponseJSON(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            if (SessionHelper.Get<string>("NumberJSON").IsNullOrEmpty() ||
                ((!SessionHelper.Get<string>("NumberJSON").IsNullOrEmpty()) &&
                 !SessionHelper.Get<string>("NumberJSON").Equals(Number)))
            {
                ComLib.LogLib.Log4NetBase.Log("调用ResponseJSON-重新获取-时间：" + DateTime.Now.ToString());
                SessionHelper.Set("GetJson", kuaidi100.GetJson(Number));
                SessionHelper.Set("NumberJSON", Number);
                context.Response.Clear();
                context.Response.Write(SessionHelper.Get<string>("GetJson"));
            }
            else
            {
                ComLib.LogLib.Log4NetBase.Log("调用ResponseJSON-缓存数据-时间：" + DateTime.Now.ToString());
                context.Response.Clear();
                context.Response.Write(SessionHelper.Get<string>("GetJson"));
            }
        }

        private void ResponseName(HttpContext context)
        {
            if (SessionHelper.Get<string>("Name").IsNullOrEmpty() ||
                ((!SessionHelper.Get<string>("Name").IsNullOrEmpty()) &&
                 !SessionHelper.Get<string>("Name").Equals(Name)))
            {
                ComLib.LogLib.Log4NetBase.Log("调用ResponseName-重新获取-时间：" + DateTime.Now.ToString());
                SessionHelper.Set("GetName", kuaidi100.GetName(Name));
                SessionHelper.Set("Name", Name);
                context.Response.Clear();
                context.Response.Write(SessionHelper.Get<string>("GetName"));
            }
            else
            {
                ComLib.LogLib.Log4NetBase.Log("调用ResponseName-缓存数据-时间：" + DateTime.Now.ToString());
                context.Response.Clear();
                context.Response.Write(SessionHelper.Get<string>("GetName"));
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

using System.Text;
/********************************************************************************
      ** 创建人：    Jim
      ** 创建时间:   2008/07/15
      ** 功能描述：  常用Js类
      **              
*********************************************************************************/
namespace ComLib.JavaScriptLib
{
    /// <summary>
    /// 常用Js类
    /// </summary>
    public class JavaScriptHelper
    {
        #region "加载js文件"
        /// <summary>
        /// 加载js文件
        /// </summary>
        /// <param name="jsFilePath"></param>
        /// <param name="isInHead"></param>
        public static void IncludeJsFile(string jsFilePath, bool isInHead)
        {
            if (isInHead)
            {
                Page currentHandler = HttpContext.Current.CurrentHandler as Page;
                HtmlGenericControl child = new HtmlGenericControl("script");
                child.Attributes.Add("type", "text/javascript");
                child.Attributes.Add("language", "javascript");
                child.Attributes.Add("src", UrlHelper.AppPath() + jsFilePath);
                currentHandler.Header.Controls.Add(child);
            }
            else
            {
                string str = string.Format("\n<script  type=\"text/javascript\" src=\"{0}\">\n</script>\n", jsFilePath);
                ((Page)HttpContext.Current.Handler).ClientScript.RegisterStartupScript(Type.GetType("System.String"), Guid.NewGuid().ToString(), str);
            }
        }
        #endregion

        #region "加载css样式表"
        /// <summary>
        /// 加载css样式表
        /// </summary>
        /// <param name="cssFilePath"></param>
        public static void IncludeCssFile(string cssFilePath)
        {
            HtmlGenericControl child = new HtmlGenericControl("link");
            child.Attributes.Add("href", cssFilePath);
            child.Attributes.Add("rel", "stylesheet");
            child.Attributes.Add("type", "text/css");
            Page handler = (Page)HttpContext.Current.Handler;
            handler.Header.Controls.Add(child);
        }
        #endregion

        #region  Alert() 简单弹出对话框功能
        /// <summary>
        /// 简单弹出对话框功能
        /// 代码调用:
        /// UIHelper.Alert(this.Page,"OKOK");
        /// 
        /// 
        /// </summary>
        /// <param name="pageCurrent">
        /// 当前的页
        /// </param>
        /// <param name="strMsg">
        /// 弹出信息的内容
        /// </param>
        public static void Alert(System.Web.UI.Page pageCurrent, string strMsg)
        {
            //Replace \n
            strMsg = strMsg.Replace("\n", "file://n/");
            //Replace \r
            strMsg = strMsg.Replace("\r", "file://r/");
            //Replace "
            strMsg = strMsg.Replace("\"", "\\\"");
            //Replace '
            strMsg = strMsg.Replace("\'", "\\\'");

            pageCurrent.ClientScript.RegisterStartupScript(pageCurrent.GetType(),
                System.Guid.NewGuid().ToString(),
                "<script>window.alert('" + strMsg + "')</script>"
                );

            //以下代码是兼?net1.1版本的，但到?.0时代此API就过时了
            //pageCurrent.RegisterStartupScript(
            //    System.Guid.NewGuid().ToString(),
            //    "<script>window.alert('" + strMsg + "')</script>"
            //    );
        }
        public static void Alert(System.Web.UI.Page pageCurrent, string strMsg, string GoBackUrl)
        {
            //Replace \n
            strMsg = strMsg.Replace("\n", "file://n/");
            //Replace \r
            strMsg = strMsg.Replace("\r", "file://r/");
            //Replace "
            strMsg = strMsg.Replace("\"", "\\\"");
            //Replace '
            strMsg = strMsg.Replace("\'", "\\\'");

            pageCurrent.ClientScript.RegisterStartupScript(pageCurrent.GetType(),
                System.Guid.NewGuid().ToString(),
                "<script>window.alert('" + strMsg + "');location='" + GoBackUrl + "'</script>"
                );

            //以下代码是兼?net1.1版本的，但到?.0时代此API就过时了
            //pageCurrent.RegisterStartupScript(
            //    System.Guid.NewGuid().ToString(),
            //    "<script>window.alert('" + strMsg + "')</script>"
            //    );
        }

        /// <summary>
        /// 终止输出流 立即JS提示并返回
        /// </summary>
        /// <param name="Message">提示信息文字</param>
        /// <param name="ReturnUrl">返回地址</param>
        /// <param name="rq"></param>
        public static void AlertNow(string Message, string ReturnUrl, HttpContext rq)
        {
            System.Text.StringBuilder msgScript = new System.Text.StringBuilder();
            msgScript.Append("<script language=JavaScript>\n");
            msgScript.Append("alert(\"" + Message + "\");\n");
            msgScript.Append("parent.location.href='" + ReturnUrl + "';\n");
            msgScript.Append("</script>\n");
            rq.Response.Write(msgScript.ToString());
            rq.Response.End();
        }

        #endregion

        #region  Redirect() 跳转页面并显示信息
        /// <summary>
        /// Add the javascript method to redirect page on client
        /// </summary>
        /// <param name="pageCurrent"></param>
        /// <param name="strPage"></param>
        public static void Redirect(System.Web.UI.Page pageCurrent, string strPage)
        {
            pageCurrent.ClientScript.RegisterStartupScript(pageCurrent.GetType(),
                    System.Guid.NewGuid().ToString(),
                    "<script>window.location.href='" + strPage + "'</script>"
                    );

        }
        /// <summary>
        /// 主要用于跳出带有框架的页?
        /// </summary>
        /// <param name="pageCurrent">当前页面如this.page</param>
        /// <param name="strPage">要跳出的页面</param>
        public static void RedirectFrame(System.Web.UI.Page pageCurrent, string strPage)
        {
            pageCurrent.ClientScript.RegisterStartupScript(pageCurrent.GetType(),
                    System.Guid.NewGuid().ToString(),
                    "<script>window.top.location.href='" + strPage + "'</script>"
                    );

            //以下方法是兼?.1?2.0过时
            //pageCurrent.RegisterStartupScript(
            //    System.Guid.NewGuid().ToString(),
            //    "<script>window.location.href='" + strPage + "'</script>"
            //    );
        }

        #endregion

        #region AddConfirm 为按钮添加确认信息
        /// <summary>
        /// Add the confirm message to button
        /// 代码调用:
        ///    UIHelper.AddConfirm(this.Button1, "真的要删了？");
        /// 点确定按钮就会执行事件中的代码，点取消不?
        /// </summary>
        /// <param name="button">The control, must be a button</param>
        /// <param name="strMsg">The popup message</param>
        public static void AddConfirm(System.Web.UI.WebControls.Button button, string strMsg)
        {
            strMsg = strMsg.Replace("\n", "file://n/");
            strMsg = strMsg.Replace("\r", "file://r/");
            strMsg = strMsg.Replace("\"", "\\\"");
            strMsg = strMsg.Replace("\'", "\\\'");
            button.Attributes.Add("onClick", "return confirm('" + strMsg + "')");
        }

        /// <summary>
        /// Add the confirm message to button
        /// Created : GuangMing Chu, 1 1,2007
        /// Modified: GuangMing Chu, 1 1,2007
        /// Modified:
        /// 代码调用:
        ///       UIHelper.AddConfirm(this.Button1, "真的要删了？?);
        /// 点确定按钮就会执行事件中的代码，点取消不?
        ///      
        /// </summary>
        /// <param name="button">The control, must be a button</param>
        /// <param name="strMsg">The popup message</param>
        public static void AddConfirm(System.Web.UI.WebControls.ImageButton button, string strMsg)
        {
            strMsg = strMsg.Replace("\n", "file://n/");
            strMsg = strMsg.Replace("\r", "file://r/");
            strMsg = strMsg.Replace("\"", "\\\"");
            strMsg = strMsg.Replace("\'", "\\\'");
            button.Attributes.Add("onClick", "return confirm('" + strMsg + "')");
        }

        /// <summary>
        /// Add the Confirm Message to LinkButton
        /// </summary>
        /// <param name="button"></param>
        /// <param name="strMsg"></param>
        public static void AddConfirm(System.Web.UI.WebControls.LinkButton button, string strMsg)
        {
            strMsg = strMsg.Replace("\n", "file://n/");
            strMsg = strMsg.Replace("\r", "file://r/");
            strMsg = strMsg.Replace("\"", "\\\"");
            strMsg = strMsg.Replace("\'", "\\\'");
            button.Attributes.Add("onClick", "return confirm('" + strMsg + "')");
        }

        /// <summary>
        /// Add the confirm message to one column of gridview
        /// 代码调用?
        ///         UIHelper myHelp = new UIHelper();
        ///         myHelp.AddConfirm(this.GridView1,1, "ok");
        /// 请使用时注意，此方法的调用必须实例化，调?
        /// </summary>
        /// <param name="grid">The control, must be a GridView</param>
        /// <param name="intColIndex">The column index. It's usually the column which has the "delete" button.</param>
        /// <param name="strMsg">The popup message</param>
        public static void AddConfirm(System.Web.UI.WebControls.GridView grid, int intColIndex, string strMsg)
        {
            strMsg = strMsg.Replace("\n", "file://n/");
            strMsg = strMsg.Replace("\r", "file://r/");
            strMsg = strMsg.Replace("\"", "\\\"");
            strMsg = strMsg.Replace("\'", "\\\'");
            for (int i = 0; i < grid.Rows.Count; i++)
            {
                grid.Rows[i].Cells[intColIndex].Attributes.Add("onclick", "return confirm('" + strMsg + "')");
            }
        }
        #endregion

        #region OpenWindow 打开窗体的对话框功能
        /// <summary>
        /// Use "window.open" to popup the window
        /// Created : Wang Hui, Feb 24,2006
        /// Modified: Wang Hui, Feb 24,2006
        /// 打开窗体的对话框功能
        /// 代码调用?
        /// 
        /// 
        ///         UIHelper.OpenWindow(this.Page, "www.sina.com.cn", 400, 300);
        ///         UIHelper.ShowDialog(this.Page, "lsdjf.com", 300, 200);
        /// 
        /// 
        /// 
        /// </summary>
        /// <param name="strUrl">The url of window, start with "/", not "http://"</param>
        /// <param name="intWidth">Width of popup window</param>
        /// <param name="intHeight">Height of popup window</param>
        public static void OpenWindow(System.Web.UI.Page pageCurrent, string strUrl, int intWidth, int intHeight, int intLeft, int intTop, string WinName)
        {
            #region 老版?
            //string strScript = "";
            //strScript += "var strFeatures = 'width:" + intWidth.ToString() + "px;height:" + intHeight.ToString() + "px';";
            //strScript += "var strName ='__WIN';";
            ////strScript += "alert(strFeatures);";

            ////--- Add by Wang Hui on Feb 27
            //if (strUrl.Substring(0, 1) == "/")
            //{
            //    strUrl = strUrl.Substring(1, strUrl.Length - 1);
            //}
            ////--- End Add by Wang Hui on Feb 27

            //strUrl = BaseUrl + strUrl;

            //strScript += "window.open(\"" + strUrl + "\",strName,strFeatures);";

            //pageCurrent.RegisterStartupScript(
            //    System.Guid.NewGuid().ToString(),
            //    "<script language='javascript'>" + strScript + "</script>"
            //    );


            //pageCurrent.RegisterStartupScript(
            //    System.Guid.NewGuid().ToString(),
            //    "<script language='javascript'>" + strScript + "</script>"
            //    );
            #endregion

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"myleft={0};mytop={1};", intLeft.ToString(), intTop.ToString());
            sb.AppendLine();
            sb.AppendFormat(@"settings='top=' + mytop + ',left=' + myleft + ',width={0},height={1},location=no,directories=no,menubar=no,toolbar=no,status=no,scrollbars=no,resizable=no,fullscreen=no';",
                             intWidth.ToString(), intHeight.ToString());
            sb.AppendLine();
            sb.AppendFormat(@"window.open('{0}','{1}', settings);", strUrl, WinName);

            pageCurrent.ClientScript.RegisterStartupScript(pageCurrent.GetType(),
                    System.Guid.NewGuid().ToString(),
                    "<script language='javascript'>" + sb.ToString() + "</script>"
                    );

        }
        #endregion

        #region CloseWindows 关闭窗体
        /// <summary>
        /// 关闭窗体,没有任何提示的关闭窗?
        /// </summary>
        /// <param name="pageCurrent"></param>
        public static void CloseWindows(System.Web.UI.Page pageCurrent)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<script>window.opener=null;window.close();</script>");
            pageCurrent.ClientScript.RegisterStartupScript(pageCurrent.GetType(),
                System.Guid.NewGuid().ToString(), sb.ToString());
        }



        /// <summary>
        /// 有提示信息的关闭窗体
        /// </summary>
        /// <param name="pageCurrent"></param>
        /// <returns></returns>
        public static void CloseWindows(System.Web.UI.Page pageCurrent, string strMessage)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<script>if(confirm(\"" + strMessage + "\")==true){window.close();}</script>");
            pageCurrent.ClientScript.RegisterStartupScript(pageCurrent.GetType(),
                                System.Guid.NewGuid().ToString(), sb.ToString());
        }
        /// <summary>
        /// 有等待时间的关闭窗体
        /// </summary>
        /// <param name="pageCurrent"></param>
        /// <param name="WaitTime">等待时间，以毫秒为记量单?/param>
        public static void CloseWindows(System.Web.UI.Page pageCurrent, int WaitTime)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<script language=\"javascript\">");
            //加入此行功能后没有提提示功能
            sb.Append("window.opener=null;");
            sb.Append("setTimeout");
            sb.Append("(");
            sb.Append("'");
            sb.Append("window.close()");
            sb.Append("'");

            sb.Append(",");
            sb.Append(WaitTime.ToString());
            sb.Append(")");
            sb.Append("</script>");
            pageCurrent.ClientScript.RegisterStartupScript(pageCurrent.GetType(),
                        System.Guid.NewGuid().ToString(), sb.ToString());

        }
        #endregion

        #region 显示一段自定义的输出代码
        /**/
        /// <summary>
        /// 显示一段自定义的输出代码
        /// </summary>
        /// <param name="page">页面指针,一般为This</param>
        public static void ResponseScript(System.Web.UI.Page page, string script)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<script language=\"javascript\"> \n");
            sb.Append(script.Trim());
            sb.Append("</script>");
            page.Response.Write(sb.ToString());
        }
        #endregion

        #region 调用客户端JavaScript函数
        /// <summary>
        /// 调用客户端JavaScript函数
        /// </summary>
        /// <param name="page">页面指针,一般为This</param>
        /// <param name="scriptName">JS函数</param>
        public static void CallClientScript(System.Web.UI.Page page, string scriptName)
        {
            String csname = "PopupScript";
            Type cstype = page.GetType();
            System.Web.UI.ClientScriptManager cs = page.ClientScript;
            if (!cs.IsStartupScriptRegistered(cstype, csname))
            {
                String cstext = scriptName;
                cs.RegisterStartupScript(cstype, csname, cstext, true);
            }
        }
        #endregion

        #region 弹出对话框不影响css样式)
        /// <summary>
        /// 弹出对话?不影响css样式)
        /// </summary>
        /// <param name="page">页面指针,一般为this</param>
        /// <param name="scriptKey">脚本唯一键值</param>
        /// <param name="message">提示信息</param>
        public static void ShowMessage(System.Web.UI.Page page, string scriptKey, string message)
        {
            System.Web.UI.ClientScriptManager csm = page.ClientScript;
            if (!csm.IsClientScriptBlockRegistered(scriptKey))
            {
                string strScript = "alert('" + message + "');";
                csm.RegisterClientScriptBlock(page.GetType(), scriptKey, strScript, true);
            }
        }
        #endregion

        #region 为控件添加确认提示对话框
        /// <summary>
        /// 为控件添加确认提示对话框
        /// </summary>
        /// <param name="Control">需要添加提示的对话框</param>
        /// <param name="message">提示信息</param>
        public static void ShowConfirm(System.Web.UI.WebControls.WebControl Control, string message)
        {
            Control.Attributes.Add("onclick", "return confirm('" + message + "');");
        }
        #endregion

        #region 显示一个弹出窗口，并转向目标页(导航)
        /**/
        /// <summary>
        /// 显示一个弹出窗口，并转向目标页(导航)
        /// </summary>
        public static void ShowAndRedirect(string message, string url, Page page)
        {
            string js = "<script language=javascript>alert('{0}');window.location.replace('{1}')</script>";
            //HttpContext.Current.Response.Write(string.Format(js, message, toURL));
            if (!page.ClientScript.IsStartupScriptRegistered(page.GetType(), "ShowAndRedirect"))
            {
                page.ClientScript.RegisterStartupScript(page.GetType(), "ShowAndRedirect", string.Format(js, message, url));
            }

        }
        #endregion

        #region 显示一个弹出窗口，重新加载当前页
        /// <summary>
        /// 显示一个弹出窗口，重新加载当前页
        /// </summary>
        public static void ShowAndReLoad(string message)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<script language=\"javascript\"> \n");
            sb.Append("alert(\"" + message.Trim() + "\"); \n");
            sb.Append("window.location.href=window.location.href;\n");
            sb.Append("</script>");

            System.Web.HttpContext.Current.Response.Write(sb.ToString());
        }
        #endregion

        #region 显示一个弹出窗口并关闭当前页
        /// <summary>
        /// 显示一个弹出窗口并关闭当前页
        /// </summary>
        /// <param name="message"></param>
        public static void ShowAndClose(string message)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("<script language=\"javascript\">\n");
            sb.Append("alert(\"" + message.Trim() + "\"); \n");
            sb.Append("window.close();\n");
            sb.Append("</script>\n");
            System.Web.HttpContext.Current.Response.Write(sb.ToString());
        }
        #endregion

        #region 显示一个弹出窗口并转向上一页
        /// <summary>
        /// 显示一个弹出窗口并转向上一页
        /// </summary>
        /// <param name="message"></param>
        public static void ShowPre(string message)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<script language=\"javascript\"> \n");
            sb.Append("alert(\"" + message.Trim() + "\"); \n");
            sb.Append("var p=document.referrer; \n");
            sb.Append("window.location.href=p;\n");
            sb.Append("</script>");

            System.Web.HttpContext.Current.Response.Write(sb.ToString());
        }
        #endregion

        #region 页面重载
        /// <summary>
        /// 页面重载
        /// </summary>
        public static void ReLoad()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<script language=\"javascript\"> \n");
            sb.Append("window.location.href=window.location.href;");
            sb.Append("</script>");
            System.Web.HttpContext.Current.Response.Write(sb.ToString());

        }
        #endregion

        #region 重定向
        /// <summary>
        /// 重定向
        /// </summary>
        public static void Redirect(string url)
        {
            //string path = "http://" + System.Web.HttpContext.Current.Request.Url.Host + ":" + System.Web.HttpContext.Current.Request.Url.Port + url;
            string path = "http://" + System.Web.HttpContext.Current.Request.Url.Host + url;
            StringBuilder sb = new StringBuilder();
            sb.Append("<script language=\"javascript\"> \n");
            sb.Append(string.Format("window.location.href='{0}';", @path.Replace("'", "")));
            sb.Append("</script>");

            System.Web.HttpContext.Current.Response.Write(sb.ToString());
        }
        #endregion

        #region 提示信息并返回原页面
        /// <summary>
        /// 提示信息并返回原页面
        /// </summary>
        /// <param name="text"></param>
        public static void show(string text)
        {
            HttpContext.Current.Response.Write("<script language='javascript'>alert('" + text + "');window.history.back();</script>");
            HttpContext.Current.Response.End();
        }
        #endregion

        #region 不提示信息只返回原页面
        /// <summary>
        /// 不提示信息只返回原页面
        /// </summary>
        public static void Up()
        {
            HttpContext.Current.Response.Write("<script language='javascript'>window.history.go(-1);</script>");
        }
        #endregion

        #region "刷新本页面"
        /// <summary>
        /// refreshPage
        /// </summary>
        /// <param name="pageCurrent"></param>
        public static void refreshPage(System.Web.UI.Page pageCurrent)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<script language=\"javascript\">");
            sb.Append("window.location.reload(true);");
            sb.Append("</script>");
            pageCurrent.ClientScript.RegisterStartupScript(pageCurrent.GetType(),
                        System.Guid.NewGuid().ToString(), sb.ToString());

        }
        #endregion

        /*网页进度条类*/
        #region "网页进度条类"
        /// <summary>
        /// 网页进度条
        /// </summary>
        public class HtmlProgressBar
        {
            /// <summary>
            /// 进度条的初始化
            /// </summary>
            public static void Start()
            {
                Start("正在加载...");
            }
            /// <summary>
            /// 进度条的初始化
            /// </summary>
            /// <param name="msg">最开始显示的信息</param>
            public static void Start(string msg)
            {
                string s = "<html xmlns=\"http://www.w3.org/1999/xhtml\">\r\n<head>\r\n<title></title>\r\n<style>body {text-align:center;margin-top: 50px;}#ProgressBarSide {height:25px;border:1px #2F2F2F;width:65%;background:#EEFAFF;}</style><script language=\"javascript\">\r\n";
                s += "function SetPorgressBar(msg, pos)\r\n";
                s += "{\r\n";
                s += "document.getElementById('ProgressBar').style.width = pos + \"%\";\r\n";
                s += "WriteText('Msg1',msg + \" 已完成\" + pos + \"%\");\r\n";
                s += "}\r\n";
                s += "function SetCompleted(msg)\r\n{\r\nif(msg==\"\")\r\nWriteText(\"Msg1\",\"完成。\");\r\n";
                s += "else\r\nWriteText(\"Msg1\",msg);\r\n}\r\n";
                s += "function WriteText(id, str)\r\n";
                s += "{\r\n";
                s += "var strTag = '<span style=\"font-family:Verdana, Arial, Helvetica;font-size=11.5px;color:#DD5800\">' + str + '</span>';\r\n";
                s += "document.getElementById(id).innerHTML = strTag;\r\n";
                s += "}\r\n";
                s += "</script>\r\n</head>\r\n<body>\r\n";
                s += "<div id=\"Msg1\"><span style=\"font-family:Verdana, Arial, Helvetica;font-size=11.5px;color:#DD5800\">" + msg + "</span></div>\r\n";
                s += "<div id=\"ProgressBarSide\" align=\"left\" style=\"color:Silver;border-width:1px;border-style:Solid;\">\r\n";
                s += "<div id=\"ProgressBar\" style=\"background-color:#008BCE; height:25px; width:0%;color:#fff;\"></div>\r\n";
                s += "</div>\r\n</body>\r\n</html>\r\n";
                HttpContext.Current.Response.Write(s);
                HttpContext.Current.Response.Flush();
            }
            /// <summary>
            /// 滚动进度条
            /// </summary>
            /// <param name="Msg">在进度条上方显示的信息</param>
            /// <param name="Pos">显示进度的百分比数字</param>
            public static void Roll(string Msg, int Pos)
            {
                string jsBlock = "<script language=\"javascript\">SetPorgressBar('" + Msg + "'," + Pos + ");</script>";
                HttpContext.Current.Response.Write(jsBlock);
                HttpContext.Current.Response.Flush();
            }
        }
        #endregion

        #region "为页面添加脚本script"
        /// <summary>
        /// 添加脚本script
        /// </summary>
        /// <param name="pageCurrent"></param>
        /// <param name="strScript">脚本内容</param>
        public static void AddScript(Page pageCurrent, string strScript)
        {
            //Replace \n
            strScript = strScript.Replace("\n", "\\n");
            //Replace \r
            strScript = strScript.Replace("\r", "\\r");


            pageCurrent.ClientScript.RegisterStartupScript(pageCurrent.GetType(),
                System.Guid.NewGuid().ToString(),
                "<script>" + strScript + "</script>"
                );
        }
        #endregion

        #region 脚本提示信息,并且跳转到最上层框架
        /// <summary>
        /// 脚本提示信息
        /// </summary>
        /// <param name="Msg">信息内容,可以为空,为空表示不出现提示窗口</param>
        /// <param name="Url">跳转地址</param>
        public static string Hint(string Msg, string Url)
        {
            System.Text.StringBuilder rStr = new System.Text.StringBuilder();

            rStr.Append("<script language='javascript'>");
            if (Msg != "")
                rStr.Append("	alert('" + Msg + "');");

            if (Url != "")
                rStr.Append("	window.top.location.href = '" + Url + "';");

            rStr.Append("</script>");

            return rStr.ToString();
        }
        #endregion

        #region 脚本提示信息,并且跳转到当前框架内
        /// <summary>
        /// 脚本提示信息
        /// </summary>
        /// <param name="Msg">信息内容,可以为空,为空表示不出现提示窗口</param>
        /// <param name="Url">跳转地址,自已可以写入脚本</param>
        /// <returns></returns>
        public static string LocalHintJs(string Msg, string Url)
        {
            System.Text.StringBuilder rStr = new System.Text.StringBuilder();

            rStr.Append("<script language='JavaScript'>\n");
            if (Msg != "")
                rStr.AppendFormat("	alert('{0}');\n", Msg);

            if (Url != "")
                rStr.Append(Url + "\n");
            rStr.Append("</script>");

            return rStr.ToString();
        }

        #endregion

        #region 脚本提示信息,并且跳转到当前框架内,地址为空时,返回上页
        /// <summary>
        /// 脚本提示信息
        /// </summary>
        /// <param name="Msg"></param>
        /// <param name="Url"></param>
        /// <returns></returns>
        public static string LocalHint(string Msg, string Url)
        {
            System.Text.StringBuilder rStr = new System.Text.StringBuilder();

            rStr.Append("<script language='JavaScript'>\n");
            if (Msg != "")
                rStr.AppendFormat("	alert('{0}');\n", Msg);

            if (Url != "")
                rStr.AppendFormat("	window.location.href = '" + Url + "';\n");
            else
                rStr.AppendFormat(" window.history.back();");

            rStr.Append("</script>\n");

            return rStr.ToString();
        }
        #endregion

    }

}

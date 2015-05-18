using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ComLib.LogLib;
using System.Net;

namespace ComLib.Exceptions
{
    public class ErrorManagerWebDefault : IErrorManager
    {
        protected string ServerIP
        {
            get
            {
                string s = "";
                System.Net.IPAddress[] addressList = Dns.GetHostByName(Dns.GetHostName()).AddressList;
                for (int i = 0; i < addressList.Length; i++)
                {
                    s += addressList[i].ToString();
                }
                return s;
            }
        }

        #region IErrorManager Members

        public void Handle(string error, Exception exception)
        {
            //ProcessExeception(exception, "1", "111");
            ShowMessage(error);
        }

        public  string ProcessExeception(Exception ex, string strUserId, string strErrorCode, params string[] strExtention)
        {
            int intLogNum = -1;

            // 判断处理的异常是否是已经被处理过的异常，避免重复处理
            // bool IsNewException = ex.GetType() != Type.GetType("ExManagement.Handler.CommEx, ZTE.PDM.PUB.Util");

            return ex.Message + "\r\n定位信息：" + ex.StackTrace;


        }

        #endregion

        public void ShowMessage(string strMessage)
        {
            string sPath = System.Web.HttpContext.Current.Request.Url.Authority.ToLower();
            string mRootPath = sPath.Substring(0, sPath.IndexOf("/", 1) + 1);

            string sUrl = "http://"+ sPath + "/message.aspx?";
            //string sUrl = "http://localhost:3227/" + "message.aspx?";
            strMessage = strMessage.Replace("\r", "");
            strMessage = strMessage.Replace("\n", "");
            string strmsg = @"<script language='javascript'>
                       function utfurlcode(src)
				{
					//编码
					var strRet, I, innerCode, H4, M6, L6;
					strRet = '';
					for(I = 0; I < src.length; I++)
					{
						innerCode = src.charCodeAt(I);
						if(innerCode < 0)
						{
							innerCode += 0x10000;
						}
						if(innerCode < 0x80)
						{
							strRet += src.charAt(I);
						}
						else
						{
							H4 = 0xe0 + ((innerCode & 0xf000) >> 12);
							M6 = 0x80 + ((innerCode & 0xfc0) >> 6);
							L6 = 0x80 + (innerCode & 0x3f);
							strRet += '%' + H4.toString(16) + '%' + M6.toString(16) + '%' + L6.toString(16);
						}
					}
					return strRet;
				}
					//内部弹出模态窗体公用函数
									function pdmmsgbox(btntype,msg)
									{
										//alert(msg);
										msg=msg.replace(""<"",""["");
										msg=msg.replace("">"",""]"");
										msg=utfurlcode(msg);
										var tmp='btntype=' + btntype + '&msg=' + msg ;
										path='" + sUrl + @"' + tmp;
										//if (getHTTPData(path).indexOf('系统提示') ==-1 )
										//{
										//	path='../../Message.aspx?' + tmp;
										//}
										var l=(screen.width-350)/2;
										var t=(screen.height-200)/2;
										var Rv=showModalDialog(path,'','status:no;dialogHeight: 200px; dialogWidth: 350px; dialogTop: ' + t + 'px; dialogLeft: ' + l + 'px');
										return Rv;
									}
							
							var Rv=pdmmsgbox(4,'" + strMessage + @"');
							</script>";
            System.Web.HttpContext.Current.Response.Write(strmsg);
        }

        public void ShowMessage(string strMessage, MessageBoxType MessageType)
        {
            if (MessageType == MessageBoxType.Information)
            {
                string sPath = System.Web.HttpContext.Current.Request.Path.ToLower();
                string mRootPath = sPath.Substring(0, sPath.IndexOf("/", 1) + 1);

                string sUrl = "http://" + this.ServerIP + mRootPath + "Message.aspx?";
                strMessage = strMessage.Replace("\r", "");
                strMessage = strMessage.Replace("\n", "");
                string strmsg = @"<script language='javascript'>
                       function utfurlcode(src)
				{
					//编码
					var strRet, I, innerCode, H4, M6, L6;
					strRet = '';
					for(I = 0; I < src.length; I++)
					{
						innerCode = src.charCodeAt(I);
						if(innerCode < 0)
						{
							innerCode += 0x10000;
						}
						if(innerCode < 0x80)
						{
							strRet += src.charAt(I);
						}
						else
						{
							H4 = 0xe0 + ((innerCode & 0xf000) >> 12);
							M6 = 0x80 + ((innerCode & 0xfc0) >> 6);
							L6 = 0x80 + (innerCode & 0x3f);
							strRet += '%' + H4.toString(16) + '%' + M6.toString(16) + '%' + L6.toString(16);
						}
					}
					return strRet;
				}
					//内部弹出模态窗体公用函数
									function pdmmsgbox(btntype,msg)
									{
										//alert(msg);
										msg=msg.replace(""<"",""["");
										msg=msg.replace("">"",""]"");
										msg=utfurlcode(msg);
										var tmp='btntype=' + btntype + '&msg=' + msg ;
										path='" + sUrl + @"' + tmp;
										//if (getHTTPData(path).indexOf('系统提示') ==-1 )
										//{
										//	path='../../Message.aspx?' + tmp;
										//}
										var l=(screen.width-350)/2;
										var t=(screen.height-200)/2;
										var Rv=showModalDialog(path,'','status:no;dialogHeight: 200px; dialogWidth: 350px; dialogTop: ' + t + 'px; dialogLeft: ' + l + 'px');
										return Rv;
									}
							
							var Rv=pdmmsgbox(3,'" + strMessage + @"');
							</script>";
                System.Web.HttpContext.Current.Response.Write(strmsg);
            }
            else
            {
                ShowMessage(strMessage);
            }
        }

        public enum MessageBoxType { Error, Information };
    }


}

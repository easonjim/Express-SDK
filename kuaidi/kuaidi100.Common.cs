using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using ComLib;
using Newtonsoft.Json;

namespace kuaidi
{
    public class kuaidi100
    {
        /// <summary>
        /// 获取自动编号
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        private static string GetAutoNumber(string number)
        {
            try
            {
                ComLib.HttpItem item = new ComLib.HttpItem();
                item.URL = "http://www.kuaidi100.com/autonumber/auto?num=" + number;
                ComLib.HttpHelper request = new ComLib.HttpHelper();
                var html = request.GetHtml(item);
                if (html != null) return html.Html;
                else return "";
            }
            catch(Exception ex)
            {
                ComLib.LogLib.Log4NetBase.Log(ex);
                return "";
            }
        }

        /// <summary>
        /// 获取HTML格式快递信息
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string GetHtml(string number)
        {
            Random random = new Random();
            int rand = random.Next(1000000000, 2088730229);
            string nu = number;

            List<Dictionary<string, string>> retjson =
                JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(kuaidi100.GetAutoNumber(number));
            if (retjson.Count == 0)
            {
                
            }

            for (int i = 0; i < retjson.Count; i++)
            {
                string com = retjson[i]["comCode"];

                HttpItem item = new HttpItem();
                item.URL = string.Format("http://wx.kuaidi100.com/queryDetail.php?com={0}&nu={1}&rand={2}", com, nu,
                                         rand);
                HttpHelper httprequest = new HttpHelper();
                var html = httprequest.GetHtml(item);
                var strhtml = html.Html.Replace("href=\"css", "href=\"http://wx.kuaidi100.com/css")
                                  .Replace("src=\"/images", "src=\"http://wx.kuaidi100.com/images")
                                  .Replace(
                                      "<h1><img id=\"title\" src=\"http://wx.kuaidi100.com/images/bg_top.png\" alt=\"快递100\" /></h1>",
                                      "")
                                  .Replace(
                                      "<div style=\"width:320px;margin:0 auto;\">\r\n    <iframe src=\"http://www.kuaidi100.com/ad/js_ad.html\" width=\"320\" height=\"70\" scrolling=\"no\" frameborder=\"0\"></iframe>\r\n</div>\r\n<div class=\"copyright\">copyright2013&emsp;@快递100&emsp;kuaidi100.com</div>\r\n<script>\r\n\tif(document.documentElement.clientWidth < 640){\r\n\t\tdocument.getElementById(\"title\").width = document.documentElement.clientWidth;\r\n\t}\r\n</script>\r\n<script type=\"text/javascript\" src=\"http://cdn.kuaidi100.com/js/share/count.js?version=201305161806\"></script>\r\n",
                                      "");
                if(!strhtml.Contains("单号无结果")) return strhtml;
            }
            return "{\"status\":\"201\",\"message\":\"错误的单号\"}";
        }

        /// <summary>
        /// 获取JSON格式快递信息
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string GetJson(string number)
        {
            Random random = new Random();
            int rand = random.Next(1000000000, 2088730229);
            string nu = number;

            List<Dictionary<string, string>> retjson =
                JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(kuaidi100.GetAutoNumber(number));
            if (retjson.Count == 0)
            {
                return "{\"status\":\"201\",\"message\":\"错误的单号\"}";
            }

            for (int i = 0; i < retjson.Count; i++)
            {
                string com = retjson[i]["comCode"];

                HttpItem item = new HttpItem();
                item.URL = string.Format("http://www.kuaidi100.com/query?type={0}&postid={1}&id=1&valicode=&temp={2}",
                                         com, nu, rand);
                HttpHelper httprequest = new HttpHelper();
                var html = httprequest.GetHtml(item);
                var strjson = html.Html;
                Dictionary<string, object> tempretjson = JsonConvert.DeserializeObject<Dictionary<string, object>>(strjson);
                if(tempretjson["status"].ToString().Equals("200")) return strjson;
            }
            return "{\"status\":\"201\",\"message\":\"错误的单号\"}";
        }

        /// <summary>
        /// 获取快递名称
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetName(string name)
        {
            HttpItem item = new HttpItem();
            item.URL = "http://www.kuaidi100.com/auto.shtml";
            HttpHelper httprequest = new HttpHelper();
            var html = httprequest.GetHtml(item);
            var strhtml = html.Html;
            var retstr = Regex.Match(strhtml, string.Format("<a data-code=\"{0}\">([\u4e00-\u9fa5]+)</a>", name)).Groups[1].Value;
            if (retstr.IsNullOrEmpty())
            {
                return "{\"status\":\"201\",\"message\":\"错误的名称\"}";
            }
            else
            {
                return "{\"status\":\"201\",\"message\":\"ok\",\"data\":\"" + retstr + "\"}";
            }
        }
    }
}
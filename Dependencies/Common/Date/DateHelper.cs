/************************************************************************************		                                    			*
 *      Description:																*
 *				时间的帮助类   													    *
 *      Author:																		*
 *				Jim												*
 *      Finish DateTime:															*
 *				2008-07-12														*
 *      History:																	*
 ***********************************************************************************/
using System;
namespace ComLib
{
    /// <summary>
    /// Date 操作常用类
    /// </summary>
    public class DateHelper
    {
        #region "上周1"
        public static string GetBeginDateOfLastWeek()
        {
            return DateTime.Now.AddDays(Convert.ToDouble((0 - Convert.ToInt16(DateTime.Now.DayOfWeek)))-7).ToShortDateString();

        }
        #endregion

        #region "上周末"
        public static string GetEndDateOfLastWeek()
        {
            return DateTime.Now.AddDays(Convert.ToDouble((6 - Convert.ToInt16(DateTime.Now.DayOfWeek)))-7).ToShortDateString();
        }
        #endregion

        #region "上月初"
        public static string GetBeginDateOfLastMoth()
        {
          return   DateTime.Parse(DateTime.Now.ToString("yyyy-MM-01")).AddMonths(-1).ToShortDateString();
        }
        #endregion

        #region "上月末"
        public static string GetEndDateOfLastMonh()
        {
            return DateTime.Parse(DateTime.Now.ToString("yyyy-MM-01")).AddDays(-1).ToShortDateString();
        }
        #endregion

        #region "得到这一周的第一天"
        /// <summary>
        /// 得到这一周的第一天
        /// </summary>
        /// <returns></returns>
        public static string GetBeginDateOfWeek()
        {
            //每一周是从周日始至周六止
            return DateTime.Now.AddDays(Convert.ToDouble((0 - Convert.ToInt16(DateTime.Now.DayOfWeek)))).ToShortDateString();
        }
        #endregion

        #region "得到这一周的最后一天"
        public static string GetEndDateOfWeek()
        {
            return DateTime.Now.AddDays(Convert.ToDouble((6 - Convert.ToInt16(DateTime.Now.DayOfWeek)))).ToShortDateString();
        }
        #endregion

        #region "得到这个月的第一天"
        /// <summary>
        /// 得到一个月的第一天
        /// </summary>
        /// <returns></returns>
        public static string GetBeginDateOfMoth()
        {
            return DateTime.Now.ToString("yyyy-MM-01");
        }
        #endregion

        #region "得到这个月的最后一天"
        /// <summary>
        /// 得到这个月的最后一天
        /// </summary>
        /// <returns></returns>
        public static string GetEndDateOfMoth()
        {
            //最后一天就是下个月一号再减一?
            return DateTime.Parse(DateTime.Now.ToString("yyyy-MM-01")).AddMonths(1).AddDays(-1).ToShortDateString();
        }
        #endregion

        #region "得到这个季度的第一天"
        public static string GetBeginDateOfQuarter()
        {
            //首先先把日期推到本季度第一个月，然后这个月的第一天就是本季度的第一天了
            return DateTime.Now.AddMonths(0 - ((DateTime.Now.Month - 1) % 3)).ToString("yyyy-MM-01");
        }
        #endregion

        #region "得到这个季度的最后一天"
        public static string GetEndDateQuarter()
        {
            //同理，本季度的最后一天就是下季度的第一天减一
            return DateTime.Parse(DateTime.Now.AddMonths(3 - ((DateTime.Now.Month - 1) % 3)).ToString("yyyy-MM-01")).AddDays(-1).ToShortDateString();
        }
        #endregion

        #region "把秒转换成分钟"
        /// <summary>
        /// 把秒转换成分钟
        /// </summary>
        /// <returns></returns>
        public static int SecondToMinute(int Second)
        {
            decimal mm = (decimal)((decimal)Second / (decimal)60);
            return Convert.ToInt32(Math.Ceiling(mm));
        }
        #endregion

        #region "返回某年某月最后一天"
        /// <summary>
        /// 返回某年某月最后一天
        /// </summary>
        /// <param name="year">年份</param>
        /// <param name="month">月份</param>
        /// <returns>日</returns>
        public static int GetMonthLastDate(int year, int month)
        {
            DateTime lastDay = new DateTime(year, month, new System.Globalization.GregorianCalendar().GetDaysInMonth(year, month));
            int Day = lastDay.Day;
            return Day;
        }
        #endregion

        #region "返回时间差格式 用于feed"
        public static string DateDiff(DateTime DateTime1, DateTime DateTime2)
        {
            string dateDiff = null;
            try
            {
                TimeSpan ts = DateTime2 - DateTime1;
                if (ts.Days >= 1)
                {
                    dateDiff = DateTime1.Month.ToString() + "月" + DateTime1.Day.ToString() + "日";
                }
                else
                {
                    if (ts.Hours > 1)
                    {
                        dateDiff = ts.Hours.ToString() + "小时前";
                    }
                    else
                    {
                        dateDiff = ts.Minutes.ToString() + "分钟前";
                    }
                }
            }
            catch
            { }
            return dateDiff;
        }
        #endregion

        #region "获得当前时间 yyyy-MM-dd HH:mm:ss 格式"
        /// <summary>
        /// 获得当前时间
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetNow(string type)
        {
            if (type == "date")
            {
                return DateTime.Now.ToShortDateString();
            }
            else
            {
                return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }
        #endregion
    }
}

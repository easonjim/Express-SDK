using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    /// <summary>
    /// 一些INT的扩展方法
    /// </summary>
    public static class IntegerExtensions
    {
        #region Loops
        public static void Times(this int ndx, Action<int> action)
        {
            for (int i = 0; i < ndx; i++)
            {
                action(i);
            }
        }


        public static void Upto(this int start, int end, Action<int> action)
        {
            for (int i = start; i <= end; i++)
            {
                action(i);
            }
        }


        public static void Downto(this int end, int start, Action<int> action)
        {
            for (int i = end; i >= start; i--)
            {
                action(i);
            }
        }
        #endregion

        #region Math
        public static bool IsOdd(this int num)
        {
            return num % 2 != 0;
        }


        public static bool IsEven(this int num)
        {
            return num % 2 == 0;
        }
        #endregion

        #region Dates
        public static bool IsLeapYear(this int year)
        {
            return year % 4 == 0 && (year % 100 != 0 || year % 400 == 0);
        }


        public static DateTime DaysAgo(this int num)
        {
            return DateTime.Now.AddDays(-num);
        }


        public static DateTime MonthsAgo(this int num)
        {
            return DateTime.Now.AddMonths(-num);
        }


        public static DateTime YearsAgo(this int num)
        {
            return DateTime.Now.AddYears(-num);
        }


        public static DateTime HoursAgo(this int num)
        {
            return DateTime.Now.AddHours(-num);
        }


        public static DateTime MinutesAgo(this int num)
        {
            return DateTime.Now.AddMinutes(-num);
        }


        public static DateTime DaysFromNow(this int num)
        {
            return DateTime.Now.AddDays(num);
        }


        public static DateTime MonthsFromNow(this int num)
        {
            return DateTime.Now.AddMonths(num);
        }


        public static DateTime YearsFromNow(this int num)
        {
            return DateTime.Now.AddYears(num);
        }


        public static DateTime HoursFromNow(this int num)
        {
            return DateTime.Now.AddHours(num);
        }


        public static DateTime MinutesFromNow(this int num)
        {
            return DateTime.Now.AddMinutes(num);
        }
        #endregion
    }
}

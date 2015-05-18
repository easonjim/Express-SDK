using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace System
{
    public static class CommonExtensions
    {
        /// <summary>
        /// 判断DataTable是否NUll或者Count<=0
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static bool IsNullOrCountLTE0(this DataTable dt)
        {
            if (dt == null || dt.Rows.Count <= 0) return true;
            return false;
        }
      

        /// <summary>
        /// 判断string IsNullOrEmpty
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string str)
        {
           
            if (string.IsNullOrEmpty(str)) return true;
            return false;
        }

        public static bool IsNullOrCountLTE0<T>(this IEnumerable<T> list)
        {
            if (list == null || list.Count() <= 0) return true;
            return false;
        }

        public  static  bool  IsNullOrTableCountLTE0(this DataSet ds)
        {
            if (ds == null || ds.Tables.Count <= 0) return true;
            return false;
        }

    }
}

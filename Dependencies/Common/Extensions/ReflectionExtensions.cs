using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ComLib;
using System.Data;

namespace System
{
    /// <summary>
    /// DataTable 直接转换成对象的扩展方法
    /// </summary>
   public static class DataTableExtensions
    {
       public static List<TResult> ToList<TResult>(this DataTable dt) where TResult : class, new()
       {
           List<TResult> list = new List<TResult>();
           if (dt == null) return list;
           DataTableEntityBuilder<TResult> eblist = DataTableEntityBuilder<TResult>.CreateBuilder(dt.Rows[0]);
           foreach (DataRow info in dt.Rows) list.Add(eblist.Build(info));
           dt.Dispose(); dt = null;
           return list;
       }

       public static List<TResult> ToList<TResult>(this IDataReader dr, bool isClose) where TResult : class, new()
       {
           IDataReaderEntityBuilder<TResult> eblist = IDataReaderEntityBuilder<TResult>.CreateBuilder(dr);
           List<TResult> list = new List<TResult>();
           if (dr == null) return list;
           while (dr.Read()) list.Add(eblist.Build(dr));
           if (isClose) { dr.Close(); dr.Dispose(); dr = null; }
           return list;
       }
       public static List<TResult> ToList<TResult>(this IDataReader dr) where TResult : class, new()
       {
           return dr.ToList<TResult>(true);
       }

       /// <summary>
       /// DataRow扩展方法：将DataRow类型转化为指定类型的实体
       /// </summary>
       /// <typeparam name="T">实体类型</typeparam>
       /// <returns></returns>
       public static T ToModel<T>(this DataRow dr) where T : class, new()
       {
           if (dr != null)
               return ToList<T>(dr.Table).First();

           return null;
       }
     

   
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
   public static class DateTimeExtension
    {
       public static DateTime Get_MinDate(this DateTime dt) {
           if (dt == null) return default(DateTime);

           return new DateTime(dt.Year, dt.Month, dt.Day,
                           0, 00, 01); 
       }

       public static DateTime Get_MaxDate(this DateTime dt) {
           if (dt == null) return default(DateTime);
           return new DateTime(dt.Year, dt.Month, dt.Day,
                      23, 59, 59);
       }
            
    }
}

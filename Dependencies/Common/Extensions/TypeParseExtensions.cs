/************************************************************************************		                                    			*
 *      Description:																*
 *				类型转换的扩展方法   													    *
 *      Author:																		*
 *				Jim												*
 *      Finish DateTime:															*
 *				2010													*
 *      History:																	*
 ***********************************************************************************/
namespace System
{
    /// <summary>
    /// 类型转换的扩展方法
    /// </summary>
    public static class TypeParse
    {
        /// <summary>
        /// 类型转换的扩展方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static T ConvertTo<T>(this object input)
        {
            object result = default(T);
            if (input == null || input == DBNull.Value)
            {
                if (typeof(T) == typeof(DateTime))
                    result = new DateTime(1900, 01, 01, 01, 01, 01);
                return (T)result;
            }
            try
            {
                if (typeof(T) == typeof(int))
                    result = System.Convert.ToInt32(input);
                else if (typeof(T) == typeof(long))
                    result = System.Convert.ToInt64(input);
                else if (typeof(T) == typeof(string))
                    result = System.Convert.ToString(input);
                else if (typeof(T) == typeof(bool))
                    result = System.Convert.ToBoolean(input);
                else if (typeof(T) == typeof(double))
                    result = System.Convert.ToDouble(input);
                else if (typeof(T) == typeof(decimal))
                    result = System.Convert.ToDecimal(input);
                else if (typeof(T) == typeof(DateTime))
                    result = System.Convert.ToDateTime(input);
            }
            catch { }
            return (T)result;
        }



    }
}

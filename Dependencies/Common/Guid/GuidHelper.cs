using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComLib
{
    /// <summary>
    /// GUID
    /// </summary>
    public class GuidHelper
    {
        /// <summary>
        /// 生成纯ID,没有-
        /// </summary>
        /// <returns></returns>
        public static string GeneratePureID()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }

        /// <summary>
        /// 生成正规的GUID
        /// </summary>
        /// <returns></returns>
        public static string GenerateId()
        {
            return Guid.NewGuid().ToString();
        }

        /// <summary>
        /// 产生字符串：（例：49f949d735f5c79e）
        /// </summary>
        /// <returns></returns>
        public static string GenerateShortId()
        {

            long i = 1;

            foreach (byte b in Guid.NewGuid().ToByteArray())
            {

                i *= ((int)b + 1);

            }

            return string.Format("{0:x}", i - DateTime.Now.Ticks);

        }

        /// <summary>
        /// 产生Int64 类型：（例：4833055965497820814）
        /// </summary>
        /// <returns></returns>
        public static long GenerateLongId()
        {

            byte[] buffer = Guid.NewGuid().ToByteArray();

            return BitConverter.ToInt64(buffer, 0);

        }
    }
}

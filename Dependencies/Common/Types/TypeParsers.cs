/*
 * Author: Kishore Reddy
 * Url: http://commonlibrarynet.codeplex.com/
 * Title: CommonLibrary.NET
 * Copyright: � 2009 Kishore Reddy
 * License: LGPL License
 * LicenseUrl: http://commonlibrarynet.codeplex.com/license
 * Description: A C# based .NET 3.5 Open-Source collection of reusable components.
 * Usage: Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace ComLib.Types
{
    public class TypeParsers
    {
        /// <summary>
        /// Parse the string as an int.
        /// </summary>
        /// <param name="val"></param>
        /// <param name="defaultVal"></param>
        /// <returns></returns>
        public static int ParseInt(string val, int defaultVal)
        {
            if (string.IsNullOrEmpty(val)) return defaultVal;

            int convertedVal = 0;

            if (!int.TryParse(val, out convertedVal))
                convertedVal = defaultVal;

            return convertedVal;
        }

                
        /// <summary>
        /// Convert to correct type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static object Convert<T>(object input)
        {
            if (typeof(T) == typeof(int))
                return System.Convert.ToInt32(input);
            else if (typeof(T) == typeof(long))
                return System.Convert.ToInt64(input);
            else if (typeof(T) == typeof(string))
                return System.Convert.ToString(input);
            else if (typeof(T) == typeof(bool))
                return System.Convert.ToBoolean(input);
            else if (typeof(T) == typeof(double))
                return System.Convert.ToDouble(input);
            else if ( typeof(T) == typeof(DateTime))
                return System.Convert.ToDateTime(input);                
            
            return default(T);
        }


        /// <summary>
        /// Convert to correct type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static T ConvertTo<T>(object input)
        {
            object result = default(T);
            if (input == null || input == DBNull.Value) return (T)result;

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
            else if (typeof(T) == typeof(DateTime))
                result = System.Convert.ToDateTime(input);

            return (T)result;
        }


        /// <summary>
        /// Convert to correct type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static object ConvertTo(Type type, object input)
        {
            object result = null;
            if (input == null || input == DBNull.Value) return null;

            if (type == typeof(int))
                result = System.Convert.ToInt32(input);
            else if (type == typeof(long))
                result = System.Convert.ToInt64(input);
            else if (type == typeof(string))
                result = System.Convert.ToString(input);
            else if (type == typeof(bool))
                result = System.Convert.ToBoolean(input);
            else if (type == typeof(double))
                result = System.Convert.ToDouble(input);
            else if (type == typeof(DateTime))
                result = System.Convert.ToDateTime(input);

            return result;
        }
    }
}

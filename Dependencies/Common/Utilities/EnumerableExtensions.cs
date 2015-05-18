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
using System.Linq;
using System.Text;

namespace ComLib
{

    /// <summary>
    /// Extension methods for collections.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Execute action on each item in enumeration.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="action"></param>
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (var item in items)
                action(item);
        }


        /// <summary>
        /// Joins the specified items.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">The items.</param>
        /// <param name="delimeter">The delimeter.</param>
        /// <returns></returns>
        public static string Join<T>(this IList<T> items, string delimeter)
        {
            if (items == null || items.Count == 0)
                return string.Empty;

            if (items.Count == 1)
                return items[0].ToString();

            StringBuilder buffer = new StringBuilder();
            buffer.Append(items[0].ToString());

            for (int ndx = 1; ndx < items.Count; ndx++)
            {
                string append = items[ndx].ToString();
                buffer.Append(delimeter + append);
            }
            return buffer.ToString();
        }


        public static string JoinDelimited<T>(this IList<T> items, string delimeter, Func<T, string> appender)
        {
            if (items == null || items.Count == 0)
                return string.Empty;

            if (items.Count == 1)
                return appender(items[0]);

            StringBuilder buffer = new StringBuilder();
            buffer.Append(appender(items[0]));

            for(int ndx = 1; ndx < items.Count; ndx++)
            {
                T item = items[ndx];
                string append = appender(item);
                buffer.Append(delimeter + append);
            }
            return buffer.ToString();
        }


        public static string JoinDelimitedWithNewLine<T>(this IList<T> items, string delimeter, int newLineAfterCount, string newLineText, Func<T, string> appender)
        {
            if (items == null || items.Count == 0)
                return string.Empty;

            if (items.Count == 1)
                return appender(items[0]);

            StringBuilder buffer = new StringBuilder();
            buffer.Append(appender(items[0]));

            for (int ndx = 1; ndx < items.Count; ndx++)
            {
                T item = items[ndx];
                string append = appender(item);
                if (ndx % newLineAfterCount == 0)
                    buffer.Append(newLineText);

                buffer.Append(delimeter + append);
            }
            return buffer.ToString();
        }


        /// <summary>
        /// Converts an enumerable collection to an delimited string.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        public static string AsDelimited<T>(this IEnumerable<T> items, string delimiter)
        {
            List<string> itemList = new List<string>();
            foreach (T item in items)
            {
                itemList.Add(item.ToString());
            }
            return String.Join(delimiter, itemList.ToArray());
        }


        #region Conditional Checks
        /// <summary>
        /// Check for any nulls.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        public static bool HasAnyNulls<T>(this IEnumerable<T> items)
        {
            return IsTrueForAny<T>(items, t => t == null);
        }


        /// <summary>
        /// Check if any of the items in the collection satisfied by the condition.
        /// </summary>
        /// <param name="items"></param>
        /// <param name="executor"></param>
        /// <returns></returns>
        public static bool IsTrueForAny<T>(this IEnumerable<T> items, Func<T, bool> executor)
        {

            foreach (T item in items)
            {
                bool result = executor(item);
                if (result)
                    return true;
            }
            return false;
        }


        /// <summary>
        /// Check if all of the items in the collection satisfied by the condition.
        /// </summary>
        /// <param name="items"></param>
        /// <param name="executor"></param>
        /// <returns></returns>
        public static bool IsTrueForAll<T>(this IEnumerable<T> items, Func<T, bool> executor)
        {            
            foreach (T item in items)
            {
                bool result = executor(item);
                if (!result)
                    return false;
            }
            return true;
        }


        /// <summary>
        /// Check if all of the items in the collection satisfied by the condition.
        /// </summary>
        /// <param name="items"></param>
        /// <param name="executor"></param>
        /// <returns></returns>
        public static IDictionary<T, T> ToDictionary<T>(this IList<T> items)
        {
            IDictionary<T, T> dict = new Dictionary<T, T>();
            foreach (T item in items)
            {
                dict[item] = item;
            }
            return dict;
        }        
        #endregion
    }
}

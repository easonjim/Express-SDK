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
using System.Collections;


namespace ComLib
{
	/// <summary>
	/// Used for simple validations
	/// </summary>      
    public sealed class Guard
    {
       
    	/// <summary>
    	/// Check that the condition is true.
    	/// </summary>
    	/// <param name="expression"></param>
		public static void IsTrue(bool condition) 
		{
            if (condition == false) 
			{
                throw new ArgumentException("The condition supplied is false");
			}
		}
    	

        /// <summary>
        /// Check that the condition is true and return error message provided.
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="message"></param>
        public static void IsTrue(bool condition, String message)
        {
            if (!condition)
            {
                throw new ArgumentException(message);
            }
        }


        /// <summary>
        /// Check that the condition is false.
        /// </summary>
        /// <param name="expression"></param>
        public static void IsFalse(bool condition)
        {
            if (condition)
            {
                throw new ArgumentException("The condition supplied is true");
            }
        }


        /// <summary>
        /// Check that the condition is false and return error message provided.
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="message"></param>
        public static void IsFalse(bool condition, String message)
        {
            if (condition)
            {
                throw new ArgumentException(message);
            }
        }


        /// <summary>
        /// Check that the object supplied is not null and throw exception
        /// with message provided.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="message"></param>
        public static void IsNotNull(Object obj, string message)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(message);
            }
        }


        /// <summary>
        /// Check that the object provided is not null.
        /// </summary>
        /// <param name="obj"></param>
        public static void IsNotNull(Object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("The argument provided cannot be null.");
            }
        }


        /// <summary>
        /// Check that the object supplied is null and throw exception
        /// with message provided.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="message"></param>
        public static void IsNull(Object obj, string message)
        {
            if (obj != null)
            {
                throw new ArgumentNullException(message);
            }
        }


        /// <summary>
        /// Check that the object provided is null.
        /// </summary>
        /// <param name="obj"></param>
        public static void IsNull(Object obj)
        {
            if (obj != null)
            {
                throw new ArgumentNullException("The argument provided cannot be null.");
            }
        }
    }

   
}

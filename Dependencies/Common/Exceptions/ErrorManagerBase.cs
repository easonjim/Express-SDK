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
using ComLib;

using ComLib.ValidationSupport;
using ComLib.LogLib;


namespace ComLib.Exceptions
{
    /// <summary>
    /// Localized error manager.
    /// </summary>
    public class ErrorManagerBase : IErrorManager
    {
        protected string _name = string.Empty;


        #region IExceptionManager Members
        /// <summary>
        /// The name of this exception manager.
        /// </summary>
        public string Name { get { return _name; } }


        /// <summary>
        /// Handles the specified error.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="arguments">The arguments.</param>
        public virtual void Handle(string error, Exception exception)
        {
            InternalHandle(error, exception);
        }
        
        /// <summary>
        /// Internal method for handling errors.
        /// </summary>
        /// <param name="error"></param>
        /// <param name="exception"></param>
        /// <param name="handler"></param>
        /// <param name="errorResults"></param>
        /// <param name="arguments"></param>
        protected virtual void InternalHandle(string error, Exception exception)
        {
            Log4NetBase.Log(error, exception);
        }
        #endregion
    }
}

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


namespace ComLib.Exceptions
{
    /// <summary>
    /// Exception manager.
    /// </summary>
    public class ErrorManager
    {
        private static IErrorManager _provider;
        private static object _syncRoot = new object();
        private static IDictionary<string, IErrorManager> _namedHandlers;



        /// <summary>
        /// 
        /// </summary>
        static ErrorManager()
        {
            //需要改处理类 直接改这里就好了
            _provider = new ErrorManagerWebDefault();
            _namedHandlers = new Dictionary<string, IErrorManager>();
            _namedHandlers[string.Empty] = _provider;
        }


        /// <summary>
        /// Initialize the provider.
        /// </summary>
        /// <param name="provider"></param>
        public static void Init(IErrorManager provider)
        {
            lock (_syncRoot)
            {
                _provider = provider;
            }
        }



        /// <summary>
        /// Register an named exception handler.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="isDefault"></param>
        /// <param name="handler"></param>
        public static void Register(string name, bool isDefault, IErrorManager handler)
        {
            lock (_syncRoot)
            {
                if (isDefault)
                    _provider = handler;

                if (!string.IsNullOrEmpty(name))
                    _namedHandlers[name] = handler;
            }
        }


        #region IExceptionManager Members
        /// <summary>
        /// Handles the specified error.
        /// </summary>
        /// <param name="error">The error.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="arguments">The arguments.</param>
        public static void Handle(string error, Exception exception)
        {
            InternalHandle(error, exception);
        }




        /// <summary>
        /// Handles the specified error.
        /// </summary>
        /// <param name="error">The error.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="errors">The error results.</param>
        /// <param name="arguments">The arguments.</param>
        private static void InternalHandle(string error, Exception exception)
        {
            
                _provider.Handle(error, exception);
                return;

           // if (!_namedHandlers.ContainsKey(handler))
            //    throw new ArgumentException("Unknown exception handler : " + handler);

           // IErrorManager exceptionManager = _namedHandlers[handler];
           // exceptionManager.Handle(error, exception);
        }


        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComLib
{

    public interface IErrors
    {
        void Add(string error);
        void Add(string key, string error);
        void Clear();
        void CopyTo(IErrors errors);
        int Count { get; }
        void Each(Action<string, string> callback);
        void EachFull(Action<string> callback);
        IList<string> ErrorList { get; set; }
        IDictionary<string, string> ErrorMap { get; set; }
        string Message();
        string Message(string separator);
        bool HasAny { get; }
        string On(string key);
        IList<string> On();
    }



    /// <summary>
    /// Class to store errors both by key/value and non-key based errors.
    /// </summary>
    public class Errors : IErrors
    {
        private IDictionary<string, string> _errorMap;
        private IList<string> _errorList;


        /// <summary>
        /// Adds an error associated with the key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="error">The error.</param>
        public void Add(string key, string error)
        {
            if (_errorMap == null) _errorMap = new Dictionary<string, string>();

            // No key? add to list.
            if (string.IsNullOrEmpty(key))
                Add(error);
            else
                _errorMap[key] = error;
        }


        /// <summary>
        /// Adds the specified error.
        /// </summary>
        /// <param name="error">The error.</param>
        public void Add(string error)
        {
            if (_errorList == null) _errorList = new List<string>();
            _errorList.Add(error);
        }


        /// <summary>
        /// Iterates over the error map and calls the callback
        /// </summary>
        /// <param name="callback">The callback.</param>
        public void Each( Action<string, string> callback)
        {
            if (_errorMap == null) return;

            foreach (KeyValuePair<string, string> pair in _errorMap)
            {
                callback(pair.Key, pair.Value);
            }
        }


        /// <summary>
        /// Iterates over the error map and error list and calls the callback.
        /// Errormap entries are combined.
        /// </summary>
        /// <param name="callback">The callback.</param>
        public void EachFull( Action<string> callback)
        {
            if (_errorMap != null)
            {
                foreach (KeyValuePair<string, string> pair in _errorMap)
                {
                    string combined = pair.Key + " " + pair.Value;
                    callback(combined);
                }
            }

            if (_errorList != null)
            {
                foreach (string error in _errorList)
                    callback(error);
            }
        }


        /// <summary>
        /// Builds a full error message of error map and list using NewLine as a separator between errors.
        /// </summary>
        /// <returns></returns>
        public string Message()
        {
            return Message(Environment.NewLine);
        }


        /// <summary>
        /// Builds a complete error message using the supplied separator for each error.
        /// </summary>
        /// <returns></returns>
        public string Message(string separator)
        {
            StringBuilder buffer = new StringBuilder();
            if (_errorList != null)
            {
                foreach (string error in _errorList)
                    buffer.Append(error + separator);
            } 
            
            if (_errorMap != null)
            {
                foreach (KeyValuePair<string, string> pair in _errorMap)
                {
                    string combined = pair.Key + " " + pair.Value;
                    buffer.Append(combined + separator);
                }
            }
            
            return buffer.ToString();
        }


        /// <summary>
        /// Gets the number of errors.
        /// </summary>
        /// <value>The number of errors.</value>
        public int Count 
        {
            get 
            { 
                int errorCount = 0;
                if (_errorList != null) errorCount += _errorList.Count;
                if (_errorMap != null) errorCount += _errorMap.Count;

                return errorCount;
            }
        }


        /// <summary>
        /// Gets a value indicating whether this instance has any errors.
        /// </summary>
        /// <value><c>true</c> if this instance has any errors; otherwise, <c>false</c>.</value>
        public bool HasAny
        {
            get { return Count > 0; }
        }


        /// <summary>
        /// Clears all the errors.
        /// </summary>
        public void Clear()
        {
            if (_errorMap != null) _errorMap.Clear();
            if (_errorList != null) _errorList.Clear();
        }


        /// <summary>
        /// Gets the error on the specified error key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public string On(string key)
        {
            if (_errorMap != null && _errorMap.ContainsKey(key))
                return _errorMap[key];

            return string.Empty;
        }


        /// <summary>
        /// Gets all the errors
        /// </summary>
        /// <returns></returns>
        public IList<string> On()
        {
            if (_errorList == null) return null;

            // ? Should return a read-only list here?
            return _errorList;
        }


        /// <summary>
        /// Gets or sets the error list.
        /// </summary>
        /// <value>The error list.</value>
        public IList<string> ErrorList
        {
            get { return _errorList; }
            set { _errorList = value; }
        }


        /// <summary>
        /// Gets or sets the error map.
        /// </summary>
        /// <value>The error map.</value>
        public IDictionary<string, string> ErrorMap
        {
            get { return _errorMap; }
            set { _errorMap = value; }
        }


        /// <summary>
        /// Copies all errors from this instance over to the supplied instance.
        /// </summary>
        /// <param name="errors">The errors.</param>
        public void CopyTo(IErrors errors)
        {
            if (errors == null) return;

            if (_errorList != null)
                foreach (string error in _errorList)
                    errors.Add(error);

            if (_errorMap != null)
                foreach (KeyValuePair<string, string> pair in _errorMap)
                    errors.Add(pair.Key, pair.Value);
        }
    }
}

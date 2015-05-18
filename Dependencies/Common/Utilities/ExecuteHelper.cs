using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Reflection;
using ComLib.LogLib;


namespace ComLib
{

    public class Try
    {
        public static void DoLog(Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                Log4NetBase.Log(ex);
            }
        }
    }



    public class ExecuteHelper
    {
        /// <summary>
        /// Executes an action inside a try catch and does not do anything when
        /// an exception occurrs.
        /// </summary>
        /// <param name="action"></param>
        public static void TryCatch(Action action)
        {
            TryCatchLog(string.Empty, action);
        }


        /// <summary>
        /// Executes an action inside a try catch and logs any exceptions.
        /// </summary>
        /// <param name="action"></param>
        public static void TryCatchLog(string errorMessage,  Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                Log4NetBase.Log(errorMessage, ex);
            }
        }


        /// <summary>
        /// Executes an action inside a try catch and does not do anything when
        /// an exception occurrs.
        /// </summary>
        /// <param name="action"></param>
        public static void TryCatchHandle(Action actionCode)
        {
            TryCatchFinallySafe(string.Empty, actionCode, (ex) => HandleException(ex), null);
        }


        /// <summary>
        /// Executes an action inside a try catch and does not do anything when
        /// an exception occurrs.
        /// </summary>
        /// <param name="action"></param>
        public static void TryCatchHandle(Action actionCode, Action finallyCode)
        {
            TryCatchFinallySafe(string.Empty, actionCode, (ex) => HandleException(ex), finallyCode);
        }


        /// <summary>
        /// Executes an action inside a try catch and logs any exceptions.
        /// </summary>
        /// <param name="action"></param>
        public static void TryCatchFinallySafe(string errorMessage, Action action, Action<Exception> exceptionHandler, Action finallyHandler)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                if (exceptionHandler != null)
                    exceptionHandler(ex);
            }
            finally
            {
                if (finallyHandler != null)
                {
                    TryCatchHandle(() => finallyHandler());
                }
            }
        }


        /// <summary>
        /// Executes an action inside a try catch and logs any exceptions.
        /// </summary>
        /// <param name="action"></param>
        public static void TryCatch( Action action, Action<Exception> exceptionHandler)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                if (exceptionHandler != null)
                    exceptionHandler(ex);
            }            
        }


        /// <summary>
        /// Executes an action inside a try catch and logs any exceptions.
        /// </summary>
        /// <param name="action"></param>
        public static T TryCatchLog<T>(string errorMessage,  Func<T> action)
        {
            return TryCatchLogRethrow<T>(errorMessage, false, action);
        }


        /// <summary>
        /// Executes an action inside a try catch and logs any exceptions.
        /// </summary>
        /// <param name="action"></param>
        public static T TryCatchLogRethrow<T>(string errorMessage, bool rethrow, Func<T> action)
        {
            T result = default(T);
            try
            {
                result = action();
            }
            catch (Exception ex)
            {
                Log4NetBase.Log(errorMessage, ex);
                if( rethrow) throw ex;
            }
            return result;
        }



        /// <summary>
        /// Executes an action inside a try catch and logs any exceptions.
        /// </summary>
        /// <param name="action"></param>
        public static BoolMessageItem<T> TryCatchLogGet<T>(string errorMessage,  Func<T> action)
        {        
            T result = default(T);
            bool success = false;
            string message = string.Empty;
            try
            {
                result = action();
                success = true;
            }
            catch (Exception ex)
            {
                Log4NetBase.Log(errorMessage, ex);
                message = ex.Message;
            }
            return new BoolMessageItem<T>(result, success, message);
        }


        /// <summary>
        /// Executes an action inside a try catch and logs any exceptions.
        /// </summary>
        /// <param name="action"></param>
        public static BoolMessageItem TryCatchLogGetBoolMessageItem(string errorMsg, Func<BoolMessageItem> action)
        {
            BoolMessageItem result = BoolMessageItem.False;
            string fullMessage = string.Empty;
            try
            {
                result = action();
            }
            catch (Exception ex)
            {
                Log4NetBase.Log(errorMsg, ex);
                fullMessage = errorMsg + Environment.NewLine
                        + ex.Message + Environment.NewLine
                        + ex.Source + Environment.NewLine
                        + ex.StackTrace;
                result = new BoolMessageItem(null, false, fullMessage);
            }
            return result;
        }


        /// <summary>
        /// Handle the highest level application exception.
        /// </summary>
        /// <param name="ex"></param>
        public static void HandleException(Exception ex)
        {
            string message = string.Format("{0}, {1} \r\n {2}", ex.Message, ex.Source, ex.StackTrace);
            Console.WriteLine(message);
            try
            {
                Log4NetBase.Log("ÏµÍ³´íÎó HandleException£º", ex);
            }
            catch {}
        }


        /// <summary>
        /// Handle the highest level application exception.
        /// </summary>
        /// <param name="ex"></param>
        public static void HandleExceptionLight(Exception ex)
        {
            string message = string.Format("{0}, {1}", ex.Message, ex.Source);
            Console.WriteLine(message);
            try
            {
                Log4NetBase.Log(LogLevel.warn, "admin", "admin", ex);
            }
            catch {}
        }

       
    }
}

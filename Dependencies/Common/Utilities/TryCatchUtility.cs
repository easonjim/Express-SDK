
#region File Comment
// +-------------------------------------------------------------------------+
// + Copyright (C), xxx Co., Ltd.
// +-------------------------------------------------------------------------+
// + FileName:MyClass.aspx.cs
// +-------------------------------------------------------------------------+
// + Author:有容乃大   Version:1.0   Date:2010-06-01
// +-------------------------------------------------------------------------+
// + Description:
// +             MyClass定义
// +-------------------------------------------------------------------------+
// + History:
// +         Jim     20110402     修改所有的委托为Action传递
// +-------------------------------------------------------------------------+
#endregion
                

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Web;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml;

namespace ComLib
{   
    public static class TryCatchUtility
    {
        #region 封装一个重试机制

        /// <summary>
        /// 封装一个重试机制。
        /// </summary>
        /// <param name="retryTimes">重试次数</param>
        /// <param name="sleepTime">下一次重试时暂停时间(毫秒)</param>
        /// <param name="fun">主处理方法</param>
        public static void DoRetry(int retryTimes, int sleepTime, Action fun)
        {
            DoRetry(retryTimes, sleepTime, fun, null, null);
        }

        /// <summary>
        /// 封装一个重试机制。
        /// </summary>
        /// <param name="retryTimes">重试次数</param>
        /// <param name="sleepTime">下一次重试时暂停时间(毫秒)</param>
        /// <param name="fun">主处理方法</param>
        /// <param name="funError">当发生异常时的处理方法</param>
        public static void DoRetry(int retryTimes, int sleepTime, Action fun, Action<Exception> funError)
        {
            DoRetry(retryTimes, sleepTime, fun, funError, null);
        }

        /// <summary>
        /// 封装一个重试机制。
        /// </summary>
        /// <param name="retryTimes">重试次数</param>
        /// <param name="sleepTime">下一次重试时暂停时间(毫秒)</param>
        /// <param name="fun">主处理方法</param>
        /// <param name="funError">当发生异常时的处理方法</param>
        /// <param name="funErrorMaxRetry">重试达到最大值时仍然失败时的处理方法</param>
        public static void DoRetry(int retryTimes, int sleepTime, Action fun, Action<Exception> funError, Action<Exception> funErrorMaxRetry)
        {
            if (null == fun)
                return;

            for (int i = 0; i < retryTimes; ++i)
            {
                try
                {
                    fun();
                    break;
                }
                catch (Exception ex)
                {
                    if (i == retryTimes - 1)
                    {
                        if (null != funErrorMaxRetry)
                            funErrorMaxRetry(ex);
                        else
                            break;
                    }
                    else
                    {
                        if (null != funError)
                            funError(ex);

                        if (sleepTime > 0)
                            System.Threading.Thread.Sleep(sleepTime);
                    }
                }
            }
        }

        #endregion

        #region 封装一个Try、Catch过程

        /// <summary>
        /// 封装一个Try、Catch过程。
        /// </summary>
        /// <param name="fun">在Try块中执行的方法</param>
        public static void DoTryCatch(Action fun)
        {
            DoTryCatch(fun, null, null);
        }


        /// <summary>
        /// 封装一个Try、Catch过程。
        /// </summary>
        /// <param name="fun">在Try块中执行的方法</param>
        /// <param name="funError">在Catch块中执行的方法</param>
        public static void DoTryCatch(Action fun, Action<Exception> funError)
        {
            DoTryCatch(fun, funError, null);
        }

        /// <summary>
        /// 封装一个Try、Catch过程。
        /// </summary>
        /// <param name="fun">在Try块中执行的方法</param>
        /// <param name="funError">在Catch块中执行的方法</param>
        /// <param name="funFinally">在finally块中执行的方法</param>
        public static void DoTryCatch(Action fun, Action<Exception> funError, Action funFinally)
        {
            if (null == fun)
                return;

            try
            {
                fun();
            }
            catch (Exception ex)
            {
                if (null != funError)
                    funError(ex);
            }
            finally
            {
                if (null != funFinally)
                    funFinally();
            }
        }

        #endregion
    }
}

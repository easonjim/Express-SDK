using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComLib.LogLib
{
    public class LogEvent
    {
        /// <summary>
        /// The log level.
        /// </summary>
        public LogLevel Level{get;set;}


        /// <summary>
        /// 用户需要记录的指令:比如说-- 调试XX功能
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Name of the computer.
        /// </summary>
        public string Computer { get; set; }


        /// <summary>
        /// Create time.
        /// </summary>
        public DateTime CreateTime { get; set; }


        /// <summary>
        /// The name of the currently executing thread that created this log entry.
        /// </summary>
        public string ThreadName { get; set; }


        /// <summary>
        /// Name of the user.
        /// </summary>
        public string UserName { get; set; }


        /// <summary>
        /// The exception.
        /// </summary>
        public Exception Ex { get; set; }

        public string FinalMessage { get; set; }

    }

    /// <summary>
    /// Level for the logging.
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// 重大的
        /// </summary>
        fatal,

        /// <summary>
        /// 一般错误
        /// </summary>
        error,

        /// <summary>
        /// 报警
        /// </summary>
        warn,

        /// <summary>
        /// 一般信息
        /// </summary>
        info,

        /// <summary>
        /// 调试信息
        /// </summary>
        debug
    };
}

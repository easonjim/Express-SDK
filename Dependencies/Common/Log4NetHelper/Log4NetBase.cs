using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace ComLib.LogLib
{
   public class Log4NetBase
    {

       //TODO:现在还没考虑多log模块 所有关于LOG的都依赖LOG4NET
       /// <summary>
       /// 
       /// </summary>
       /// <param name="level">级别</param>
       /// <param name="UserName">所属用户 默认admin</param>
       /// <param name="StrAction">需要输出的信息</param>
       /// <param name="ex"></param>
        public static void Log(LogLevel level, string UserName, string StrAction, System.Exception ex)
        {


            LogEvent logevent = new LogEvent();
            logevent.Level = level;
            logevent.Message = StrAction;
            logevent.Ex = ex;
            logevent.Computer = System.Environment.MachineName;
            logevent.CreateTime = DateTime.Now;
            logevent.ThreadName = System.Threading.Thread.CurrentThread.Name;
            logevent.UserName = string.IsNullOrEmpty(UserName)?"admin":UserName;
            logevent.FinalMessage = LogFormatter.Format(null, logevent);

       

            switch (level)
            {
                case LogLevel.fatal:
                    LogBLL.fatal(logevent.FinalMessage);
                    break;
                case LogLevel.error:
                    LogBLL.error(logevent.FinalMessage);
                    break;
                case LogLevel.warn:
                    LogBLL.warn(logevent.FinalMessage);
                    break;
                case LogLevel.info:
                    LogBLL.info(logevent.FinalMessage);
                    break;
                case LogLevel.debug:
                    LogBLL.debug(logevent.FinalMessage);
                    break;
                default:
                    LogBLL.info(logevent.FinalMessage);
                    break;
            }

           

        }
        public static void Log(string StrAction, System.Exception ex) {
            Log(LogLevel.warn, "", StrAction, ex);
        }

        public static void Log(string StrAction)
        {
            Log(LogLevel.warn, "", StrAction, null);
        }
        public static void Log(System.Exception ex)
        {
            Log(LogLevel.warn, "", "系统错误", ex);
        }
        public static void LogWithDebugView(string StrAction, System.Exception ex) {
            if (ex != null)
            {
                Log(LogLevel.warn, "", StrAction, ex);
                Debug.DebugView.PrintDebug(StrAction, ex.Message);
            }
            else {
                Log(StrAction);
                Debug.DebugView.PrintDebug(StrAction);
            }
        }
        public static void LogWithDebugView(string StrAction)
        {
            LogWithDebugView(StrAction, null);
        }
    }

   /// <summary>
   /// Log formatter.
   /// </summary>
   public class LogFormatter
   {
       /// <summary>
       /// Quick formatter that toggles between delimited and xml.
       /// </summary>
       /// <param name="formatter"></param>
       /// <param name="logEvent"></param>
       public static string Format(string formatter, LogEvent logEvent)
       {
           if (string.IsNullOrEmpty(formatter))
               return Format(logEvent);

           if (formatter.ToLower().Trim() == "xml")
               return FormatXml(logEvent);

           return Format(logEvent);
       }


       /// <summary>
       /// Builds the log message using message and arguments.
       /// </summary>
       /// <param name="message">The message.</param>
       /// <param name="args">The args.</param>
       /// <returns></returns>
       public static string Format(LogEvent logEvent)
       {
           // Build a delimited string
           // <time>:<thread>:<level>:<message>
           StringBuilder line = new StringBuilder();
           if (!string.IsNullOrEmpty(logEvent.ThreadName))
               line.Append(logEvent.ThreadName+" | ");
           line.Append(logEvent.Level.ToString() + " | ");
           line.Append(logEvent.Message + " | ");
           line.Append(logEvent.Ex == null ? "无异常描述" : logEvent.Ex.Message);
           return line.ToString();
       }


       /// <summary>
       /// Builds the log message using message and arguments.
       /// </summary>
       /// <param name="message">The message.</param>
       /// <param name="args">The args.</param>
       /// <returns></returns>
       public static string FormatXml(LogEvent logEvent)
       {
           // Build a delimited string
           // <time>:<thread>:<level>:<loggername>:<message>
           string line = string.Format("<time>{0}</time>", logEvent.CreateTime.ToString());
           if (!string.IsNullOrEmpty(logEvent.ThreadName)) line += string.Format("<thread>{0}</thread>", logEvent.ThreadName);
           line += string.Format("<level>{0}</level>", logEvent.Level.ToString());
           line += string.Format("<message>{0}</message>", logEvent.Message);
           return line;
       }
   }
}

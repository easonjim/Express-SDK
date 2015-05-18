namespace ComLib.LogLib
{
    using log4net;
    using System;

    public class LogBLL
    {
        public static void debug(string message)
        {
            ILog logger = LogManager.GetLogger("LogSystem");
            if (logger.IsDebugEnabled)
            {
                logger.Debug(message);
            }
            logger = null;
        }

        public static void error(string message)
        {
            ILog logger = LogManager.GetLogger("LogSystem");
            if (logger.IsErrorEnabled)
            {
                logger.Error(message);
            }
            logger = null;
        }

        public static void fatal(string message)
        {
            ILog logger = LogManager.GetLogger("LogSystem");
            if (logger.IsFatalEnabled)
            {
                logger.Fatal(message);
            }
            logger = null;
        }

        public static void info(string message)
        {
            ILog logger = LogManager.GetLogger("LogSystem");
            if (logger.IsInfoEnabled)
            {
                logger.Info(message);
            }
            logger = null;
        }

        public static void warn(string message)
        {
            ILog logger = LogManager.GetLogger("LogSystem");
            if (logger.IsWarnEnabled)
            {
                logger.Warn(message);
            }
            logger = null;
        }
    }
}


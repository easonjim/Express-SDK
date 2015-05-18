namespace ComLib.LogLib
{
    using System;

    public class LogInfo : IComparable
    {
        private DateTime _date;
        private string _level;
        private string _logger;
        private int _logid;
        private string _message;
        private string _thread;

        public LogInfo(DateTime date, string thread, string level, string logger, string message)
        {
            this._date = date;
            this._thread = thread;
            this._level = level;
            this._logger = logger;
            this._message = message;
        }

        public int CompareTo(object o)
        {
            LogInfo info = o as LogInfo;
            if (info._date > this.Date)
            {
                return 1;
            }
            if (info._date < this.Date)
            {
                return -1;
            }
            return 0;
        }

        public DateTime Date
        {
            get
            {
                return this._date;
            }
            set
            {
                this._date = value;
            }
        }

        public string Level
        {
            get
            {
                return this._level;
            }
            set
            {
                this._level = value;
            }
        }

        public string Logger
        {
            get
            {
                return this._logger;
            }
            set
            {
                this._logger = value;
            }
        }

        public int LogID
        {
            get
            {
                return this._logid;
            }
            set
            {
                this._logid = value;
            }
        }

        public string Message
        {
            get
            {
                return this._message;
            }
            set
            {
                this._message = value;
            }
        }

        public string Thread
        {
            get
            {
                return this._thread;
            }
            set
            {
                this._thread = value;
            }
        }
    }
}


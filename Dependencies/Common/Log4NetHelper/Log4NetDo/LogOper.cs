namespace ComLib.LogLib
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    public static class LogOper
    {
        public static string LogPath = (AppDomain.CurrentDomain.BaseDirectory + @"\Log\");

        public static bool DeleteFile(string path)
        {
            bool flag = false;
            try
            {
                new FileInfo(path).Delete();
                flag = true;
            }
            catch
            {
            }
            return flag;
        }

        public static SortedList<MyDateTime, string> GetFileList()
        {
            SortedList<MyDateTime, string> list = new SortedList<MyDateTime, string>();
            DirectoryInfo info = new DirectoryInfo(LogPath);
            foreach (FileSystemInfo info2 in info.GetFileSystemInfos())
            {
                if (info2.Attributes != FileAttributes.Directory)
                {
                    list.Add(new MyDateTime(info2.LastWriteTime), info2.Name);
                }
            }
            return list;
        }

        public static List<LogInfo> GetFileTxtLogs()
        {
            string filePath = "";
            if (GetFileList().Count > 0)
            {
                filePath = Path.Combine(LogPath, GetFileList().Values[0]);
            }
            return GetFileTxtLogs(filePath);
        }

        public static List<LogInfo> GetFileTxtLogs(string FilePath)
        {
            List<LogInfo> list = new List<LogInfo>();
            string[] strArray = LoadFile(FilePath).Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < strArray.Length; i++)
            {
                string[] strArray2 = strArray[i].Split(new char[] { '|' });
                list.Add(new LogInfo(DateTime.Parse(strArray2[0].Trim()), strArray2[1], strArray2[2], strArray2[3], strArray2[4]));
            }
            return list;
        }

        public static string LoadFile(string path)
        {
            if (!File.Exists(path))
            {
                return "";
            }
            FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            if (stream == null)
            {
                throw new IOException("Unable to open the file: " + path);
            }
            StreamReader reader = new StreamReader(stream, Encoding.GetEncoding("gb2312"));
            string str = reader.ReadToEnd();
            reader.Close();
            return str;
        }

        public class MyDateTime : IComparable
        {
            public DateTime s_DateTime;

            public MyDateTime(DateTime d)
            {
                this.s_DateTime = d;
            }

            public int CompareTo(object o)
            {
                LogOper.MyDateTime time = o as LogOper.MyDateTime;
                if (time.s_DateTime > this.s_DateTime)
                {
                    return 1;
                }
                if (time.s_DateTime < this.s_DateTime)
                {
                    return -1;
                }
                return 0;
            }

            public static string RndNum(int VcodeNum)
            {
                StringBuilder builder = new StringBuilder(VcodeNum);
                Random random = new Random();
                for (int i = 1; i < (VcodeNum + 1); i++)
                {
                    int num2 = random.Next(9);
                    builder.AppendFormat("{0}", num2);
                }
                return builder.ToString();
            }
        }
    }
}


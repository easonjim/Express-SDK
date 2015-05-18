using System;
using System.IO;
             
namespace ComLib
{
    /// <summary>
    /// 文件操作的常用方法
    /// </summary>
    public class FileManage
    {
        #region 构造函数
        private bool _alreadyDispose = false;
        public FileManage()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }
        ~FileManage()
        {
            Dispose(); ;
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (_alreadyDispose) return;
            //if (isDisposing)
            //{
            //    if (xml != null)
            //    {
            //        xml = null;
            //    }
            //}
            _alreadyDispose = true;
        }
        #endregion

        #region IDisposable 成员

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region 取得文件后缀名
        /****************************************
         * 函数名称：GetPostfixStr
         * 功能说明：取得文件后缀名
         * 参    数：filename:文件名称
         * 调用示列：
         *           string filename = "aaa.aspx";        
         *           string s = EC.FileObj.GetPostfixStr(filename);         
        *****************************************/
        /// 
        /// 取后缀名
        /// 
        /// 文件名
        /// .gif|.html格式
        public static string GetPostfixStr(string filename)
        {
            int start = filename.LastIndexOf(".");
            int length = filename.Length;
            string postfix = filename.Substring(start, length - start);
            return postfix;
        }
        #endregion

        #region 写文件
        /****************************************
         * 函数名称：WriteFile
         * 功能说明：当文件不存时，则创建文件，并追加文件
         * 参    数：Path:文件路径,Strings:文本内容
         * 调用示列：
         *           string Path = Server.MapPath("Default2.aspx");       
         *           string Strings = "这是我写的内容啊";
         *           EC.FileObj.WriteFile(Path,Strings);
        *****************************************/
        /// 
        /// 写文件
        /// 
        /// 文件路径
        /// 文件内容
        public static void WriteFile(string Path, string Strings)
        {

            if (!System.IO.File.Exists(Path))
            {
                //Directory.CreateDirectory(Path);
                System.IO.FileStream f = System.IO.File.Create(Path);
                f.Close();
                f.Dispose();
            }
            System.IO.StreamWriter f2 = new System.IO.StreamWriter(Path, true, System.Text.Encoding.UTF8);
            f2.WriteLine(Strings);
            f2.Close();
            f2.Dispose();
        }
        #endregion

        #region 读文件
        /****************************************
         * 函数名称：ReadFile
         * 功能说明：读取文本内容
         * 参    数：Path:文件路径
         * 调用示列：
         *           string Path = Server.MapPath("Default2.aspx");       
         *           string s = EC.FileObj.ReadFile(Path);
        *****************************************/
        /// 
        /// 读文件
        /// 
        /// 文件路径
        /// 
        public static string ReadFile(string Path)
        {
            string s = "";
            if (!System.IO.File.Exists(Path))
                s = "";
            else
            {
                StreamReader f2 = new StreamReader(Path, System.Text.Encoding.GetEncoding("gb2312"));
                s = f2.ReadToEnd();
                f2.Close();
                f2.Dispose();
            }

            return s;
        }
        #endregion

        #region 追加文件
        /****************************************
         * 函数名称：FileAdd
         * 功能说明：追加文件内容
         * 参    数：Path:文件路径,strings:内容
         * 调用示列：
         *           string Path = Server.MapPath("Default2.aspx");     
         *           string Strings = "新追加内容";
         *           EC.FileObj.FileAdd(Path, Strings);
        *****************************************/
        /// 
        /// 追加文件
        /// 
        /// 文件路径
        /// 内容
        public static void FileAdd(string Path, string strings)
        {
            StreamWriter sw = File.AppendText(Path);
            sw.Write(strings);
            sw.Flush();
            sw.Close();
            sw.Dispose();
        }
        #endregion

        #region 拷贝文件
        /****************************************
         * 函数名称：FileCoppy
         * 功能说明：拷贝文件
         * 参    数：OrignFile:原始文件,NewFile:新文件路径
         * 调用示列：
         *           string OrignFile = Server.MapPath("Default2.aspx");     
         *           string NewFile = Server.MapPath("Default3.aspx");
         *           EC.FileObj.FileCoppy(OrignFile, NewFile);
        *****************************************/
        /// 
        /// 拷贝文件
        /// 
        /// 原始文件
        /// 新文件路径
        public static void FileCoppy(string OrignFile, string NewFile)
        {
            File.Copy(OrignFile, NewFile, true);
        }

        #endregion

        #region 删除文件
        /****************************************
         * 函数名称：FileDel
         * 功能说明：删除文件
         * 参    数：Path:文件路径
         * 调用示列：
         *           string Path = Server.MapPath("Default3.aspx");    
         *           EC.FileObj.FileDel(Path);
        *****************************************/
        /// 
        /// 删除文件
        /// 
        /// 路径
        public static void FileDel(string Path)
        {
            File.Delete(Path);
        }
        #endregion

        #region 移动文件
        /****************************************
         * 函数名称：FileMove
         * 功能说明：移动文件
         * 参    数：OrignFile:原始路径,NewFile:新文件路径
         * 调用示列：
         *            string OrignFile = Server.MapPath("../说明.txt");    
         *            string NewFile = Server.MapPath("../../说明.txt");
         *            EC.FileObj.FileMove(OrignFile, NewFile);
        *****************************************/
        /// 
        /// 移动文件
        /// 
        /// 原始路径
        /// 新路径
        public static void FileMove(string OrignFile, string NewFile)
        {
            File.Move(OrignFile, NewFile);
        }
        #endregion

        #region 在当前目录下创建目录
        /****************************************
         * 函数名称：FolderCreate
         * 功能说明：在当前目录下创建目录
         * 参    数：OrignFolder:当前目录,NewFloder:新目录
         * 调用示列：
         *           string OrignFolder = Server.MapPath("test/");    
         *           string NewFloder = "new";
         *           EC.FileObj.FolderCreate(OrignFolder, NewFloder); 
        *****************************************/
        /// 
        /// 在当前目录下创建目录
        /// 
        /// 当前目录
        /// 新目录
        public static void FolderCreate(string OrignFolder, string NewFloder)
        {
            Directory.SetCurrentDirectory(OrignFolder);
            Directory.CreateDirectory(NewFloder);
        }

        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="Path"></param>
        public static void FolderCreate(string Path)
        {
            // 判断目标目录是否存在如果不存在则新建之
            if (!Directory.Exists(Path))
                Directory.CreateDirectory(Path);
        }


        #region "在当前路径上创建日期格式目录(20060205)"
        /// <summary>
        /// 在当前路径上创建日期格式目录(20060205)
        /// </summary>
        /// <param name="sPath">返回目录名</param>
        /// <returns></returns>
        public static string CrateDirByDate(string sPath)
        {
            string sTemp = System.DateTime.Today.Year.ToString() + System.DateTime.Today.Month.ToString("00") + System.DateTime.Today.Day.ToString("00");
            sPath += sTemp;
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(@sPath); //构造函数创建目录
            if (di.Exists == false)
            {
                di.Create();
            }

            return sTemp;
        }
        #endregion

        #endregion

        #region 创建文件
        public static void FileCreate(string Path)
        {
            FileInfo CreateFile = new FileInfo(Path); //创建文件 
            if (!CreateFile.Exists)
            {
                FileStream FS = CreateFile.Create();
                FS.Close();
            }
        }


        #endregion

        #region 递归删除文件夹目录及文件
        /****************************************
         * 函数名称：DeleteFolder
         * 功能说明：递归删除文件夹目录及文件
         * 参    数：dir:文件夹路径
         * 调用示列：
         *           string dir = Server.MapPath("test/");  
         *           EC.FileObj.DeleteFolder(dir);       
        *****************************************/
        /// 
        /// 递归删除文件夹目录及文件
        /// 
        ///   
        /// 
        public static void DeleteFolder(string dir)
        {
            if (Directory.Exists(dir)) //如果存在这个文件夹删除之 
            {
                foreach (string d in Directory.GetFileSystemEntries(dir))
                {
                    if (File.Exists(d))
                    {
                        File.Delete(d); //直接删除其中的文件 
                    }
                    else
                    {
                        DeleteFolder(d); //递归删除子文件夹 
                    }

                }
                Directory.Delete(dir, true); //删除已空文件夹                 
            }
        }

        #endregion

        #region "删除文件夹下所有文件"
        /// <summary>
        /// 删除文件夹下所有文件
        /// </summary>
        /// <param name="dir"></param>
        public static void Delete_FolderFiles(string dir)
        {
            if (Directory.Exists(dir))
            {
                foreach (string d in Directory.GetFileSystemEntries(dir))
                {
                    if (File.Exists(d))
                        File.Delete(d); //直接删除其中的文件                        
                    else
                        DeleteFolder(d); //递归删除子文件夹 
                }
            }
        }
        #endregion

        #region 将指定文件夹下面的所有内容copy到目标文件夹下面 果目标文件夹为只读属性就会报错。
        /****************************************
         * 函数名称：CopyDir
         * 功能说明：将指定文件夹下面的所有内容copy到目标文件夹下面 果目标文件夹为只读属性就会报错。
         * 参    数：srcPath:原始路径,aimPath:目标文件夹
         * 调用示列：
         *           string srcPath = Server.MapPath("test/");  
         *           string aimPath = Server.MapPath("test1/");
         *           EC.FileObj.CopyDir(srcPath,aimPath);   
        *****************************************/
        /// 
        /// 指定文件夹下面的所有内容copy到目标文件夹下面
        /// 
        /// 原始路径
        /// 目标文件夹
        public static void CopyDir(string srcPath, string aimPath)
        {
            try
            {
                // 检查目标目录是否以目录分割字符结束如果不是则添加之
                if (aimPath[aimPath.Length - 1] != Path.DirectorySeparatorChar)
                    aimPath += Path.DirectorySeparatorChar;
                // 判断目标目录是否存在如果不存在则新建之
                if (!Directory.Exists(aimPath))
                    Directory.CreateDirectory(aimPath);
                // 得到源目录的文件列表，该里面是包含文件以及目录路径的一个数组
                //如果你指向copy目标文件下面的文件而不包含目录请使用下面的方法
                //string[] fileList = Directory.GetFiles(srcPath);
                string[] fileList = Directory.GetFileSystemEntries(srcPath);
                //遍历所有的文件和目录
                foreach (string file in fileList)
                {
                    //先当作目录处理如果存在这个目录就递归Copy该目录下面的文件

                    if (Directory.Exists(file))
                        CopyDir(file, aimPath + Path.GetFileName(file));
                    //否则直接Copy文件
                    else
                        File.Copy(file, aimPath + Path.GetFileName(file), true);
                }
            }
            catch (Exception ee)
            {
                throw new Exception(ee.ToString());
            }
        }
        #endregion

        #region 获取指定文件夹下所有子目录及文件(树形)
        /****************************************
         * 函数名称：GetFoldAll(string Path)
         * 功能说明：获取指定文件夹下所有子目录及文件(树形)
         * 参    数：Path:详细路径
         * 调用示列：
         *           string strDirlist = Server.MapPath("templates");       
         *           this.Literal1.Text = EC.FileObj.GetFoldAll(strDirlist);  
        *****************************************/
        /// 
        /// 获取指定文件夹下所有子目录及文件
        /// 
        /// 详细路径
        public static string GetFoldAll(string Path)
        {

            string str = "";
            DirectoryInfo thisOne = new DirectoryInfo(Path);
            str = ListTreeShow(thisOne, 0, str);
            return str;

        }

        /// 
        /// 获取指定文件夹下所有子目录及文件函数
        /// 
        /// 指定目录
        /// 默认起始值,调用时,一般为0
        /// 用于迭加的传入值,一般为空
        /// 
        public static string ListTreeShow(DirectoryInfo theDir, int nLevel, string Rn)//递归目录 文件
        {
            DirectoryInfo[] subDirectories = theDir.GetDirectories();//获得目录
            foreach (DirectoryInfo dirinfo in subDirectories)
            {

                if (nLevel == 0)
                {
                    Rn += "├";
                }
                else
                {
                    string _s = "";
                    for (int i = 1; i <= nLevel; i++)
                    {
                        _s += "│ ";
                    }
                    Rn += _s + "├";
                }
                Rn += "" + dirinfo.Name.ToString() + "";
                FileInfo[] fileInfo = dirinfo.GetFiles();   //目录下的文件
                foreach (FileInfo fInfo in fileInfo)
                {
                    if (nLevel == 0)
                    {
                        Rn += "│ ├";
                    }
                    else
                    {
                        string _f = "";
                        for (int i = 1; i <= nLevel; i++)
                        {
                            _f += "│ ";
                        }
                        Rn += _f + "│ ├";
                    }
                    Rn += fInfo.Name.ToString() + " ";
                }
                Rn = ListTreeShow(dirinfo, nLevel + 1, Rn);


            }
            return Rn;
        }


        /// 获取文件夹大小
        /// 
        /// 文件夹路径
        /// 
        public static long GetDirectoryLength(string dirPath)
        {
            if (!Directory.Exists(dirPath))
                return 0;
            long len = 0;
            DirectoryInfo di = new DirectoryInfo(dirPath);
            foreach (FileInfo fi in di.GetFiles())
            {
                len += fi.Length;
            }
            DirectoryInfo[] dis = di.GetDirectories();
            if (dis.Length > 0)
            {
                for (int i = 0; i < dis.Length; i++)
                {
                    len += GetDirectoryLength(dis[i].FullName);
                }
            }
            return len;
        }
        #endregion

        #region 获取指定文件详细属性
        /****************************************
         * 函数名称：GetFileAttibe(string filePath)
         * 功能说明：获取指定文件详细属性
         * 参    数：filePath:文件详细路径
         * 调用示列：
         *           string file = Server.MapPath("robots.txt");  
         *            Response.Write(EC.FileObj.GetFileAttibe(file));         
        *****************************************/
        /// 
        /// 获取指定文件详细属性
        /// 
        /// 文件详细路径
        /// 
        public static string GetFileAttibe(string filePath)
        {
            string str = "";
            System.IO.FileInfo objFI = new System.IO.FileInfo(filePath);
            str += "详细路径:" + objFI.FullName + "文件名称:" + objFI.Name + "文件长度:" + objFI.Length.ToString() + "字节创建时间" + objFI.CreationTime.ToString() + "最后访问时间:" + objFI.LastAccessTime.ToString() + "修改时间:" + objFI.LastWriteTime.ToString() + "所在目录:" + objFI.DirectoryName + "扩展名:" + objFI.Extension;
            return str;
        }
        #endregion
    }
}


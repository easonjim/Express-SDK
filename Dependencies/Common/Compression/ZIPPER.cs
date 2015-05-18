/*          使用方法如下
            List<string> arrayList = new List<string>();
            arrayList.Add(@"C:\testtemp\Jsoft.Common.dll");
            arrayList.Add(@"C:\testtemp\ComLib.dll");
            arrayList.Add(@"C:\testtemp\ComLib.XML");
            ZIPHelper zipper =  new ZIPHelper();
            zipper.FileNamesToZIP = arrayList;
            zipper.FileNameZipped = @"C:\testtemp\myziper.zip";
            zipper.ZipFiles();

            zipper.FolderToZIP =  @"C:\testtemp\";
            zipper.FileNameZipped = @"d:\myziperFolder.zip";
            zipper.ZipFolder();
 *          解压缩文件
            zipper.FolderUnZIP = @"d:\testtemp\";
            zipper.FileNameZipped = @"d:\myziperFolder.zip";
            zipper.UnpackFiles();
 * 
 *          20101227 Jim  修改 
 *          1.添加解压缩方法
 *          2.修改ArrayList为 list<string>
 *          3.添加压缩文件夹支持多级目录 和 空目录压缩
*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.GZip;

namespace ComLib
{
    /**/
    ///<summary>
    ///******************************************************************
    ///**  Creator     : Venus Feng 
    ///**  Create Date : 2006-9-19 16:21
    ///ICSharpZipLib**  Modifier    : 
    ///**  Modify Date : 
    ///**  Description : Use ICSharpZipLib to ZIP File
    ///**  Version No  : 1.0.0
    ///** 
    ///******************************************************************
    ///</summary>
    public class ZIPHelper : IDisposable
    {
        private string m_FolderToZIP;
        private List<string> m_FileNamesToZIP;
        private string m_FileNameZipped;
        private string m_FolderUnZIP;

        private ZipOutputStream m_ZipStream = null;
        private Crc32 m_Crc;

        List<string> files = new List<string>();    //all files

        List<string> emptyFolders = new List<string>();    //all empty folders


        #region Begin for Public Properties
        /**/
        /// <summary>
        /// Folder Name to ZIP
        /// Like "C:Test"
        /// </summary>
        public string FolderToZIP
        {
            get { return m_FolderToZIP; }
            set { m_FolderToZIP = value; }
        }
        /// <summary>
        /// 解压缩ZIP文件至此文件夹
        /// </summary>
        public  string FolderUnZIP
        {
            get { return m_FolderUnZIP; }
            set { m_FolderUnZIP = value; }
        }

        /**/
        /// <summary>
        /// File Name to ZIP
        /// Like "C:TestTest.txt"
        /// </summary>
        public List<string> FileNamesToZIP
        {
            get { return m_FileNamesToZIP; }
            set { m_FileNamesToZIP = value; }
        }

        /**/
        /// <summary>
        /// Zipped File Name 
        /// Like "C:TestMyZipFile.ZIP"
        /// </summary>
        public string FileNameZipped
        {
            get { return m_FileNameZipped; }
            set { m_FileNameZipped = value; }
        }
        #endregion

        /**/
        /// <summary>
        /// The construct
        /// </summary>
        public ZIPHelper()
        {
            this.m_FolderToZIP = "";
            this.m_FileNamesToZIP = new List<string>();
            this.m_FileNameZipped = "";
        }

        #region ZipFolder
        /// <summary>
        /// Zip one folder :修改为支持多级 和 空目录
        /// Before doing this event, you must set the Folder and the ZIP file name you want
        /// </summary>
        public void ZipFolder()
        {
            if (this.m_FolderToZIP.Trim().Length == 0)
            {
                throw new Exception("You must setup the folder name you want to zip!");
            }

            if (Directory.Exists(this.m_FolderToZIP) == false)
            {
                throw new Exception("The folder you input does not exist! Please check it!");
            }

            if (this.m_FileNameZipped.Trim().Length == 0)
            {
                throw new Exception("You must setup the zipped file name!");
            }

            GetAllDirectories(m_FolderToZIP);

            while (m_FolderToZIP.LastIndexOf("\\") + 1 == m_FolderToZIP.Length)//check if the path endwith "\\"
            {

                m_FolderToZIP = m_FolderToZIP.Substring(0, m_FolderToZIP.Length - 1);//remove "\\"

            }
            string rootMark = m_FolderToZIP.Substring(0, m_FolderToZIP.LastIndexOf("\\") + 1);


            //string[] fileNames = Directory.GetFiles(this.m_FolderToZIP.Trim());

            Crc32 crc = new Crc32();

            ZipOutputStream outPutStream = new ZipOutputStream(File.Create(m_FileNameZipped));

            outPutStream.SetLevel(9); // 0 - store only to 9 - means best compression

            foreach (string file in files)
            {

                FileStream fileStream = File.OpenRead(file);

                byte[] buffer = new byte[fileStream.Length];

                fileStream.Read(buffer, 0, buffer.Length);

                ZipEntry entry = new ZipEntry(file.Replace(rootMark, string.Empty));

                entry.DateTime = DateTime.Now;

                // set Size and the crc, because the information

                // about the size and crc should be stored in the header

                // if it is not set it is automatically written in the footer.

                // (in this case size == crc == -1 in the header)

                // Some ZIP programs have problems with zip files that don't store

                // the size and crc in the header.

                entry.Size = fileStream.Length;

                fileStream.Close();

                crc.Reset();

                crc.Update(buffer);

                entry.Crc = crc.Value;

                outPutStream.PutNextEntry(entry);

                outPutStream.Write(buffer, 0, buffer.Length);

            }

            this.files.Clear();

            foreach (string emptyPath in emptyFolders)
            {

                ZipEntry entry = new ZipEntry(emptyPath.Replace(rootMark, string.Empty) + "\\");

                outPutStream.PutNextEntry(entry);

            }

            this.emptyFolders.Clear();

            outPutStream.Finish();

            outPutStream.Close();

        }
        #endregion

        #region ZipFiles
        /**/
        /// <summary>
        /// Zip files
        /// Before doing this event, you must set the Files name and the ZIP file name you want
        /// </summary>
        public void ZipFiles()
        {
            if (this.m_FileNamesToZIP.Count == 0)
            {
                throw new Exception("You must setup the files name you want to zip!");
            }

            foreach (object file in this.m_FileNamesToZIP)
            {
                if (File.Exists(((string)file).Trim()) == false)
                {
                    throw new Exception("The file(" + (string)file + ") you input does not exist! Please check it!");
                }
            }

            if (this.m_FileNameZipped.Trim().Length == 0)
            {
                throw new Exception("You must input the zipped file name!");
            }

            // Create the Zip File
            this.CreateZipFile(this.m_FileNameZipped);

            // Zip this File
            foreach (object file in this.m_FileNamesToZIP)
            {
                this.ZipSingleFile((string)file);
            }

            // Close the Zip File
            this.CloseZipFile();
        }
        #endregion

        #region CreateZipFile
        /**/
        /// <summary>
        /// Create Zip File by FileNameZipped
        /// </summary>
        /// <param name="fileNameZipped">zipped file name like "C:TestMyZipFile.ZIP"</param>
        private void CreateZipFile(string fileNameZipped)
        {
            this.m_Crc = new Crc32();
            this.m_ZipStream = new ZipOutputStream(File.Create(fileNameZipped));
            this.m_ZipStream.SetLevel(6); // 0 - store only to 9 - means best compression
        }
        #endregion

        #region CloseZipFile
        /**/
        /// <summary>
        /// Close the Zip file
        /// </summary>
        private void CloseZipFile()
        {
            this.m_ZipStream.Finish();
            this.m_ZipStream.Close();
            this.m_ZipStream = null;
        }
        #endregion

        #region ZipSingleFile
        /**/
        /// <summary>
        /// Zip single file 
        /// </summary>
        /// <param name="fileName">file name like "C:TestTest.txt"</param>
        private void ZipSingleFile(string fileNameToZip)
        {
            // Open and read this file
            FileStream fso = File.OpenRead(fileNameToZip);

            // Read this file to Buffer
            byte[] buffer = new byte[fso.Length];
            fso.Read(buffer, 0, buffer.Length);

            // Create a new ZipEntry
            ZipEntry zipEntry = new ZipEntry(fileNameToZip.Split('\\')[fileNameToZip.Split('\\').Length - 1]);
            zipEntry.DateTime = DateTime.Now;
            // set Size and the crc, because the information
            // about the size and crc should be stored in the header
            // if it is not set it is automatically written in the footer.
            // (in this case size == crc == -1 in the header)
            // Some ZIP programs have problems with zip files that don't store
            // the size and crc in the header.
            zipEntry.Size = fso.Length;

            fso.Close();
            fso = null;

            // Using CRC to format the buffer
            this.m_Crc.Reset();
            this.m_Crc.Update(buffer);
            zipEntry.Crc = this.m_Crc.Value;

            // Add this ZipEntry to the ZipStream
            this.m_ZipStream.PutNextEntry(zipEntry);
            this.m_ZipStream.Write(buffer, 0, buffer.Length);
        }
        #endregion

        #region IDisposable member

        /**/
        /// <summary>
        /// Release all objects
        /// </summary>
        public void Dispose()
        {
            if (this.m_ZipStream != null)
            {
                this.m_ZipStream.Close();
                this.m_ZipStream = null;
            }
        }

        #endregion

        #region "新加入 解压缩文件方法"
        /// <summary>
        /// 解压缩文件
        /// </summary>
        /// <returns></returns>
        public  bool UnpackFiles( )
        {
            try
            {
                if (!Directory.Exists(FolderUnZIP))
                    Directory.CreateDirectory(FolderUnZIP);

                ZipInputStream s = new ZipInputStream(File.OpenRead(m_FileNameZipped));

                ZipEntry theEntry;
                while ((theEntry = s.GetNextEntry()) != null)
                {

                    string directoryName = Path.GetDirectoryName(theEntry.Name);
                    string fileName = Path.GetFileName(theEntry.Name);

                    if (directoryName != String.Empty)
                        Directory.CreateDirectory(FolderUnZIP + directoryName);

                    if (fileName != String.Empty)
                    {
                        FileStream streamWriter = File.Create(Path.Combine(FolderUnZIP, theEntry.Name));

                        int size = 2048;
                        byte[] data = new byte[2048];
                        while (true)
                        {
                            size = s.Read(data, 0, data.Length);
                            if (size > 0)
                            {
                                streamWriter.Write(data, 0, size);
                            }
                            else
                            {
                                break;
                            }
                        }

                        streamWriter.Close();
                    }
                }
                s.Close();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region "得到所有目录文件"
        private void GetAllDirectories(string rootPath)
        {

            string[] subPaths = Directory.GetDirectories(rootPath);

            foreach (string path in subPaths)
            {

                GetAllDirectories(path);

            }

            string[] filesArray = Directory.GetFiles(rootPath);

            foreach (string file in filesArray)
            {

                this.files.Add(file); //put all files in current folder into list

            }

            if (subPaths.Length == filesArray.Length && filesArray.Length == 0) //if it is an empty folder
            {

                this.emptyFolders.Add(rootPath);//add it to the list

            }

        }
        #endregion

    }
}
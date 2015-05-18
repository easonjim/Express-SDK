using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Text;
using System.Web.UI.WebControls;
using System.IO;
using System.Web;
using System.Drawing;

namespace ComLib.ImageLib
{
    /// <summary>
    /// 图片上传通用类
    /// </summary>
    public class ImageUpLoadCommon
    {
        #region "Private Variables"
        private string _path;                   // 上传文件的路径
        private string _fileName;               // 文件名称
        private string _newFileName  ="";            // 新的文件名

        private string _webFilePath = "";            // 服务器端文件路径

        private string _errorMsg;               // 错误信息
        private bool _isDate = true;            // 是否加上日期
     
     
        //private bool _isImage = true;           // 是否只充许图片
        private int _width = 0;               // 缩图宽度
        private int _height = 0;               // 缩图高度
        private string _mode = "Auto";        // 缩图模式 "HW"://指定高宽缩放（可能变形）  "W"//指定宽，高按比例 "H"//指定高，宽按比例  "Cut"://指定高宽裁减（不变形） Auto:自动 

        private int _fileSize = 0;          // 图片大小
        private int _fileWidth = 0;         // 图片宽度
        private int _fileHeight = 0;        // 图片高度
        private string _fileType;           // 文件类型
     
        #endregion
        #region 属性 -- 旧文件名
        /// &ltsummary>
        /// 旧文件名
        /// </summary>
        public string fileName
        {
            get
            {
                return _fileName;
            }
        }
        #endregion
        #region 属性 -- 上传后新文件名
        /// &ltsummary>
        /// 上传后新文件名
        /// </summary>
        public string newFileName
        {
            get
            {
                return _newFileName;
            }
        }
        #endregion

        public string WebFilePath {
            get { return _webFilePath; }
        } 

        #region 属性 -- 上传的文件的路径
        /// &ltsummary>
        /// 上传的文件的路径
        /// </summary>
        public string path
        {
            get
            {
                return _path;
            }
        }
        #endregion
        #region 属性 -- 文件大小
        /// &ltsummary>
        /// 文件大小
        /// </summary>
        public int fileSize
        {
            get
            {
                return _fileSize;
            }
        }
        #endregion
        #region 属性 -- 图片原始的宽度
        /// &ltsummary>
        /// 图片原始的宽度
        /// </summary>
        public int fileWidth
        {
            get
            {
                return _fileWidth;
            }
        }
        #endregion
        #region 属性 -- 图片原始的高度
        /// &ltsummary>
        /// 图片原始的高度
        /// </summary>
        public int fileHeight
        {
            get { return _fileHeight; }
        }
        #endregion
        #region 属性 -- 文件的类型
        /// &ltsummary>
        /// 文件的类型
        /// </summary>
        public string fileType
        {
            get { return _fileType; }
        }
        #endregion


        
        #region 构造函数
        /// &ltsummary>
        /// 构造函数
        /// </summary>
        public ImageUpLoadCommon()
            : this("/public/", true)
        {
        }
        /// &ltsummary>
        /// 构造函数
        /// </summary>
        /// &ltparam name="filePath">文件路径</param>
        /// &ltparam name="isDate">是否按日期创建目录</param>
        public ImageUpLoadCommon(string filePath, bool isDate)
        {
            _path = filePath;
            _isDate = isDate;
            if (_isDate)
                _path += DateTime.Now.ToString("yyyyMMdd") + "/";
            string p = HttpContext.Current.Server.MapPath(_path);
            //如果目录不存在,将创建目录
            if (!Directory.Exists(p))
                Directory.CreateDirectory(p);
        }
        #endregion
        #region 方法 -- 保存文件
        /// &ltsummary>
        /// 指定缩图的宽高
        /// </summary>
        /// &ltparam name="fu">文件类型</param>
        /// &ltparam name="Width">宽</param>
        /// &ltparam name="Height">高</param>
        /// &ltreturns></returns>
        public bool SaveImage(FileUpload fu, int Width, int Height)
        {
            _width = Width;
            _height = Height;
            return SaveFile(fu);
        }
        /// &ltsummary>
        /// 指定缩图的宽高
        /// </summary>
        /// &ltparam name="fu">文件类型</param>
        /// &ltparam name="Width">宽</param>
        /// &ltparam name="Height">高</param>
        /// &ltparam name="Mode">缩图模式 "HW"://指定高宽缩放（可能变形）  "W"//指定宽，高按比例 "H"//指定高，宽按比例  "Cut"://指定高宽裁减（不变形） </param>
        /// &ltreturns></returns>
        public bool SaveImage(FileUpload fu, int Width, int Height, string Mode)
        {
            _width = Width;
            _height = Height;
            _mode = Mode;
            return SaveFile(fu);
        }
        #endregion
        #region 方法 -- 保存文件
        /// &ltsummary>
        /// 保存文件
        /// </summary>
        /// &ltparam name="fu">上传文件对象</param>
        /// &ltparam name="IsImage">是否邮件</param>
        public bool SaveFile(FileUpload fu)
        {
            if (fu.HasFile)
            {
                string fileContentType = fu.PostedFile.ContentType;
                string name = fu.PostedFile.FileName;                        // 客户端文件路径
                FileInfo file = new FileInfo(name);
                _fileType = fu.PostedFile.ContentType;
                _fileSize = fu.PostedFile.ContentLength;
                bool isfileTypeImages = false;
                if (fileContentType == "image/x-png" || fileContentType == "image/png" || fileContentType == "image/bmp" || fileContentType == "image/gif" || fileContentType == "image/pjpeg")
                {
                    isfileTypeImages = true;
                }

                
                //检测文件扩展名是否正确
                var ImgExtention = file.Extension.Substring(1).ToLower();

                if (ImgExtention != "gif" && ImgExtention != "jpg" && ImgExtention != "jpeg" && ImgExtention != "png")
                {
                    _errorMsg = string.Format("文件扩展名不符合系统需求:{0}",
                        "gif|jpg|jpeg|png");
                    fu.Dispose();
                    return false;
                }
                if (_fileSize / 1024 > 2048)
                {
                    _errorMsg = string.Format("上传文件超过系统允许大小:{0}K", "2048");
                    fu.Dispose();
                    return false;  
                }
                _fileName = file.Name;                                // 文件名称
                _newFileName = CreateFileName() + file.Extension;
                _webFilePath = HttpContext.Current.Server.MapPath(_path + _newFileName);         // 服务器端文件路径
                if (isfileTypeImages)
                {
                      //检查文件是否存在
                    if (!File.Exists(_webFilePath))
                    {
                        try
                        {
                            fu.SaveAs(_webFilePath);                                 // 使用 SaveAs 方法保存文件     
                           
                            // 只有上传完了,才能获取图片大小
                            if (File.Exists(_webFilePath))
                            {
                                System.Drawing.Image originalImage = System.Drawing.Image.FromFile(_webFilePath);
                                try
                                {
                                    _fileHeight = originalImage.Height;
                                    _fileWidth = originalImage.Width;
                                }
                                finally
                                {
                                    originalImage.Dispose();
                                }
                            }
                            _errorMsg = string.Format("提示：文件“{0}”成功上传，文件类型为：{1}，文件大小为：{2}B", _newFileName, fu.PostedFile.ContentType, fu.PostedFile.ContentLength);
                            fu.Dispose();
                            return true;
                        }
                        catch (Exception ex)
                        {
                            _errorMsg = "提示：文件上传失败，失败原因：" + ex.Message;
                        }
                    }
                    else
                    {
                        _errorMsg = "提示：文件已经存在，请重命名后上传";
                    }
                }
                else
                {
                    //上传文件
                    //检查文件是否存在
                    if (!File.Exists(_webFilePath))
                    {
                        try
                        {
                            fu.SaveAs(_webFilePath);                                 // 使用 SaveAs 方法保存文件
                            _errorMsg = string.Format("提示：文件“{0}”成功上传，文件类型为：{1}，文件大小为：{2}B", _newFileName,  fu.PostedFile.ContentType, fu.PostedFile.ContentLength);
                            fu.Dispose();
                            return true;
                        }
                        catch (Exception ex)
                        {
                            _errorMsg = "提示：文件上传失败，失败原因：" + ex.Message;
                        }
                    }
                    else
                        _errorMsg = "提示：文件已经存在，请重命名后上传";
                }
            }
            fu.Dispose();
            return false;
        }
        #endregion
        #region 方法 -- 创建新的文件名
        /// &ltsummary>
        /// 创建新的文件名
        /// </summary>
        /// &ltreturns></returns>
        public string CreateFileName()
        {
            string guid = System.Guid.NewGuid().ToString().ToLower();
            guid = guid.Replace("-", "");
            return DateTime.Now.ToString("yyyyMMddhhmmss") + guid.Substring(0, 4);
        }
        #endregion
        #region 方法 -- 删除文件
        /// &ltsummary>
        /// 删除文件
        /// </summary>
        /// &ltparam name="filename"></param>
        public static void DeleteFile(string filename)
        {
            string s = HttpContext.Current.Server.MapPath(filename);
            if (File.Exists(s))
            {
                try
                {
                    File.Delete(s);
                }
                catch
                { }
            }
        }
        #endregion
    }
}


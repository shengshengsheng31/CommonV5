using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CommonV5
{
    /// <summary>
    /// 显示所有文件，下载文件，上传文件
    /// </summary>
    public class FtpHelper
    {
        private string _userName;
        private string _password;

        public FtpHelper(string userName,string password)
        {
            _userName = userName;
            _password = password;
        }

        /// <summary>
        /// 获取目标ftp路径的文件列表
        /// </summary>
        /// <param name="ftpPath"></param>
        /// <returns></returns>
        public async Task<List<string>> GetFtpFileList(string ftpPath)
        {
            List<string> filePathList = new List<string>();
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri(ftpPath));
                request.Credentials = new NetworkCredential(_userName, _password);
                request.Method = WebRequestMethods.Ftp.ListDirectory;
                // 异步获取响应
                using (FtpWebResponse response = (FtpWebResponse)await request.GetResponseAsync())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        
                        while (reader.Peek() != -1)
                        {
                            string filePath = $"{ftpPath}{reader.ReadLine()}";
                            filePathList.Add(filePath);
                        }
                        Log.Debug($"获取{ftpPath}-ok");
                        return filePathList;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex,ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 下载文件到内存流
        /// </summary>
        /// <param name="ftpFilePath"></param>
        /// <returns></returns>
        public async Task<Stream> DownloadFile2Stream(string ftpFilePath) 
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri(ftpFilePath));
                request.Credentials = new NetworkCredential(_userName, _password);
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                using (FtpWebResponse response = (FtpWebResponse)await request.GetResponseAsync())
                {
                    MemoryStream memoryStream = new MemoryStream();
                    byte[] buffer = new byte[1024];
                    while (true)
                    {
                        int sz = response.GetResponseStream().Read(buffer, 0, 1024);
                        if (sz == 0) break;
                        memoryStream.Write(buffer, 0, sz);
                    }
                    memoryStream.Position = 0;
                    Log.Debug($"下载{ftpFilePath}-ok");
                    return memoryStream;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 下载文件到本地
        /// </summary>
        /// <param name="ftpFilePath"></param>
        /// <param name="localFilePath"></param>
        /// <returns></returns>
        public async Task<bool> DownloadFile2Local(string ftpFilePath,string localFilePath)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri(ftpFilePath));
                request.Credentials = new NetworkCredential(_userName, _password);
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                using (FtpWebResponse response = (FtpWebResponse)await request.GetResponseAsync())
                {
                    using (FileStream fs = new FileStream(localFilePath, FileMode.Create))
                    {
                        byte[] buffer = new byte[1024];
                        while (true)
                        {
                            int sz = response.GetResponseStream().Read(buffer, 0, 1024);
                            if (sz == 0) break;
                            fs.Write(buffer, 0, sz);
                        }
                        Log.Debug($"下载{ftpFilePath}到{localFilePath}-ok");
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 上传流到ftp生成文件
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="ftpFilePath"></param>
        public async Task<bool> UploadFile(Stream stream,string ftpFilePath)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpFilePath));
                request.Credentials = new NetworkCredential(_userName, _password);
                request.Method = WebRequestMethods.Ftp.UploadFile;
                using (Stream requestStream = await request.GetRequestStreamAsync())
                {
                    byte[] buffer = new byte[1024];
                    while (true)
                    {
                        int sz = stream.Read(buffer, 0, 1024);
                        if (sz == 0) break;
                        requestStream.Write(buffer, 0, sz);
                    }
                    stream.Close();
                    Log.Debug($"上传{ftpFilePath}-ok");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 上传文件到ftp生成同名文件
        /// </summary>
        /// <param name="localFile"></param>
        /// <param name="ftpPath"></param>
        /// <returns></returns>
        public async Task<bool> UploadFile(FileInfo localFile,string ftpPath)
        {
            try
            {
                string ftpFilePath = ftpPath + localFile.Name;
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri(ftpFilePath));
                request.Credentials = new NetworkCredential(_userName, _password);
                request.Method = WebRequestMethods.Ftp.UploadFile;
                using (FileStream fs = new FileStream(localFile.FullName, FileMode.Open))
                {
                    using (Stream requestStream = await request.GetRequestStreamAsync())
                    {
                        byte[] buffer = new byte[1024];
                        while (true)
                        {
                            int sz = fs.Read(buffer, 0, 1024);
                            if (sz == 0) break;
                            requestStream.Write(buffer, 0, sz);
                        }
                        Log.Debug($"上传{ftpFilePath}-ok");
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="ftpFilePath"></param>
        public async Task<bool> Delete(string ftpFilePath)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri(ftpFilePath));
                request.Credentials = new NetworkCredential(_userName, _password);
                request.Method = WebRequestMethods.Ftp.DeleteFile;
                using (FtpWebResponse response = (FtpWebResponse)await request.GetResponseAsync())
                {
                    Log.Debug($"删除{ftpFilePath}-ok");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="ftpDirPath"></param>
        /// <returns></returns>
        public async Task<bool> MakeDir(string ftpDirPath)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpDirPath));
                request.Credentials = new NetworkCredential(_userName, _password);
                request.Method = WebRequestMethods.Ftp.MakeDirectory;
                using (FtpWebResponse response = (FtpWebResponse)await request.GetResponseAsync())
                {
                    Log.Debug($"创建{ftpDirPath}-ok");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return false;
            }

        }
    }
}

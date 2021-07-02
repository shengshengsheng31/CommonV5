using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonV5
{
    public class ArchiveHelper
    {
        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="fileList"></param>
        /// <param name="zipFilePath"></param>
        /// <returns></returns>
        public static bool Zip(FileInfo[] fileList,string zipFilePath,bool deleteSource=false)
        {
            try
            {
                using (FileStream zipToOpen = new FileStream(zipFilePath, FileMode.OpenOrCreate))
                {
                    using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                    {
                        foreach (FileInfo file in fileList)
                        {
                            string filePath = file.FullName;
                            string fileName = file.Name;
                            // 同名文件覆盖
                            if (archive.GetEntry(fileName) != null)
                            {
                                archive.GetEntry(fileName).Delete();
                                Log.Debug($"删除同名{fileName}-ok");
                            }
                            ZipArchiveEntry archiveEntry = archive.CreateEntryFromFile(filePath, fileName);
                            Log.Debug($"压缩{fileName}-ok");
                            if (deleteSource)
                            {
                                file.Delete();
                                Log.Debug($"删除源文件{fileName}-ok");
                            }
                        }
                        Log.Information($"完成压缩{zipFilePath}-ok");
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

        public static bool Extract(string zipFilePath,string extractPath)
        {
            try
            {
                using (ZipArchive archive = ZipFile.Open(zipFilePath, ZipArchiveMode.Read))
                {
                    // 中文编码
                    //Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                    //ZipFile.ExtractToDirectory(zipFilePath, extractPath,Encoding.GetEncoding("GB2312"));
                    ZipFile.ExtractToDirectory(zipFilePath, extractPath);
                    //archive.ExtractToDirectory(extractPath);
                    Log.Information($"{zipFilePath}解压{extractPath}-ok");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.Information(ex, ex.Message);
                return false;
            }
            
        }
    }
}

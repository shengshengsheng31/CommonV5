using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CommonV5
{
    public class DownloadHelper
    {
        /// <summary>
        /// 下载文件到本地
        /// </summary>
        /// <param name="url"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public async static Task<bool> Download(string url, string filePath)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                HttpResponseMessage response = await httpClient.GetAsync(url);
                using (Stream stream = await response.Content.ReadAsStreamAsync())
                {
                    using (FileStream fs = File.Create(filePath))
                    {
                        //await stream.CopyToAsync(fs);
                        byte[] buffer = new byte[1024];
                        while (true)
                        {
                            int sz = stream.Read(buffer, 0, 1024);
                            if (sz == 0) break;
                            fs.Write(buffer, 0, sz);
                        }
                        Log.Information($"{url}下载{filePath}-ok");
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
    }
}

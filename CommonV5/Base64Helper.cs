using Serilog;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonV5
{
    public class Base64Helper
    {
        /// <summary>
        /// 流转base64
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static async Task<string> Stream2Base64(Stream stream)
        {
            try
            {
                byte[] buffer = new byte[stream.Length];
                await stream.ReadAsync(buffer, 0, buffer.Length);
                stream.Position = 0;
                string result = Convert.ToBase64String(buffer);
                Log.Information($"stream:{stream.Length}转base64-ok");
                stream.Close();
                return result;
            }
            catch (Exception ex)
            {
                Log.Error(ex,ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 文件转base64
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static async Task<string> File2Base64(FileInfo file)
        {
            try
            {
                using (FileStream fs = new FileStream(file.FullName, FileMode.Open))
                {
                    byte[] buffer = new byte[fs.Length];
                    await fs.ReadAsync(buffer, 0, buffer.Length);
                    string result = Convert.ToBase64String(buffer);
                    Log.Information($"file:{file.Name}转base64-ok");
                    return result;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return null;
            }
        }

        /// <summary>
        /// base64保存为图片
        /// </summary>
        /// <param name="imgPath"></param>
        /// <param name="base64String"></param>
        /// <returns></returns>
        public static bool Base64SaveImage(string imgPath,string base64String)
        {
            try
            {
                // 替换因http产生的脏数据，base64长度需要为4的整数倍
                base64String = base64String.Trim()
                    .Replace("data:image/png;base64,", "")
                    .Replace("data:image/jpg;base64,", "")
                    .Replace("data:image/jpeg;base64,", "")
                    .Replace("%", "")
                    .Replace(",", "")
                    .Replace(" ", "+");
                    
                if (base64String.Length % 4 != 0)
                {
                    base64String += "==";
                }
                byte[] buffer = Convert.FromBase64String(base64String);
                using (MemoryStream stream = new MemoryStream(buffer))
                {
                    using (Bitmap bitmap = new Bitmap(stream))
                    {
                        // 后续可定制保存格式
                        bitmap.Save(imgPath, System.Drawing.Imaging.ImageFormat.Jpeg);
                        Log.Information($"base64转{imgPath}-ok");
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

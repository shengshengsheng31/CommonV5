using CommonV5;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinForms
{
    public partial class Form1 : Form
    {
        CommonHelper commonHelper = new CommonHelper();
        string ftpPath = "ftp://153.37.151.130/ftper/";
        FtpHelper ftpHelper = new FtpHelper("ftper", "Admin12345");

        int taskCount = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTime.Text = commonHelper.Test();
        }

        //多线程测试
        private void button1_Click(object sender, EventArgs e)
        {
            Log.Information("多线程测试");
            ThreadTest();
        }
        
        // 异步多线程
        public async void ThreadTest()
        {
            taskCount++;
            await Task.Run(() =>
            {
                int i = 0;
                while (i < 100)
                {
                    i++;
                    string threadId = Thread.CurrentThread.ManagedThreadId.ToString();
                    Invoke(new Action(() =>
                    {
                        textBox1.Text += $"task{taskCount}-{i} threadId:{threadId}\r\n";
                    }));
                }
            });

            //await Task.Run(() =>
            //{
            //    int i = 0;
            //    while (i < 500)
            //    {
            //        i++;
            //        string threadId = Thread.CurrentThread.ManagedThreadId.ToString();
            //        Invoke(new Action(() =>
            //        {
            //            textBox1.Text += $"task2-{i} threadId:{threadId}\r\n";
            //        }));
            //    }
            //});

            textBox1.Text += $"app threadId:{Thread.CurrentThread.ManagedThreadId}\r\n";
        }

        // 实时滚动到底部
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1.SelectionStart = textBox1.Text.Length;
            textBox1.ScrollToCaret();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox2.Text += $"{Application.ExecutablePath}\r\n";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string filepath = "C:\\Users\\Administrator\\Desktop\\公安自建推给雪亮的视频监控.xls";
            DataTable dt = ExcelHelper.ExcelToDataTable(filepath);
            dataGridView1.DataSource = dt;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable("table") { };
            DataColumn dc1 = new DataColumn("A");
            DataColumn dc2 = new DataColumn("B");
            dt.Columns.Add(dc1);
            dt.Columns.Add(dc2);
            for (int i = 0; i < 10; i++)
            {
                DataRow dr = dt.NewRow();
                dr[0] = $"a{i}";
                dr[1] = $"b{i}";
                dt.Rows.Add(dr);
            }
            if(ExcelHelper.DataTableToExcel(dt, "C:\\Users\\Administrator\\Desktop\\test.xlsx"))
            {
                textBox3.Text = "ok";
            }
            else
            {
                textBox3.Text = "Error";
            }
            
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable("table") { };
            DataColumn dc1 = new DataColumn("A");
            DataColumn dc2 = new DataColumn("B");
            dt.Columns.Add(dc1);
            dt.Columns.Add(dc2);
            for (int i = 0; i < 10; i++)
            {
                DataRow dr = dt.NewRow();
                dr[0] = $"a{i}";
                dr[1] = $"b{i}";
                dt.Rows.Add(dr);
            }
            if (CsvHelper.DataTableToCsv(dt, "C:\\Users\\Administrator\\Desktop\\test.csv"))
            {
                textBox4.Text = "ok";
            }
            else
            {
                textBox4.Text = "Error";
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string filepath = "C:\\Users\\Administrator\\Desktop\\test.csv";
            DataTable dt = CsvHelper.CsvToDataTable(filepath);
            dataGridView1.DataSource = dt;
        }

        private async void button7_Click(object sender, EventArgs e)
        {
            List<string> filePathList = await ftpHelper.GetFtpFileList(ftpPath);
            comboBox1.Items.AddRange(filePathList.ToArray());
            comboBox1.SelectedIndex = 0;
        }

        private async void button8_Click(object sender, EventArgs e)
        {
            string ftpFilePath = comboBox1.SelectedItem.ToString();
            using (Stream memoryStream = await ftpHelper.DownloadFile2Stream(ftpFilePath))
            {
                using(FileStream fs = new FileStream("C:\\Users\\Administrator\\Desktop\\ftpfile", FileMode.Create))
                {
                    byte[] buffer = new byte[1024];
                    while (true)
                    {
                        int sz = memoryStream.Read(buffer, 0, 1024);
                        if (sz == 0) break;
                        fs.Write(buffer, 0, sz);
                    }
                    MessageBox.Show("下载完成");
                }
            }
            
        }

        private async void button9_Click(object sender, EventArgs e)
        {
            string ftpFilePath = comboBox1.SelectedItem.ToString();
            string localFilePath = "C:\\Users\\Administrator\\Desktop\\" + ftpFilePath.Split("/").ToList()[ftpFilePath.Split("/").ToList().Count-1];
            await ftpHelper.DownloadFile2Local(ftpFilePath, localFilePath);
            MessageBox.Show("完成");
        }

        private async void button10_Click(object sender, EventArgs e)
        {
            using (FileStream fs = new FileStream("C:\\Users\\Administrator\\Desktop\\新建文本文档.txt", FileMode.Open))
            {
               await ftpHelper.UploadFile(fs, "ftp://153.37.151.130/ftper/test.txt");
                MessageBox.Show("ok");
            }

        }

        private async void button11_Click(object sender, EventArgs e)
        {
            DialogResult result= openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                FileInfo localFile = new FileInfo(openFileDialog1.FileName);
                if(await ftpHelper.UploadFile(localFile, "ftp://153.37.151.130/ftper/"))
                {
                    MessageBox.Show("ok");
                }
            }
           
        }

        private async void button12_Click(object sender, EventArgs e)
        {
            string ftpFilePath = comboBox1.SelectedItem.ToString();
            await ftpHelper.Delete(ftpFilePath);
        }

        private async void button13_Click(object sender, EventArgs e)
        {
            string ftpDicPath = ftpPath + textBox5.Text;
            if(await ftpHelper.MakeDir(ftpDicPath))
            {
                MessageBox.Show("ok");
            }
        }

        private async void button14_Click(object sender, EventArgs e)
        {
            string filePath = "C:\\Users\\Administrator\\Desktop\\道闸.jpg";
            using (FileStream fs = new FileStream(filePath,FileMode.Open))
            {
                string result=  await Base64Helper.Stream2Base64(fs);
                textBox6.Text = $"{result.Substring(0, 1000)}..."; 
            }
        }

        private async void button15_Click(object sender, EventArgs e)
        {
            FileInfo fileInfo = new FileInfo("C:\\Users\\Administrator\\Desktop\\道闸.jpg");
            string result = await Base64Helper.File2Base64(fileInfo);
            textBox6.Text = $"{result.Substring(0, 1000)}...";
        }

        private void button16_Click(object sender, EventArgs e)
        {
            string base64String = textBox6.Text;
            string imgPath = "C:\\Users\\Administrator\\Desktop\\22.jpg";
            bool result = Base64Helper.Base64SaveImage(imgPath, base64String);
            if (result)
            {
                MessageBox.Show("ok");
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            ConfigHelper.AddConfig("Name", "KK");
            string name = ConfigHelper.GetConfig("Name");
            bool result = ConfigHelper.SetConfig("Name","asd");
            bool result2 = ConfigHelper.DelConfig("Name");
        }

        private void button18_Click(object sender, EventArgs e)
        {
            string dirPath = @"C:\Users\Administrator\Desktop\新建文件夹";
            DirectoryInfo directory = new DirectoryInfo(dirPath);
            FileInfo[] fileList = directory.GetFiles();
            string zipFilePath = "C:\\Users\\Administrator\\Desktop\\1.zip";
            
           if(ArchiveHelper.Zip(fileList, zipFilePath,false))
            {
                MessageBox.Show("ok");
            }
        }

        private void button19_Click(object sender, EventArgs e)
        {
            string zipFilePath = "C:\\Users\\Administrator\\Desktop\\1.zip";
            string extractPath = "C:\\Users\\Administrator\\Desktop\\111";
            if(ArchiveHelper.Extract(zipFilePath, extractPath))
            {
                MessageBox.Show("ok");
            }
        }

        private async void button20_Click(object sender, EventArgs e)
        {
            //string url = "https://vimsky.com/wp-content/themes/mytux/images/vimsky-logo.png";
            string url = "https://www.baidu.com/link?url=GIdzSbSFhn7hLC0uXZUxuZb3O8vm46CTBtScgQc8YL8Z1Aihuo-1u9OEVA5T5pjR_zz5jp9kQlY8Q7wN9yGKjVqPwFgwboHpoeC8FMcGLwC&wd=&eqid=93439034000242180000000560ded9d8";
            string filePath = "2.jpg";
            if (await DownloadHelper.Download(url, filePath))
            {
                MessageBox.Show("ok");
            }
        }
    }
}

using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinForms
{
    public partial class Form1 : Form
    {
        CommonV5.CommonHelper commonHelper = new CommonV5.CommonHelper();
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
                while (i < 50)
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
    }
}

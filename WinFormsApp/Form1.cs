using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async  void button1_Click(object sender, EventArgs e)
        {
            int i = 0;
            while (i<100)
            {
                i++;
                await Task.Run(() =>
                {
                    Invoke(new Action(() =>
                    {
                        textBox1.Text += "1\r\n";
                    }));
                });

                await  Task.Run(() =>
                {
                    Invoke(new Action(() =>
                    {
                        textBox1.Text += "2\r\n";
                    }));
                });
            }
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1.SelectionStart = textBox1.Text.Length;
            textBox1.ScrollToCaret();
        }
    }
}

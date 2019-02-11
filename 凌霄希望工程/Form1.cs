using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 凌霄希望工程
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            IPAddress ip = IPAddress.Parse("119.3.78.168");
            IPEndPoint point = new IPEndPoint(ip, 2333);
            C.s.Connect(point);
            //C.client.Start();
        }

        private void SendCode(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
                return;
            C.qq = textBox1.Text;
            C.s.Send(Encoding.UTF8.GetBytes($"CODE {textBox1.Text}"));
            label3.Text = "甜狼给你发了一个验证码";
            textBox1.Text = "";
            button1.Text = "登录";
            button1.Click -= new EventHandler(SendCode);
            button1.Click += new EventHandler(Login);
        }
        private void Login(object sender, EventArgs e)
        {
            C.s.Send(Encoding.UTF8.GetBytes($"LOGIN {textBox1.Text}"));
            byte[] buffer = new byte[10];
            while (C.s.Receive(buffer) == 0)
                continue;
            string s = Encoding.Default.GetString(buffer).Trim();
            //MessageBox.Show(s);
            if (s.StartsWith("SUCCESS"))
                MessageBox.Show("登录成功");
            else
                MessageBox.Show("登录失败");
        }
    }
}

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

        private void button1_Click(object sender, EventArgs e)
        {
            C.s.Send(Encoding.UTF8.GetBytes($"CODE {textBox1.Text}"));

        }
    }
}

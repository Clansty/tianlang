using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElecListGen2020
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            var rds = new RedisClient("101.132.178.136", 6379, "qVAo9C1tCbD2PEiR");
            var set = rds.GetAllItemsFromSet("elec2020");
            foreach (var i in set)
            {
                listBox1.Items.Add($"{rds.GetValueFromHash($"u{i}", "nick")}({rds.GetValueFromHash($"u{i}", "name")})({i})");
            }
            rds.Dispose();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Clipboard.SetText((string)listBox1.SelectedItem);
        }
    }
}

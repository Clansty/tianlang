using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Clansty.tianlang
{
    public partial class SendTest : Form
    {
        public SendTest()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            S.Test(textBox1.Text);
        }
    }
}

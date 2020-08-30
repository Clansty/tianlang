using System;
using System.Windows.Forms;

namespace Clansty.tianlang
{
    internal partial class SendTest : Form
    {
        internal SendTest()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            S.Test(textBox1.Text);
        }
    }
}

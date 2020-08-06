using System;
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

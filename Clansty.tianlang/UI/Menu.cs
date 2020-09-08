using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Clansty.tianlang.SIAMC;
using Microsoft.VisualBasic;

namespace Clansty.tianlang
{
    internal partial class Menu : Form
    {
        internal Menu()
        {
            InitializeComponent();
            版本ToolStripMenuItem.Text = C.Version;
        }

        private void 检查群名片ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UserInfo.CheckAllQmpAsync();
        }

        private void 乌萨斯学生自治团ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new SendTest().Show();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            new Loading().Show();
        }

        private void 保存数据库ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Db.Commit();
        }
    }
}
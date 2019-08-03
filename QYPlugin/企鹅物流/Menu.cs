using Microsoft.VisualBasic;
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
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
            版本ToolStripMenuItem.Text = C.Version;
        }


        private void ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            new Push().Show();
        }

        private void 确认退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void NotifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
        }

        private void 最小化ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void 喀兰贸易ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                string em = Interaction.InputBox("*龙门粗口*");
                try
                {
                    string key = (em.GetLeft(" ") == "" ? em : em.GetLeft(" ")).UnEscape().ToLower();
                    string act = em.GetRight(" ").UnEscape();
                    if (Cmds.gcmds.ContainsKey(key))
                    {
                        GroupCommand m = Cmds.gcmds[key];
                        C.WriteLn(Cmds.gcmds[key].Func(act), ConsoleColor.White);
                    }

                }
                catch (Exception ex)
                {
                    C.WriteLn(ex.Message, ConsoleColor.Red);
                }
            });
        }

        private void 检查群名片ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UserInfo.CheckAllQmpAsync();
        }

        private void 乌萨斯学生自治团ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new SendTest().Show();
        }
    }
}

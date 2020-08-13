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


        private void ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("???");
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
                    var key = (em.GetLeft(" ") == "" ? em : em.GetLeft(" ")).ToLower();
                    var act = em.GetRight(" ");
                    if (Cmds.gcmds.ContainsKey(key))
                    {
                        var m = Cmds.gcmds[key];
                        C.WriteLn(Cmds.gcmds[key].Func(act));
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

        private void 准备信息整理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("???");
        }

        private void 开关选举记录ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            C.recording = !C.recording;
            C.WriteLn($"记录模式{(C.recording ? "开启" : "关闭")}");
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            toolStripMenuItem3.Visible = false;
            UserInfo.InitQmpCheckTask();
            MemberList.UpdateMajor();
            C.WriteLn("Memberlist updated");
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            new Loading().Show();
        }
    }
}
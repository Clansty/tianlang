using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
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

        private void 准备信息整理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                var all = new HashSet<string>();
                var fs = new FileStream(@"C:\Users\Administrator\Desktop\names.bin", FileMode.Create);
                var bw = new BinaryWriter(fs);
                //准备所有人集合
                const string maj = "646751705";
                var gms = GetMembers(maj);//大群
                foreach (var i in gms)
                {
                    all.Add(i.QQ);
                }
                const string g2019 = "872937333";
                gms = GetMembers(g2019);//2019
                foreach (var i in gms)
                {
                    all.Add(i.QQ);
                }
                const string g2018 = "285395983";
                gms = GetMembers(g2018);//2018
                foreach (var i in gms)
                {
                    all.Add(i.QQ);
                }
                const string g2017 = "659856134";
                gms = GetMembers(g2017);//2017
                foreach (var i in gms)
                {
                    all.Add(i.QQ);
                }
                C.WriteLn(all.Count.ToString());
                //写入
                bw.Write(all.Count);
                foreach (var i in all)
                {
                    bw.Write(i);
                    bw.Write(GetCard(g2017, i));
                    bw.Write(GetCard(g2018, i));
                    bw.Write(GetCard(g2019, i));
                }


                List<GroupMember> GetMembers(string group)
                {
                    long.TryParse(group, out long t);
                    string b64 = Marshal.PtrToStringAnsi(QY_getGroupMemberList(Robot.AuthCode, 839827911, t));
                    Unpack u = new Unpack(b64);
                    int count = u.NextInt;
                    List<GroupMember> l = new List<GroupMember>();
                    for (int i = 0; i < count; i++)
                    {
                        Unpack n = new Unpack(u.NextToken);
                        l.Add(new GroupMember(n.NextLong, n.NextStr, n.NextStr, n.NextInt, n.NextInt, n.NextStr, n.NextInt, n.NextInt, n.NextStr, n.NextInt, n.NextInt, n.NextStr, n.NextInt, n.NextInt));
                    }
                    return l;
                }
                string GetCard(string group, string member)
                {
                    bool isok = long.TryParse(group, out long t);
                    if (!isok)
                        return "";
                    isok = long.TryParse(member, out long m);
                    if (!isok)
                        return "";
                    string b64 = Marshal.PtrToStringAnsi(QY_getGroupMemberCard(Robot.AuthCode, 839827911, t, m, false));
                    Unpack u = new Unpack(b64);
                    return u.NextStr;
                }
            });
        }
        [DllImport("QYOffer.dll")]
        private static extern IntPtr QY_getGroupMemberCard(int authCode, long qqID, long targ, long s, bool a);

        [DllImport("QYOffer.dll")]
        private static extern IntPtr QY_getGroupMemberList(int authCode, long qqID, long targ);

        private void ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            new SIAMC.Loading().Show();
        }
    }
}

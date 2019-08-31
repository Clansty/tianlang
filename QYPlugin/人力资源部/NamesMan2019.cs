using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Clansty.tianlang
{
    public partial class NamesMan2019 : Form
    {
        public NamesMan2019()
        {
            InitializeComponent();
            label4.Text = lst.Count.ToString();
        }

        int cur = -1;
        int all = 0;
        User u = null;
        List<GroupMember> lst = GetMembers("872937333");

        [DllImport("QYOffer.dll")]
        private static extern IntPtr QY_getGroupMemberList(int authCode, long qqID, long targ);
        static List<GroupMember> GetMembers(string group)
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

        private void Button2_Click(object sender, EventArgs e)
        {
            cur++;
            label1.Text = cur.ToString();
            if (cur >= all)
            {
                MessageBox.Show("Test");
                return;
            }
            label2.Text = lst[cur].QQ;
            textBox4.Text = lst[cur].Card;
            u = new User(label2.Text);
            label3.Text = u.ProperNamecard;
            textBox5.Text = u.Name;
            pictureBox1.LoadAsync("http://q.qlogo.cn/headimg_dl?bs=qq&dst_uin=" + label2.Text + "&spec=640");
        }


        private void Button5_Click(object sender, EventArgs e)
        {
            u.Name = textBox4.Text;

        }

        private void Button6_Click(object sender, EventArgs e)
        {
            u.Name = textBox5.Text;

        }
    }
}

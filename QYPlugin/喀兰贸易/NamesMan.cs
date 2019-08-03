using ServiceStack.Redis;
using System;
using System.IO;
using System.Windows.Forms;

namespace Clansty.tianlang
{
    public partial class NamesMan : Form
    {
        public NamesMan()
        {
            InitializeComponent();
            Rds.pool = new PooledRedisClientManager(233, 10, "101.132.178.136:6379");
        }

        FileStream fs = null;
        BinaryReader br = null;
        int cur = 830;
        int all = 0;
        User u = null;

        private void Button1_Click(object sender, EventArgs e)
        {
            fs = new FileStream(textBox1.Text, FileMode.Open);
            br = new BinaryReader(fs);
            all = br.ReadInt32();
            label1.Text = cur.ToString();
            for (int i = 0; i < 830 * 4; i++)
                br.ReadString();
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
            label2.Text = br.ReadString();
            textBox2.Text = br.ReadString();
            textBox3.Text = br.ReadString();
            textBox4.Text = br.ReadString();
            u = new User(label2.Text);
            label3.Text = u.ProperNamecard;
            textBox5.Text = u.Name;
            pictureBox1.LoadAsync("http://q.qlogo.cn/headimg_dl?bs=qq&dst_uin=" + label2.Text + "&spec=640");
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            u.Name = textBox2.Text;
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            u.Name = textBox3.Text;
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

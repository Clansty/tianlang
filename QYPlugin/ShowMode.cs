using ServiceStack.Redis;
using System;
using System.Windows.Forms;

namespace Clansty.tianlang
{
    public partial class ShowMode : Form
    {
        public ShowMode()
        {
            InitializeComponent();
            label1.Text = isOn ? "开" : "关";
            label2.Text = allowRecall ? "开" : "关";
        }

        public static bool isOn = false;
        public static bool allowRecall = false;
        private void Button1_Click(object sender, EventArgs e)
        {
            isOn = !isOn;
            label1.Text = isOn ? "开" : "关";
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            IRedisClient client = Rds.GetClient();
            foreach (string i in client.GetAllItemsFromSet("show"))
            {
                User u = new User(i);
                textBox1.AppendText($"{u.Nick}({u.Name})\n");
            }
            client.Dispose();
        }

        public static void NewMsg(GroupMsgArgs e)
        {
            if (!isOn)
                return;
            User u = new User(e.FromQQ);
            if (u.Enrollment != 2019 && allowRecall)
            {
                e.Recall();
                return;
            }
            Rds.SAdd("show", e.FromQQ);
            Rds.LAdd("showRec", e.FromQQ + ":" + e.Msg);
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            allowRecall = !allowRecall;
            label2.Text = allowRecall ? "开" : "关";
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Clansty.tianlang
{
    public partial class Push : Form
    {
        public Push()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            string pmsg = textBox1.Text;
            string ftxt = comboBox1.Text;
            new Thread(() =>
            {
                List<GroupMember> l = Robot.Group.GetMembers(G.major);
                foreach (GroupMember i in l)
                {
                    if (i.Card.IndexOf(ftxt) > -1)
                    {
                        Robot.Send.QY_sendGroupTmpMsg(Robot.AuthCode, 168375232, long.Parse(G.major), long.Parse(i.QQ), pmsg);
                        C.WriteLn($"{i.Card} <-", ConsoleColor.Magenta);
                        Thread.Sleep(60000);
                    }
                }
            }).Start();
            Close();
        }
    }
}

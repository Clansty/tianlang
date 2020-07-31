using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Clansty.tianlang.SIAMC
{
    public partial class Loading : Form
    {
        public Loading(int method)
        {
            InitializeComponent();
            var siamc = new Siamc();
            Task.Run(() =>
            {
                HashSet<string> uins = null;
                if (method == 0)
                {
                    var rcl = Rds.GetClient();
                    uins = rcl.GetAllItemsFromSet("users");
                    rcl.Dispose();
                }
                else if (method == 1)
                {
                    uins = new HashSet<string>();
                    var members = Robot.Group.GetMembers(G.major);
                    foreach (var i in members)
                    {
                        uins.Add(i.QQ);
                    }
                }
                else
                {
                    var rcl = Rds.GetClient();
                    var tmp = rcl.GetAllItemsFromSet("users");
                    rcl.Dispose();
                    uins = new HashSet<string>();
                    foreach (var i in tmp)
                    {
                        var u = new User(i);
                        if (u.Enrollment == method)
                            uins.Add(i);
                    }
                }

                var rows = siamc.data.Rows;
                Invoke(new Action(() =>
                {
                    label3.Text = uins.Count.ToString();
                    progressBar1.Maximum = uins.Count;
                }));
                foreach (var i in uins)
                {
                    var u = new User(i);
                    Invoke(new Action(() =>
                    {
                        rows.Add(u.Uin, u.Branch, u.Enrollment, u.Junior, u.Grade, u.Nick, u.Name, u.Class, u.VerifyMsg, u.Role, "保存此行");
                        progressBar1.PerformStep();
                        label1.Text = progressBar1.Value.ToString();
                    }));
                }
                Invoke(new Action(() =>
                {
                    siamc.Show();
                    Dispose();
                }));
            });
        }
    }
}

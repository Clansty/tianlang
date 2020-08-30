using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Clansty.tianlang.SIAMC
{
    internal partial class Loading : Form
    {
        internal Loading()
        {
            InitializeComponent();
            var siamc = new Siamc();
            Task.Run(() =>
            {
                HashSet<long> uins = new HashSet<long>();
                var t = C.Robot.GetGroupMembers(G.major);
                t.Wait();
                var members = t.Result;
                foreach (var i in members)
                {
                    uins.Add(i.UIN);
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
                        rows.Add(u.Uin, u.Branch, u.Enrollment, u.Junior, u.Grade, u.Nick, u.Name, 0, u.VerifyMsg, u.Role, "保存此行");
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

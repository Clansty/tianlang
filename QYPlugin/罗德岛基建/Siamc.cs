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

namespace Clansty.tianlang.SIAMC
{
    public partial class Siamc : Form
    {
        public Siamc()
        {
            InitializeComponent();
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void Data_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 9)
            {
                var row = data.Rows[e.RowIndex];
                User u = new User((string)row.Cells[0].Value);
                u.Branch = (bool)row.Cells[1].Value;
                u.Enrollment = int.Parse((string)row.Cells[2].Value);
                u.Junior = (bool)row.Cells[3].Value;
                u.Name = (string)row.Cells[6].Value;
                row.Cells[7].Value = u.VerifyMsg;
                MessageBox.Show("已更新");
            }
        }
    }
}

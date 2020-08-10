using System;
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
            if (e.ColumnIndex == 10)
            {
                var row = data.Rows[e.RowIndex];
                var u = new User(row.Cells[0].Value.ToString());
                u.Enrollment = int.Parse(row.Cells[2].Value.ToString());
                u.Junior = (bool)row.Cells[3].Value;
                u.Name = row.Cells[6].Value.ToString();
                row.Cells[8].Value = u.VerifyMsg;
                row.Cells[1].Value = u.Branch;
                row.Cells[2].Value = u.Enrollment;
                row.Cells[3].Value = u.Junior;
                MessageBox.Show("已更新");
            }
        }
    }
}

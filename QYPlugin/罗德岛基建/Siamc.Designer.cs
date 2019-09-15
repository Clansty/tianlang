namespace Clansty.tianlang.SIAMC
{
    partial class Siamc
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.data = new System.Windows.Forms.DataGridView();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.罗德岛ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.退出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.branch = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.enrollment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.junior = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.grade = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nick = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cls = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.verifyMsg = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.role = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.update = new System.Windows.Forms.DataGridViewButtonColumn();
            ((System.ComponentModel.ISupportInitialize)(this.data)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // data
            // 
            this.data.AllowUserToAddRows = false;
            this.data.AllowUserToDeleteRows = false;
            this.data.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.data.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.uin,
            this.branch,
            this.enrollment,
            this.junior,
            this.grade,
            this.nick,
            this.name,
            this.cls,
            this.verifyMsg,
            this.role,
            this.update});
            this.data.Dock = System.Windows.Forms.DockStyle.Fill;
            this.data.Location = new System.Drawing.Point(0, 24);
            this.data.Name = "data";
            this.data.Size = new System.Drawing.Size(955, 426);
            this.data.TabIndex = 0;
            this.data.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Data_CellContentClick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.罗德岛ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(955, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 罗德岛ToolStripMenuItem
            // 
            this.罗德岛ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.退出ToolStripMenuItem});
            this.罗德岛ToolStripMenuItem.Name = "罗德岛ToolStripMenuItem";
            this.罗德岛ToolStripMenuItem.Size = new System.Drawing.Size(58, 20);
            this.罗德岛ToolStripMenuItem.Text = "罗德岛";
            // 
            // 退出ToolStripMenuItem
            // 
            this.退出ToolStripMenuItem.Name = "退出ToolStripMenuItem";
            this.退出ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.退出ToolStripMenuItem.Text = "退出";
            this.退出ToolStripMenuItem.Click += new System.EventHandler(this.退出ToolStripMenuItem_Click);
            // 
            // uin
            // 
            this.uin.Frozen = true;
            this.uin.HeaderText = "uin";
            this.uin.Name = "uin";
            this.uin.ReadOnly = true;
            // 
            // branch
            // 
            this.branch.HeaderText = "branch";
            this.branch.Name = "branch";
            // 
            // enrollment
            // 
            this.enrollment.HeaderText = "enrollment";
            this.enrollment.Name = "enrollment";
            // 
            // junior
            // 
            this.junior.HeaderText = "junior";
            this.junior.Name = "junior";
            this.junior.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.junior.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // grade
            // 
            this.grade.HeaderText = "grade";
            this.grade.Name = "grade";
            this.grade.ReadOnly = true;
            this.grade.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.grade.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // nick
            // 
            this.nick.HeaderText = "nick";
            this.nick.Name = "nick";
            // 
            // name
            // 
            this.name.HeaderText = "name";
            this.name.Name = "name";
            // 
            // cls
            // 
            this.cls.HeaderText = "class";
            this.cls.Name = "cls";
            this.cls.ReadOnly = true;
            // 
            // verifyMsg
            // 
            this.verifyMsg.HeaderText = "verifyMsg";
            this.verifyMsg.Name = "verifyMsg";
            this.verifyMsg.ReadOnly = true;
            // 
            // role
            // 
            this.role.HeaderText = "role";
            this.role.Name = "role";
            this.role.ReadOnly = true;
            this.role.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.role.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // update
            // 
            this.update.HeaderText = "update";
            this.update.Name = "update";
            // 
            // Siamc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(955, 450);
            this.Controls.Add(this.data);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Siamc";
            this.Text = "罗德岛基建";
            ((System.ComponentModel.ISupportInitialize)(this.data)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.DataGridView data;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 罗德岛ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 退出ToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn uin;
        private System.Windows.Forms.DataGridViewCheckBoxColumn branch;
        private System.Windows.Forms.DataGridViewTextBoxColumn enrollment;
        private System.Windows.Forms.DataGridViewCheckBoxColumn junior;
        private System.Windows.Forms.DataGridViewTextBoxColumn grade;
        private System.Windows.Forms.DataGridViewTextBoxColumn nick;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn cls;
        private System.Windows.Forms.DataGridViewTextBoxColumn verifyMsg;
        private System.Windows.Forms.DataGridViewTextBoxColumn role;
        private System.Windows.Forms.DataGridViewButtonColumn update;
    }
}
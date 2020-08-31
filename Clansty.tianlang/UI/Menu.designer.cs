﻿using System.ComponentModel;

namespace Clansty.tianlang
{
    partial class Menu
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.龙门ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.版本ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.喀兰贸易ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.检查群名片ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.保存数据库ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.乌萨斯学生自治团ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.龙门ToolStripMenuItem,
            this.喀兰贸易ToolStripMenuItem,
            this.乌萨斯学生自治团ToolStripMenuItem,
            this.toolStripMenuItem2,
            this.toolStripMenuItem3});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(7, 3, 0, 3);
            this.menuStrip1.Size = new System.Drawing.Size(965, 62);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 龙门ToolStripMenuItem
            // 
            this.龙门ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.版本ToolStripMenuItem});
            this.龙门ToolStripMenuItem.Name = "龙门ToolStripMenuItem";
            this.龙门ToolStripMenuItem.Size = new System.Drawing.Size(82, 56);
            this.龙门ToolStripMenuItem.Text = "龙门";
            // 
            // 版本ToolStripMenuItem
            // 
            this.版本ToolStripMenuItem.Enabled = false;
            this.版本ToolStripMenuItem.Name = "版本ToolStripMenuItem";
            this.版本ToolStripMenuItem.Size = new System.Drawing.Size(195, 44);
            this.版本ToolStripMenuItem.Text = "版本";
            // 
            // 喀兰贸易ToolStripMenuItem
            // 
            this.喀兰贸易ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.检查群名片ToolStripMenuItem,
            this.保存数据库ToolStripMenuItem});
            this.喀兰贸易ToolStripMenuItem.Name = "喀兰贸易ToolStripMenuItem";
            this.喀兰贸易ToolStripMenuItem.Size = new System.Drawing.Size(130, 56);
            this.喀兰贸易ToolStripMenuItem.Text = "喀兰贸易";
            // 
            // 检查群名片ToolStripMenuItem
            // 
            this.检查群名片ToolStripMenuItem.Name = "检查群名片ToolStripMenuItem";
            this.检查群名片ToolStripMenuItem.Size = new System.Drawing.Size(267, 44);
            this.检查群名片ToolStripMenuItem.Text = "检查群名片";
            this.检查群名片ToolStripMenuItem.Click += new System.EventHandler(this.检查群名片ToolStripMenuItem_Click);
            // 
            // 保存数据库ToolStripMenuItem
            // 
            this.保存数据库ToolStripMenuItem.Name = "保存数据库ToolStripMenuItem";
            this.保存数据库ToolStripMenuItem.Size = new System.Drawing.Size(267, 44);
            this.保存数据库ToolStripMenuItem.Text = "保存数据库";
            this.保存数据库ToolStripMenuItem.Click += new System.EventHandler(this.保存数据库ToolStripMenuItem_Click);
            // 
            // 乌萨斯学生自治团ToolStripMenuItem
            // 
            this.乌萨斯学生自治团ToolStripMenuItem.Name = "乌萨斯学生自治团ToolStripMenuItem";
            this.乌萨斯学生自治团ToolStripMenuItem.Size = new System.Drawing.Size(226, 56);
            this.乌萨斯学生自治团ToolStripMenuItem.Text = "乌萨斯学生自治团";
            this.乌萨斯学生自治团ToolStripMenuItem.Click += new System.EventHandler(this.乌萨斯学生自治团ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(82, 56);
            this.toolStripMenuItem2.Text = "基建";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(130, 56);
            this.toolStripMenuItem3.Text = "准备就绪";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.toolStripMenuItem3_Click);
            // 
            // Menu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(965, 62);
            this.Controls.Add(this.menuStrip1);
            this.Location = new System.Drawing.Point(233, 0);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "Menu";
            this.Opacity = 0.8D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "罗德岛";
            this.TopMost = true;
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem 版本ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 检查群名片ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 喀兰贸易ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 龙门ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 乌萨斯学生自治团ToolStripMenuItem;

        #endregion

        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem 保存数据库ToolStripMenuItem;
    }
}
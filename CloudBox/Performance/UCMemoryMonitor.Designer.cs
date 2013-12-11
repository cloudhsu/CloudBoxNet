namespace CloudBox.MemoryDr
{
    partial class UCMemoryMonitor
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.gpbMain = new System.Windows.Forms.GroupBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.lblMax = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblCurrent = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.gpbModule = new System.Windows.Forms.GroupBox();
            this.lsvModule = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.gpbMain.SuspendLayout();
            this.gpbModule.SuspendLayout();
            this.SuspendLayout();
            // 
            // gpbMain
            // 
            this.gpbMain.Controls.Add(this.btnClose);
            this.gpbMain.Controls.Add(this.label4);
            this.gpbMain.Controls.Add(this.lblMax);
            this.gpbMain.Controls.Add(this.label6);
            this.gpbMain.Controls.Add(this.label3);
            this.gpbMain.Controls.Add(this.lblCurrent);
            this.gpbMain.Controls.Add(this.label1);
            this.gpbMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.gpbMain.Location = new System.Drawing.Point(0, 0);
            this.gpbMain.Name = "gpbMain";
            this.gpbMain.Size = new System.Drawing.Size(800, 49);
            this.gpbMain.TabIndex = 0;
            this.gpbMain.TabStop = false;
            this.gpbMain.Text = "Process Memory";
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(776, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(24, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "X";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Left;
            this.label4.Location = new System.Drawing.Point(241, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(21, 12);
            this.label4.TabIndex = 5;
            this.label4.Text = "KB";
            // 
            // lblMax
            // 
            this.lblMax.AutoSize = true;
            this.lblMax.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblMax.Location = new System.Drawing.Point(212, 18);
            this.lblMax.Name = "lblMax";
            this.lblMax.Size = new System.Drawing.Size(29, 12);
            this.lblMax.TabIndex = 4;
            this.lblMax.Text = "1024";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Left;
            this.label6.Location = new System.Drawing.Point(128, 18);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(84, 12);
            this.label6.TabIndex = 3;
            this.label6.Text = "Maximun Usage:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Left;
            this.label3.Location = new System.Drawing.Point(107, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(21, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "KB";
            // 
            // lblCurrent
            // 
            this.lblCurrent.AutoSize = true;
            this.lblCurrent.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblCurrent.Location = new System.Drawing.Point(78, 18);
            this.lblCurrent.Name = "lblCurrent";
            this.lblCurrent.Size = new System.Drawing.Size(29, 12);
            this.lblCurrent.TabIndex = 1;
            this.lblCurrent.Text = "1024";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Location = new System.Drawing.Point(3, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "Current Usage:";
            // 
            // gpbModule
            // 
            this.gpbModule.Controls.Add(this.lsvModule);
            this.gpbModule.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gpbModule.Location = new System.Drawing.Point(0, 49);
            this.gpbModule.Name = "gpbModule";
            this.gpbModule.Size = new System.Drawing.Size(800, 481);
            this.gpbModule.TabIndex = 1;
            this.gpbModule.TabStop = false;
            this.gpbModule.Text = "Module Memory";
            // 
            // lsvModule
            // 
            this.lsvModule.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.lsvModule.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lsvModule.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lsvModule.Location = new System.Drawing.Point(3, 18);
            this.lsvModule.MultiSelect = false;
            this.lsvModule.Name = "lsvModule";
            this.lsvModule.Size = new System.Drawing.Size(794, 460);
            this.lsvModule.TabIndex = 0;
            this.lsvModule.UseCompatibleStateImageBehavior = false;
            this.lsvModule.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Module Name";
            this.columnHeader1.Width = 200;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Current Usage(KB)";
            this.columnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeader2.Width = 200;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Maximum Usage(KB)";
            this.columnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeader3.Width = 200;
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // UCMemoryMonitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gpbModule);
            this.Controls.Add(this.gpbMain);
            this.Name = "UCMemoryMonitor";
            this.Size = new System.Drawing.Size(800, 530);
            this.Load += new System.EventHandler(this.UCMemoryMonitor_Load);
            this.VisibleChanged += new System.EventHandler(this.UCMemoryMonitor_VisibleChanged);
            this.ControlRemoved += new System.Windows.Forms.ControlEventHandler(this.UCMemoryMonitor_ControlRemoved);
            this.gpbMain.ResumeLayout(false);
            this.gpbMain.PerformLayout();
            this.gpbModule.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gpbMain;
        private System.Windows.Forms.GroupBox gpbModule;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblMax;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblCurrent;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView lsvModule;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btnClose;
    }
}

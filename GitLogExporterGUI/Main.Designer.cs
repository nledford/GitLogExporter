namespace GitLogExporterGUI {
    partial class Main {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.label1 = new System.Windows.Forms.Label();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.btnFindPath = new System.Windows.Forms.Button();
            this.pnlDateRanges = new System.Windows.Forms.Panel();
            this.grpDateRanges = new System.Windows.Forms.GroupBox();
            this.dateTo = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.dateFrom = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.btnExportGitLog = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.txtPreviewLog = new System.Windows.Forms.RichTextBox();
            this.pnlDateRanges.SuspendLayout();
            this.grpDateRanges.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Repository Path:";
            // 
            // txtPath
            // 
            this.txtPath.Location = new System.Drawing.Point(104, 10);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(258, 20);
            this.txtPath.TabIndex = 1;
            this.txtPath.TextChanged += new System.EventHandler(this.txtPath_TextChanged);
            // 
            // btnFindPath
            // 
            this.btnFindPath.Location = new System.Drawing.Point(368, 8);
            this.btnFindPath.Name = "btnFindPath";
            this.btnFindPath.Size = new System.Drawing.Size(84, 23);
            this.btnFindPath.TabIndex = 2;
            this.btnFindPath.Text = "Browse...";
            this.btnFindPath.UseVisualStyleBackColor = true;
            this.btnFindPath.Click += new System.EventHandler(this.btnFindPath_Click);
            // 
            // pnlDateRanges
            // 
            this.pnlDateRanges.Controls.Add(this.grpDateRanges);
            this.pnlDateRanges.Location = new System.Drawing.Point(16, 36);
            this.pnlDateRanges.Name = "pnlDateRanges";
            this.pnlDateRanges.Size = new System.Drawing.Size(436, 86);
            this.pnlDateRanges.TabIndex = 3;
            // 
            // grpDateRanges
            // 
            this.grpDateRanges.Controls.Add(this.dateTo);
            this.grpDateRanges.Controls.Add(this.label3);
            this.grpDateRanges.Controls.Add(this.dateFrom);
            this.grpDateRanges.Controls.Add(this.label2);
            this.grpDateRanges.Location = new System.Drawing.Point(3, 3);
            this.grpDateRanges.Name = "grpDateRanges";
            this.grpDateRanges.Size = new System.Drawing.Size(430, 79);
            this.grpDateRanges.TabIndex = 0;
            this.grpDateRanges.TabStop = false;
            this.grpDateRanges.Text = "Date Range";
            // 
            // dateTo
            // 
            this.dateTo.Location = new System.Drawing.Point(47, 47);
            this.dateTo.Name = "dateTo";
            this.dateTo.Size = new System.Drawing.Size(377, 20);
            this.dateTo.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(23, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "To:";
            // 
            // dateFrom
            // 
            this.dateFrom.Location = new System.Drawing.Point(47, 20);
            this.dateFrom.Name = "dateFrom";
            this.dateFrom.Size = new System.Drawing.Size(377, 20);
            this.dateFrom.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "From:";
            // 
            // btnExportGitLog
            // 
            this.btnExportGitLog.Location = new System.Drawing.Point(12, 407);
            this.btnExportGitLog.Name = "btnExportGitLog";
            this.btnExportGitLog.Size = new System.Drawing.Size(359, 23);
            this.btnExportGitLog.TabIndex = 4;
            this.btnExportGitLog.Text = "Export Git Log";
            this.btnExportGitLog.UseVisualStyleBackColor = true;
            this.btnExportGitLog.Click += new System.EventHandler(this.btnExportGitLog_Click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(377, 407);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
            this.btnExit.TabIndex = 5;
            this.btnExit.Text = "E&xit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // txtPreviewLog
            // 
            this.txtPreviewLog.Location = new System.Drawing.Point(19, 129);
            this.txtPreviewLog.Name = "txtPreviewLog";
            this.txtPreviewLog.Size = new System.Drawing.Size(433, 272);
            this.txtPreviewLog.TabIndex = 6;
            this.txtPreviewLog.Text = "";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 442);
            this.Controls.Add(this.txtPreviewLog);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnExportGitLog);
            this.Controls.Add(this.pnlDateRanges);
            this.Controls.Add(this.btnFindPath);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.label1);
            this.Name = "Main";
            this.Text = "Git Log Exporter";
            this.Load += new System.EventHandler(this.Main_Load);
            this.pnlDateRanges.ResumeLayout(false);
            this.grpDateRanges.ResumeLayout(false);
            this.grpDateRanges.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Button btnFindPath;
        private System.Windows.Forms.Panel pnlDateRanges;
        private System.Windows.Forms.GroupBox grpDateRanges;
        private System.Windows.Forms.DateTimePicker dateFrom;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dateTo;
        private System.Windows.Forms.Button btnExportGitLog;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.RichTextBox txtPreviewLog;
    }
}


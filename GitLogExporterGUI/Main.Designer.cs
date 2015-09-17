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
            this.label2 = new System.Windows.Forms.Label();
            this.dateFrom = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.button1 = new System.Windows.Forms.Button();
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
            // 
            // btnFindPath
            // 
            this.btnFindPath.Location = new System.Drawing.Point(368, 8);
            this.btnFindPath.Name = "btnFindPath";
            this.btnFindPath.Size = new System.Drawing.Size(84, 23);
            this.btnFindPath.TabIndex = 2;
            this.btnFindPath.Text = "Browse...";
            this.btnFindPath.UseVisualStyleBackColor = true;
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
            this.grpDateRanges.Controls.Add(this.dateTimePicker1);
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
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "From:";
            // 
            // dateFrom
            // 
            this.dateFrom.Location = new System.Drawing.Point(47, 20);
            this.dateFrom.Name = "dateFrom";
            this.dateFrom.Size = new System.Drawing.Size(377, 20);
            this.dateFrom.TabIndex = 1;
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
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(47, 47);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(377, 20);
            this.dateTimePicker1.TabIndex = 3;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(351, 227);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(101, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Export Git Log";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 262);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.pnlDateRanges);
            this.Controls.Add(this.btnFindPath);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.label1);
            this.Name = "Main";
            this.Text = "Git Log Exporter";
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
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Button button1;
    }
}


using System;
using System.Drawing;
using System.Windows.Forms;
using GitLogExporterCore.Extensions;

namespace GitLogExporterGUI {
    public partial class Main : Form {
        private DateTime _end;

        private DateTime _start;

        public Main() {
            InitializeComponent();
        }

        private string Path {
            get { return txtPath.Text; }
            set { txtPath.Text = value; }
        }

        private void Main_Load(object sender,
                               EventArgs e) {
            InitializeDates();

            dateFrom.Value = _start;
            dateTo.Value = _end;

            txtPreviewLog.BackColor = Color.White;
            txtPreviewLog.ForeColor = Color.Black;
        }

        private void btnFindPath_Click(object sender,
                                       EventArgs e) {
            using (var dialog = new FolderBrowserDialog()) {
                if (dialog.ShowDialog() == DialogResult.OK) {
                    Path = dialog.SelectedPath;
                }
            }
        }

        private void InitializeDates() {
            _start = DateTime.Now.DayOfWeek == DayOfWeek.Monday
                         ? DateTime.Now
                         : DateTime.Today.Previous(DayOfWeek.Monday);

            _end = DateTime.Now.DayOfWeek == DayOfWeek.Saturday ? DateTime.Now : DateTime.Today.Next(DayOfWeek.Saturday);
        }
    }
}

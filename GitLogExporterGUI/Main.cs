using System;
using System.Drawing;
using System.Windows.Forms;
using GitLogExporterGUI.Extensions;

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

            btnExportGitLog.Enabled = false;
        }

        private void btnFindPath_Click(object sender,
                                       EventArgs e) {
            using (var dialog = new FolderBrowserDialog()) {
                if (dialog.ShowDialog() == DialogResult.OK) {
                    Path = dialog.SelectedPath;
                }
            }
        }

        private void btnExit_Click(object sender,
                                   EventArgs e) {
            Close();
        }

        private void txtPath_TextChanged(object sender,
                                         EventArgs e) {
            btnExportGitLog.Enabled = Path.Length > 0;
        }

        private async void btnExportGitLog_Click(object sender,
                                           EventArgs e) {
            txtPreviewLog.AppendText($"Generating git log for {Path}, please wait...");

            var exporter = new Exporter();

            var log = await exporter.ExportGitLog(Path, _start, _end);

            if (!string.IsNullOrWhiteSpace(log) && log != "ERROR") {
                txtPreviewLog.Clear();
                txtPreviewLog.AppendText(log);
            }

            //File.WriteAllText(@path + $"\\Commit Log for {projectName}.txt", Sb.ToString());
        }

        private void InitializeDates() {
            _start = DateTime.Now.DayOfWeek == DayOfWeek.Monday
                         ? DateTime.Now
                         : DateTime.Today.Previous(DayOfWeek.Monday);

            _end = DateTime.Now.DayOfWeek == DayOfWeek.Saturday ? DateTime.Now : DateTime.Today.Next(DayOfWeek.Saturday);
        }
    }
}

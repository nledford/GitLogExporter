using System;
using System.Drawing;
using System.Windows.Forms;
using GitLogExporterGUI.Extensions;
using LibGit2Sharp;

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
                dialog.ShowNewFolderButton = false;
                dialog.Description = "Select the folder of the repository containing the git log you want to export";
                dialog.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Projects";

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

        private void btnExportGitLog_Click(object sender,
                                           EventArgs e) {
            txtPreviewLog.AppendText($"Generating git log for {Path}, please wait...");

            var exporter = new Exporter();

            string log;

            try {
                log = exporter.ExportGitLog(Path, _start, _end);
            } catch (RepositoryNotFoundException) {
                MessageBox.Show(
                    "No git log was found in your selected folder.  Please make sure the folder contains a .git directory and try again.",
                    "Repository Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                txtPreviewLog.Clear();
                txtPath.SelectAll();
                txtPath.Focus();
                return;
            } catch (BareRepositoryException) {
                MessageBox.Show(
                    "The selected folder contains a .git directory, but no commits were found.  Please make sure commits exist in the git log and try again.",
                    "Bare Repository Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                txtPreviewLog.Clear();
            }

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

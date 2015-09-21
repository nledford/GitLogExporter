using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using GitLogExporterGUI.Exporters;
using GitLogExporterGUI.Extensions;
using LibGit2Sharp;

namespace GitLogExporterGUI {
    public partial class Main : Form {
        private const string Version = "v1.0.0";
        private DateTime _end;
        private DateTime _start;

        public Main() {
            InitializeComponent();
        }

        private string Log { get; set; }

        private string Path {
            get { return txtPath.Text; }
            set { txtPath.Text = value; }
        }

        private void Main_Load(object sender,
                               EventArgs e) {
            Text += $" - {Version}";

            InitializeDates();
            dateFrom.Value = _start;
            dateTo.Value = _end;

            txtPreviewLog.BackColor = Color.White;
            txtPreviewLog.ForeColor = Color.Black;

            btnExportGitLog.Enabled = false;
            btnSaveGitLog.Enabled = false;
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
            _start = dateFrom.Value;
            _end = dateTo.Value;

            Log = null;
            txtPreviewLog.AppendText($"Generating git log for {Path}, please wait...");

            try {
                var txtExporter = new TxtExporter();
                Log = txtExporter.ExportGitLog(Path, _start, _end);
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
                return;
            }

            if (Log != null) {
                txtPreviewLog.Clear();
                txtPreviewLog.AppendText(Log);
                btnSaveGitLog.Enabled = true;
            }
        }

        private void btnSaveGitLog_Click(object sender,
                                         EventArgs e) {
            var dialog = new SaveFileDialog {OverwritePrompt = false, CreatePrompt = false, InitialDirectory = Path};

            if (fmtTxt.Checked) {
                using (dialog) {
                    dialog.FileName +=
                        $"Changes to {TxtExporter.ProjectName} from {_start.ToString("yyyy-MM-dd")} to {_end.ToString("yyyy-MM-dd")}.txt";
                    dialog.Filter = "Text files (*.txt)|*.txt";

                    if (dialog.ShowDialog() == DialogResult.OK) {
                        File.WriteAllText(dialog.FileName, Log);
                    }
                }
            }
            else {
                using (dialog) {
                    dialog.FileName +=
                        $"Changes to {TxtExporter.ProjectName} from {_start.ToString("yyyy-MM-dd")} to {_end.ToString("yyyy-MM-dd")}.xlsx";
                    dialog.Filter = "Microsoft Excel files (*.xlsx)|*.xlsx";

                    if (dialog.ShowDialog() == DialogResult.OK) {
                        var excelExporter = new ExcelExporter();
                        excelExporter.ExportGitLog(Path, dialog.FileName, _start, _end);
                    }
                }
            }
        }

        private void InitializeDates() {
            _start = DateTime.Now.DayOfWeek == DayOfWeek.Monday
                         ? DateTime.Now.Date
                         : DateTime.Today.Previous(DayOfWeek.Monday);

            _end = DateTime.Now.DayOfWeek == DayOfWeek.Saturday
                       ? DateTime.Now.Date
                       : DateTime.Today.Next(DayOfWeek.Saturday);
        }
    }
}

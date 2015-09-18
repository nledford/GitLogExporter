using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using GitLogExporterGUI.Extensions;
using LibGit2Sharp;
using OfficeOpenXml;

namespace GitLogExporterGUI.Exporters {
    public class ExcelExporter {
        private List<Commit> _commits;
        private DateTime _start;
        private DateTime _end;
        private Repository _repo;

        public static string ProjectName { get; private set; }

        public void ExportGitLog(string path, string fileName,
                                 DateTime from,
                                 DateTime to) {
            _start = from;
            _end = to;
            ProjectName = string.Empty;
            FileInfo report;

            using (_repo = new Repository(path)) {
                ProjectName = _repo.Config.Get<string>("core.ProjectName").Value;

                _commits = (from c in _repo.Commits
                            where c.Committer.When.DateTime >= _start && c.Committer.When.DateTime <= _end
                            orderby c.Committer.When.DateTime descending
                            select c).ToList();

                report = new FileInfo(Path.Combine(fileName));
                if (report.Exists) {
                    report.Delete();
                    report = new FileInfo(fileName);
                }

                using (var package = new ExcelPackage(report)) {
                    foreach (var day in DateTimeExtensions.EachDay(_start, _end)) {
                        var currentCommits = _commits.Where(c => c.Committer.When.DateTime.Date == day.Date);

                        if (!currentCommits.Any()) {
                            continue;
                        }

                        var ws = package.Workbook.Worksheets.Add(day.ToString("dddd"));

                        // Headers
                        ws.Cells [1, 1, 1, 2].Merge = true;
                        ws.Cells [1, 1].Value = currentCommits.First().Committer.When.DateTime.ToString("D");

                        ws.Cells [2, 1, 2, 2].Merge = true;
                        ws.Cells [2, 1].Value = $"Commits: {currentCommits.Count()}";

                        ws.Cells [4, 1, 4, 2].Merge = true;
                        ws.Cells [4, 1].Value = $"Total Commits: {_commits.Count}";

                        ws.Cells [5, 1, 5, 2].Merge = true;
                        ws.Cells [5, 1].Value = $"Avg Commits Per Day: {Commits.CalculateAverageCommitsPerDay(_commits, _start, _end)}";

                        for (var i = 7; i < currentCommits.Count() + 7; i++) {
                            BuildCommit(ws, i, currentCommits.ElementAt(i - 7));
                        }

                        ws.Cells[ws.Dimension.Address].AutoFitColumns();
                    }

                    package.SaveAs(report);
                }
            }
        }

        private void BuildCommit(ExcelWorksheet ws, int currentRow, Commit commit) {
            ws.Cells [currentRow, 1].Value = commit.Committer.When.DateTime.ToString("h:mm:ss tt");
            ws.Cells [currentRow, 2].Value = commit.Message;
        }
    }
}

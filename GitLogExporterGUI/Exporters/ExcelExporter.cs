using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GitLogExporterGUI.Extensions;
using LibGit2Sharp;
using OfficeOpenXml;

namespace GitLogExporterGUI.Exporters {
    public class ExcelExporter {
        private List<Commit> _commits;
        private DateTime _end;
        private Repository _repo;
        private DateTime _start;

        public static string ProjectName { get; private set; }

        public void ExportGitLog(string path,
                                 string fileName,
                                 DateTime from,
                                 DateTime to) {
            _start = from;
            _end = to;
            ProjectName = string.Empty;

            using (_repo = new Repository(path)) {
                ProjectName = _repo.Config.Get<string>("core.ProjectName").Value;

                _commits = (from c in _repo.Commits
                            where c.Committer.When.DateTime >= _start && c.Committer.When.DateTime <= _end
                            orderby c.Committer.When.DateTime descending
                            select c).ToList();

                var report = new FileInfo(Path.Combine(fileName));
                if (report.Exists) {
                    report.Delete();
                    report = new FileInfo(fileName);
                }

                using (var package = new ExcelPackage(report)) {
                    if (_commits.Any()) {
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

                            ws.Cells [2, 3, 2, 4].Merge = true;
                            ws.Cells [2, 3].Value = $"Total Commits: {_commits.Count}";

                            ws.Cells [2, 5, 2, 6].Merge = true;
                            ws.Cells [2, 5].Value =
                                $"Avg Per Day: {Commits.CalculateAverageCommitsPerDay(_commits, _start, _end)}";

                            ws.View.FreezePanes(3,1);

                            BuildCommits(currentCommits, ws);

                            ws.Cells [ws.Dimension.Address].AutoFitColumns();
                        }
                    }
                    else {
                        var ws = package.Workbook.Worksheets.Add("No Commits Found");

                        ws.Cells [1, 1, 1, 2].Merge = true;
                        ws.Cells [1, 1].Value = "No Commits Found";

                        ws.Cells [1, 4, 1, 5].Merge = true;
                        ws.Cells [1, 4].Value = $"Total Commits: {_commits.Count}";

                        ws.Cells [2, 4, 2, 5].Merge = true;
                        ws.Cells [2, 4].Value = $"Avg Per Day: 0";
                    }

                    package.SaveAs(report);
                }
            }
        }

        private void BuildCommits(IEnumerable<Commit> currentCommits,
                                  ExcelWorksheet ws) {
            for (var i = 7; i < currentCommits.Count() + 7; i++) {
                BuildCommit(ws, i, currentCommits.ElementAt(i - 7));
            }
        }

        private void BuildCommit(ExcelWorksheet ws,
                                 int currentRow,
                                 Commit commit) {
            ws.Cells [currentRow, 1].Value = commit.Committer.When.DateTime.ToString("h:mm:ss tt");
            ws.Cells [currentRow, 2].Value = commit.Message;
        }
    }
}

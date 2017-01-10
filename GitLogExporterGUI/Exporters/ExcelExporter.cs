using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GitLogExporterGUI.Extensions;
using LibGit2Sharp;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace GitLogExporterGUI.Exporters
{
    public class ExcelExporter
    {
        private IList<Commit> _commits;
        private DateTime _end;
        private Repository _repo;
        private DateTime _start;

        public static string ProjectName { get; private set; }

        /// <summary>
        ///     Exports git log to an Excel spreadsheet
        /// </summary>
        /// <param name="path">The path to the git repositiory</param>
        /// <param name="fileName">The desired filename of the generated spreadsheet</param>
        /// <param name="from">The starting date</param>
        /// <param name="to">The ending date</param>
        public void ExportGitLog(string path,
            string fileName,
            DateTime from,
            DateTime to)
        {
            _start = from;
            _end = to;
            ProjectName = string.Empty;

            using (_repo = new Repository(path))
            {
                ProjectName = _repo.Config.Get<string>("core.ProjectName").Value;

                _commits = (from c in _repo.Commits
                    where c.Committer.When.DateTime >= _start && c.Committer.When.DateTime <= _end
                    orderby c.Committer.When.DateTime descending
                    select c).ToList();

                var report = new FileInfo(Path.Combine(fileName));
                if (report.Exists)
                {
                    report.Delete();
                    report = new FileInfo(fileName);
                }

                using (var package = new ExcelPackage(report))
                {
                    if (_commits.Any())
                    {
                        foreach (var day in DateTimeExtensions.EachDay(_start, _end))
                        {
                            var currentCommits =
                                _commits.Where(c => c.Committer.When.DateTime.Date == day.Date).ToList();

                            if (!currentCommits.Any()) continue;

                            var ws = package.Workbook.Worksheets.Add(day.ToString("dddd, MM-dd"));

                            // Headers
                            ws.Cells[1, 1, 1, 2].Merge = true;
                            var titleCell = ws.Cells[1, 1];
                            titleCell.Value = currentCommits.First().Committer.When.DateTime.ToString("D");
                            titleCell.Style.Font.Bold = true;

                            ws.Cells[2, 1, 2, 2].Merge = true;
                            ws.Cells[2, 1].Value = $"Commits: {currentCommits.Count}";

                            ws.Cells[2, 3, 2, 4].Merge = true;
                            ws.Cells[2, 3].Value = $"Total Commits: {_commits.Count}";

                            ws.Cells[2, 5, 2, 6].Merge = true;
                            ws.Cells[2, 5].Value =
                                $"Avg Per Day: {Commits.CalculateAverageCommitsPerDay(_commits, _start, _end)}";

                            ws.View.FreezePanes(3, 1);

                            BuildCommits(currentCommits, ws, 4);

                            ws.Cells[ws.Dimension.Address].AutoFitColumns();

                            ws.Column(2).Width = 60;
                        }
                    }
                    else
                    {
                        var ws = package.Workbook.Worksheets.Add("No Commits Found");

                        ws.Cells[1, 1, 1, 2].Merge = true;
                        ws.Cells[1, 1].Value = "No Commits Found";

                        ws.Cells[1, 4, 1, 5].Merge = true;
                        ws.Cells[1, 4].Value = $"Total Commits: {_commits.Count}";

                        ws.Cells[2, 4, 2, 5].Merge = true;
                        ws.Cells[2, 4].Value = $"Avg Per Day: 0";
                    }

                    package.SaveAs(report);
                }
            }
        }

        /// <summary>
        ///     Builds all commits for a given date
        /// </summary>
        /// <param name="currentCommits">A ReadOnlyCollection of commits for a given date</param>
        /// <param name="ws">The current worksheet</param>
        /// <param name="startRow">The row from which to start building commit rows (One-based index)</param>
        private static void BuildCommits(IReadOnlyCollection<Commit> currentCommits,
            ExcelWorksheet ws,
            int startRow)
        {
            for (var i = startRow; i < currentCommits.Count + startRow; i++)
                BuildCommit(ws, i, currentCommits.ElementAt(i - startRow));
        }

        /// <summary>
        ///     Builds a commit row in an Excel spreadsheet
        /// </summary>
        /// <param name="ws">The current worksheet</param>
        /// <param name="currentRow">The current row from the worksheet</param>
        /// <param name="commit">The current commit</param>
        private static void BuildCommit(ExcelWorksheet ws,
            int currentRow,
            Commit commit)
        {
            ws.Cells[currentRow, 1].Value = commit.Committer.When.DateTime.ToString("h:mm:ss tt");
            ws.Cells[currentRow, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            var commitCell = ws.Cells[currentRow, 2];
            commitCell.Value = commit.Message;
            commitCell.Style.WrapText = true;
            commitCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Justify;

            ws.Row(currentRow).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        }
    }
}
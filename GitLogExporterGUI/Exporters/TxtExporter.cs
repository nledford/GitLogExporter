using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LibGit2Sharp;

namespace GitLogExporterGUI.Exporters {
    public class TxtExporter {
        private static List<Commit> _commits;
        private static string _divider;
        private static readonly StringBuilder Sb = new StringBuilder();
        private static DateTime _end;
        private static Repository _repo;
        private static DateTime _start;

        public static string ProjectName { get; private set; }

        /// <summary>
        ///     Exports git log to a string.  Built using a StringBuilder.
        /// </summary>
        /// <param name="path">The path to the git repositiory</param>
        /// <param name="from">The starting date</param>
        /// <param name="to">The ending date</param>
        /// <returns>String containing exported git log</returns>
        public string ExportGitLog(string path,
                                   DateTime from,
                                   DateTime to) {
            _start = from;
            _end = to;

            Sb.Clear();
            _commits.Clear();
            ProjectName = string.Empty;

            using (_repo = new Repository(path)) {
                ProjectName = _repo.Config.Get<string>("core.ProjectName").Value;

                _commits =
                    _repo.Commits.Where(c => c.Committer.When.DateTime >= _start && c.Committer.When.DateTime <= _end)
                         .OrderByDescending(c => c.Committer.When.DateTime)
                         .ToList();

                BuildReportHeader(ProjectName);

                if (!_commits.Any()) {
                    return Sb.ToString();
                }

                BuildCommitDivider();
                BuildCommits();
            }

            return Sb.ToString();
        }

        /// <summary>
        ///     Builds a string of hypens used to divide individual commits
        /// </summary>
        private static void BuildCommitDivider() {
            var dividerLength = (from c in _commits
                                 orderby c.Message.Length descending
                                 select c.Message.Length).First();
            var length = dividerLength > 80 ? 80 : dividerLength;

            _divider = "";
            for (var i = 0; i < length; i++) {
                _divider += "-";
            }
        }

        /// <summary>
        ///     Builds the initial lines of the report, containing the project name, the date range, and commit stats
        /// </summary>
        /// <param name="projectName">The name of the project</param>
        private static void BuildReportHeader(string projectName) {
            Sb.AppendLine($"Git log for {projectName} from {_start.ToShortDateString()} to {_end.ToShortDateString()}");
            Sb.AppendLine($"Total Commits: {_commits.Count}");
            Sb.AppendLine($"Average Commits Per Day: {Commits.CalculateAverageCommitsPerDay(_commits, _start, _end)}");
            Sb.AppendLine();
        }

        /// <summary>
        ///     Adds all relevent info from git log to a StringBuilder
        /// </summary>
        private static void BuildCommits() {
            var previousDate = _commits.First().Committer.When.DateTime;
            Sb.AppendLine($"{previousDate.ToString("D")}");
            Sb.AppendLine(
                $"Total commits: {_commits.Count(c => c.Committer.When.DateTime.Date == _commits.First().Committer.When.DateTime.Date)}");
            Sb.AppendLine();

            foreach (var commit in _commits) {
                if (commit.Committer.When.DateTime.Date != previousDate.Date) {
                    Sb.AppendLine();
                    Sb.AppendLine($"\n{commit.Committer.When.DateTime.ToString("D")}");
                    Sb.AppendLine(
                        $"Total commits: {_commits.Count(c => c.Committer.When.DateTime.Date == commit.Committer.When.DateTime.Date)}");
                    Sb.AppendLine();
                }

                BuildCommitBlock(commit);

                previousDate = commit.Committer.When.DateTime;
            }
        }

        /// <summary>
        ///     Builds an individual commit block
        /// </summary>
        /// <param name="commit">An individual commit</param>
        private static void BuildCommitBlock(Commit commit) {
            var dateAndTime = $"{commit.Committer.When.DateTime.ToString("h:mm:ss tt")}";

            var author = $"{commit.Committer.Name} <{commit.Committer.Email}>";
            var message = $"{commit.Message.Trim()}";

            Sb.AppendLine($"{dateAndTime} - {author}");
            Sb.AppendLine(message);
            Sb.AppendLine(_divider);
        }
    }
}

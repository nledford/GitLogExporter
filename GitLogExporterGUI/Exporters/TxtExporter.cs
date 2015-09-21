using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LibGit2Sharp;

namespace GitLogExporterGUI.Exporters {
    public class TxtExporter {
        private readonly StringBuilder _sb = new StringBuilder();
        private List<Commit> _commits;
        private string _divider;
        private DateTime _end;
        private Repository _repo;
        private DateTime _start;

        public static string ProjectName { get; private set; }

        public string ExportGitLog(string path,
                                   DateTime from,
                                   DateTime to) {
            _start = from;
            _end = to;

            _sb.Clear();
            ProjectName = string.Empty;

            using (_repo = new Repository(path)) {
                ProjectName = _repo.Config.Get<string>("core.ProjectName").Value;

                _commits =
                    _repo.Commits.Where(c => c.Committer.When.DateTime >= _start && c.Committer.When.DateTime <= _end)
                         .OrderByDescending(c => c.Committer.When.DateTime)
                         .ToList();

                BuildReportHeader(ProjectName);

                if (!_commits.Any()) {
                    return _sb.ToString();
                }

                BuildCommitDivider();
                BuildCommits();
            }

            return _sb.ToString();
        }

        /// <summary>
        ///     Builds a string of hypens used to divide individual commits
        /// </summary>
        private void BuildCommitDivider() {
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
        private void BuildReportHeader(string projectName) {
            _sb.AppendLine($"Git log for {projectName} from {_start.ToShortDateString()} to {_end.ToShortDateString()}");
            _sb.AppendLine($"Total Commits: {_commits.Count}");
            _sb.AppendLine($"Average Commits Per Day: {Commits.CalculateAverageCommitsPerDay(_commits, _start, _end)}");
            _sb.AppendLine();
        }

        private void BuildCommits() {
            var previousDate = _commits.First().Committer.When.DateTime;
            _sb.AppendLine($"{previousDate.ToString("D")}");
            _sb.AppendLine(
                $"Total commits: {_commits.Count(c => c.Committer.When.DateTime.Date == _commits.First().Committer.When.DateTime.Date)}");
            _sb.AppendLine();

            foreach (var commit in _commits) {
                if (commit.Committer.When.DateTime.Date != previousDate.Date) {
                    _sb.AppendLine();
                    _sb.AppendLine($"\n{commit.Committer.When.DateTime.ToString("D")}");
                    _sb.AppendLine(
                        $"Total commits: {_commits.Count(c => c.Committer.When.DateTime.Date == commit.Committer.When.DateTime.Date)}");
                    _sb.AppendLine();
                }

                BuildCommitBlock(commit);

                previousDate = commit.Committer.When.DateTime;
            }
        }

        /// <summary>
        ///     Builds an individual commit block
        /// </summary>
        /// <param name="commit">An individual commit</param>
        private void BuildCommitBlock(Commit commit) {
            var dateAndTime = $"{commit.Committer.When.DateTime.ToString("h:mm:ss tt")}";

            var author = $"{commit.Committer.Name} <{commit.Committer.Email}>";
            var message = $"{commit.Message.Trim()}";

            _sb.AppendLine($"{dateAndTime} - {author}");
            _sb.AppendLine(message);
            _sb.AppendLine(_divider);
        }
    }
}

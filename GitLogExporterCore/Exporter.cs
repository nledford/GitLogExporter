using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GitLogExporterCore.Extensions;
using LibGit2Sharp;

namespace GitLogExporterCore {
    public class Exporter {
        private static readonly StringBuilder Sb = new StringBuilder();
        private Repository _repo;
        private static string _divider;

        private List<Commit> _commits; 

        private DateTime _start;
        private DateTime _end;

        public async void ExportGitLog(string path,
                                 DateTime from,
                                 DateTime to) {
            _start = from;
            _end = to;

            using (_repo = new Repository(path)) {
                var projectName = _repo.Config.Get<string>("core.ProjectName").Value;

                _commits =
                    _repo.Commits.Where(c => c.Committer.When.DateTime >= from && c.Committer.When.DateTime <= to)
                         .OrderByDescending(c => c.Committer.When.DateTime)
                         .ToList();

                await Task.Run(() => BuildReportHeader(projectName));

                await Task.Run(() => BuildCommitDivider());

                await Task.Run(() => BuildCommits());
            }
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
            Sb.AppendLine($"Git log for {projectName} from {_start.ToShortDateString()} to {_end.ToShortDateString()}");
            Sb.AppendLine($"Total Commits: {_commits.Count()}");
            Sb.AppendLine($"Average Commits Per Day: {CalculateAverageCommitsPerDay()}");
            Sb.AppendLine();
        }

        private async void BuildCommits() {
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

                await Task.Run(() => BuildCommitBlock(commit));

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

            Sb.AppendLine($"{dateAndTime} - {author}");
            Sb.AppendLine(message);
            Sb.AppendLine(_divider);
        }

        private string CalculateAverageCommitsPerDay() {
            var commitsPerDay = new List<int>();
            var hasCommits = false;

            foreach (var day in DateTimeExtensions.EachDay(_start, _end)) {
                if (day.Date > DateTime.Now.Date) {
                    break;
                }

                var count = _commits.Count(c => c.Committer.When.DateTime.Date == day.Date);

                if (count <= 0 && !hasCommits) {
                    continue;
                } else if (count > 0) {
                    hasCommits = true;
                    commitsPerDay.Add(count);
                }
            }

            var result = Math.Round(commitsPerDay.Average(), 2).ToString(CultureInfo.CurrentCulture);

            return result;
        }
    }
}

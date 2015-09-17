using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using LibGit2Sharp;

namespace GitLogExporter {
    public class Program {
        private static readonly StringBuilder Sb = new StringBuilder();
        private static Repository _repo;
        private static string _divider;

        private static void Main(string[] args) {
            Console.WriteLine("Git Log Exporter v1.0.0");

            Console.Write("Enter path of repository to export log: ");
            var result = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(result)) {
                Console.WriteLine("Git Repository Path was not provided. Exiting...");
                Console.ReadKey();
                return;
            }

            var path = Path.GetFullPath(result);
            Console.WriteLine($"Opening repositiory: \"{path}\"...");

            var start = DateTime.Now.DayOfWeek == DayOfWeek.Monday
                            ? DateTime.Now
                            : DateTime.Today.Previous(DayOfWeek.Monday);
            var end = DateTime.Now.DayOfWeek == DayOfWeek.Saturday
                          ? DateTime.Now
                          : DateTime.Today.Next(DayOfWeek.Saturday);

            using (_repo = new Repository(path)) {
                var projectName = _repo.Config.Get<string>("core.ProjectName").Value;

                var commits = (from c in _repo.Commits
                               where c.Committer.When.DateTime >= start && c.Committer.When.DateTime <= end
                               orderby c.Committer.When.DateTime descending
                               select c);

                Sb.AppendLine($"Git log for {projectName} from {start.ToShortDateString()} to {end.ToShortDateString()}");
                Sb.AppendLine($"Total Commits: {commits.Count()}");
                Sb.AppendLine($"Average Commits Per Day: {CalculateAverageCommitsPerDay(commits, start, end)}");
                Sb.AppendLine();

                BuildCommitDivider(commits);

                var previousDate = commits.First().Committer.When.DateTime;
                Sb.AppendLine($"{previousDate.ToString("D")}");
                Sb.AppendLine(
                    $"Total commits: {commits.Count(c => c.Committer.When.DateTime.Date == commits.First().Committer.When.DateTime.Date)}");
                Sb.AppendLine();

                foreach (var commit in commits) {
                    if (commit.Committer.When.DateTime.Date != previousDate.Date) {
                        Sb.AppendLine();
                        Sb.AppendLine($"\n{commit.Committer.When.DateTime.ToString("D")}");
                        Sb.AppendLine(
                            $"Total commits: {commits.Count(c => c.Committer.When.DateTime.Date == commit.Committer.When.DateTime.Date)}");
                        Sb.AppendLine();
                    }

                    BuildCommitBlock(Sb, commit);

                    previousDate = commit.Committer.When.DateTime;
                }

                Console.WriteLine(Sb.ToString());

                Console.Write("Export git log to text file? (y/n) ");
                var writeToFile = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(writeToFile) && writeToFile.ToUpperInvariant() == "Y") {
                    File.WriteAllText(@path + $"\\Commit Log for {projectName}.txt", Sb.ToString());

                    Console.WriteLine("File saved successfully!");
                }
            }
        }

        private static void BuildCommitBlock(StringBuilder sb,
                                             Commit commit) {
            var dateAndTime = $"{commit.Committer.When.DateTime.ToString("h:mm:ss tt")}";

            var author = $"{commit.Committer.Name} <{commit.Committer.Email}>";
            var message = $"{commit.Message.Trim()}";

            sb.AppendLine($"{dateAndTime} - {author}");
            sb.AppendLine(message);
            sb.AppendLine(_divider);
        }

        private static void BuildCommitDivider(IEnumerable<Commit> commits) {
            var dividerLength = (from c in commits
                                 orderby c.Message.Length descending
                                 select c.Message.Length).First();

            var length = dividerLength > 80 ? 80 : dividerLength;

            _divider = "";
            for (var i = 0; i < length; i++) {
                _divider += "-";
            }
        }

        private static string CalculateAverageCommitsPerDay(IEnumerable<Commit> commits,
                                                            DateTime start,
                                                            DateTime end) {
            var result =
                Math.Round(
                    DateTimeExtensions.EachDay(start, end)
                                      .TakeWhile(day => day.Date <= DateTime.Now.Date)
                                      .Select(day => commits.Count(c => c.Committer.When.DateTime.Date == day.Date))
                                      .ToArray()
                                      .Average(),
                    2).ToString(CultureInfo.CurrentCulture);

            return result;
        }
    }
}

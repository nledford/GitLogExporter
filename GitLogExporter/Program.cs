using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using GitLogExporter.Extensions;
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

                Sb.AppendLine(
                    $"Git log for {projectName} from {start.ToShortDateString()} to {end.ToShortDateString()}\n");

                var commits = (from c in _repo.Commits
                               where c.Committer.When.DateTime >= start && c.Committer.When.DateTime <= end
                               orderby c.Committer.When.DateTime descending
                               select c);

                BuildCommitDivider(commits);

                var previousDate = commits.First().Committer.When.DateTime;
                Sb.AppendLine($"{previousDate.ToString("D")}\n");
                foreach (var commit in commits) {
                    if (commit.Committer.When.DateTime.Date != previousDate.Date) {
                        Sb.AppendLine($"\n{commit.Committer.When.DateTime.ToString("D")}\n");
                    }

                    BuildCommitBlock(Sb, commit);

                    previousDate = commit.Committer.When.DateTime;
                }

                Console.WriteLine(Sb.ToString());
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
    }
}

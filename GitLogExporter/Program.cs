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
        private static Commit _last;

        private static void Main(string[] args) {
            Console.WriteLine("Git Log Exporter v1.0.0");

            if (!args.Any()) {
                Console.WriteLine("Git Repository Path was not provided. Exiting...");
                Console.ReadKey();
                return;
            }

            var path = Path.GetFullPath(args.First());
            Console.WriteLine($"Opening repositiory: \"{path}\"...");

            var start = DateTime.Now.DayOfWeek == DayOfWeek.Monday
                            ? DateTime.Now
                            : DateTime.Today.Previous(DayOfWeek.Monday);
            var end = DateTime.Now.DayOfWeek == DayOfWeek.Saturday
                          ? DateTime.Now
                          : DateTime.Today.Next(DayOfWeek.Saturday);

            using (var repo = new Repository(path)) {
                var projectName = repo.Config.Get<string>("core.ProjectName").Value;

                Sb.AppendLine(
                    $"Git log for {projectName} from {start.ToShortDateString()} to {end.ToShortDateString()}\n");

                var commits = (from c in repo.Commits
                               where c.Committer.When.DateTime >= start && c.Committer.When.DateTime <= end
                               orderby c.Committer.When.DateTime descending
                               select c);

                _last = commits.Last();

                var divider = BuildCommitDivider(commits);

                foreach (var commit in commits) {
                    var dateAndTime =
                        $"{commit.Committer.When.DateTime.ToString("D")} - {commit.Committer.When.DateTime.ToString("h:mm:ss tt")}";
                    var message = $"{commit.Message.Trim()}";

                    Sb.AppendLine(dateAndTime);
                    Sb.AppendLine(message);

                    if (!commit.Equals(_last)) {
                        Sb.AppendLine(divider);
                    }
                }

                Console.WriteLine(Sb.ToString());
            }

            //Console.ReadKey();
        }
        
        private static void BuildCommitBlock(StringBuilder sb,
                                             Commit commit) {
            var dateAndTime =
                        $"{commit.Committer.When.DateTime.ToString("D")} - {commit.Committer.When.DateTime.ToString("h:mm:ss tt")}";
            var message = $"{commit.Message.Trim()}";

            sb.AppendLine(dateAndTime);
            sb.AppendLine(message);

            if (!commit.Equals(_last)) {
                sb.AppendLine(divider);
            }
        }

        private static string BuildCommitDivider(IEnumerable<Commit> commits) {
            var dividerLength = (from c in commits
                                 orderby c.Message.Length descending
                                 select c.Message.Length).First();

            var length = dividerLength > 80 ? 80 : dividerLength;

            var divider = "";
            for (var i = 0; i < length; i++) {
                divider += "-";
            }

            return divider;
        }
    }
}

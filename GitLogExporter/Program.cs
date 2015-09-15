using System;
using System.IO;
using System.Linq;
using System.Text;
using GitLogExporter.Extensions;
using LibGit2Sharp;

namespace GitLogExporter {
    public class Program {
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

            var sb = new StringBuilder();

            sb.AppendLine($"Git log from {start.ToShortDateString()} to {end.ToShortDateString()}\n");

            using (var repo = new Repository(path)) {
                var commits = (from c in repo.Commits
                               where c.Committer.When.DateTime >= start && c.Committer.When.DateTime <= end
                               select c);

                foreach (var commit in commits) {
                    sb.AppendLine(
                        $"{commit.Committer.When.DateTime.ToString("D")} - {commit.Committer.When.DateTime.ToString("h:mm:ss tt")}");
                    sb.AppendLine($"{commit.Message}");
                }

                Console.WriteLine(sb.ToString());
            }

            //Console.ReadKey();
        }
    }
}

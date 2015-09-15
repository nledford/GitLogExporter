using System;
using System.Globalization;
using System.Linq;
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

            var path = args.First();
            Console.WriteLine($"Opening repositiory: \"{path}\"...");

            var today = DateTime.Today;
            var monday = DateTime.Now.DayOfWeek == DayOfWeek.Monday
                             ? DateTime.Now
                             : DateTime.Today.Previous(DayOfWeek.Monday);
            var saturday = DateTime.Now.DayOfWeek == DayOfWeek.Saturday
                               ? DateTime.Now
                               : DateTime.Today.Next(DayOfWeek.Saturday);

            using (var repo = new Repository(path)) {
                var i = 0;
                foreach (var commit in repo.Commits) {
                    Console.WriteLine($"{commit.Committer.When.Date} {commit.Message}");

                    i++;

                    if (i == 10) {
                        break;
                    }
                }
            }

            //Console.ReadKey();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibGit2Sharp;

namespace GitLogExporter {
    class Program {
        static void Main(string[] args) {
            Console.WriteLine("Git Log Exporter v1.0.0");

            //C:\Users\nledford\projects\dotnet\ifs\automation

            if (!args.Any()) {
                Console.WriteLine("Git Repository Path was not provided. Exiting...");
                Console.ReadKey();
                return;
            }

            var path = args.First();
            Console.WriteLine(path);

            using (var repo = new Repository(path)) {
                foreach (var commit in repo.Commits) {
                    Console.WriteLine($"{commit.Message}");
                }
            }

            Console.ReadKey();
        }
    }
}

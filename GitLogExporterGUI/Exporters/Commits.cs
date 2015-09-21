using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GitLogExporterGUI.Extensions;
using LibGit2Sharp;

namespace GitLogExporterGUI.Exporters {
    internal static class Commits {
        public static string CalculateAverageCommitsPerDay(IEnumerable<Commit> commits, DateTime start, DateTime end) {
            var commitsPerDay = new List<int>();
            var hasCommits = false;

            foreach (var day in DateTimeExtensions.EachDay(start, end)) {
                if (day.Date > DateTime.Now.Date) {
                    break;
                }

                var count = commits.Count(c => c.Committer.When.DateTime.Date == day.Date);

                if (count <= 0 && !hasCommits) { }
                if (count > 0) {
                    hasCommits = true;
                    commitsPerDay.Add(count);
                }
            }

            return commitsPerDay.Any() ? Math.Round(commitsPerDay.Average(), 2).ToString(CultureInfo.CurrentCulture) : "0";
        }
    }
}

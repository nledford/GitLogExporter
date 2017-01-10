using System;
using System.Collections.Generic;

namespace GitLogExporterGUI.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime Next(this DateTime from,
            DayOfWeek dayOfWeek)
        {
            var start = (int) from.DayOfWeek;
            var target = (int) dayOfWeek;
            if (target <= start) target += 7;
            return from.AddDays(target - start);
        }

        public static DateTime Previous(this DateTime from,
            DayOfWeek dayOfWeek)
        {
            var start = (int) from.DayOfWeek;
            var target = (int) dayOfWeek;
            if (target > start) target -= 7;
            return from.AddDays(target - start);
        }

        public static IEnumerable<DateTime> EachDay(DateTime from,
            DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1)) yield return day;
        }
    }
}
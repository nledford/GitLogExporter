using System;

namespace GitLogExporter.Extensions {
    public static class DateTimeExtensions {
        public static DateTime Next(this DateTime from,
                                    DayOfWeek dayOfWeek) {
            var start = (int) from.DayOfWeek;
            var target = (int) dayOfWeek;
            if (target <= start) {
                target += 7;
            }
            return from.AddDays(target - start);
        }

        public static DateTime Previous(this DateTime from,
                                        DayOfWeek dayOfWeek) {
            var start = (int) from.DayOfWeek;
            var target = (int) dayOfWeek;
            if (target > start) {
                target -= 7;
            }
            return from.AddDays(target - start);
        }
    }
}

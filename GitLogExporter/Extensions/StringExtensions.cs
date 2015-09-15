namespace GitLogExporter.Extensions {
    public static class StringExtensions {
        public static string UppercaseFirst(this string s) {
            if (string.IsNullOrWhiteSpace(s)) {
                return string.Empty;
            }
            return char.ToUpper(s [0]) + s.Substring(1);
        }
    }
}

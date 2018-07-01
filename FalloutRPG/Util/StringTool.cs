using System.Text.RegularExpressions;

namespace FalloutRPG.Util
{
    public static class StringTool
    {
        public static string Truncate(string source, int length)
        {
            if (source.Length > length)
                source = source.Substring(0, length);

            return source;
        }

        public static bool IsOnlyLetters(string source)
        {
            return Regex.IsMatch(source, @"^[a-zA-Z]+$");
        }
    }
}

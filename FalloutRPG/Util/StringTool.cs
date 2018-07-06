using System.Text.RegularExpressions;

namespace FalloutRPG.Util
{
    public static class StringTool
    {
        /// <summary>
        /// Shortens a string to the specified length.
        /// </summary>
        public static string Truncate(string source, int length)
        {
            if (source.Length > length)
                source = source.Substring(0, length);

            return source;
        }

        /// <summary>
        /// Checks if a string contains only letters.
        /// </summary>
        public static bool IsOnlyLetters(string source)
        {
            return Regex.IsMatch(source, @"^[a-zA-Z]+$");
        }
    }
}

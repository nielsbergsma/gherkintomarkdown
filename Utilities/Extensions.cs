using System.Collections.Generic;

namespace GherkinToMarkdown
{
    public static class StringExtensions
    {
        public static string TrimChar(this string input, char character)
        {
            if (input.StartsWith(character))
            {
                input = input.Substring(1);
            }
            
            if (input.EndsWith(character))
            {
                input = input.Substring(0, input.Length - 1);
            }

            return input;
        }
    }

    public static class ListExtensions
    {
        public static List<T> Purge<T>(this List<T> list)
        {
            var items = new List<T>(list);
            list.Clear();
            return items;
        }
    }
    
    public static class OptionalExtensions
    {
        public static Optional<string> Optional(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return Optional<string>.None();
            }
            else
            {
                return Optional<string>.Some(value);
            }
        }
    }
}
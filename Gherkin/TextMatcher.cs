using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace GherkinToMarkdown.Gherkin
{
    public interface TextMatch
    {
        string Text { get; }
        
        int Offset { get; }
    }

    public class UnmatchedText : TextMatch
    {
        private readonly string text;
        private readonly int offset;

        public UnmatchedText(string text, int offset)
        {
            this.text = text;
            this.offset = offset;
        }

        public string Text => text;

        public int Offset => offset;
    }
    
    public class MatchedText : TextMatch
    {
        private readonly string text;
        private readonly int offset;

        public MatchedText(string text, int offset)
        {
            this.text = text;
            this.offset = offset;
        }

        public string Text => text;

        public int Offset => offset;
    }
    
    public static class TextMatcher
    {
        public static IEnumerable<TextMatch> MatchSplit(Regex pattern, string input)
        {
            var matches = new List<TextMatch>();
            
            var offset = 0;
            foreach (var capture in Match(pattern, input))
            {
                if (offset < capture.Index)
                {
                    var text = input.Substring(offset, capture.Index - offset);
                    matches.Add(new UnmatchedText(text, offset));
                }

                matches.Add(new MatchedText(capture.Value, capture.Index));
                offset = capture.Index + capture.Value.Length;

            }

            var remainder = input.Substring(offset);
            if (!string.IsNullOrWhiteSpace(remainder))
            {
                matches.Add(new UnmatchedText(remainder, offset));
            }

            return matches;
        }

        private static List<Capture> Match(Regex pattern, string input)
        {
            var captures = new List<Capture>();
            
            var matches = pattern.Matches(input);
            for (var m = 0; m < matches.Count; m++)
            {
                var matchCaptures = matches[m].Captures;
                for (var mc = 0; mc < matchCaptures.Count; mc++)
                {
                    captures.Add(matchCaptures[mc]);   
                }
            }

            return captures;
        }
    }
}
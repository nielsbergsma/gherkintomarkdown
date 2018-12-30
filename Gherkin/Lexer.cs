using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace GherkinToMarkdown.Gherkin
{
    public class Lexer
    {
        private readonly Regex keywords = new Regex(@"(^|\n)([ \t\f]*)(Feature|""""""|Scenario(s| outline|template)?|Scenarios|Examples|Scenario|Background|Given|When|Then|And|But|Examples|@|#):?", RegexOptions.IgnoreCase);

        private Lexer()
        {
        }

        public static TokenStream LexString(string input)
        {
            return new Lexer().Lex(input);
        }
        
        private TokenStream Lex(string input)
        {
            var matches = TextMatcher
                .MatchSplit(keywords, input)
                .ToList();
            
            var tokens = new List<Token>();
            for (var m = 0; m < matches.Count; m++)
            {
                var part = matches[m];
                if (part is UnmatchedText)
                {
                    continue;
                }

                var keyword = SanatizeKeyword(part.Text);
                var tokenType = Token.ParseType(keyword);
                var content = m + 1 < matches.Count && matches[m+1] is UnmatchedText 
                    ? matches[m + 1].Text.Trim() 
                    : string.Empty;

                switch (tokenType)
                {
                    case TokenType.Unknown:
                        //ignore
                        break;
                    
                    case TokenType.Tag:
                        tokens.AddRange(
                            TokenizeTags(content).Select(t => new Token(TokenType.Tag, t))
                        );
                        break;
                    
                    case TokenType.Doc:
                        var doc = String.Empty;
                        for (m += 1; m < matches.Count; m++)
                        {
                            doc += matches[m].Text;
                            if (matches[m].Text.Contains(Token.DocSeparator))
                            {
                                doc = SanatizeContent(doc.Replace(Token.DocSeparator, ""));
                                tokens.Add(new Token(tokenType, doc));
                                break;
                            }
                        }
                        break;
                    
                    default:
                        tokens.Add(new Token(tokenType, SanatizeContent(content)));
                        break;
                }
            }
            
            return new TokenStream(tokens);
        }

        private IEnumerable<string> TokenizeTags(string input)
        {
            return input
                .Split(Token.TagSeparator)
                .Select(p => p.Trim())
                .Where(p => !string.IsNullOrEmpty(p))
                .ToList();
        }

        private string SanatizeKeyword(string keyword)
        {
            keyword = keyword.Trim();

            if (keyword.EndsWith(":"))
            {
                keyword = keyword.Substring(0, keyword.Length - 1).TrimEnd();
            }

            return keyword;
        }

        private string SanatizeContent(string content)
        {
            //remove indentation
            return string.Join("\n", content
                .Split("\n")
                .Select(l => l.Trim())
            );
        }

        
    }
}
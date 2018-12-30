using System.Collections.Generic;

namespace GherkinToMarkdown.Gherkin
{
    public enum TokenType
    {
        Unknown,
        Feature,
        Scenario,
        ScenarioOutline,
        Background,
        Given,
        When,
        Then,
        And,
        But,
        Tag,
        Comment,
        Doc,
        Example
    }

    public class Token
    {
        public const string DocSeparator = "\"\"\"";
        public const string TagSeparator = "@";

        private static readonly Dictionary<string, TokenType> keywordToTokenType = new Dictionary<string, TokenType> {
            { "#", TokenType.Comment },
            { "@", TokenType.Tag },
            { "\"\"\"", TokenType.Doc },
            { "feature", TokenType.Feature },
            { "scenario outline", TokenType.ScenarioOutline },
            { "scenario template", TokenType.ScenarioOutline },
            { "examples", TokenType.Example },
            { "scenarios", TokenType.Example },
            { "background", TokenType.Background },
            { "scenario", TokenType.Scenario },
            { "given", TokenType.Given },
            { "when", TokenType.When },
            { "then", TokenType.Then },
            { "and", TokenType.And },
            { "but", TokenType.But }
        };


        public static TokenType ParseType(string keyword)
        {
            var type = TokenType.Unknown;
            keywordToTokenType.TryGetValue(keyword.ToLower(), out type);
            return type;
        }
        
        private readonly TokenType type;
        private readonly string text;

        public Token(TokenType type, string text)
        {
            this.type = type;
            this.text = text;
        }

        public TokenType Type => type;

        public string Text => text;
    }
}
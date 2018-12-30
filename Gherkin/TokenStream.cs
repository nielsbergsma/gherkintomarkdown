using System.Collections.Generic;

namespace GherkinToMarkdown.Gherkin
{
    public class TokenStream
    {
        private readonly List<Token> tokens;
        private int index;

        public TokenStream(IEnumerable<Token> tokens)
        {
            this.tokens = new List<Token>(tokens);
        }

        public Token Head => HasHead ? tokens[index] : null;

        public bool HasHead => index < tokens.Count;

        public Token Accept()
        {
            var token = Head;
            index++;
            return token;
        }
    }
}
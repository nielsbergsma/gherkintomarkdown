using System.Collections.Generic;
using System.Linq;

namespace GherkinToMarkdown.Gherkin
{
    public class Example
    {
        private readonly List<string[]> rows;

        public Example(IEnumerable<string[]> rows)
        {
            this.rows = rows.ToList();
        }

        public string[] Header => rows.Count > 0 ? rows[0] : new string[0];

        public List<string[]> Data => rows.Skip(1).ToList();
    }
}
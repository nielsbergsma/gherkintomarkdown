using System;
using System.Collections.Generic;
using System.Linq;

namespace GherkinToMarkdown.MarkdownGenerator
{
    public static class MD
    {
        
        public static Fragment Fragment(params Element[] children)
        {
            return new Fragment(children);
        }
        
        public static Element Container(params object[] elements)
        {
            return new Container(elements.SelectMany(e =>
            {
                if (typeof(Element).IsAssignableFrom(e.GetType()))
                {
                    return new[] {e as Element};
                }
                if (typeof(IEnumerable<Element>).IsAssignableFrom(e.GetType()))
                {
                    return e as IEnumerable<Element>;
                }
                return new Element[0];
            }).ToArray());
        }
        
        public static TableOfContents TOC()
        {
            return new TableOfContents();
        }
        
        public static Header H1(params Element[] children)
        {
            return new Header(1, children);
        }
        
        public static Header H2(params Element[] children)
        {
            return new Header(2, children);
        }
        
        public static Header H3(params Element[] children)
        {
            return new Header(3, children);
        }
        
        public static Header H4(params Element[] children)
        {
            return new Header(4, children);
        }
        
        public static Element Empty()
        {
            return new Fragment();
        }

        public static Text Text(string format, params object[] arguments)
        {
            return new Text(EscapeText(string.Format(format, arguments)));
        }
        
        public static ListItem ListItem(params Element[] children)
        {
            return new ListItem(children);
        }
        
        public static Element NewLine()
        {
            return new Text("  \n");
        }
        
        public static Element Multiline(string text, Func<string, Element> transformation)
        {
            return Container(
                text.Split("\n").SelectMany(l => new[] {transformation(l), NewLine()})
            );
        }

        public static Emphasis Emphasis(params Element[] children)
        {
            return new Emphasis(children);
        }
        
        public static Strong Strong(params Element[] children)
        {
            return new Strong(children);
        }
        
        public static Link Link(string text, string link = null)
        {
            return new Link(text, link);
        }

        public static Table Table(string[] columns, IEnumerable<string[]> rows)
        {
            return new Table(columns, rows);
        }

        private static string EscapeText(string input)
        {
            return input
                .Replace("&", "\\&")
                .Replace("<", "\\<");
        } 
        
    }
}
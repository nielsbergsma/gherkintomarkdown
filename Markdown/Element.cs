using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GherkinToMarkdown.MarkdownGenerator
{
    public class Element
    {
        protected readonly List<Element> children;

        protected Element(params Element[] children)
        {
            this.children = new List<Element>(children);
        }

        public virtual void Write(StringBuilder stream)
        {
            foreach (var child in children)
            {
                child.Write(stream);
            }
        }
    }
    
    public class Fragment : Element
    {
        public Fragment(params Element[] children) : base(children)
        {
        }

        public void Append(params Element[] children)
        {
            this.children.AddRange(children);
        }
    }
    
    public class Container : Element
    {
        public Container(params Element[] children) : base(children)
        {
        }

        public void Append(params Element[] children)
        {
            this.children.AddRange(children);
        }

        public override void Write(StringBuilder stream)
        {
            base.Write(stream);
            stream.Append("  \n\n");
        }
    }
    
    public class Header : Element
    {
        private readonly int size;
        
        public Header(int size, params Element[] children) : base(children)
        {
            this.size = size;
        }

        public override void Write(StringBuilder stream)
        {
            stream.Append(new string('#', size) + " ");
            base.Write(stream);
            stream.Append("\n");
        }
    }

    public class Emphasis : Element
    {
        public Emphasis(params Element[] children) : base(children)
        {
        }

        public override void Write(StringBuilder stream)
        {
            stream.Append("_");
            base.Write(stream);
            stream.Append("_");
        }
    }
    
    public class Strong : Element
    {
        public Strong(params Element[] children) : base(children)
        {
        }

        public override void Write(StringBuilder stream)
        {
            stream.Append("__");
            base.Write(stream);
            stream.Append("__");
        }
    }

    public class Text : Element
    {
        private readonly string text;
        
        public Text(string text)
        {
            this.text = text;
        }

        public override void Write(StringBuilder stream)
        {
            stream.Append(text);
        }
    }

    public class Link : Element
    {
        private readonly string text;
        private readonly string link;

        public Link(string text, string link)
        {
            this.text = text;
            this.link = link;
        }

        public override void Write(StringBuilder stream)
        {
            stream.Append($"[{text}]");
            stream.Append($"({link ?? ""})");
        }
    }

    public class ListItem : Element
    {
        public ListItem(params Element[] children) : base(children)
        {
            
        }

        public override void Write(StringBuilder stream)
        {
            stream.Append("- ");
            base.Write(stream);
            stream.Append("  \n");
        }
    }

    public class Table : Element
    {
        private readonly string[] columns;
        private readonly IEnumerable<string[]> rows;

        public Table(string[] columns, IEnumerable<string[]> rows)
        {
            this.columns = columns;
            this.rows = rows;
        }

        public override void Write(StringBuilder stream)
        {
            stream.Append("\nExamples  \n\n");
            stream.Append("| " + string.Join(" | ",columns) + " |  \n");
            stream.Append("| " + string.Join(" | ",columns.Select(c => "---")) + " |  \n");

            foreach (var row in rows)
            {
                stream.Append("| " + string.Join(" | ",row) + " |  \n");
            }
        }
    }

    public class TableOfContents : Element
    {
        public override void Write(StringBuilder stream)
        {
            stream.Append("\n{:toc}\n");
        }
    }
}
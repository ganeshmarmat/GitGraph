using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace GitGraph
{
    public class Node
    {
        public Ui UI = new Ui();
        public List<string> Parent { get; set; } = new List<string>();
        public List<string> Child { get; set; } = new List<string>();
        public List<string> AbbrevParent { get; set; } = new List<string>();
        public List<string> AbbrevChild { get; set; } = new List<string>();
        public string Id { get; set; }
        public string AbbrevId { get; set; }
        public string Datestr { get; set; }
        public DateTime DateTime { get; set; }
        public string Subject { get; set; }
        public string AuthorName { get; set; }
        public string AuthorEmail { get; set; }
        public string CommitterName { get; set; }
        public string CommiterEmail { get; set; }
        public List<Node> ChildsObj { get; set; } = new List<Node>();
        public List<Node> ParentObj { get; set; } = new List<Node>();
    }

    public class Ui
    {
        public int Level { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }
        public System.Windows.Point StartPoint { get; set; }
        public List<Line> Links { get; set; } = new List<Line>();
        public bool IsSelected { get; set; }
    }
}

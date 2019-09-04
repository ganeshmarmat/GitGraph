using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace WpfApp1
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
    }
    
    public class Ui
    {
        public int Level;
        public int Row;
        public int Column;
        public double Height;
        public double Width;
        public System.Windows.Point StartPoint;
        public List<Line> Links=new List<Line>();
    }
}

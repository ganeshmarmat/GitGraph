using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp1
{
    public class Operations
    {
        const char seprator = '$';
        public double Height = 100;
        public double Width = 100;
        public List<string> tempList = new List<string>();
        public bool GenerateLogFile(string gitDir, string logFilePath)
        {
            RunCmd(gitDir, "/c git log --pretty=format:\" %H " + seprator + " %h " + seprator + " %P " + seprator + " %p " + seprator + " %an " + seprator + " %ae " + seprator + " %cn " + seprator + " %ce " + seprator + " %cd " + seprator + " %s \" > " + logFilePath);
            return true;
        }

        public List<Node> LoadNodeCollectionFromFile(string filePath)
        {
            List<Node> lstNode = new List<Node>();
            var allLines = File.ReadAllLines(filePath);
            foreach (var line in allLines)
            {
                Node objNode = new Node();
                var fields = line.Split(seprator).ToList();
                objNode.Id = fields[0].Trim();
                objNode.AbbrevId = fields[1].Trim();
                objNode.Parent = fields[2].Trim().Split(' ').ToList();
                objNode.AbbrevParent = fields[3].Trim().Split(' ').ToList();
                objNode.AuthorName = fields[4].Trim();
                objNode.AuthorEmail = fields[5].Trim();
                objNode.CommitterName = fields[6].Trim();
                objNode.CommiterEmail = fields[7].Trim();
                objNode.Datestr = fields[8].Trim();
                objNode.Subject = fields[9].Trim();
                objNode.AbbrevParent.RemoveAll(x => x == string.Empty);
                objNode.Parent.RemoveAll(x => x == string.Empty);
                lstNode.Add(objNode);
            }

            return lstNode;
        }
        private bool RunCmd(string dir, string argument)
        {
            try
            {
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
                startInfo.FileName = "cmd.exe";
                startInfo.WorkingDirectory = dir;
                startInfo.Arguments = argument;
                process.StartInfo = startInfo;
                process.Start();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public void ProcessParentList(List<Node> nodes)
        {
            var roots = nodes.Where(x => x.AbbrevParent.Count == 0);
            Node root = roots.Last();

            root.UI.Row = 0;
            root.UI.Column = 0;
            root.UI.StartPoint = new System.Windows.Point(0, 0);
            root.UI.Height = Height;
            root.UI.Width = Width;

            ParentFillData(nodes, root, null);

        }
        private void ParentFillData(List<Node> nodes, Node parent, Point? lastnodePoint)
        {
            if (tempList.Contains(parent.AbbrevId))
                return;
            tempList.Add(parent.AbbrevId);
            int _col = 0;
            int mid = 50;
            parent.UI.StartPoint = new System.Windows.Point(parent.UI.Row * 50, parent.UI.Column * 50);
            parent.UI.Height = Height;
            parent.UI.Width = Width;
            if (lastnodePoint != null)
            {
                Point lstpoint = (Point)lastnodePoint;
                parent.UI.Links.Add(new System.Windows.Shapes.Line() { X1 = lstpoint.X + mid, Y1 = lstpoint.Y + mid*2 });
                foreach (var link in parent.UI.Links)
                {
                    link.X2 = parent.UI.StartPoint.X + mid*2;
                    link.Y2 = parent.UI.StartPoint.Y +mid;
                }
            }
            var childs = nodes.Where(x => x.AbbrevParent.Contains(parent.AbbrevId)).ToList();
           
            foreach (var child in childs)
            {
                child.UI.Row = parent.UI.Row + 1;
                child.UI.Column = _col;
                ParentFillData(nodes, child,parent.UI.StartPoint);
                _col++;

            }
        }
    }
}

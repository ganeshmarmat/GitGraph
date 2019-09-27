using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GitGraph
{
    public class Operations
    {
        const char seprator = '$';
        public double Height = 50;
        public double Width = 100;
        int spacebetweenrows = 50;
        int spacebetweenColumns = 100;
        List<int> arrholdcol;
        string logtext = "";
        public List<string> tempList = new List<string>();
        private Queue<Node> Nodequeue = new Queue<Node>();
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

            root.UI.Row = 1;
            root.UI.Column = 1;
            root.UI.StartPoint = new System.Windows.Point(0, 0);
            root.UI.Height = Height;
            root.UI.Width = Width;

            CreateListwithChilds(nodes);
            arrholdcol = new List<int>();
            arrholdcol.Add(0);
            arrholdcol.Add(0);
            //Nodequeue.Enqueue(root);

            BFSSearchFillRowColumn(root);
            FillWithUILocation(nodes);
            //ParentFillData(nodes, root, null);
            //File.WriteAllText(@"E:\mysdtuff\GitGraph\skip.log", logtext.ToString());

        }
        public void CreateListwithChilds(List<Node> nodes)
        {
            foreach (var node in nodes)
            {
                var childs = nodes.Where(x => x.AbbrevParent.Contains(node.AbbrevId)).ToList();
                node.AbbrevChild = childs.Select(x => x.AbbrevId).ToList();
                node.Child = childs.Select(x => x.Id).ToList();
                node.ChildsObj = childs;
                foreach (var findchild in node.Parent)
                {
                    var parents = nodes.Where(x => x.Id == findchild).ToList();
                    if (parents != null)
                        node.ParentObj.AddRange(parents);
                }
            }

            //if (root.AbbrevChild != null)
            //    return; ;
            //var childs = nodes.Where(x => x.AbbrevParent.Contains(root.AbbrevId)).ToList();
            //root.AbbrevChild = childs.Select(x=>x.AbbrevId).ToList();
            //foreach (var child in childs)
            //{
            //    Nodequeue.Enqueue(child);
            //}
            //while (Nodequeue.Count != 0)
            //{
            //    Nodequeue.Dequeue();
            //}
        }
        private void BFSSearchFillRowColumn(Node currentnode)
        {
            if (tempList.Contains(currentnode.AbbrevId))
            {
                return;
            }
            tempList.Add(currentnode.AbbrevId);
            if (arrholdcol.Count-1 <= currentnode.UI.Row)
                arrholdcol.Add(0);
            foreach (var item in currentnode.ChildsObj)
            {
                if (!Nodequeue.Contains(item)&& !tempList.Contains(item.AbbrevId))
                {
                    item.UI.Row = currentnode.UI.Row + 1;
                    arrholdcol[item.UI.Row] += 1;
                    item.UI.Column = arrholdcol[item.UI.Row];
                    Nodequeue.Enqueue(item);
                }
            }
            while (Nodequeue.Count != 0)
            {
                BFSSearchFillRowColumn(Nodequeue.Dequeue());
            }
        }
        private void safeSearchforDuplicateRowCol()
        {

        }
        private void FillWithUILocation(List<Node> nodes)
        {
            foreach (var node in nodes)
            {
                node.UI.Height = Height;
                node.UI.Width = Width;
                node.UI.StartPoint=new Point(node.UI.Column * (Width + spacebetweenColumns), node.UI.Row * (Height + spacebetweenrows));
                foreach (var parent in node.ParentObj)
                {
                    node.UI.Links.Add(new System.Windows.Shapes.Line() {
                        X1 = node.UI.StartPoint.X + (Width / 2),
                        Y1= node.UI.StartPoint.Y,
                        X2= (parent.UI.Column * (Width + spacebetweenColumns)) + (Width/2),
                        Y2 =(parent.UI.Row * (Height + spacebetweenrows))+Height
                    });
                }
            }
        }
        private void ParentFillData(List<Node> nodes, Node parent, Point? lastnodePoint)
        {
            if (tempList.Contains(parent.AbbrevId))
            {
                //logtext.Append(parent.AbbrevId + "\n");
                //File.WriteAllText(@"E:\mysdtuff\GitGraph\skip.log", parent.AbbrevId + "\n");
                return;
            }
            tempList.Add(parent.AbbrevId);
            int _col = 1;
            int mid = (int)Width / 2;
            parent.UI.StartPoint = new System.Windows.Point((parent.UI.Column * Width) + spacebetweenColumns, ((Point)(lastnodePoint ?? new Point(0, 0))).Y + Height + spacebetweenrows);
            parent.UI.Height = Height;
            parent.UI.Width = Width;
            if (lastnodePoint != null)
            {
                Point lstpoint = (Point)lastnodePoint;
                parent.UI.Links.Add(new System.Windows.Shapes.Line() { X1 = lstpoint.X + mid, Y1 = lstpoint.Y + Height });
                foreach (var link in parent.UI.Links)
                {
                    link.X2 = parent.UI.StartPoint.X + mid;
                    link.Y2 = parent.UI.StartPoint.Y;
                }
            }
            var childs = nodes.Where(x => x.AbbrevParent.Contains(parent.AbbrevId)).ToList();

            foreach (var child in childs)
            {
                child.UI.Row = parent.UI.Row + 1;
                child.UI.Column = _col;
                if (!Nodequeue.Contains(child))
                    Nodequeue.Enqueue(child);
                //ParentFillData(nodes, child, parent.UI.StartPoint);
                _col++;

            }
            while (Nodequeue.Count != 0)
            {

                ParentFillData(nodes, Nodequeue.Dequeue(), parent.UI.StartPoint);
                //if (tempList.Contains(parent.AbbrevId))
                //{
                //    logtext=logtext+parent.AbbrevId + "\n";
                //}
            }
        }
    }
}

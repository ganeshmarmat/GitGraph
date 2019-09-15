using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            CommonData.GitLogFilePath = @"D:\IM_Clone\log.txt";
            CommonData.GitRepoPath = @"D:\IM_Clone\im\";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            double maxheight = 500;
            double maxwidth = 500;

            //Line n = new Line();
            //n.X1 = 10;
            //n.Y1 = 10;
            //n.X2 = 200;
            //n.Y2 = 200;
            //n.StrokeThickness = 20;

            //n.Stroke = new SolidColorBrush() { Color = Colors.Red };
            //Canvaswin.Children.Add(n);

            //new Operations().GenerateLogFile(CommonData.GitRepoPath, CommonData.GitLogFilePath);
            List<Node> nodeobj = new Operations().LoadNodeCollectionFromFile(@"E:\mysdtuff\GitGraph\GitGraph\\log.txt");//(CommonData.GitLogFilePath);
            new Operations().ProcessParentList(nodeobj);
            nodeobj.Reverse();
            foreach (var item in nodeobj)
            {
                NodeUI uiobj = new NodeUI();
                uiobj.Height = item.UI.Height;
                uiobj.Width = item.UI.Width;
                uiobj.DataContext = item;
                Canvas.SetTop(uiobj, item.UI.StartPoint.Y);
                Canvas.SetLeft(uiobj, item.UI.StartPoint.X);
                
                Canvaswin.Children.Add(uiobj);
                foreach (var link in item.UI.Links)
                {
                    link.Stroke = new SolidColorBrush() { Color = Colors.Blue };
                    link.StrokeThickness = 2;
                    link.SetValue(Canvas.ZIndexProperty, 2);
                    Canvaswin.Children.Add(link);
                }
                
               if(maxheight< item.UI.StartPoint.Y+100)
                {
                    maxheight = item.UI.StartPoint.Y + 100;
                }
                if (maxwidth < item.UI.StartPoint.X + 100)
                {
                    maxwidth = item.UI.StartPoint.X + 100;
                }
            }
            Canvaswin.Height = maxheight;
            //Scrollbarviewer.Height = maxheight;
            Canvaswin.Width = 10000;
        }
    }
}

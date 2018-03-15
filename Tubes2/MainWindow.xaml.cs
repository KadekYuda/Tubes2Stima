using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace Tubes2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    class ViewerSample
    {
        public static void view(List<List<string>> graphdetails)
        {
            //create a form 
            System.Windows.Forms.Form form = new System.Windows.Forms.Form();
            //create a viewer object 
            Microsoft.Msagl.GraphViewerGdi.GViewer viewer = new Microsoft.Msagl.GraphViewerGdi.GViewer();
            //create a graph object 
            Microsoft.Msagl.Drawing.Graph graph = new Microsoft.Msagl.Drawing.Graph("graph");
            //create the graph content 
            foreach(List<string> L in graphdetails)
            {
                graph.AddNode(L[0]);
                for (int i=1; i<L.Count; i++)
                {
                    graph.AddEdge(L[i], L[0]);
                }
            }
            //bind the graph to the viewer 
            viewer.Graph = graph;
            //associate the viewer with the form 
            form.Width = 600;
            form.Height = 600;
            form.SuspendLayout();
            viewer.Dock = System.Windows.Forms.DockStyle.Fill;
            form.Controls.Add(viewer);
            form.ResumeLayout();
            //show the form 
            form.Show();
        }
    }
    public class Parser
    {
        public List<List<string>> Parse(string filename)
        {
            string ret = "";
            List<List<string>> graphdetail = new List<List<string>>();
            try
            {
                // Create an instance of StreamReader to read from a file.
                // The using statement also closes the StreamReader.
                using (StreamReader sr = new StreamReader(filename))
                {
                    string line;
                    int numVertex = 0;
                    // Read and display lines from the file until 
                    // the end of the file is reached. 
                    while ((line = sr.ReadLine()) != null)
                    {
                        //Console.WriteLine(line);
                        ret += line;
                        numVertex++;
                    }
                    // Create an instance of StreamReader to read from a file.
                    // The using statement also closes the StreamReader
                    //iterate every char in ret to get vertex and egde
                    int iterator = 0;
                    string tempword = "";
                    List<string> tempVertex = new List<string>();
                    while (iterator <= ret.Length - 1)
                    {
                        char curr = ret[iterator];
                        if (curr == ',')
                        {
                            tempVertex.Add(tempword);
                            tempword = "";
                            iterator++;
                        }
                        else if (curr == '.')
                        {
                            tempVertex.Add(tempword);
                            tempword = "";
                            graphdetail.Add(tempVertex);
                            tempVertex = new List<string>();
                            iterator++;
                        }
                        else if (curr != ' ' && curr != '\n')
                        {
                            tempword += curr;
                            iterator++;
                        }
                        else
                        {
                            iterator++;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                // Let the user know what went wrong.
                MessageBox.Show($"The file could not be read: {e.Message}");
                return null;
            }
            return graphdetail;
        }
    }
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void apply_btn(object sender, RoutedEventArgs e)
        {
            Parser p = new Parser();
            List<List<string>> graphdetails = p.Parse(this.FileField.Text);
            if (graphdetails != null)
            {
                string output = "";
                foreach (List<string> L in graphdetails)
                {
                    output += $"Mata Kuliah: {L[0]}\n";
                    for (int i = 1; i < L.Count; i++)
                    {
                        output += $"\tPrequisite: {L[i]}\n";
                    }
                }
                GraphInfo.Text = output;
                ViewerSample.view(graphdetails);
            }
        }
        private void reset_btn(object sender, RoutedEventArgs e)
        {
            this.FileField.Text = String.Empty;
            GraphInfo.Text = "";
        }
        private void browse_btn(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if ((bool)openFileDialog1.ShowDialog())
            {
                FileField.Text = openFileDialog1.FileName;
            }
        }
    }
}

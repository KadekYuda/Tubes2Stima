using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    class GraphAnimation
    {
        public static void viewBFS(Queue<Tuple<String, int>> BFSRes, List<List<string>> graphdetail)
        {
            System.Windows.Forms.Form form = new System.Windows.Forms.Form();
            Microsoft.Msagl.GraphViewerGdi.GViewer viewer = new Microsoft.Msagl.GraphViewerGdi.GViewer();
            Microsoft.Msagl.Drawing.Graph graph = new Microsoft.Msagl.Drawing.Graph("graph");
            //bind the graph to the viewer 
            viewer.Graph = graph;
            form.Width = 600;
            form.Height = 600;
            form.Text = "BFS";
            //associate the viewer with the form 
            form.SuspendLayout();
            viewer.Dock = System.Windows.Forms.DockStyle.Fill;
            form.Controls.Add(viewer);
            form.ResumeLayout();
            //show the form 
            form.Show();
            int semester = 0;
            AnimateGraphBFS(2000, BFSRes, semester, (s, e) => {
                viewer.Graph = null;
                semester++;
                while (BFSRes.Count() > 0 && BFSRes.Peek().Item2 == semester)
                {
                    Tuple<String, int> currentTuple = BFSRes.Dequeue();
                    String currentMK = currentTuple.Item1;
                    graph.AddNode(currentMK);
                    // cari index dari 
                    int indexMK = graphdetail.FindIndex(L => L[0] == currentMK);
                    List<String> MKPreqList = graphdetail[indexMK];
                    for (int i = 1; i < MKPreqList.Count; i++)
                    {
                        graph.AddEdge(MKPreqList[i], MKPreqList[0]);
                    }
                }
                viewer.Graph = graph;
            });
            //timer
        }
        static void AnimateGraphBFS(double interval, Queue<Tuple<String, int>> BFSRes, int semester, System.Timers.ElapsedEventHandler handler)
        {
            var timer = new System.Timers.Timer(interval);
            timer.Elapsed += handler;
            timer.Elapsed += (s, e) => {
                if (BFSRes.Count() == 0)
                {
                    timer.Stop();
                }
                else
                {
                    semester++;
                }
            };
            timer.Start();
        }

        public static void viewDFS(DFSSorter DFSRes, List<List<String>> graphdetail)
        {
            // create a form
            System.Windows.Forms.Form form = new System.Windows.Forms.Form();
            // create viewer object
            Microsoft.Msagl.GraphViewerGdi.GViewer viewer = new Microsoft.Msagl.GraphViewerGdi.GViewer();
            // create a graph object
            Microsoft.Msagl.Drawing.Graph graph = new Microsoft.Msagl.Drawing.Graph("graph");
            // bind graph to viewer
            viewer.Graph = graph;
            // set form properties
            form.Text = "DFS";
            form.Height = 600;
            form.Width = 600;
            form.SuspendLayout();
            viewer.Dock = System.Windows.Forms.DockStyle.Fill;
            form.Controls.Add(viewer);
            form.ResumeLayout();
            //show the form 
            form.Show();
            int currentTimestamp = 1;
            int maxTimestamp = graphdetail.Count() * 2;
            int size = graphdetail.Count;
            List<bool> visited = new List<bool>(size);
            for(int i = 0; i < size; i++)
            {
                visited.Add(false);
            }
            AnimateGraphDFS(2000, currentTimestamp, maxTimestamp, (s, e) =>
            {
                while (!DFSSorter.Timestamp_Start.Contains(currentTimestamp))
                {
                    currentTimestamp++;
                }
                if (DFSSorter.Timestamp_Start.Contains(currentTimestamp))
                {
                    viewer.Graph = null;
                    int nodeIdx = DFSSorter.Timestamp_Start.IndexOf(currentTimestamp);
                    visited[nodeIdx] = true;
                    graph.AddNode(graphdetail[nodeIdx][0]);
                    for (int i = 0; i < graphdetail.Count; i++)
                    {
                        if (graphdetail[i].Contains(graphdetail[nodeIdx][0]))
                        {
                            if(i == nodeIdx)
                            {
                                for (int j = 1; j < graphdetail[i].Count; j++)
                                {
                                    int indexMK = graphdetail.FindIndex(L => L[0] == graphdetail[i][j]);
                                    if (visited[indexMK])
                                    {
                                        graph.AddEdge(graphdetail[i][j], graphdetail[i][0]);
                                    }
                                }
                            }
                            else
                            {
                                if (visited[i])
                                {
                                    graph.AddEdge(graphdetail[nodeIdx][0], graphdetail[i][0]);
                                }
                            }
                        }
                    }
                    currentTimestamp++;
                    viewer.Graph = graph;
                }
            });
        }
        static void AnimateGraphDFS(double interval, int Timestamp, int maxTimestamp, System.Timers.ElapsedEventHandler handler)
        {
            var timer = new System.Timers.Timer(interval);
            timer.Elapsed += handler;
            timer.Elapsed += (s, e) => {
                if (Timestamp == maxTimestamp)
                {
                    timer.Stop();
                }
            };
            timer.Start();
        }
    }
    class GraphViewer
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
            foreach (List<string> L in graphdetails)
            {
                graph.AddNode(L[0]);
                for (int i = 1; i < L.Count; i++)
                {
                    graph.AddEdge(L[i], L[0]);
                }
            }
            //bind the graph to the viewer 
            viewer.Graph = graph;
            //associate the viewer with the form 
            form.Width = 300;
            form.Height = 300;
            form.Text = "Graph";
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
        private void bfs_btn(object sender, RoutedEventArgs e)
        {
            Parser p = new Parser();
            List<List<string>> graphdetails = p.Parse(this.FileField.Text);
            if (graphdetails != null)
            {
                graphdetails = graphdetails.OrderBy(x => x[0]).ToList();
                Queue<Tuple<String, int>> BFSResult = BFSSorter.topoSortBFS(graphdetails);
                // print Mata Kuliah dan Prerequsitenya.
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
                // print urutan pengambilan semesteran
                string resultOut = "";
                int sem = 0;
                Queue<Tuple<String, int>> copyRes = new Queue<Tuple<string, int>>(BFSResult);
                while (copyRes.Count() > 0)
                {
                    sem++;
                    resultOut += $"Semester {sem}: ";
                    while (copyRes.Count() > 0 && copyRes.Peek().Item2 == sem)
                    {
                        Tuple<String, int> currTuple = copyRes.Dequeue();
                        resultOut += $"{currTuple.Item1}";
                        if (copyRes.Count() > 0 && copyRes.Peek().Item2 == sem)
                        {
                            resultOut += ", ";
                        }
                    }
                    resultOut += "\n";
                }
                GraphResult.Text = resultOut;

                // buat form baru untuk animasi BFS
                GraphAnimation.viewBFS(BFSResult, graphdetails);
            }
        }

        private void dfs_btn(object sender, RoutedEventArgs e)
        {
            Parser p = new Parser();
            List<List<string>> graphdetails = p.Parse(this.FileField.Text);
            if (graphdetails != null)
            {
                graphdetails = graphdetails.OrderBy(x => x[0]).ToList();
                DFSSorter DFSResult = new DFSSorter();
                DFSSorter.init_topoSort(graphdetails);
                DFSSorter.sort_semester();
                string output = "";
                int maxSem = DFSSorter.Semester.Max<int>();
                Debug.WriteLine($"maxSem = {maxSem}");
                for (int k = 0; k < DFSSorter.Semester.Count(); k++)
                {
                    Debug.WriteLine($"{graphdetails[k][0]} start {DFSSorter.Timestamp_Start[k]} stop {DFSSorter.Timestamp[k]} semester {DFSSorter.Semester[k]}");
                }
                foreach (List<string> L in graphdetails)
                {
                    output += $"Mata Kuliah: {L[0]}\n";
                    for (int i = 1; i < L.Count; i++)
                    {
                        output += $"\tPrequisite: {L[i]}\n";
                    }
                }
                GraphInfo.Text = output;
                string outputRes = "";
                for (int j = 1; j <= maxSem; j++)
                {
                    outputRes += $"Semester {j}: ";
                    while (DFSSorter.Semester.Contains(j))
                    {
                        int idxCurrentMK = DFSSorter.Semester.IndexOf(j);
                        string currentMK = graphdetails[idxCurrentMK][0];
                        outputRes += $"{currentMK}";
                        DFSSorter.Semester[idxCurrentMK] = 0;
                        if (DFSSorter.Semester.Contains(j))
                        {
                            outputRes += " ,";
                        }
                    }
                    outputRes += "\n";
                }
                GraphResult.Text = outputRes;
                GraphAnimation.viewDFS(DFSResult, graphdetails);
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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tubes2
{
    class BFSSorter
    {
        //Do a topological sort by using a BFS algorithm
        public static Queue<Tuple<String, int>> topoSortBFS(List<List<String>> graphdetail)
        {
            //Create matrix that lists what classes each class depends on
            List<List<bool>> dependent_matrix = Tree.createDependentMatrix(graphdetail);

            //Creates an empty matrix of Tuple<class_code, semester_to_take>
            Queue<Tuple<String, int>> ordered_graph = new Queue<Tuple<String, int>>();

            //Queue to store result of BFS topological sort
            List<bool> queued = new List<bool>(new bool[dependent_matrix.Count]);

            //Counter to keep track which semester the current iteration is
            int semester = 1;

            //Loop through as long as the Queue's member count is less than the amount of classes
            while (ordered_graph.Count < dependent_matrix.Count)
            {
                //If a class has no more unresolved dependencies, enqueue it
                for(int i = 0; i < dependent_matrix.Count; i++)
                {
                    Tuple<String, int> temp = Tuple.Create(graphdetail[i][0], semester);
                    if (!(dependent_matrix[i].Contains(true)) && !queued[i])
                    {
                        //Note down which classes have been taken
                        queued[i] = true;

                        //Queue the class that has no more dependencies and is to be taken this semester
                        ordered_graph.Enqueue(temp);
                    }
                }

                //Update dependencies, set dependency of every class towards classes that have been taken to false
                for (int i = 0; i < dependent_matrix.Count; i++)
                {
                    if (queued[i])
                    {
                        for(int j = 0; j < dependent_matrix.Count; j++)
                        {
                            dependent_matrix[j][i] = false;
                        }
                    }
                }

                //Go forward by one semester
                semester++;
            }

            return ordered_graph;
        }
    }
}

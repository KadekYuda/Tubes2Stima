using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tubes2
{
    class BFSSorter
    {
        public static Queue<String> topoSortBFS(List<List<String>> graphdetail)
        {
            List<List<bool>> dependent_matrix = Tree.createDependentMatrix(graphdetail);

            Queue<String> ordered_graph = new Queue<String>();

            while(dependent_matrix.Exists(x => x.Contains(true)))
            {
                for(int i = 0; i < dependent_matrix.Count; i++)
                {
                    if(!dependent_matrix[i].Contains(true))
                    {
                        ordered_graph.Enqueue(graphdetail[i][0]);
                        for(int j = 0; j < dependent_matrix.Count; j++)
                        {
                            dependent_matrix[j][i] = false;
                        }
                    }
                }
            }

            return ordered_graph;
        }
    }
}

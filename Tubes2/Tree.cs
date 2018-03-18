using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tubes2
{
    class Tree
    {
        public static List<List<int>> createDependencyMatrix(List<List<String>> graphdetail)
        {
            //Sort by class code
            graphdetail = graphdetail.OrderBy(x => x[0]).ToList();

            List<List<int>> dependency_matrix = new List<List<int>>();

            //Initialize matrix with 
            for(int i = 0; i < graphdetail.Count; i++)
            {
                dependency_matrix.Add(new List<int>(new int[graphdetail.Count]));
            }

            for(int i = 0; i < graphdetail.Count;i++)
            {
                foreach(String dependency in graphdetail[i].Skip(1))
                {
                    int j = graphdetail.FindIndex(x => (x[0] == dependency));
                    dependency_matrix[j][i] = 1;
                }
            }

            return dependency_matrix;
        }
    }
}

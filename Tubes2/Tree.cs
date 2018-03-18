using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tubes2
{
    class Tree
    {
        public static List<List<bool>> createDependencyMatrix(List<List<String>> graphdetail)
        {
            List<List<bool>> dependency_matrix = new List<List<bool>>();

            //Initialize matrix with 
            for(int i = 0; i < graphdetail.Count; i++)
            {
                dependency_matrix.Add(new List<bool>(new bool[graphdetail.Count]));
            }

            //Fill in the matrix
            for(int i = 0; i < graphdetail.Count;i++)
            {
                foreach(String dependency in graphdetail[i].Skip(1))
                {
                    int j = graphdetail.FindIndex(x => (x[0] == dependency));
                    dependency_matrix[j][i] = true;
                }
            }

            return dependency_matrix;
        }

        public static List<List<bool>> createDependentMatrix(List<List<String>> graphdetail)
        {
            List<List<bool>> dependency_matrix = new List<List<bool>>();

            //Initialize matrix with 
            for (int i = 0; i < graphdetail.Count; i++)
            {
                dependency_matrix.Add(new List<bool>(new bool[graphdetail.Count]));
            }

            //Fill in the matrix
            for (int i = 0; i < graphdetail.Count; i++)
            {
                foreach (String dependency in graphdetail[i].Skip(1))
                {
                    int j = graphdetail.FindIndex(x => (x[0] == dependency));
                    dependency_matrix[i][j] = true;
                }
            }

            return dependency_matrix;
        }
    }
}

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

    class DFSSorter {
        public static List<int> Timestamp;
        public static List<int> Timestamp_Start;
        public static int currentSemester = 1;
        public static List<int> Semester;
        public static int timer = 1;

        public static void sort_semester(){
            int max = -1;
            int idxMax = -1;
            List<int> TS = new List<int>(Timestamp);
            while (currentSemester <= TS.Count){
                max = -1;
                for (int i = 0; i < TS.Count; i++){                
                    if (TS[i] > max){
                        max = TS[i];
                        idxMax = i;
                    }
                }

                Semester[idxMax] = currentSemester;
                currentSemester++;
                TS[idxMax] = -1;
                Debug.WriteLine("MASUK SINI LHO");
            }
        }

        public static void init_topoSort(List<List<String>> graphdetail)
        {
            List<List<bool>> dMat = Tree.createDependentMatrix(graphdetail);

            Timestamp = new List<int>(new int[dMat.Count]);
            Timestamp_Start = new List<int>(new int[dMat.Count]);
            Semester = new List<int>(new int[dMat.Count]);

            for (int i = 0; i < dMat.Count; i++)
            {
                if (!dMat[i].Contains(true))
                {
                    Timestamp[i] = timer;
                    Timestamp_Start[i] = timer;
                    // Semester[i] = currentSemester;
                    timer++;        
                    topoSortDFS(Tree.createDependencyMatrix(graphdetail), i);
                }
            }
        }

        public static void topoSortDFS(List<List<bool>> dMat, int i){
            // ++currentSemester;

            // Tidak ada mata kuliah yang membutuhkan mata kuliah i lagi
            if (!dMat[i].Contains(true)){
                Timestamp[i] = timer;
                timer++;
                // currentSemester--;
            }

            else {
                for (int j = 0; j < dMat[i].Count; j++){
                    if (dMat[i][j] && Timestamp[j] == 0){
                        dMat[i][j] = false;
                        // Semester[j] = currentSemester;
                        Timestamp_Start[j] = timer;
                        Timestamp[j] = timer;
                        timer++;
                        topoSortDFS(dMat, j);
                    }
                }
                Timestamp[i] = timer;
                timer++;
                // currentSemester--;
            }
        }

    }
}

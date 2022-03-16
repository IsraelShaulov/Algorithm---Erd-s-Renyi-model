using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GraphProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            int timesRunPerP = 500;
            int numOfVertecis = 1000;
            //conntivtiy
            double[] pArrTr1 = {
                0.0050 , 0.0056 , 0.0058 , 0.0062 , 0.0064 ,
                0.0085 , 0.0090 , 0.0095 , 0.0100 , 0.0150
            };
            //diameter
            double[] pArrTr2 = {
                0.1131 , 0.1142 , 0.11520 , 0.1154 , 0.1162 ,
                0.1182 , 0.1184 , 0.1186 , 0.1188 , 0.1190
            };
            //isolted
            double[] pArrTr3 = {
                0.0050 , 0.0056 , 0.0058 , 0.0062 , 0.0064 ,
                0.0085 , 0.0090 , 0.0095 , 0.0100 , 0.0150
            };

            List<int> connectivityList = new List<int>(timesRunPerP);
            List<int> diameterList = new List<int>(timesRunPerP);
            List<int> isIsolatedList = new List<int>(timesRunPerP);

            List<double> resultsTr1 = new List<double>();
            List<double> resultsTr2 = new List<double>();
            List<double> resultsTr3 = new List<double>();

            RunGraphConnectivity(pArrTr1, numOfVertecis, timesRunPerP, resultsTr1);

            RunGraphDiameter(pArrTr2, numOfVertecis, timesRunPerP, resultsTr2);

            RunGraphIsIsolated(pArrTr3, numOfVertecis, timesRunPerP, resultsTr3);

            double trashhold1 = 0.0069; // | conntivtiy
            double trashhold2 = 0.1175; // | diameter
            double trashhold3 = 0.0069; // | isolted

            WriteFeatureToCsv("Results.csv", pArrTr1, pArrTr2, pArrTr3, trashhold1, trashhold2, trashhold3, resultsTr1, resultsTr2, resultsTr3);

        }

        public static void RunGraphConnectivity(double[] ary, int numOfVertecis, int timesRunPerP, List<double> results)//run the graph for connectivity and count the numbers of Characteristics happen's,if its happends add to the List Structure and divide by 500 times the graph runs
        {
            List<int> connectivityList = new List<int>();

            for (int i = 0; i < ary.Length; i++)
            {
                connectivityList.Clear();

                for (int k = 0; k < timesRunPerP; k++)
                {
                    Graph graph = new Graph(numOfVertecis, ary[i]);

                    graph.build_random_graph();

                    graph.PrintGraphData();

                    int connectivity = graph.connectivity(graph.GraphList);

                    Console.WriteLine(
                        " --- connectivity : {0}",
                       connectivity);

                    connectivityList.Add(connectivity);

                }

                double count = connectivityList.Count(n => n == 1);
                results.Add((double)(count / connectivityList.Count));
            }
        }

        public static void RunGraphDiameter(double[] ary, int numOfVertecis, int timesRunPerP, List<double> results)//run the graph for diam == 2 and count the numbers of Characteristics happen's,if its happends add to the List Structure and divide by 500 times the graph runs
        {
            List<int> diameterList = new List<int>();

            for (int i = 0; i < ary.Length; i++)
            {
                diameterList.Clear();

                for (int k = 0; k < timesRunPerP; k++)
                {
                    Graph graph = new Graph(numOfVertecis, ary[i]);

                    graph.build_random_graph();

                    graph.PrintGraphData();

                    int diameter = graph.diameter(graph.GraphList);

                    Console.WriteLine(
                        " --- Diameter : {0}",
                       diameter);

                    diameterList.Add(diameter);

                }

                double count = diameterList.Count(n => n == 2);
                results.Add((double)(count / diameterList.Count));
            }

        }

        public static void RunGraphIsIsolated(double[] ary, int numOfVertecis, int timesRunPerP, List<double> results)//run the graph for isolted and count the numbers of Characteristics happen's,if its happends add to the List Structure and divide by 500 times the graph runs
        {
            List<int> isIsolatedList = new List<int>();

            for (int i = 0; i < ary.Length; i++)
            {
                isIsolatedList.Clear();

                for (int k = 0; k < timesRunPerP; k++)
                {
                    Graph graph = new Graph(numOfVertecis, ary[i]);

                    graph.build_random_graph();

                    graph.PrintGraphData();

                    int isolated = graph.Is_Isolated(graph.GraphList);

                    Console.WriteLine(
                        " --- isIsolated : {0}",
                       isolated);

                    isIsolatedList.Add(isolated);

                }

                double count = isIsolatedList.Count(n => n == 1);
                results.Add((double)(count / isIsolatedList.Count));
            }
        }


        private static void WriteFeatureToCsv(
            string path, double[] p1, double[] p2, double[] p3,
            double trashholds1, double trashholds2, double trashholds3,
            List<double> resultsTr1, List<double> resultsTr2, List<double> resultsTr3)// writing the result's to csv file statistics of this big project for each attribute
        {
            StringBuilder sb = new StringBuilder(1000);

            // Connctiviy
            // P,results

            sb.AppendLine("P,Connectivity Result");
            for (int i = 0; i < resultsTr1.Count; i++)
            {
                sb.AppendLine(string.Format("{0},{1:0.00}", p1[i], resultsTr1[i]));
            }
            sb.AppendLine();

            // Daimeter
            // P,results
            sb.AppendLine("P,Daimeter Result");
            for (int i = 0; i < resultsTr2.Count; i++)
            {
                sb.AppendLine(string.Format("{0},{1:0.00}", p2[i], resultsTr2[i]));
            }
            sb.AppendLine();

            // Isolated
            // P,results
            sb.AppendLine("P,Isolated Result");
            for (int i = 0; i < resultsTr3.Count; i++)
            {
                sb.AppendLine(string.Format("{0},{1:0.00}", p3[i], resultsTr3[i]));
            }

            File.WriteAllText(path, sb.ToString());
            sb.Clear();
        }
    }

    //all methods of the project with graph class using List Structure 
    public class Node
    {
        public List<int> Neighbors = new List<int>();
    }

    public class Graph
    {
        private static Random rand = new Random();//its for make random graph
        public int Vertecis { get; set; } // num of vertecis in the graph

        double propability;

        int maxEdges = 0; 

        public List<Node> GraphList = new List<Node>(); //using List Structure

        public Graph(int vertecis, double p)//graph constructor
        {
            propability = p;

            Vertecis = vertecis;

            maxEdges = ((Vertecis * (Vertecis - 1)) / 2); //for example V = 5 , Egdes [Max] = (n * n-1) / 2 = 10

            initGraph();
        }

        private void initGraph()
        {
            for (int i = 0; i < Vertecis; i++) // initialization Graph, add new node each i
            {
                GraphList.Add(new Node());
            }
        }

        int numOfEgdes = 0;//מספר הצלעות

        public void build_random_graph()//making random graph
        {
            const int offsetToP = 1000;

            int myP = (int)(propability * offsetToP);

            int numResEgde = 0;

            int numberOfExploredEgdes = 0;

            int indexOfVertecis = 0;

            while (numberOfExploredEgdes < maxEdges)
            {
                int length = GraphList.Count - indexOfVertecis - 1;

                for (int i = 0; i < length; i++)
                {
                    numResEgde = (int)(rand.Next(0, offsetToP));//random the number of edge in graph(its the P)

                    if (numResEgde <= myP)
                    {
                        GraphList[indexOfVertecis].Neighbors.Add(indexOfVertecis + i + 1);
                        numOfEgdes++;
                    }

                    numberOfExploredEgdes++;
                }

                indexOfVertecis++;
            }


            List<int> sizes = new List<int>();

            for (int i = 0; i < GraphList.Count; i++) // גודל לפני השינויים
            {
                sizes.Add(GraphList[i].Neighbors.Count);
            }

            for (int i = 0; i < GraphList.Count; i++) // make graph Neighbors edge same to sides
            {
                for (int k = 0; k < sizes[i]; k++)
                {
                    int neibor = GraphList[i].Neighbors[k];
                    GraphList[neibor].Neighbors.Add(i);
                }
            }

        }


        public void PrintGraphData()//printing all graph data (number of Vertecis,Edges)
        {
            Console.WriteLine("Vertecis : {0} , Edges: {1}", Vertecis, numOfEgdes);
            Console.WriteLine();

            for (int i = 0; i < GraphList.Count; i++)
            {
                Console.WriteLine(" {0}: ", i);
                for (int k = 0; k < GraphList[i].Neighbors.Count; k++)
                {
                    Console.Write("    {0} , ", GraphList[i].Neighbors[k]);
                }

                Console.WriteLine();
            }
        }
        /*
        for example:
                 index : {Neighbors} 
                    0: { 3 , 2 }
                    1: { }
                    2: { 0 , 3 }
                    3: { 0 , 2 }
        */

        public int Is_Isolated(List<Node> graph)// isolted method , searching for neighbors in the graph and count if there neighbors 
        {
            int answer = 0;
            for (int i = 0; i < graph.Count; i++)
            {
                if (graph[i].Neighbors.Count <= 0)
                {
                    answer = 1;
                    break;
                }
            }

            return answer;
        }

        public int connectivity(List<Node> graph)//connectivity method, using visited array of vertecis using queue like the originally algorithm,adding vertex to queue when find and delet when done with the vertex
        {
            bool[] visited = new bool[Vertecis];

            Queue<int> queue = new Queue<int>();

            queue.Enqueue(0);
            visited[0] = true;

            int indexVertex = 0;
            int lengthOfNeighorArr = 0;
            int indexOfNeighor = 0;

            while (queue.Count > 0)
            {
                indexVertex = queue.Dequeue();

                lengthOfNeighorArr = graph[indexVertex].Neighbors.Count;
                for (int i = 0; i < lengthOfNeighorArr; i++)
                {
                    indexOfNeighor = graph[indexVertex].Neighbors[i];

                    if (visited[indexOfNeighor] == false)
                    {
                        queue.Enqueue(indexOfNeighor);
                        visited[indexOfNeighor] = true;
                    }
                }
            }

            int answer = 1;

            for (int i = 0; i < visited.Length; i++)
            {
                if (!visited[i])
                {
                    answer = 0;
                    break;
                }
            }

            return answer;
        }

        public int diameter(List<Node> graph)//diamater method , using connectivity method and BFS algorihtm 
        {
            if (connectivity(graph) != 1)
            {
                return -1; // infinity
            }

            int pathLength = 0;
            int maxPath = 0;

            pathLength = maxPath = BFS(graph, 0);

            for (int i = 1; i < graph.Count; i++)
            {
                pathLength = BFS(graph, i);
                maxPath = maxPath > pathLength ? maxPath : pathLength;
            }

            return maxPath;
        }


        private int BFS(List<Node> graph, int vertex)//Originally BFS algorightm using visited array with queue on verteces
        {
            bool[] visited = new bool[Vertecis];
            Queue<int> queue = new Queue<int>();

            int indexVertex = 0;
            int lengthOfNeighorArr = 0;
            int indexOfNeighor = 0;
            int diameterValue = 0;
            int length = 0;

            queue.Enqueue(vertex);
            visited[vertex] = true;

            while (queue.Count > 0)
            {
                length = queue.Count;
                for (int k = 0; k < length; k++)
                {
                    indexVertex = queue.Dequeue();

                    lengthOfNeighorArr = graph[indexVertex].Neighbors.Count;
                    for (int i = 0; i < lengthOfNeighorArr; i++)
                    {
                        indexOfNeighor = graph[indexVertex].Neighbors[i];

                        if (visited[indexOfNeighor] == false)
                        {
                            queue.Enqueue(indexOfNeighor);
                            visited[indexOfNeighor] = true;
                        }
                    }
                }

                diameterValue++;
            }

            diameterValue -= 1;
            return diameterValue;
        }
    }
}

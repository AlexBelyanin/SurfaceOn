using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurfaceOn
{
    class DLA
    {
        public static List<int> Pure(int[,] graph, double[,] probability, int n)
        {
            List<int> vertexes = new List<int>();
            for (int i = 0; i < graph.GetLength(0); i++) vertexes.Add(i);
            vertexes.RemoveAt(0);
            Random random = new Random();
            int r;
            double q;
            for (int i = 0; i < n;)
            {
                if (vertexes.Count == 0) break;
                r = vertexes[random.Next(vertexes.Count)];
                //MessageBox.Show(r + "");
                for (int j = 0; j < 1000; j++)
                {
                    if (!vertexes.Contains(graph[r, 0]) || !vertexes.Contains(graph[r, 1]) || (!vertexes.Contains(graph[r, 2]) & graph[r, 2] != -1))
                    {
                        vertexes.Remove(r);
                        i++;
                        break;
                    }
                    else
                    {
                        q = random.NextDouble();
                        // MessageBox.Show(r + " " + q);
                        if (probability[r, 0] > q) r = graph[r, 0];
                        else if (probability[r, 1] > q) r = graph[r, 1];
                        else r = graph[r, 2];
                    }
                }
            }
            return vertexes;
        }

    }
}

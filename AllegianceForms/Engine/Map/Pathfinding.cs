using System;
using System.Collections.Generic;

namespace AllegianceForms.Engine.Map
{
    /// <summary>
    /// Original code from: https://github.com/mburst/dijkstras-algorithm
    /// </summary>
    /// <typeparam name="T">The vertices of the graph</typeparam>
    public class PathfindingGraph<T> where T : IComparable
    {
        private Dictionary<T, Dictionary<T, int>> _vertices = new Dictionary<T, Dictionary<T, int>>();

        public void AddVertex(T name, Dictionary<T, int> edges)
        {
            _vertices[name] = edges;
        }

        public List<T> ShortestPath(T start, T finish)
        {
            var previous = new Dictionary<T, T>();
            var distances = new Dictionary<T, int>();
            var nodes = new List<T>();

            List<T> path = null;

            foreach (var vertex in _vertices)
            {
                if (vertex.Key.CompareTo(start) == 0)
                {
                    distances[vertex.Key] = 0;
                }
                else
                {
                    distances[vertex.Key] = int.MaxValue;
                }

                nodes.Add(vertex.Key);
            }

            while (nodes.Count != 0)
            {
                nodes.Sort((x, y) => distances[x] - distances[y]);

                var smallest = nodes[0];
                nodes.Remove(smallest);

                if (smallest.CompareTo(finish) == 0)
                {
                    path = new List<T>();
                    while (previous.ContainsKey(smallest))
                    {
                        path.Add(smallest);
                        smallest = previous[smallest];
                    }

                    break;
                }

                if (distances[smallest] == int.MaxValue)
                {
                    break;
                }

                foreach (var neighbor in _vertices[smallest])
                {
                    var alt = distances[smallest] + neighbor.Value;
                    if (alt < distances[neighbor.Key])
                    {
                        distances[neighbor.Key] = alt;
                        previous[neighbor.Key] = smallest;
                    }
                }
            }

            return path;
        }
    }
}
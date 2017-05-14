using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AF.UnityUtilities
{
    public class Dijkstra<TNode> where TNode : class
    {
        public static List<TNode> FindPath(GraphNode<TNode> start, Dictionary<TNode, GraphNode<TNode>> nodes)
        {
            List<TNode> path = new List<TNode>();
            GraphNode<TNode> currentNode = start;

            while (currentNode.parent != null)
            {
                path.Add(currentNode.data);
                currentNode = currentNode.parent;
            }
            path.Reverse();

            return path;
        }

        public static Dictionary<TNode, GraphNode<TNode>> MakeMap(TNode startingNode, IGraph<TNode> graph)
        {
            var set = new HashSet<TNode>();
            set.Add(startingNode);
            return MakeMap(set, graph);
        }

        public static Dictionary<TNode, GraphNode<TNode>> MakeMap(HashSet<TNode> startingNodes, IGraph<TNode> graph)
        {
            var comparer = new FunctionalComparer<GraphNode<TNode>>((a, b) => -a.distance.CompareTo(b.distance));
            IHeap<GraphNode<TNode>> queue = new Heap<GraphNode<TNode>>(comparer);
            Dictionary<TNode, GraphNode<TNode>> nodeData = new Dictionary<TNode, GraphNode<TNode>>();

            graph.GetAllNodes().ForEach(n =>
            {
                var distance = startingNodes.Contains(n) ? 0f : float.MaxValue;
                var node = new GraphNode<TNode>(n, distance, null);
                queue.Add(node);
                nodeData.Add(n, node);
            });

            while (!queue.IsEmpty())
            {
                var u = queue.RemoveFirst();
                graph.GetNeighbors(u.data).ForEach(n =>
                {
                    var v = nodeData[n];
                    var alt = u.distance + graph.GetEdgeCost(u.data, v.data);
                    if (alt < v.distance)
                    {
                        v.distance = alt;
                        v.parent = u;
                        queue.UpdateItem(v);
                    }
                });
            }

            return nodeData;
        }
    }

    public class GraphNode<TNode> where TNode : class
    {
        public TNode data { get; set; }
        public GraphNode<TNode> parent { get; set; }
        public float distance;

        public GraphNode(TNode data, float distance, GraphNode<TNode> parent)
        {
            this.data = data;
            this.distance = distance;
            this.parent = parent;
        }
    }
}
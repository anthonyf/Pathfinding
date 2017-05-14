using AF.UnityUtilities;
using System.Collections.Generic;

namespace AF.Pathfinding
{
    public static class AStar<TNode> where TNode : class
    {
        public static List<TNode> FindPath(TNode start, TNode target, IGraph<TNode> graph)
        {
            IHeap<NodeData> openSet = new Heap<NodeData>(new GraphNodeDataComparer());
            HashSet<NodeData> closedSet = new HashSet<NodeData>();
            Dictionary<TNode, NodeData> nodeData = new Dictionary<TNode, NodeData>();

            nodeData[start] = new NodeData(start);
            nodeData[target] = new NodeData(target);
            openSet.Add(nodeData[start]);

            while (openSet.Count > 0)
            {
                var currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);
                if (Equals(currentNode, nodeData[target]))
                {
                    return RetracePath(nodeData[start], nodeData[target]);
                }

                foreach (var neighborNode in graph.GetNeighbors(currentNode.data))
                {
                    NodeData neighbor;
                    if (!nodeData.ContainsKey(neighborNode))
                    {
                        nodeData[neighborNode] = new NodeData(neighborNode);
                    }
                    neighbor = nodeData[neighborNode];

                    if (closedSet.Contains(neighbor))
                    {
                        continue;
                    }

                    float newMovementCostToNeighbor = currentNode.gCost + graph.GetEdgeCost(currentNode.data, neighbor.data);
                    if (newMovementCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                    {
                        neighbor.gCost = newMovementCostToNeighbor;
                        neighbor.hCost = graph.GetEdgeCost(neighbor.data, target);
                        neighbor.parent = currentNode;

                        if (!openSet.Contains(neighbor))
                        {
                            openSet.Add(neighbor);
                        }
                        else
                        {
                            openSet.UpdateItem(neighbor);
                        }
                    }
                }
            }

            // no path found
            return null;
        }

        private static List<TNode> RetracePath(NodeData start, NodeData target)
        {
            List<TNode> path = new List<TNode>();
            NodeData currentNode = target;

            while (!Equals(currentNode, start))
            {
                path.Add(currentNode.data);
                currentNode = currentNode.parent;
            }
            path.Reverse();

            return path;
        }

        private class GraphNodeDataComparer : IComparer<NodeData>
        {
            public int Compare(NodeData a, NodeData b)
            {
                int compare = a.fCost.CompareTo(b.fCost);
                if (compare == 0)
                {
                    compare = a.hCost.CompareTo(b.hCost);
                }
                return -compare;
            }
        }

        private class NodeData
        {
            public TNode data;
            public NodeData parent;
            public float gCost;
            public float hCost;
            public float fCost { get { return gCost + hCost; } }

            public NodeData(TNode data)
            {
                this.data = data;
            }
        }
    }
}
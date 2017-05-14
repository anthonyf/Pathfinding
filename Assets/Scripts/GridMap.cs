using AF.UnityUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AF.Pathfinding
{

    public class GridMap<T> : IGraph<GridMap<T>.Node>
    {
        Node[,] _map;

        public GridMap(int width, int height)
        {
            _map = new Node[width, height];
            for (int x = 0; x < _map.GetLength(0); x++)
            {
                for (int y = 0; y < _map.GetLength(1); y++)
                {
                    _map[x, y] = new Node(x, y);
                }
            }
        }

        public Node GetNodeAt(int x, int y)
        {
            return _map[x, y];
        }

        public T GetCellData(int x, int y)
        {
            return _map[x, y].data;
        }

        public void SetCellData(int x, int y, T data)
        {
            _map[x, y].data = data;
        }

        public List<Node> GetAllNodes()
        {
            var list = new List<Node>();
            for (int x = 0; x < _map.GetLength(0); x++)
            {
                for (int y = 0; y < _map.GetLength(1); y++)
                {
                    list.Add(_map[x, y]);
                }
            }
            return list;
        }

        public float GetEdgeCost(Node a, Node b)
        {
            return UnityExtensions.Distance(a.x, a.y, b.x, b.y) * b.cost;
            //return UnityExtensions.ManhattanDistance(a.x, a.y, b.x, b.y);
        }

        public List<Node> GetNeighbors(Node node)
        {
            List<Node> neighbors = new List<Node>();
            UnityExtensions.MapNeighborIndexes(node.x, node.y, _map.GetLength(0), _map.GetLength(1), (x, y) =>
            {
                var ox = x - node.x;
                var oy = y - node.y;
                var isDiagonal = ox != 0 && oy != 0;
                var isDiagonallyWalkable = !isDiagonal || (_map[x, y - oy].isWalkable || _map[x - ox, y].isWalkable);
                if (_map[x, y].isWalkable && isDiagonallyWalkable)
                {
                    neighbors.Add(_map[x, y]);
                }
            });
            return neighbors;
        }

        public class Node
        {
            public T data { get; set; }
            public float cost { get; set; }
            public bool isWalkable { get; set; }
            public int x { get; private set; }
            public int y { get; private set; }

            public Node(int x, int y)
            {
                this.isWalkable = true;
                this.cost = 1;
                this.x = x;
                this.y = y;
            }
        }
    }

}
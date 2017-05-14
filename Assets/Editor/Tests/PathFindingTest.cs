using NUnit.Framework;
using System.Linq;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace AF.Pathfinding
{
    public class PathFindingTest
    {
        private const int testMapSize = 10;

        [Test]
        public void SimpleGridTest()
        {

            TestNode[,] map = new TestNode[testMapSize, testMapSize];
            int n = 0;
            for (int x = 0; x < testMapSize; x++)
            {
                for (int y = 0; y < testMapSize; y++)
                {
                    map[x, y] = new TestNode() { x = x, y = y, value = n++ };
                }
            }
            var graph = new TestGraph(map);
            var path = AStar<TestNode>.FindPath(
                map[0, 0], map[9, 9], graph);

            Assert.AreEqual(path.Count, 9);

            path = AStar<TestNode>.FindPath(
                map[0, 0], map[1, 1], graph);

            Assert.AreEqual(path.Count, 1);

        }

        [Test]
        public void GetAllNodesTest()
        {
            TestNode[,] map = new TestNode[testMapSize, testMapSize];
            var graph = new TestGraph(map);
            Assert.AreEqual(testMapSize * testMapSize, graph.GetAllNodes().Count);
        }

        public class TestGraph : IGraph<TestNode>
        {
            TestNode[,] _map;
            public TestGraph(TestNode[,] map)
            {
                _map = map;
            }

            public List<TestNode> GetAllNodes()
            {
                var items = from TestNode item in _map select item;
                return items.ToList();
            }

            public float GetEdgeCost(TestNode a, TestNode b)
            {
                return Mathf.Sqrt((a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y));

            }

            public List<TestNode> GetNeighbors(TestNode a)
            {
                List<TestNode> neighbors = new List<TestNode>();
                for (int x = -1; x <= 1; x++)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        if (x == 0 && y == 0) continue;
                        int nx = a.x + x;
                        int ny = a.y + y;
                        if (nx >= 0 && nx < testMapSize && ny >= 0 && ny < testMapSize)
                        {
                            neighbors.Add(_map[nx, ny]);
                        }
                    }
                }
                return neighbors;
            }
        }

        public class TestNode
        {
            public int x;
            public int y;
            public int value;
        }
    }
}
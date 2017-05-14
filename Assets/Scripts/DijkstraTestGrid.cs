using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AF.Pathfinding
{

    public class DijkstraTestGrid : MonoBehaviour
    {

        [SerializeField]
        int width;

        [SerializeField]
        int height;

        [SerializeField]
        DijkstraTestGridCell _cellPrefab;

        GridMap<DijkstraTestGridCell> _map;

        HashSet<GridMap<DijkstraTestGridCell>.Node> _targetCells;
        HashSet<GridMap<DijkstraTestGridCell>.Node> _sourceCells;

        public enum DisplayMode
        {
            DijkstraMap,
            TerrainMap
        }

        DisplayMode _currentDisplayMode = DisplayMode.DijkstraMap;

        void Start()
        {
            _targetCells = new HashSet<GridMap<DijkstraTestGridCell>.Node>();

            _map = new GridMap<DijkstraTestGridCell>(width, height);
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var go = Instantiate(_cellPrefab);
                    go.transform.SetParent(transform, false);
                    go.transform.localPosition = MapIndexToWorldPosition(x, y);
                    _map.SetCellData(x, y, go);
                    go.node = _map.GetNodeAt(x, y);
                }
            }
        }

        public void SetDisplayMode(DisplayMode displayMode)
        {
            _currentDisplayMode = displayMode;
            UpdateCells();
        }

        public void OnReset()
        {
            _targetCells.Clear();
            _map.GetAllNodes().ForEach(n =>
            {
                n.cost = 1;
                n.isWalkable = true;
            });
            UpdateCells();
        }

        public void SetTargetNode(DijkstraTestGridCell cell, bool isAdd)
        {
            var node = _map.GetNodeAt(cell.node.x, cell.node.y);
            if (isAdd)
            {
                _targetCells.Add(node);
            }
            else
            {
                _targetCells.Remove(node);
            }
            UpdateCells();
        }

        public void SetTerrainCost(DijkstraTestGridCell cell, bool isIncrease)
        {
            var node = _map.GetNodeAt(cell.node.x, cell.node.y);

            if (!isIncrease)
            {
                while (node.cost <= 1)
                {
                    // bump up all costs because dijkstra doesnt work on negative costs
                    _map.GetAllNodes().ForEach(n => n.cost += 1);
                }
            }

            node.cost += isIncrease ? 1 : -1;
            UpdateCells();
        }

        public void SetWalkable(DijkstraTestGridCell cell, bool isWalkable)
        {
            var node = _map.GetNodeAt(cell.node.x, cell.node.y);
            node.isWalkable = isWalkable;
            UpdateCells();
        }

        public void SetSourceNode(DijkstraTestGridCell cell, bool isIncrease)
        {
            //TODO
            UpdateCells();
        }

        void UpdateCells()
        {
            var nodeMap = Dijkstra<GridMap<DijkstraTestGridCell>.Node>.MakeMap(_targetCells, _map);
            switch (_currentDisplayMode)
            {
                case DisplayMode.DijkstraMap:
                    var maxDistance = nodeMap.Values.Max(n =>
                    {
                        return (n.data.isWalkable && n.distance != float.MaxValue) ? n.distance : 0f;
                    });
                    nodeMap.Values.ToList().ForEach(n =>
                    {
                        n.data.data.SetValue(n.distance, maxDistance, n.data.isWalkable);
                    });
                    break;
                case DisplayMode.TerrainMap:
                    var maxCost = nodeMap.Values.Max(n => n.data.cost);
                    nodeMap.Values.ToList().ForEach(n =>
                    {
                        n.data.data.SetValue(n.data.cost, maxCost, n.data.isWalkable);
                    });
                    break;
            }
        }

        Vector3 MapIndexToWorldPosition(int x, int y)
        {
            return Vector3.zero + Vector3.up * (y - height / 2f + .5f) + Vector3.right * (x - width / 2f + .5f);
        }

        void OnDrawGizmos()
        {
            if (!Application.isPlaying)
            {
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        Gizmos.DrawCube(MapIndexToWorldPosition(x, y), Vector3.one * .9f);
                    }
                }
            }
        }

    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace AF.UnityUtilities
{

    public class MapTools : MonoBehaviour
    {

        [SerializeField]
        DijkstraTestGridInput.DisplayModeEvent OnDisplayChanged;

        [SerializeField]
        DijkstraTestGridInput.NodeBoolEvent OnSetTargetNode;

        [SerializeField]
        DijkstraTestGridInput.NodeBoolEvent OnSetNodeCost;

        [SerializeField]
        DijkstraTestGridInput.NodeBoolEvent OnSetWalkable;

        [SerializeField]
        DijkstraTestGridInput.NodeBoolEvent OnSetSourceNodeNode;


        [SerializeField]
        UnityEvent OnQuit;

        [SerializeField]
        UnityEvent OnReset;

        [SerializeField]
        Toggle _targetPointsToggle;

        [SerializeField]
        Toggle _terrainCostToggle;

        [SerializeField]
        Toggle _wallsToggle;

        [SerializeField]
        Toggle _sourcePointsToggle;

        [SerializeField]
        Toggle _terrainMapToggle;

        [SerializeField]
        Toggle _dijkstraMapToggle;

        // Use this for initialization
        void Start()
        {

        }

        public void SetDisplayMode(DijkstraTestGrid.DisplayMode mode)
        {
            switch (mode)
            {
                case DijkstraTestGrid.DisplayMode.DijkstraMap:
                    _dijkstraMapToggle.isOn = true;
                    break;
                case DijkstraTestGrid.DisplayMode.TerrainMap:
                    _terrainMapToggle.isOn = true;
                    break;
            }
        }

        public void OnDijkstraMapValueChanged(bool value)
        {
            if (value && OnDisplayChanged != null)
            {
                OnDisplayChanged.Invoke(DijkstraTestGrid.DisplayMode.DijkstraMap);
            }
        }

        public void OnTerrainMapValueChanged(bool value)
        {
            if (value && OnDisplayChanged != null)
            {
                OnDisplayChanged.Invoke(DijkstraTestGrid.DisplayMode.TerrainMap);
            }
        }

        public void OnQuitClicked()
        {
            if (OnQuit != null) OnQuit.Invoke();
        }

        public void CellClicked(DijkstraTestGridCell cell, bool leftClick)
        {
            if (_targetPointsToggle.isOn)
            {
                if (OnSetTargetNode != null) OnSetTargetNode.Invoke(cell, leftClick);
            }
            else if (_wallsToggle.isOn)
            {
                if (OnSetWalkable != null) OnSetWalkable.Invoke(cell, !leftClick);
            }
            else if (_sourcePointsToggle.isOn)
            {
                if (OnSetSourceNodeNode != null) OnSetSourceNodeNode.Invoke(cell, leftClick);
            }
            else if (_terrainCostToggle.isOn)
            {
                if (OnSetNodeCost != null) OnSetNodeCost.Invoke(cell, leftClick);
            }
        }

        public void OnBrushChanged(DijkstraTestGridInput.BrushType brushType)
        {
            switch (brushType)
            {
                case DijkstraTestGridInput.BrushType.TargetNodes:
                    _targetPointsToggle.isOn = true;
                    break;
                case DijkstraTestGridInput.BrushType.SourceNodes:
                    _sourcePointsToggle.isOn = true;
                    break;
                case DijkstraTestGridInput.BrushType.TerrainCost:
                    _terrainCostToggle.isOn = true;
                    break;
                case DijkstraTestGridInput.BrushType.Walls:
                    _wallsToggle.isOn = true;
                    break;
            }
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AF.UnityUtilities
{

    public class DijkstraTestGridInput : MonoBehaviour
    {

        [Serializable]
        public class DisplayModeEvent : UnityEvent<DijkstraTestGrid.DisplayMode> { }

        [Serializable]
        public class BrushChangeEvent : UnityEvent<BrushType> { }

        [Serializable]
        public class NodeBoolEvent : UnityEvent<DijkstraTestGridCell, bool> { }

        [SerializeField]
        NodeBoolEvent OnNodeClicked;

        [SerializeField]
        BrushChangeEvent OnBrushChanged;

        [SerializeField]
        DisplayModeEvent OnSetDisplayMode;

        public enum BrushType
        {
            TargetNodes,
            Walls,
            TerrainCost,
            SourceNodes,
        }

        // Use this for initialization
        void Start()
        {

        }

        void Update()
        {
            HandleMouseInput();
            HandleKeyboardInput();
        }

        private void HandleKeyboardInput()
        {
            if (Input.GetKeyUp(KeyCode.Alpha1))
            {
                if (OnBrushChanged != null) OnBrushChanged.Invoke(BrushType.TargetNodes);
            }
            else if (Input.GetKeyUp(KeyCode.Alpha2))
            {
                if (OnBrushChanged != null) OnBrushChanged.Invoke(BrushType.TerrainCost);
            }
            else if (Input.GetKeyUp(KeyCode.Alpha3))
            {
                if (OnBrushChanged != null) OnBrushChanged.Invoke(BrushType.Walls);
            }
            else if (Input.GetKeyUp(KeyCode.Alpha4))
            {
                if (OnBrushChanged != null) OnBrushChanged.Invoke(BrushType.SourceNodes);
            }
            else if (Input.GetKeyUp(KeyCode.D))
            {
                if (OnSetDisplayMode != null)
                {
                    OnSetDisplayMode.Invoke(DijkstraTestGrid.DisplayMode.DijkstraMap);
                }
            }
            else if (Input.GetKeyUp(KeyCode.T))
            {
                if (OnSetDisplayMode != null)
                {
                    OnSetDisplayMode.Invoke(DijkstraTestGrid.DisplayMode.TerrainMap);
                }
            }
        }

        private void HandleMouseInput()
        {
            if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    var cell = hit.collider.GetComponent<DijkstraTestGridCell>();
                    if (cell != null)
                    {
                        NodeClicked(cell, Input.GetMouseButtonUp(0) ? 0 : 1);
                    }
                }
            }
        }

        private void NodeClicked(DijkstraTestGridCell cell, int button)
        {
            if (OnNodeClicked != null)
            {
                OnNodeClicked.Invoke(cell, button == 0);
            }
        }
    }
}
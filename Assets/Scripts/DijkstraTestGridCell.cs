﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AF.UnityUtilities
{

    public class DijkstraTestGridCell : MonoBehaviour
    {

        [SerializeField]
        private Material _material;

        [SerializeField]
        TextMesh _valueText;

        public GridMap<DijkstraTestGridCell>.Node node;

        MeshRenderer _mesh;

        [SerializeField]
        private float _value;

        [SerializeField]
        private bool _isWalkable;

        public Color color
        {
            set
            {
                _mesh.material.color = value;
            }
        }

        // Use this for initialization
        void Start()
        {
            _mesh = GetComponent<MeshRenderer>();
            _mesh.material = _material;
        }

        internal void SetValue(float value, float max, bool isWalkable)
        {
            _value = value;
            _isWalkable = isWalkable;
            if (isWalkable)
            {
                _valueText.text = value == float.MaxValue ? "" : String.Format("{0:0.0}", value);
                _mesh.material.color = UnityExtensions.MakeHeatColor(0, max, value, true);
            }
            else
            {
                _valueText.text = "#";
                _mesh.material.color = new Color(1, 1, 1);
            }
        }
    }
}
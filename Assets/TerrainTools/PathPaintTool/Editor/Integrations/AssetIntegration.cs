﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor.Experimental.TerrainAPI
{
    public abstract class AssetIntegration
    {
        [SerializeField]
        public bool Active { get; set; }

        [SerializeField]
        public bool Enabled { get; set; }

        public void OnInspectorGUI()
        {
            EditorGUILayout.LabelField( GetName(), EditorStyles.boldLabel);

            Active = EditorGUILayout.Toggle("Active", Active);
        }

        public abstract string GetName();

        public abstract void Update(StrokeSegment[] segments, IOnPaint editContext);

        /// <summary>
        /// Will be performed once after the painting is finished.
        /// Example usage: Update the entire vegetation system of vegetation studio pro
        /// </summary>
        public abstract void OnPaintFinished();

    }
}

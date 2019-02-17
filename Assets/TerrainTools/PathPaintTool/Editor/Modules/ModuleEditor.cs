using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor.Experimental.TerrainAPI
{
    public abstract class ModuleEditor
    {
        public bool Active { get; set; }

        public abstract string GetName();

        public abstract string GetDescription();

        public abstract void OnInspectorGUI(Terrain terrain, IOnInspectorGUI editContext);

        public abstract void OnSceneGUI(Terrain currentTerrain, IOnSceneGUI editContext);

        public abstract void PaintSegments(StrokeSegment[] segments, IOnPaint editContext);
    }
}
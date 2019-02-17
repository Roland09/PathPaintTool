using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor.Experimental.TerrainAPI
{
    public interface IPaintMode
    {
        string GetName();

        string GetDescription();

        void OnInspectorGUI(Terrain terrain, IOnInspectorGUI editContext);

        void OnSceneGUI(Terrain currentTerrain, IOnSceneGUI editContext);

        StrokeSegment[] OnPaint(Terrain terrain, IOnPaint editContext);
    }
}

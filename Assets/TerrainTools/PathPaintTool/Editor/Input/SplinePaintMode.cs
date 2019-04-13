using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Experimental.TerrainAPI;


namespace UnityEditor.Experimental.TerrainAPI
{
    public class SplinePaintMode : IPaintMode
    {
        public string GetName()
        {
            return "Spline";
        }

        public string GetDescription()
        {
            return "";
        }

        public void OnInspectorGUI(Terrain terrain, IOnInspectorGUI editContext, BrushSettings brushSettings)
        {
            EditorGUILayout.LabelField("Spline", EditorStyles.boldLabel);

            EditorGUILayout.HelpBox(PathPaintStyles.notImplemented.text, MessageType.Error);
        }

        public StrokeSegment[] OnPaint(Terrain terrain, IOnPaint editContext, BrushSettings brushSettings)
        {
            // TODO implement feature
            return null;
        }

        public void OnSceneGUI(Terrain currentTerrain, IOnSceneGUI editContext, BrushSettings brushSettings)
        {
            // TODO implement feature
        }
    }
}

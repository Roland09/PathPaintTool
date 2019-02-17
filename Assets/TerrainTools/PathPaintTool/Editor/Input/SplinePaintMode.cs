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

        public void OnInspectorGUI(Terrain terrain, IOnInspectorGUI editContext)
        {
            EditorGUILayout.LabelField("Spline", EditorStyles.boldLabel);

            EditorGUILayout.HelpBox(PathPaintStyles.notImplemented.text, MessageType.Error);
        }

        public StrokeSegment[] OnPaint(Terrain terrain, IOnPaint editContext)
        {
            // TODO implement feature
            return null;
        }

        public void OnSceneGUI(Terrain currentTerrain, IOnSceneGUI editContext)
        {
            // TODO implement feature
        }
    }
}

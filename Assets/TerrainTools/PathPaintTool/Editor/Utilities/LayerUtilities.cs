using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor.Experimental.TerrainAPI
{
    public class LayerUtilities
    {

        public static int ShowTerrainLayersSelection(string title, Terrain terrain, int activeTerrainLayer)
        {
            GUIContent terrainLayers = EditorGUIUtility.TrTextContent(title);
            GUILayout.Label(terrainLayers, EditorStyles.boldLabel);
            GUI.changed = false;
            bool doubleClick;
            int selectedTerrainLayer = activeTerrainLayer;

            if (terrain.terrainData.terrainLayers.Length > 0)
            {
                TerrainLayer[] layers = terrain.terrainData.terrainLayers;
                Texture2D[] layerIcons = new Texture2D[layers.Length];
                for (int i = 0; i < layerIcons.Length; ++i)
                {
                    layerIcons[i] = (layers[i] == null || layers[i].diffuseTexture == null) ? EditorGUIUtility.whiteTexture : AssetPreview.GetAssetPreview(layers[i].diffuseTexture) ?? layers[i].diffuseTexture;
                }

                GUIContent errNoLayersFound = EditorGUIUtility.TrTextContent("No terrain layers founds. You can create a new terrain layer using the Asset/Create/Terrain Layer menu command.");
                selectedTerrainLayer = AspectSelectionGrid(activeTerrainLayer, layerIcons, 64, new GUIStyle("GridList"), errNoLayersFound, out doubleClick);
            }
            else
                selectedTerrainLayer = -1;

            return selectedTerrainLayer;
        }


        public static int AspectSelectionGrid(int selected, Texture[] textures, int approxSize, GUIStyle style, GUIContent errorMessage, out bool doubleClick)
        {
            GUILayout.BeginVertical("box", GUILayout.MinHeight(approxSize));
            int retval = 0;

            doubleClick = false;

            if (textures.Length != 0)
            {
                int columns = (int)(EditorGUIUtility.currentViewWidth - 150) / approxSize;
                int rows = (int)Mathf.Ceil((textures.Length + columns - 1) / columns);
                Rect r = GUILayoutUtility.GetAspectRect((float)columns / (float)rows);
                Event evt = Event.current;
                if (evt.type == EventType.MouseDown && evt.clickCount == 2 && r.Contains(evt.mousePosition))
                {
                    doubleClick = true;
                    evt.Use();
                }

                retval = GUI.SelectionGrid(r, System.Math.Min(selected, textures.Length - 1), textures, (int)columns, style);
            }
            else
            {
                GUILayout.Label(errorMessage);
            }

            GUILayout.EndVertical();
            return retval;
        }

        public static int FindTerrainLayerIndex(Terrain terrain, TerrainLayer inputLayer)
        {
            for (int i = 0; i < terrain.terrainData.terrainLayers.Length; i++)
            {
                if (terrain.terrainData.terrainLayers[i] == inputLayer)
                    return i;
            }
            return -1;
        }

        public static TerrainLayer FindTerrainLayer(Terrain terrain, int layerIndex)
        {
            if (layerIndex < 0 || layerIndex >= terrain.terrainData.terrainLayers.Length)
                return null;

            return terrain.terrainData.terrainLayers[layerIndex];
        }

    }

}

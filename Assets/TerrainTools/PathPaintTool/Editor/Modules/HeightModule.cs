using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.TerrainAPI;


namespace UnityEditor.Experimental.TerrainAPI
{
    public class HeightModule : ModuleEditor
    {

        #region Fields

        [SerializeField]
        float heightBrushSize = 100f;

        [SerializeField]
        float heightBrushStrength = 80f;

        #endregion Fields

        private static Color heightBrushColor = new Color(1.0f, 1.0f, 0.4f, 0.3f);

        public HeightModule(bool active, int sceneGuiOrder, int paintSegmentOrder) : base(active, sceneGuiOrder, paintSegmentOrder)
        {
        }

        override public string GetName()
        {
            return "Height";
        }

        override public string GetDescription()
        {
            return "Left click to raise. Shift and left click to lower the terrain.";
        }


        override public void OnSceneGUI(Terrain currentTerrain, IOnSceneGUI editContext)
        {
            if (editContext.hitValidTerrain)
            {
                Terrain terrain = currentTerrain;

                // the smooth brush size is relative to the main brush size
                float brushSize = editContext.brushSize * heightBrushSize / 100f;

                BrushTransform brushXform = TerrainPaintUtility.CalculateBrushTransform(terrain, editContext.raycastHit.textureCoord, brushSize, 0.0f);
                PaintContext ctx = TerrainPaintUtility.BeginPaintHeightmap(terrain, brushXform.GetBrushXYBounds(), 1);
                Material brushPreviewMat = BrushUtilities.GetDefaultBrushPreviewMaterial();
                brushPreviewMat.color = heightBrushColor;
                BrushUtilities.DrawBrushPreview(ctx, BrushUtilities.BrushPreview.SourceRenderTexture, editContext.brushTexture, brushXform, brushPreviewMat, 0);
                TerrainPaintUtility.ReleaseContextResources(ctx);
            }
        }

        override public void OnInspectorGUI(Terrain terrain, IOnInspectorGUI editContext)
        {
            EditorGUILayout.LabelField("Height", EditorStyles.boldLabel);

            heightBrushSize = EditorGUILayout.Slider(new GUIContent("Brush Size [% of Main Brush]", ""), heightBrushSize, 0.0f, 200.0f);
            heightBrushStrength = EditorGUILayout.Slider(new GUIContent("Brush Strength", ""), heightBrushStrength, 0, 200);

        }

        override public void PaintSegments(StrokeSegment[] segments, IOnPaint editContext)
        {
            for (int i = 0; i < segments.Length; i++)
            {

                StrokeSegment segment = segments[i];

                Height(segment.currTerrain, editContext, segment.currUV, segment.prevUV);
            }
        }


        private bool Height(Terrain terrain, IOnPaint editContext, Vector2 currUV, Vector2 prevUV)
        {
            // the brush size is relative to the main brush size
            float brushSize = editContext.brushSize * heightBrushSize / 100f;

            BrushTransform brushXform = TerrainPaintUtility.CalculateBrushTransform(terrain, currUV, brushSize, 0.0f);
            PaintContext paintContext = TerrainPaintUtility.BeginPaintHeightmap(terrain, brushXform.GetBrushXYBounds(), 1);

            paintContext.sourceRenderTexture.filterMode = FilterMode.Bilinear;

            Material mat = TerrainPaintUtility.GetBuiltinPaintMaterial();

            float brushStrength = heightBrushStrength / 100f;

            brushStrength = Event.current.shift ? -brushStrength : brushStrength;

            brushStrength *= 0.001f; // magic number ...

            Vector4 brushParams = new Vector4( brushStrength, 0.0f, 0.0f, 0.0f);

            mat.SetTexture("_BrushTex", editContext.brushTexture);
            mat.SetVector("_BrushParams", brushParams);

            TerrainPaintUtility.SetupTerrainToolMaterialProperties(paintContext, brushXform, mat);
            Graphics.Blit(paintContext.sourceRenderTexture, paintContext.destinationRenderTexture, mat, 0);

            TerrainPaintUtility.EndPaintHeightmap(paintContext, "Terrain Paint - Raise or Lower Height");

            return true;
        }
    }
}

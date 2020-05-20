using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.TerrainAPI;


namespace UnityEditor.Experimental.TerrainAPI
{
    public class SmoothModule : ModuleEditor
    {
        #region Fields

        [SerializeField]
        float smoothBrushSize = 150f;

        [SerializeField]
        float smoothBrushStrength = 20;

        [SerializeField]
        float smoothDirection = 0.0f;     // -1 to 1

        #endregion Fields

        private static Color smoothBrushColor = new Color(0.5f, 0.5f, 0.5f, 0.8f);

        public SmoothModule( bool active, int sceneGuiOrder, int paintSegmentOrder) : base(active, sceneGuiOrder, paintSegmentOrder)
        {
        }

        override public string GetName()
        {
            return "Smooth";
        }

        override public string GetDescription()
        {
            return "";
        }


        override public void OnSceneGUI(Terrain currentTerrain, IOnSceneGUI editContext, BrushSettings brushSettings)
        {
            if (editContext.hitValidTerrain)
            {
                Terrain terrain = currentTerrain;

                // the brush size is relative to the main brush size
                float brushSize = brushSettings.brushSize * this.smoothBrushSize / 100f;

                BrushTransform brushXform = TerrainPaintUtility.CalculateBrushTransform(terrain, editContext.raycastHit.textureCoord, brushSize, brushSettings.brushRotationDegrees);
                PaintContext ctx = TerrainPaintUtility.BeginPaintHeightmap(terrain, brushXform.GetBrushXYBounds(), 1);
                Material brushPreviewMat = BrushUtilities.GetDefaultBrushPreviewMaterial();
                brushPreviewMat.color = smoothBrushColor;
                TerrainPaintUtilityEditor.DrawBrushPreview(ctx, TerrainPaintUtilityEditor.BrushPreview.SourceRenderTexture, editContext.brushTexture, brushXform, brushPreviewMat, 0);
                TerrainPaintUtility.ReleaseContextResources(ctx);
            }
        }

        override public void OnInspectorGUI(Terrain terrain, IOnInspectorGUI editContext, BrushSettings brushSettings)
        {

            EditorGUILayout.LabelField("Smooth", EditorStyles.boldLabel);

            smoothBrushSize = EditorGUILayout.Slider(new GUIContent("Brush Size [% of Main Brush]", ""), smoothBrushSize, 0.0f, 300.0f);
            smoothBrushStrength = EditorGUILayout.Slider(new GUIContent("Brush Strength", ""), smoothBrushStrength, 0.0f, 100.0f);
            smoothDirection = EditorGUILayout.Slider(new GUIContent("Blur Direction", "Blur only up (1.0), only down (-1.0) or both (0.0)"), smoothDirection, -1.0f, 1.0f);
        }

        override public void PaintSegments(StrokeSegment[] segments, IOnPaint editContext, BrushSettings brushSettings)
        {
            for (int i = 0; i < segments.Length; i++)
            {
                StrokeSegment segment = segments[i];

                Smooth(segment.currTerrain, editContext, segment.currUV, brushSettings);

            }
        }

        private bool Smooth(Terrain terrain, IOnPaint editContext, Vector2 currUV, BrushSettings brushSettings)
        {
            // the brush size is relative to the main brush size
            float brushSize = brushSettings.brushSize * this.smoothBrushSize / 100f;

            BrushTransform brushXform = TerrainPaintUtility.CalculateBrushTransform(terrain, editContext.uv, brushSize, brushSettings.brushRotationDegrees);
            PaintContext paintContext = TerrainPaintUtility.BeginPaintHeightmap(terrain, brushXform.GetBrushXYBounds());

            Material mat = TerrainPaintUtility.GetBuiltinPaintMaterial();

            float brushStrength = smoothBrushStrength / 100f; // editContext.brushStrength;

            // brushStrength = Event.current.shift ? -brushStrength : brushStrength;
            Vector4 smoothWeights = new Vector4(
               Mathf.Clamp01(1.0f - Mathf.Abs(smoothDirection)),   // centered
               Mathf.Clamp01(-smoothDirection),                    // min
               Mathf.Clamp01(smoothDirection),                     // max
               0.0f);                                          // unused

            Vector4 brushParams = new Vector4(brushStrength, 0.0f, 0.0f, 0.0f);
            mat.SetTexture("_BrushTex", editContext.brushTexture);
            mat.SetVector("_BrushParams", brushParams);
            mat.SetVector("_SmoothWeights", smoothWeights);

            TerrainPaintUtility.SetupTerrainToolMaterialProperties(paintContext, brushXform, mat);

            Graphics.Blit(paintContext.sourceRenderTexture, paintContext.destinationRenderTexture, mat, (int)TerrainPaintUtility.BuiltinPaintMaterialPasses.SmoothHeights);

            TerrainPaintUtility.EndPaintHeightmap(paintContext, "Terrain Paint - Smooth Height");

            return true;
        }
    }
}

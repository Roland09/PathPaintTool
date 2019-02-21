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


        override public void OnSceneGUI(Terrain currentTerrain, IOnSceneGUI editContext)
        {
            if (editContext.hitValidTerrain)
            {
                Terrain terrain = currentTerrain;

                // the brush size is relative to the main brush size
                float brushSize = editContext.brushSize * this.smoothBrushSize / 100f;

                BrushTransform brushXform = TerrainPaintUtility.CalculateBrushTransform(terrain, editContext.raycastHit.textureCoord, brushSize, 0.0f);
                PaintContext ctx = TerrainPaintUtility.BeginPaintHeightmap(terrain, brushXform.GetBrushXYBounds(), 1);
                Material brushPreviewMat = BrushUtilities.GetDefaultBrushPreviewMaterial();
                brushPreviewMat.color = smoothBrushColor;
                BrushUtilities.DrawBrushPreview(ctx, BrushUtilities.BrushPreview.SourceRenderTexture, editContext.brushTexture, brushXform, brushPreviewMat, 0);
                TerrainPaintUtility.ReleaseContextResources(ctx);
            }
        }

        override public void OnInspectorGUI(Terrain terrain, IOnInspectorGUI editContext)
        {

            EditorGUILayout.LabelField("Smooth", EditorStyles.boldLabel);

            smoothBrushSize = EditorGUILayout.Slider(new GUIContent("Brush Size [% of Main Brush]", ""), smoothBrushSize, 0.0f, 300.0f);
            smoothBrushStrength = EditorGUILayout.Slider(new GUIContent("Brush Strength", ""), smoothBrushStrength, 0.0f, 100.0f);
        }

        override public void PaintSegments(StrokeSegment[] segments, IOnPaint editContext)
        {
            for (int i = 0; i < segments.Length; i++)
            {
                StrokeSegment segment = segments[i];

                Smooth(segment.currTerrain, editContext, segment.currUV);

            }
        }

        private bool Smooth(Terrain terrain, IOnPaint editContext, Vector2 currUV)
        {
            // the brush size is relative to the main brush size
            float brushSize = editContext.brushSize * this.smoothBrushSize / 100f;

            BrushTransform brushXform = TerrainPaintUtility.CalculateBrushTransform(terrain, editContext.uv, brushSize, 0.0f);
            PaintContext paintContext = TerrainPaintUtility.BeginPaintHeightmap(terrain, brushXform.GetBrushXYBounds());

            Material mat = TerrainPaintUtility.GetBuiltinPaintMaterial();

            float brushStrength = smoothBrushStrength / 100f; // editContext.brushStrength;

            // brushStrength = Event.current.shift ? -brushStrength : brushStrength;

            Vector4 brushParams = new Vector4(brushStrength, 0.0f, 0.0f, 0.0f);
            mat.SetTexture("_BrushTex", editContext.brushTexture);
            mat.SetVector("_BrushParams", brushParams);

            TerrainPaintUtility.SetupTerrainToolMaterialProperties(paintContext, brushXform, mat);

            Graphics.Blit(paintContext.sourceRenderTexture, paintContext.destinationRenderTexture, mat, (int)TerrainPaintUtility.BuiltinPaintMaterialPasses.SmoothHeights);

            TerrainPaintUtility.EndPaintHeightmap(paintContext, "Terrain Paint - Smooth Height");

            return true;
        }
    }
}

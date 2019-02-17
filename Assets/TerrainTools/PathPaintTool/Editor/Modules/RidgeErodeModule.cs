using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.TerrainAPI;


namespace UnityEditor.Experimental.TerrainAPI
{
    public class RidgeErodeModule : ModuleEditor
    {

        #region Materials
        Material material = null;
        Material GetMaterial()
        {
            if (material == null)
                material = new Material(Shader.Find("Hidden/TerrainTools/PathPaintTool/RidgeErode"));
            return material;
        }
        #endregion Materials

        #region Fields

        [SerializeField]
        float ridgeErodeBrushSize = 150f;

        [SerializeField]
        float ridgeErodeBrushStrength = 16.0f;

        [SerializeField]
        float erosionStrength = 16f;

        [SerializeField]
        float mixStrength = 0.7f;

        #endregion Fields

        private static Color ridgeErodeBrushColor = new Color(1.0f, 0.7f, 0.5f, 0.2f);


        override public string GetName()
        {
            return "Ridge Erosion";
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

                // the smooth brush size is relative to the main brush size
                float brushSize = editContext.brushSize * ridgeErodeBrushSize / 100f;

                BrushTransform brushXform = TerrainPaintUtility.CalculateBrushTransform(terrain, editContext.raycastHit.textureCoord, brushSize, 0.0f);
                PaintContext ctx = TerrainPaintUtility.BeginPaintHeightmap(terrain, brushXform.GetBrushXYBounds(), 1);
                Material brushPreviewMat = BrushUtilities.GetDefaultBrushPreviewMaterial();
                brushPreviewMat.color = ridgeErodeBrushColor;
                BrushUtilities.DrawBrushPreview(ctx, BrushUtilities.BrushPreview.SourceRenderTexture, editContext.brushTexture, brushXform, brushPreviewMat, 0);
                TerrainPaintUtility.ReleaseContextResources(ctx);
            }
        }

        override public void OnInspectorGUI(Terrain terrain, IOnInspectorGUI editContext)
        {
            EditorGUILayout.LabelField("Ridge Erode", EditorStyles.boldLabel);

            ridgeErodeBrushSize = EditorGUILayout.Slider(new GUIContent("Brush Size [% of Main Brush]", ""), ridgeErodeBrushSize, 0.0f, 300.0f);
            ridgeErodeBrushStrength = EditorGUILayout.Slider(new GUIContent("Brush Strength", ""), ridgeErodeBrushStrength, 0.0f, 100.0f);
            erosionStrength = EditorGUILayout.Slider(new GUIContent("Erosion Strength", ""), erosionStrength, 0.0f, 128.0f);
            mixStrength = EditorGUILayout.Slider(new GUIContent("Sharpness", ""), mixStrength, 0.0f, 1.0f);

        }

        override public void PaintSegments(StrokeSegment[] segments, IOnPaint editContext)
        {
            for (int i = 0; i < segments.Length; i++)
            {
                // first item doesn't have prevUV
                if (i == 0)
                    continue;

                StrokeSegment segment = segments[i];

                Smudge(segment.currTerrain, editContext, segment.currUV, segment.prevUV);
            }
        }


        private bool Smudge(Terrain terrain, IOnPaint editContext, Vector2 currUV, Vector2 prevUV)
        {
            // the brush size is relative to the main brush size
            float brushSize = editContext.brushSize * ridgeErodeBrushSize / 100f;

            BrushTransform brushXform = TerrainPaintUtility.CalculateBrushTransform(terrain, currUV, brushSize, 0.0f);
            PaintContext paintContext = TerrainPaintUtility.BeginPaintHeightmap(terrain, brushXform.GetBrushXYBounds(), 1);

            Vector2 smudgeDir = editContext.uv - prevUV;

            paintContext.sourceRenderTexture.filterMode = FilterMode.Bilinear;

            Material mat = GetMaterial();

            float brushStrength = ridgeErodeBrushStrength / 100f; // editContext.brushStrength;

            Vector4 brushParams = new Vector4(brushStrength, erosionStrength, mixStrength, 0);
            mat.SetTexture("_BrushTex", editContext.brushTexture);
            mat.SetVector("_BrushParams", brushParams);

            TerrainPaintUtility.SetupTerrainToolMaterialProperties(paintContext, brushXform, mat);
            Graphics.Blit(paintContext.sourceRenderTexture, paintContext.destinationRenderTexture, mat, 0);

            TerrainPaintUtility.EndPaintHeightmap(paintContext, "Terrain Paint - Ridge Erode");

            return true;
        }
    }
}

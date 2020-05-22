using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.TerrainAPI;


namespace UnityEditor.Experimental.TerrainAPI
{
    public class BridgeModule : ModuleEditor
    {
        #region Material
        Material material = null;
        Material GetMaterial()
        {
            if (material == null)
                material = new Material(Shader.Find("Hidden/TerrainTools/PathPaintTool/SetExactHeight"));
            return material;
        }
        #endregion Material

        #region Fields
        [SerializeField]
        AnimationCurve widthProfile = AnimationCurve.Linear(0, 1, 1, 1);

        [SerializeField]
        AnimationCurve heightProfile = AnimationCurve.Linear(0, 0, 1, 0);

        [SerializeField]
        AnimationCurve strengthProfile = AnimationCurve.Linear(0, 1, 1, 1);

        #endregion Fields

        private Color bridgeBrushColor = new Color(0.6f, 0.6f, 1.0f, 1.0f);

        public BridgeModule(bool active, int sceneGuiOrder, int paintSegmentOrder) : base(active, sceneGuiOrder, paintSegmentOrder)
        {
        }

        override public string GetName()
        {
            return "Bridge";
        }

        override public string GetDescription()
        {
            return "";
        }

        override public void OnSceneGUI(Terrain currentTerrain, IOnSceneGUI editContext, BrushSettings brushSettings)
        {
            
            Terrain terrain = currentTerrain;
            float brushSize = brushSettings.brushSize;

            BrushTransform brushXform = TerrainPaintUtility.CalculateBrushTransform(terrain, editContext.raycastHit.textureCoord, brushSize, brushSettings.brushRotationDegrees);
            PaintContext ctx = TerrainPaintUtility.BeginPaintHeightmap(terrain, brushXform.GetBrushXYBounds(), 1);
            Material brushPreviewMat = BrushUtilities.GetDefaultBrushPreviewMaterial();
            brushPreviewMat.color = bridgeBrushColor;
            BrushUtilities.DrawBrushPreview(ctx, BrushUtilities.BrushPreview.SourceRenderTexture, editContext.brushTexture, brushXform, brushPreviewMat, 0);
            TerrainPaintUtility.ReleaseContextResources(ctx);

        }

        override public void OnInspectorGUI(Terrain terrain, IOnInspectorGUI editContext, BrushSettings brushSettings)
        {
            EditorGUILayout.LabelField("Bridge", EditorStyles.boldLabel);

            //"Controls the width of the bridge over the length of the stroke"
            widthProfile = EditorGUILayout.CurveField(PathPaintStyles.widthProfileContent, widthProfile);
            heightProfile = EditorGUILayout.CurveField(PathPaintStyles.heightProfileContent, heightProfile);
            strengthProfile = EditorGUILayout.CurveField(PathPaintStyles.strengthProfileContent, strengthProfile);

        }



        override public void PaintSegments(StrokeSegment[] segments, IOnPaint editContext, BrushSettings brushSettings)
        {
            for (int i = 0; i < segments.Length; i++)
            {
                StrokeSegment segment = segments[i];

                Bridge(segment.currTerrain, editContext, segment.currUV, segment.pct, segment.stroke, segment.startPoint, brushSettings);

            }
        }

        private bool Bridge(Terrain terrain, IOnPaint editContext, Vector2 currUV, float pct, Vector3 stroke, Vector3 startPoint, BrushSettings brushSettings)
        {
            float heightOffset = heightProfile.Evaluate(pct) / terrain.terrainData.size.y;
            float strengthScale = strengthProfile.Evaluate(pct);
            float widthScale = widthProfile.Evaluate(pct);

            float finalHeight = ( startPoint + pct * stroke).z + heightOffset;
            int finalBrushSize = (int)(widthScale * (float)brushSettings.brushSize);

            BrushTransform brushXform = TerrainPaintUtility.CalculateBrushTransform(terrain, currUV, finalBrushSize, brushSettings.brushRotationDegrees);
            PaintContext paintContext = TerrainPaintUtility.BeginPaintHeightmap(terrain, brushXform.GetBrushXYBounds());

            Material mat = GetMaterial();
            Vector4 brushParams = new Vector4();

            mat.SetTexture("_BrushTex", editContext.brushTexture);

            brushParams.x = brushSettings.brushStrength * strengthScale;
            brushParams.y = 0.5f * finalHeight;

            mat.SetVector("_BrushParams", brushParams);

            TerrainPaintUtility.SetupTerrainToolMaterialProperties(paintContext, brushXform, mat);

            Graphics.Blit(paintContext.sourceRenderTexture, paintContext.destinationRenderTexture, mat, 0);

            // custom Undo. otherwise undo of mixing texture paint and terrain modification won't work
            BrushUndo.RegisterUndo(terrain, paintContext, "PathPaintTool");

            // no undo, we have our own
            TerrainPaintUtility.EndPaintHeightmap(paintContext, null);

            return true;
        }


    }
}

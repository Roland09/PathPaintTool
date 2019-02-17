using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Experimental.TerrainAPI;


namespace UnityEditor.Experimental.TerrainAPI
{
    public class StrokePaintMode : IPaintMode
    {
        private Terrain m_StartTerrain = null;
        private Vector3 m_StartPoint = Vector3.zero;

        #region Anchor

        [SerializeField]
        AnimationCurve jitterProfile = AnimationCurve.Linear(0, 0, 1, 0);

        [SerializeField]
        float m_Spacing = 0.01f;

        private Color anchorBrushColor = new Color(0.5f, 0.5f, 1.0f, 0.6f);

        public string GetName()
        {
            return "Stroke";
        }

        public string GetDescription()
        {
            return "";
        }

        public void OnSceneGUI(Terrain currentTerrain, IOnSceneGUI editContext)
        {
            if (m_StartTerrain != null && m_StartPoint != null)
            {
                // anchor is placed on the start terrain, independent of the active one; needed for multi-tiles
                Terrain terrain = m_StartTerrain;

                BrushTransform anchorBrushXform = TerrainPaintUtility.CalculateBrushTransform(terrain, m_StartPoint, editContext.brushSize, 0.0f);
                PaintContext anchorCtx = TerrainPaintUtility.BeginPaintHeightmap(terrain, anchorBrushXform.GetBrushXYBounds(), 1);
                Material brushPreviewMat = BrushUtilities.GetDefaultBrushPreviewMaterial();
                brushPreviewMat.color = anchorBrushColor;
                BrushUtilities.DrawBrushPreview(anchorCtx, BrushUtilities.BrushPreview.SourceRenderTexture, editContext.brushTexture, anchorBrushXform, brushPreviewMat, 0);
                TerrainPaintUtility.ReleaseContextResources(anchorCtx);
                
            }
        }

        #endregion Anchor


        public void OnInspectorGUI(Terrain terrain, IOnInspectorGUI editContext)
        {
            EditorGUILayout.LabelField("Stroke", EditorStyles.boldLabel);

            if (m_StartTerrain == null || m_StartPoint == null)
            {
                EditorGUILayout.HelpBox(PathPaintStyles.anchorRequiredContent.text, MessageType.Warning);
            }

            EditorGUILayout.HelpBox(PathPaintStyles.strokeUsageContent.text, MessageType.None);

            jitterProfile = EditorGUILayout.CurveField(PathPaintStyles.jitterProfileContent, jitterProfile);

            m_Spacing = EditorGUILayout.Slider(PathPaintStyles.splatDistanceContent, m_Spacing, 1.0f, 100.0f);
        }


        public StrokeSegment[] OnPaint(Terrain terrain, IOnPaint editContext)
        {

            //grab the starting position & height
            if (Event.current.shift)
            {
                UpdateStartPosition(terrain, editContext);

                return null;
            }

            if (!m_StartTerrain || (Event.current.type == EventType.MouseDrag))
            {
                return null;
            }

            StrokeSegment[] segments = GetStrokeSegments(terrain, editContext);

            // next start position
            UpdateStartPosition(terrain, editContext);

            return segments;
        }

        #region Input
        private void UpdateStartPosition(Terrain terrain, IOnPaint editContext)
        {
            bool repaintInspector = m_StartTerrain == null;

            Vector2 uv = editContext.uv;

            float height = terrain.terrainData.GetInterpolatedHeight(uv.x, uv.y) / terrain.terrainData.size.y;
            m_StartPoint = new Vector3(uv.x, uv.y, height);
            m_StartTerrain = terrain;

            // repaint the inspector, otherwise the helpbox about creating an anchor point would still be visible
            if (repaintInspector)
            { 
                InternalEditorUtility.RepaintAllViews();
            }
        }
        #endregion Input


        private StrokeSegment[] GetStrokeSegments(Terrain terrain, IOnPaint editContext)
        {

            Vector2 uv = editContext.uv;

            //get the target position & height
            float targetHeight = terrain.terrainData.GetInterpolatedHeight(uv.x, uv.y) / terrain.terrainData.size.y;
            Vector3 targetPos = new Vector3(uv.x, uv.y, targetHeight);

            if (terrain != m_StartTerrain)
            {
                //figure out the stroke vector in uv,height space
                Vector2 targetWorld = TransformUtilities.transformToWorld(terrain, uv);
                Vector2 targetUVs = TransformUtilities.transformToUVSpace(m_StartTerrain, targetWorld);
                targetPos.x = targetUVs.x;
                targetPos.y = targetUVs.y;
            }

            Vector3 stroke = targetPos - m_StartPoint;
            float strokeLength = stroke.magnitude;
            int numSplats = (int)(strokeLength / (0.001f * m_Spacing));

            Terrain currTerrain = m_StartTerrain;

            Vector2 posOffset = new Vector2(0.0f, 0.0f);
            Vector2 currUV = new Vector2();

            Vector2 jitterVec = new Vector2(-stroke.z, stroke.x); //perpendicular to stroke direction
            jitterVec.Normalize();

            Vector2 prevUV = Vector2.zero;

            StrokeSegment[] segments = new StrokeSegment[numSplats];

            for (int i = 0; i < numSplats; i++)
            {
                float pct = (float)i / (float)numSplats;

                float jitterOffset = jitterProfile.Evaluate(pct) / Mathf.Max(currTerrain.terrainData.size.x, currTerrain.terrainData.size.z);

                Vector3 currPos = m_StartPoint + pct * stroke;

                //add in jitter offset (needs to happen before tile correction)
                currPos.x += posOffset.x + jitterOffset * jitterVec.x;
                currPos.y += posOffset.y + jitterOffset * jitterVec.y;

                if (currPos.x >= 1.0f && (currTerrain.rightNeighbor != null))
                {
                    currTerrain = currTerrain.rightNeighbor;
                    currPos.x -= 1.0f;
                    posOffset.x -= 1.0f;
                }
                if (currPos.x <= 0.0f && (currTerrain.leftNeighbor != null))
                {
                    currTerrain = currTerrain.leftNeighbor;
                    currPos.x += 1.0f;
                    posOffset.x += 1.0f;
                }
                if (currPos.y >= 1.0f && (currTerrain.topNeighbor != null))
                {
                    currTerrain = currTerrain.topNeighbor;
                    currPos.y -= 1.0f;
                    posOffset.y -= 1.0f;
                }
                if (currPos.y <= 0.0f && (currTerrain.bottomNeighbor != null))
                {
                    currTerrain = currTerrain.bottomNeighbor;
                    currPos.y += 1.0f;
                    posOffset.y += 1.0f;
                }

                currUV.x = currPos.x;
                currUV.y = currPos.y;


                StrokeSegment ctx = new StrokeSegment();

                ctx.pct = pct;
                ctx.currTerrain = currTerrain;
                ctx.currUV = currUV;
                ctx.stroke = stroke;
                ctx.prevUV = prevUV;

                ctx.startPoint = m_StartPoint;

                segments[i] = ctx;

                prevUV = currUV;

            }

            return segments;
        }

    }
}

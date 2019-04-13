using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Experimental.TerrainAPI;


namespace UnityEditor.Experimental.TerrainAPI
{
    public class PaintBrushPaintMode : IPaintMode
    {
        private Terrain m_StartTerrain = null;
        private Vector3 m_StartPoint = Vector3.zero;

        EventType m_PreviousEvent = EventType.Ignore;

        // TODO: currently not supported. see StrokePaintMode when adding the feature later
        [SerializeField]
        AnimationCurve jitterProfile = AnimationCurve.Linear(0, 0, 1, 0);

        // TODO: currently not supported. see StrokePaintMode when adding the feature later
        /*
        [SerializeField]
        float m_Spacing = 0.01f;
        */

        public string GetName()
        {
            return "Paint Brush";
        }

        public string GetDescription()
        {
            return "";
        }

        public void OnSceneGUI(Terrain currentTerrain, IOnSceneGUI editContext, BrushSettings brushSettings)
        {
        }

        public void OnInspectorGUI(Terrain terrain, IOnInspectorGUI editContext, BrushSettings brushSettings)
        {
            EditorGUILayout.LabelField("Paint Brush", EditorStyles.boldLabel);

            EditorGUILayout.HelpBox(PathPaintStyles.paintBrushUsageContent.text, MessageType.None);

        }


        public StrokeSegment[] OnPaint(Terrain terrain, IOnPaint editContext, BrushSettings brushSettings)
        {

            if (Event.current.type == EventType.MouseDown)
            {
                UpdateStartPosition(terrain, editContext);

                return null;
            }


            if (m_StartTerrain == null)
            {
                return null;
            }

            StrokeSegment[] segments = null;

            if (Event.current.type == EventType.MouseDrag && m_PreviousEvent == EventType.MouseDrag)
            {
                Vector2 uv = editContext.uv;

                float height = terrain.terrainData.GetInterpolatedHeight(uv.x, uv.y) / terrain.terrainData.size.y;
                Vector3 currentPoint = new Vector3(uv.x, uv.y, height);

                Vector2 currPointWS = TransformUtilities.transformToWorld(terrain, uv);
                Vector2 prevPointWS = TransformUtilities.transformToWorld(terrain, new Vector2(m_StartPoint.x, m_StartPoint.y));

                Vector2 paintDirection = currPointWS - prevPointWS;

                // TODO: collect positions and perform modifications in a batch in the background
                if (paintDirection.magnitude > 1) // 1 just a "magic number" for now; see also  int numSplats = 1;
                {
                    segments = GetStrokeSegments(terrain, editContext, brushSettings);

                    // next start position
                    UpdateStartPosition(terrain, editContext);
                }

            }

            m_PreviousEvent = Event.current.type;

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


        private StrokeSegment[] GetStrokeSegments(Terrain terrain, IOnPaint editContext, BrushSettings brushSettings)
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
            int numSplats = 1; // (int)(strokeLength / (0.001f * m_Spacing)); <= TODO keeping numsplats at 1 for now, otherwise it would drain performance in paint mode; see also paintDirection.magnitude > 1

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

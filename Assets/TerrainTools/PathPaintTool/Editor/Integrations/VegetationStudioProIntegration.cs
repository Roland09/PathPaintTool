using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if VEGETATION_STUDIO_PRO
using AwesomeTechnologies.VegetationSystem;
using AwesomeTechnologies.VegetationStudio;
#endif

namespace UnityEditor.Experimental.TerrainAPI
{
    public class VegetationStudioProIntegration : AssetIntegration
    {
        public VegetationStudioProIntegration()
        {
#if VEGETATION_STUDIO_PRO

            Enabled = true;

#endif
        }

        public override string GetName()
        {
            return "Vegetation Studio Pro";
        }


        override public void Update(StrokeSegment[] segments, IOnPaint editContext)
        {
#if VEGETATION_STUDIO_PRO

            if (segments.Length > 0)
            {
                StrokeSegment segment = segments[0];

                Vector3 brushBounds = new Vector3(editContext.brushSize, editContext.brushSize, editContext.brushSize);

                Vector2 center2D = TransformUtilities.transformToWorld(segment.currTerrain, segment.currUV);
                Vector3 center = new Vector3(center2D.x, 0, center2D.y);

                Bounds bounds = new Bounds( center, brushBounds);

                for (int i = 1; i < segments.Length; i++)
                {
                    segment = segments[i];

                    Vector2 center2DNext = TransformUtilities.transformToWorld(segment.currTerrain, segment.currUV);
                    Vector3 centerNext = new Vector3(center2DNext.x, 0, center2DNext.y);

                    Bounds boundsNext = new Bounds(centerNext, brushBounds);

                    bounds.Encapsulate( boundsNext);
                }

                VegetationStudioManager.RefreshTerrainHeightMap(bounds);
            }
#endif
        }
    }
}

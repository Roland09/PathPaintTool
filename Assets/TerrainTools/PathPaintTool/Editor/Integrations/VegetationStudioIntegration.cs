using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if VEGETATION_STUDIO
using AwesomeTechnologies.VegetationStudio;
#endif

namespace UnityEditor.Experimental.TerrainAPI
{
    public class VegetationStudioIntegration : AssetIntegration
    {

        public VegetationStudioIntegration()
        {
#if VEGETATION_STUDIO

            Enabled = true;

#endif
        }

        public override string GetName()
        {
            return "Vegetation Studio";
        }

        public override void Update(StrokeSegment[] segments)
        {
#if VEGETATION_STUDIO

            if (segments.Length > 0)
            {
                StrokeSegment segment = segments[0];
                Bounds bounds = new Bounds(new Vector3(segment.currUV.x, 0, segment.currUV.y), Vector3.zero); ;
                for (int i = 1; i < segments.Length; i++)
                {
                    segment = segments[i];

                    Vector2 boundsWS = TransformUtilities.transformToWorld(segment.currTerrain, segment.currUV);
                    bounds.Encapsulate(new Vector3(boundsWS.x, 0, boundsWS.y));
                }

                VegetationStudioManager.RefreshTerrainHeightMap(bounds);

            }
#endif
        }
    }
}

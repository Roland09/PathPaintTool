using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;

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

        override public void OnPaintFinished()
        {
#if VEGETATION_STUDIO_PRO

            Debug.Log("Refreshing VS Pro Vegetation");

            VegetationStudioManager VegetationStudioInstance = FindVegetationStudioInstance();
            List<VegetationSystemPro> VegetationSystemList = VegetationStudioInstance.VegetationSystemList;

            for (int i = 0; i <= VegetationSystemList.Count - 1; i++)
            {
                VegetationSystemPro vegetationSystemPro = VegetationSystemList[i];

                vegetationSystemPro.RefreshVegetationSystem();

                SetSceneDirty(vegetationSystemPro);
            }
#endif
        }

#if VEGETATION_STUDIO_PRO
        public static void RefreshVegetation()
        {
            VegetationStudioManager VegetationStudioInstance = FindVegetationStudioInstance();

            List<VegetationSystemPro> VegetationSystemList = VegetationStudioInstance.VegetationSystemList;
            for (int i = 0; i <= VegetationSystemList.Count - 1; i++)
            {
                VegetationSystemPro vegetationSystemPro = VegetationSystemList[i];

                vegetationSystemPro.ClearCache();
                vegetationSystemPro.RefreshTerrainHeightmap();
                SceneView.RepaintAll();

                SetSceneDirty(vegetationSystemPro);
            }
        }

        public static void SetSceneDirty(VegetationSystemPro vegetationSystemPro)
        {
            if (!Application.isPlaying)
            {
                EditorSceneManager.MarkSceneDirty(vegetationSystemPro.gameObject.scene);
                EditorUtility.SetDirty(vegetationSystemPro);
            }
        }

        public static VegetationStudioManager FindVegetationStudioInstance()
        {
            return (VegetationStudioManager)Object.FindObjectOfType(typeof(VegetationStudioManager));
        }
#endif
    }
}
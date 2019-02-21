using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor.Experimental.TerrainAPI
{
    public class StrokeSegment
    {
        public float pct;
        public Terrain currTerrain;
        public Vector2 currUV;
        public Vector2 prevUV;
        public Vector3 stroke;

        /// <summary>
        /// The anchor point. In a list of stroke segments this would be the same for all of them.
        /// </summary>
        public Vector3 startPoint;
    }
}

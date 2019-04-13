using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor.Experimental.TerrainAPI
{
    public class BrushSettings
    {
        [SerializeField]
        public float brushSize = 40f;

        [SerializeField]
        public float brushStrength = 0.1f;

        [SerializeField]
        public float brushRotationDegrees = 0.0f;
    }
}

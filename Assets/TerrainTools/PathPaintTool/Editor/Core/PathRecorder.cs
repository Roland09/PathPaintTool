using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor.Experimental.TerrainAPI
{
    /// <summary>
    /// Record the paint positions
    /// </summary>
    public class PathRecorder
    {

        private List<Vector3> positions = new List<Vector3>();

        public void AddPosition(Vector3 pos)
        {
            positions.Add(pos);
        }

        public List<Vector3> GetPositions()
        {
            return positions;
        }

        public void StartRecording()
        {
            positions.Clear();
        }

        public void Clear()
        {
            positions.Clear();
        }


    }

}

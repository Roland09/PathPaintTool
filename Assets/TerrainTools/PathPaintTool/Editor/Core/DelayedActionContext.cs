using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor.Experimental.TerrainAPI
{
    /// <summary>
    /// Container for terrain paint data which will be applied delayed in a postprocess step
    /// </summary>
    public class DelayedActionContext
    {
        public StrokeSegment[] segments;
        public IOnPaint editContext;

        public DelayedActionContext(StrokeSegment[] segments, IOnPaint editContext)
        {
            this.segments = segments;
            this.editContext = editContext;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor.Experimental.TerrainAPI
{

    public class DelayedActionHandler
    {

        /// <summary>
        /// How many actions will be in the buffer
        /// </summary>
        int delayedActionBufferCount = 10;

        private List<DelayedActionContext> actionContextList = new List<DelayedActionContext>();
        private List<IDelayedAction> actionList = new List<IDelayedAction>();

        public void AddDelayedAction(IDelayedAction action)
        {
            actionList.Add(action);
        }

        public void AddDelayedAction(StrokeSegment[] segments, IOnPaint editContext)
        {
            actionContextList.Add(new DelayedActionContext(segments, editContext));
        }

        public void StartDelayedActions()
        {
            actionContextList.Clear();
        }

        public void ApplyDelayedActions()
        {
            while (actionContextList.Count > delayedActionBufferCount)
            {
                DelayedActionContext actionContext = actionContextList[0];
                actionContextList.RemoveAt(0);

                ApplyDelayedAction(actionContext);

            }

        }

        public void ApplyAllDelayedActions()
        {
            foreach (DelayedActionContext actionContext in actionContextList)
            {
                ApplyDelayedAction(actionContext);
            }

            actionContextList.Clear();

        }

        private void ApplyDelayedAction(DelayedActionContext actionContext)
        {
            foreach(IDelayedAction action in actionList)
            {
                action.OnActionPerformed(actionContext);
            }

        }

    }

}
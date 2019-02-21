using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor.Experimental.TerrainAPI
{
    public class PathPaintStyles
    {
        public static readonly GUIContent activeTerrainToolsContent = new GUIContent("Terrain Tools", "Active terrain tools which will be applied while the path is being painted");
        public static readonly GUIContent paintModesContent = new GUIContent("Paint Mode", "");
        public static readonly GUIContent debugContent = new GUIContent("Debug", "");

        public static readonly GUIContent anchorRequiredContent = new GUIContent("Please place an anchor on the terrain with shift + click", "");

        public static readonly GUIContent notImplemented = new GUIContent("Not implemented yet");

        #region BridgeModule
        public static readonly GUIContent widthProfileContent = new GUIContent("Width Profile", "A multiplier that controls the width of the bridge over the length of the stroke");
        public static readonly GUIContent heightProfileContent = new GUIContent("Height Offset Profile", "Adds a height offset to the bridge along the length of the stroke (World Units)");
        public static readonly GUIContent strengthProfileContent = new GUIContent("Strength Profile", "A multiplier that controls influence of the bridge along the length of the stroke");
        #endregion BridgeModule
        public static readonly GUIContent noTextureSelectedContent = new GUIContent("No texture selected");
        #region PaintModule

        #endregion PaintModule

        #region PaintBrushPaintMode
        public static readonly GUIContent paintBrushUsageContent = new GUIContent("Paint the path by dragging the mouse.");
        #endregion PaintBrushPaintMode


        #region StrokePaintMode
        public static readonly GUIContent strokeUsageContent = new GUIContent("Shift + Click to set the start point, click to connect the path.");
        public static readonly GUIContent jitterProfileContent = new GUIContent("Horizontal Offset Profile", "Adds an offset perpendicular to the stroke direction (World Units)");
        public static readonly GUIContent splatDistanceContent = new GUIContent("Brush Spacing", "Distance between brush splats");
        #endregion StrokePaintMode

        public static GUIStyle buttonActiveStyle = null;
        public static GUIStyle buttonNormalStyle = null;

        static PathPaintStyles()
        {
            buttonNormalStyle = "Button";
            buttonActiveStyle = new GUIStyle("Button");
            buttonActiveStyle.normal.background = buttonNormalStyle.active.background;
        }

        public static GUIStyle GetButtonToggleStyle(bool isToggled)
        {
            return isToggled ? buttonActiveStyle : buttonNormalStyle;
        }
    }
}



## **Path Paint Tool**

The Path Paint Tool is a free and Open Source extension for the Unity Terrain Tools. It is a Path Painter which allows you to modify the Unity terrain.


## Introduction

Let's see in a video how it looks like in action:

[![IMAGE ALT TEXT HERE](https://img.youtube.com/vi/K_XxgpzNZxc/0.jpg)](https://www.youtube.com/watch?v=K_XxgpzNZxc)
 
## Requirements

Unity 2018.3+

## Quick Setup

* create a new Unity 2018.3 project
* download and import PathPaintTool-Demo.unitypackage from here:

   https://www.dropbox.com/s/l7g5twg1wl9di9r/PathPaintTool-Demo.unitypackage
   
* open the demo scene TerrainToolsDemo/Scenes/Rocky Green Plateaus
* select a terrain
* click the *Paint Terrain* brush in the inspector of the terrain and select *Path Paint Tool*

Have Fun :-)

This is to get you started quickly. Delta updates on the code will be committed in this GitHub repository. Just replace the PathPaintTool folder with the latest commit.

## Features
- Supported  Terrain Tools:
Any combination of Paint, Bridge, Smooth, Ridge Erosion, Smudge.

 - Various Paint Modes
 
   * Paint Brush: Paint by dragging the mouse
   * Stroke: Create strokes by placing an anchor point and subsequently create strokes form the previous anchor point.
   * Automatic Waypoint creation and Spline manipulation are in development
   
- Multi Tile Terrain
- Unity 2019+ Support
- Vegetation Studio and Vegetation Studio Pro Support
- Open Source, **FREE** for everyone, no DLL
 
## Integrations

 - Vegeation Studio 
 - Vegetation Studio Pro 
## The Idea

Unity created and provided various tools for terrain manipulation for free. When I studied them I figured that it would make sense to combine them. So I tried and implemented a quick tool which combines varions Terrain Tools in 1 paint stroke. I still have to find the "golden settings" which work for everything, but I guess that's a not so easy task to accomplish. Sometimes you want smoothing, sometimes ridged erosion. So currently - until some feedback is gathered the advanced mode with full flexibility and all the settings available is the one to go.



## Notes

The demo unitypackage is provided to get you started with a tiled and textures terrain. Future updates will be done on the code alone.

## Important

The Undo feature is currently in development and not fully woring. Please backup your terrain before you start modifying it.

Credits
-------------------------------------
Full credit and a BIG THANK YOU(!!!) to the very skilled and most awesome developers at Unity who provided the Terrain Tool Samples for free for the community.

Demo Scene: 

World Creator 2 with which the creation of the demo terrain was possible within minutes. Most of all thank you to Yanik for providing the base terrain.

The textures for the terrain after importing into Unity are Creative Common textures which are freely available and can be used without restriction. 

Credit to these providers:

* CC0 Textures

	[https://cc0textures.com](https://cc0textures.com)

* cgbookcase

	[https://www.cgbookcase.com](https://www.cgbookcase.com)
	
## Roadmap

* Automatic Waypoint finder and shaping the terrain
* Spline creation, Spline saving and flexible adjustment
* Embankment
* Substance Support
* Additional Terrain Tool support
* Quick Access Settings

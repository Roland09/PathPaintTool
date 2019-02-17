


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
* click the *Paint Terrain* menu button in the terrain inspector and select *Path Paint Tool*

Have Fun :-)

This is to get you started quickly. Delta updates of the code will be committed to this GitHub repository. Just replace the PathPaintTool folder with the latest commit if you get the latest version.

## Features
- Supported  Terrain Tools:
Any combination of Paint, Bridge, Smooth, Ridge Erosion, Smudge.

 - Various Paint Modes
 
   * Paint Brush: Paint by dragging the mouse
   * Stroke: Create strokes by placing an anchor point and subsequently create strokes from the previous anchor point.
   * Automatic Waypoint creation and Spline manipulation are in development
   
- Create paths, roads, plateaus, ramps, lake and river beds, mountain spurs
   
- Multi Tile Terrain

- Unity 2018+ and 2019+ Support

- Vegetation Studio and Vegetation Studio Pro Support

- Open Source, **FREE** for everyone, no DLL

 
## Integrations

 - Vegeation Studio 
 - Vegetation Studio Pro 

For Vegetation Studio and Vegetation Studio Pro I recommend to use the include and exclude terrain texture rules.
 
## The Idea

Unity created and provided various tools for terrain manipulation for free. When I studied them I figured that it would make sense to combine them. So I tried and implemented a quick tool which combines varions Terrain Tools in 1 paint stroke. I still have to find the "golden settings" which work for everything, but I guess that's a not so easy task to accomplish. Sometimes you want smoothing, sometimes ridged erosion. So currently - until some feedback is gathered the advanced mode with full flexibility and all the settings available is the one to go.

So the basic idea is this:  
  
Have multiple brushes overlapping, e. g.:
  
* inner brush: texture tool  
* middle brush: bridge tool  
* outer brush: smooth tool  
  
To better visualize it, looks like this:

![Example](https://github.com/Roland09/PathPaintTool/blob/master/Documentation/Screenshots/stroke-example.jpg)

The yellow circle is the Paint Texture brush, the blue one is the Bridge tool, the grey one is the Smooth tool. There are others optionally available. All of the tools are applied in sequence. To the left in the screenshot is the brush itself, the right disc is the anchor point from where a stroke is painted to the brush location. This is the Stroke paint mode. There is also the Paint Brush mode. At one point the Stroke mode is better suited, at another point the Brush paint mode. Stroke mode is e. g. preferred for longer distances, to paint a path along a mountain side. Or from top of a region to the bottom of a region. I'll create presets depending on feedback.

As of the time of writing this readme file, the inspector looks like this:

![Inspector](https://github.com/Roland09/PathPaintTool/blob/master/Documentation/Screenshots/inspector.jpg)

The result is a Path Paint Tool which you can see here animated:

![Demo](https://github.com/Roland09/PathPaintTool/blob/master/Documentation/Screenshots/pathpainttool-motocross-track.gif)

So creating e. g. a motocross track was a matter of a minute:

![FinalScene](https://github.com/Roland09/PathPaintTool/blob/master/Documentation/Screenshots/motocross-track.jpg)


## Notes

The demo unitypackage is provided to get you started with a tiled and textured terrain. Future updates will be done on the code alone.

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
* Presets & Quick Access Settings

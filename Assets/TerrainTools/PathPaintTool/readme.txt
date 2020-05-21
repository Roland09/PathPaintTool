Path Paint Tool
-------------------------------------

Path Paint Tool is a Unity Terrain Tools extension. It allows you to paint paths along a terrain in Unity.

The code is heavily based on the free available Terrain Tool Samples provided by the awesome developers of Unity. You can get the original terrain tools here:
   
https://github.com/Unity-Technologies/TerrainToolSamples

All credit to them for their most impressive work. 

Basically this Path Paint Tool combines a set of the Terrain Tools in order to paint paths.

License
-------------------------------------

Since this is based on the TerrainToolSamples provided by Unity, the license is the same as the TerrainToolSamples:

https://github.com/Unity-Technologies/TerrainToolSamples/blob/master/LICENSE.md

Credits
-------------------------------------
The textures for the terrain are Creative Common textures which are freely available and can be used withour restriction. 

Credit to these providers:

* CC0 Textures

	https://cc0textures.com

* cgbookcase.com

	https://www.cgbookcase.com

Version History
-------------------------------------
* 1.00
   
  + created 2019.3 version

  changes in 2019.3 version:

	  + implemented undo for textures combined with height map changes. there are still rare cases where it doesn't seem to work fully, so better backup your terrain
	  + using full vegetation system refresh for vs pro after painting is finished

* 0.04
  
  + bugfix: moved Utilities folder to Editor, otherwise the build wouldn't compile

* 0.03
  
  custom brush:
  + resize: ctrl + mouse drag left/right
  + rotation: ctrl + mouse wheel

* 0.02

  + Added Underlay: Paint underlay texture below the main texture. 
    This is currently done via a delayed action, otherwise the brush of the underlay texture would paint over the main texture while dragging.
  
  + Preparation for Post Processing via Delayed Action

  + Changed default brush strengths

  + Added constructor for modules so that they have to be setup only at 2 places: the instantiation and the module list. The ordered lists are generated via the order number.

  + Moved style class to inspector folder


* 0.01 

  + Initial release
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformUtilities
{
    public static Vector2 transformToWorld(Terrain t, Vector2 uvs)
    {
        Vector3 tilePos = t.GetPosition();
        return new Vector2(tilePos.x, tilePos.z) + uvs * new Vector2(t.terrainData.size.x, t.terrainData.size.z);
    }

    public static Vector2 transformToUVSpace(Terrain originTile, Vector2 worldPos)
    {
        Vector3 originTilePos = originTile.GetPosition();
        Vector2 uvPos = new Vector2((worldPos.x - originTilePos.x) / originTile.terrainData.size.x,
                                    (worldPos.y - originTilePos.z) / originTile.terrainData.size.z);
        return uvPos;
    }

    public static Vector2 TerrainUVFromBrushLocation(Terrain terrain, Vector3 posWS)
    {
        // position relative to Terrain-space. doesnt handle rotations,
        // since that's not really supported at the moment
        Vector3 posTS = posWS - terrain.transform.position;
        Vector3 size = terrain.terrainData.size;

        return new Vector2(posTS.x / size.x, posTS.z / size.z);
    }
}

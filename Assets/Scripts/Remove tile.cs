using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Removetile : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase selectedTile;

    void Start()
    {
        RemoveSelectedTiles();
    }

    void RemoveSelectedTiles()
    {
        BoundsInt bounds = tilemap.cellBounds;
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);

        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = allTiles[x + y * bounds.size.x];

                if (tile == selectedTile)
                {
                    tilemap.SetTile(new Vector3Int(x + bounds.x, y + bounds.y, 0), null);
                }
            }
        }
    }
}

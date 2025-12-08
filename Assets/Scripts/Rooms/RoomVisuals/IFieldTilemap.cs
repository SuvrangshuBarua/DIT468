using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public interface IFieldTilemap
{
    void PlaceTile(Vector3Int position, TileBase tile);
    void ChangeColor(Vector3Int position, Color color);
}

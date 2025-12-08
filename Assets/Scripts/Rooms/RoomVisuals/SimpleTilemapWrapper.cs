using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SimpleTilemapWrapper : MonoBehaviour, IFieldTilemap
{
    [SerializeField] Tilemap _tilemap;

    public void ChangeColor(Vector3Int position, Color color)
    {
        _tilemap.SetColor(position, color);
        _tilemap.RefreshTile(position);
    }

    public void PlaceTile(Vector3Int pos, TileBase tile)
    {
        _tilemap.SetTile(pos, tile);
        _tilemap.RefreshTile(pos);
    }
    
}

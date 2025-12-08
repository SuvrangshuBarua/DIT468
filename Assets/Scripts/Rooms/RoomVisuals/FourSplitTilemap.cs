using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FourSplitTilemap : MonoBehaviour, IFieldTilemap
{
    [SerializeField] Tilemap _tilemap;

    public void PlaceTile(Vector3Int pos, TileBase tile)
    {
        pos = pos * 2;
        
        _tilemap.SetTile(new Vector3Int(pos.x, pos.y), tile);
        _tilemap.SetTile(new Vector3Int(pos.x + 1, pos.y), tile);
        _tilemap.SetTile(new Vector3Int(pos.x, pos.y + 1), tile);
        _tilemap.SetTile(new Vector3Int(pos.x + 1, pos.y + 1), tile);        
    }


    public void ChangeColor(Vector3Int position, Color color)
    {
        throw new System.NotImplementedException();
    }

    public void Refresh()
    {
        _tilemap.RefreshAllTiles();
    }
}

using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class PopulateRoomVisual : MonoBehaviour
{
    [SerializeField] Tilemap _collision;
    [SerializeField] FourSplitTilemap _background;
    
    [SerializeField] TileBase _backgroundTile;

    List<Vector3Int> _positionsChecked = new List<Vector3Int>();
    
    void Start()
    {
        Vector3Int startPosition = Vector3Int.FloorToInt(gameObject.transform.position);
        FloodFill(startPosition);
        _background.Refresh();
        _collision.gameObject.GetComponent<TilemapRenderer>().enabled = false;
    }    

    void FloodFill(Vector3Int cellPosition)
    {
        if (_positionsChecked.Contains(cellPosition))
        {
            return;
        }

        _positionsChecked.Add(cellPosition);
        

        TileBase tile = _collision.GetTile(cellPosition);
        if(tile != null)
        {
            return;
        }

        _background.PlaceTile(cellPosition, _backgroundTile);

        FloodFill(cellPosition + new Vector3Int(0, 1));
        FloodFill(cellPosition + new Vector3Int(0, -1));
        FloodFill(cellPosition + new Vector3Int(1, 0));
        FloodFill(cellPosition + new Vector3Int(-1, 0));
    }
    
}

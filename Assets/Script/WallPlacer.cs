using UnityEngine;
using UnityEngine.Tilemaps;

public class WallPlacer : MonoBehaviour
{
    public TileBase wallTile;  // Inspector에 벽 타일 넣기

    void Update()
    {
        if (Input.GetMouseButtonDown(0))  
        {
            PlaceWall();
        }
    }

    void PlaceWall()
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPos.z = 0;

        //GridManager.Instance.SetWallTile(worldPos, wallTile);
    }
}
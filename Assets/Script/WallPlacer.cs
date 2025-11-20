using UnityEngine;
using UnityEngine.Tilemaps;

public class WallPlacer : MonoBehaviour
{
    public Tilemap wallMap;
    public TileBase wallTile;  // Inspector에서 벽 타일 등록
    public Transform player;   // 플레이어 Transform 넣는 곳

    void Update()
    {
        if (Input.GetMouseButtonDown(1))   // 우클릭
        {
            PlaceWallTile();
        }
    }

    void PlaceWallTile()
    {
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPos = wallMap.WorldToCell(mouseWorld);
        cellPos.z = 0;

        // 실제 타일 설치
        wallMap.SetTile(cellPos, wallTile);

        // GridManager의 walkable 업데이트
        int gx = cellPos.x - GridManager.Instance.Bounds.x;
        int gy = cellPos.y - GridManager.Instance.Bounds.y;

        if (gx >= 0 && gx < GridManager.Instance.SizeX &&
            gy >= 0 && gy < GridManager.Instance.SizeY)
        {
            GridManager.Instance.Grid[gx, gy].Walkable = false;
        }

        // BFS 갱신 (플레이어를 기준으로)
        BFSManager.Instance.BuildDistanceField(player.position);
    }
}
using UnityEngine;
using UnityEngine.Tilemaps;

public class AStarGridManager : MonoBehaviour
{
    public static AStarGridManager Instance;

    public Tilemap ground;
    public Tilemap wall;

    public AStarNode[,] Grid;
    public int SizeX, SizeY;
    public BoundsInt bounds;

    void Awake()
    {
        Instance = this;
        BuildGrid();
    }

    public void BuildGrid()
    {
        bounds = ground.cellBounds;
        SizeX = bounds.size.x;
        SizeY = bounds.size.y;

        Grid = new AStarNode[SizeX, SizeY];

        for (int x = 0; x < SizeX; x++)
        {
            for (int y = 0; y < SizeY; y++)
            {
                Vector3Int cell = new(bounds.x + x, bounds.y + y);
                bool walkable = ground.HasTile(cell) && !wall.HasTile(cell);
                Vector3 wp = ground.CellToWorld(cell) + new Vector3(0.5f, 0.5f);

                Grid[x, y] = new AStarNode(walkable, wp, x, y);
            }
        }
    }

    public AStarNode NodeFromWorld(Vector3 world)
    {
        Vector3Int cell = ground.WorldToCell(world);
        int gx = cell.x - bounds.x;
        int gy = cell.y - bounds.y;

        if (gx < 0 || gx >= SizeX || gy < 0 || gy >= SizeY)
            return null;

        return Grid[gx, gy];
    }
}
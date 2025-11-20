using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class FlowFieldManager : MonoBehaviour
{
    public static FlowFieldManager Instance;
    
    [Header("타일맵")]
    public Tilemap ground;
    public Tilemap wall;

    private Node[,] _grids;
    private int _sizeX, _sizeY;
    private BoundsInt _bounds;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        BuildGrid();
    }

    public void BuildGrid()
    {
        _bounds = ground.cellBounds;

        _sizeX = _bounds.size.x;
        _sizeY = _bounds.size.y;

        _grids = new Node[_sizeX, _sizeY];

        for(int x = 0; x < _sizeX; x++)
        {
            for(int y = 0; y < _sizeY; y++)
            {
                Vector3Int cell = new(_bounds.x + x, _bounds.y + y);
                bool walk = ground.HasTile(cell) && !wall.HasTile(cell);

                Vector3 wp = ground.CellToWorld(cell) + new Vector3(0.5f, 0.5f);

                _grids[x, y] = new Node(walk, wp, x, y);
            }
        }
    }

    bool InRange(int x, int y) => x >= 0 && x < _sizeX && y >= 0 && y < _sizeY;

    // Integration Field (BFS)
    public void BuildIntegrationField(Vector3 targetWorld)
    {
        Node target = NodeFromWorld(targetWorld);
        if (target == null) return;

        foreach (var grid in _grids) grid.Integration = int.MaxValue;

        target.Integration = 0;

        Queue<Node> q = new();
        q.Enqueue(target);

        int[,] directional = {
            {1,0}, {-1,0}, {0,1}, {0,-1},
            {1,1}, {-1,1}, {1,-1}, {-1,-1}
        };

        while(q.Count > 0)
        {
            Node current = q.Dequeue();

            for(int i=0;i<8;i++)
            {
                int nx = current.X + directional[i,0];
                int ny = current.Y + directional[i,1];
                
                if (!InRange(nx,ny)) continue;
                Node next = _grids[nx,ny];
                if (!next.Walkable) continue;

                // 코너컷 방지 (대각선일 경우)
                bool diag = directional[i,0]!=0 && directional[i,1]!=0;
                if(diag)
                {
                    Node side1 = _grids[current.X + directional[i,0], current.Y];
                    Node side2 = _grids[current.X, current.Y + directional[i,1]];
                    if (!side1.Walkable || !side2.Walkable)
                        continue;
                }

                int newCost = current.Integration + next.Cost;
                if (newCost < next.Integration)
                {
                    next.Integration = newCost;
                    q.Enqueue(next);
                }
            }
        }
    }

    // 2) Flow Field: 방향 벡터 생성
    public void BuildFlowField()
    {
        int[,] directional = {
            {1,0}, {-1,0}, {0,1}, {0,-1},
            {1,1}, {-1,1}, {1,-1}, {-1,-1}
        };

        foreach (var node in _grids)
        {
            if (!node.Walkable) continue;

            int best = node.Integration;
            Node bestNode = null;

            for(int i=0;i<8;i++)
            {
                int nx = node.X + directional[i,0];
                int ny = node.Y + directional[i,1];

                if (!InRange(nx,ny)) continue;

                Node next = _grids[nx,ny];
                if (next.Integration < best)
                {
                    best = next.Integration;
                    bestNode = next;
                }
            }

            if (bestNode != null)
                node.Direction = (bestNode.WorldPosition - node.WorldPosition).normalized;
        }
    }

    
    public Node NodeFromWorld(Vector3 world)
    {
        Vector3Int cell = ground.WorldToCell(world);
        
        int gx = cell.x - _bounds.x;
        int gy = cell.y - _bounds.y;

        if (!InRange(gx,gy)) return null;

        return _grids[gx,gy];
    }
    
    void OnDrawGizmos()
    {
        if (_grids == null) return;

        Gizmos.color = Color.yellow;

        float arrowSize = 0.3f;

        for (int x = 0; x < _sizeX; x++)
        {
            for (int y = 0; y < _sizeY; y++)
            {
                Node n = _grids[x, y];
                if (!n.Walkable) continue;

                Vector3 pos = n.WorldPosition;

                // 방향이 없으면 그리지 않음
                if (n.Direction == Vector2.zero) continue;

                Vector3 dir = new Vector3(n.Direction.x, n.Direction.y, 0);

                // 기본 선 (화살표 몸통)
                Gizmos.DrawLine(pos, pos + dir * arrowSize);

                // 화살촉 그리기
                Vector3 right = Quaternion.Euler(0, 0, 150) * dir * (arrowSize * 0.4f);
                Vector3 left = Quaternion.Euler(0, 0, -150) * dir * (arrowSize * 0.4f);

                Gizmos.DrawLine(pos + dir * arrowSize, pos + dir * arrowSize + right);
                Gizmos.DrawLine(pos + dir * arrowSize, pos + dir * arrowSize + left);
            }
        }
    }
}
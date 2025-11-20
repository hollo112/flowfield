using UnityEngine;
using System.Collections.Generic;

public class AStarPathfinder : MonoBehaviour
{
    public static AStarPathfinder Instance;

    void Awake()
    {
        Instance = this;
    }

    public List<AStarNode> FindPath(Vector3 startWorld, Vector3 goalWorld)
    {

        // 0) GridManager 유효성 체크
        if (AStarGridManager.Instance == null)
        {
            Debug.LogError("A*: GridManager.Instance is NULL!");
            return null;
        }

        AStarNode[,] grid = AStarGridManager.Instance.Grid;
        if (grid == null)
        {
            Debug.LogError("A*: Grid is NULL! BuildGrid()가 실행되지 않음");
            return null;
        }

        // 1) 월드좌표 → 그리드 노드 변환
        AStarNode start = AStarGridManager.Instance.NodeFromWorld(startWorld);
        AStarNode goal = AStarGridManager.Instance.NodeFromWorld(goalWorld);

        if (start == null)
        {
            Debug.LogError($"A*: START node null → {startWorld}");
            return null;
        }
        if (goal == null)
        {
            Debug.LogError($"A*: GOAL node null → {goalWorld}");
            return null;
        }
        if (!goal.Walkable)
        {
            // 목표가 벽인 경우 경로 없음
            return null;
        }

        // 2) A* 초기화
        List<AStarNode> open = new();
        HashSet<AStarNode> closed = new();

        foreach (AStarNode n in grid)
        {
            n.G = int.MaxValue;
            n.H = 0;
            n.Parent = null;
        }

        start.G = 0;
        start.H = Heuristic(start, goal);

        open.Add(start);

        // 8방향
        int[,] dirs =
        {
        { 1,  0}, {-1,  0}, { 0,  1}, { 0, -1},
        { 1,  1}, {-1,  1}, { 1, -1}, {-1, -1}
    };

        // 3) A*
        while (open.Count > 0)
        {
            // F값이 가장 작은 노드를 찾는다
            open.Sort((a, b) => a.F.CompareTo(b.F));
            AStarNode cur = open[0];

            // 목표 도달
            if (cur == goal)
                return RetracePath(start, goal);

            open.RemoveAt(0);
            closed.Add(cur);

            // 8방향 이웃 검사
            for (int i = 0; i < 8; i++)
            {
                int nx = cur.X + dirs[i, 0];
                int ny = cur.Y + dirs[i, 1];

                // 맵 범위 체크
                if (nx < 0 || nx >= AStarGridManager.Instance.SizeX ||
                    ny < 0 || ny >= AStarGridManager.Instance.SizeY)
                    continue;

                AStarNode next = grid[nx, ny];

                // 벽 or 이미 방문
                if (!next.Walkable || closed.Contains(next))
                    continue;

                // 🔥 코너컷 방지 (대각선 이동 시)
                if (dirs[i, 0] != 0 && dirs[i, 1] != 0)
                {
                    AStarNode sideA = grid[cur.X + dirs[i, 0], cur.Y];
                    AStarNode sideB = grid[cur.X, cur.Y + dirs[i, 1]];

                    if (!sideA.Walkable || !sideB.Walkable)
                        continue;
                }

                // G 비용 계산
                int moveCost = (dirs[i, 0] != 0 && dirs[i, 1] != 0) ? 14 : 10;
                int newG = cur.G + moveCost;

                // open에 없거나 더 좋은 경로면 갱신
                if (newG < next.G)
                {
                    next.G = newG;
                    next.H = Heuristic(next, goal);
                    next.Parent = cur;

                    if (!open.Contains(next))
                        open.Add(next);
                }
            }
        }

        return null; // 경로 없음
    }

    int Heuristic(AStarNode a, AStarNode b)
    {
        int dx = Mathf.Abs(a.X - b.X);
        int dy = Mathf.Abs(a.Y - b.Y);
        return 10 * (dx + dy) + (14 - 2 * 10) * Mathf.Min(dx, dy);
    }

    List<AStarNode> RetracePath(AStarNode start, AStarNode end)
    {
        List<AStarNode> path = new();
        AStarNode cur = end;

        while (cur != start)
        {
            path.Add(cur);
            cur = cur.Parent;
        }

        path.Reverse();
        return path;
    }
}
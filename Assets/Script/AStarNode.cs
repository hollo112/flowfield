using UnityEngine;

public class AStarNode
{
    public bool Walkable;
    public Vector3 WorldPos;
    public int X, Y;

    public int G; // 시작 → 현재까지 비용
    public int H; // 휴리스틱(현재 → 목표)
    public int F => G + H;

    public AStarNode Parent;

    public AStarNode(bool walkable, Vector3 worldPos, int x, int y)
    {
        Walkable = walkable;
        WorldPos = worldPos;
        X = x;
        Y = y;
    }
}
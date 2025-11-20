using UnityEngine;

using UnityEngine;

public class Node
{
    public bool Walkable;
    public Vector3 WorldPos;
    public int X, Y;
    public int Distance;  // BFS °Å¸®

    public Node(bool walkable, Vector3 worldPos, int x, int y)
    {
        Walkable = walkable;
        WorldPos = worldPos;
        X = x;
        Y = y;
        Distance = int.MaxValue;
    }
}
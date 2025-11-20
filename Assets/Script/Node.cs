using UnityEngine;

public class Node
{
    private bool _walkable;
    public bool Walkable => _walkable;
    public int Cost{get; private set;}
    public int Integration;
    public Vector2 Direction;
    private int _x, _y;
    public int X => _x;
    public int Y => _y;
    public Vector3 WorldPosition{get; private set;}

    public Node(bool walkable, Vector3 worldPosition, int x, int y)
    {
        this._walkable = walkable;
        this.WorldPosition = worldPosition;
        this._x = x;
        this._y = y;
        
        Cost = walkable ? 1 : int.MaxValue;
        Integration = int.MaxValue;
        Direction = Vector2.zero;
    }
}
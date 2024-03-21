using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoomType{
    StartRoom,
    DefaultRoom,
    FinalRoom
}

public class Room
{
    public Dictionary<Vector2Int,bool> Doors = new Dictionary<Vector2Int, bool>(){
        {Vector2Int.up, false},
        {Vector2Int.down, false},
        {Vector2Int.left, false},
        {Vector2Int.right, false}
    };

    //<direção,posição>
    public Dictionary<Vector2Int,Vector2Int> Neighbors = new Dictionary<Vector2Int, Vector2Int>();

    public RoomType Type;

    public Room(RoomType _type = RoomType.DefaultRoom){
        Type = _type;
    }
}

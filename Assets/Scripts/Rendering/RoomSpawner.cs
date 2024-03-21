using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public event Action<int> OnGenerationFinished;

    [SerializeField] private LevelGenerator _generator;
    [SerializeField] private float _xMultiplier;
    [SerializeField] private float _yMultiplier;
    [SerializeField] private RoomRenderer _roomPrefab;
    private Transform _parent;

    private Dictionary<Vector2Int, Room> _generatedRooms;

    public void StartGeneration(GenerationParams generationParams){
        _generator.InitGenerator(generationParams);

        _generatedRooms = _generator.Generate();

        if(_parent != null){
            Destroy(_parent.gameObject);
        }
        _parent = new GameObject().transform;
        _parent.name = "Rooms";

        SpawnRooms();

        OnGenerationFinished?.Invoke(_generatedRooms.Count);
    }

    private void SpawnRooms(){
        foreach(var item in _generatedRooms){
            Vector2Int position = item.Key;
            Room room = item.Value;

            if(room.Type != RoomType.StartRoom){
                SpawnRoom(position,room);
            }
        }
    }

    private void SpawnRoom(Vector2Int position, Room room){
        Vector3 realPosition = new Vector3(position.x * _xMultiplier, position.y * _yMultiplier, 0);

        RoomRenderer spawnRoom = Instantiate(_roomPrefab, realPosition, Quaternion.identity, _parent);

        if(room.Type == RoomType.FinalRoom){
            spawnRoom.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        }

        if(room.Doors[Vector2Int.up]){
            spawnRoom.OpenDoor(Vector2Int.up,true);
        }
        if(room.Doors[Vector2Int.down]){
            spawnRoom.OpenDoor(Vector2Int.down,true);
        }
        if(room.Doors[Vector2Int.left]){
            spawnRoom.OpenDoor(Vector2Int.left,true);
        }
        if(room.Doors[Vector2Int.right]){
            spawnRoom.OpenDoor(Vector2Int.right,true);
        }
    }
}

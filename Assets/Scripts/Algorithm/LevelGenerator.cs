using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    private int _maxRoomsApprox;
    private int _minRooms;
    private int _seed;
    private bool _useSeed;

    private int _upperBound;
    private int _lowerBound;
    private int _leftBound;
    private int _rightBound;

    private int _dictionaryIndex;
    private Dictionary<Vector2Int, Room> _mapRooms = new Dictionary<Vector2Int, Room>();

    private Room _startRoom;

    public void InitGenerator(GenerationParams generationParams){
        _maxRoomsApprox = generationParams.MaxRoomsApprox;
        _minRooms = generationParams.MinRooms;
        _seed = generationParams.Seed;
        _useSeed = generationParams.UseSeed;
        _upperBound = generationParams.UpperBound;
        _lowerBound = generationParams.LowerBound;
        _leftBound = generationParams.LeftBound;
        _rightBound = generationParams.RightBound;
        _startRoom = generationParams.StartRoom;
    }


    public Dictionary<Vector2Int, Room> Generate(){
        
        if(_useSeed){
            Random.InitState(_seed);
        } else {
            Random.InitState((int)System.DateTime.Now.Ticks);
        }

        _mapRooms.Clear();

        //Setup da primeira sala
        _mapRooms.Add(Vector2Int.zero, _startRoom);

        _dictionaryIndex = 0;//Reset do index


        while(_mapRooms.Count < _maxRoomsApprox) {

            DictionaryIteration();

            //Parar loop
            if(_dictionaryIndex >= _mapRooms.Count){
                break;
            }
            //Executa mais uma iteração para fechar as salas
            if(!(_mapRooms.Count < _maxRoomsApprox)){
                DictionaryIteration();
            }
        }

        if(!IsBigEnough() && !_useSeed){
            return Generate();
        }

        FindFinalRoom();

        return _mapRooms;
    }

    void DictionaryIteration(){
        for(int i = _dictionaryIndex, count = _mapRooms.Count; i < count; i++){
            var item = _mapRooms.ElementAt(i);
            Room room = item.Value;
            Vector2Int roomPos = item.Key;

            if(room.Doors[Vector2Int.up] && !_mapRooms.ContainsKey(roomPos + Vector2Int.up)){
                CreateRoom(roomPos + Vector2Int.up);
            }
            if(room.Doors[Vector2Int.down] && !_mapRooms.ContainsKey(roomPos + Vector2Int.down)){
                CreateRoom(roomPos + Vector2Int.down);
            }
            if(room.Doors[Vector2Int.left] && !_mapRooms.ContainsKey(roomPos + Vector2Int.left)){
                CreateRoom(roomPos + Vector2Int.left);
            }
            if(room.Doors[Vector2Int.right] && !_mapRooms.ContainsKey(roomPos + Vector2Int.right)){
                CreateRoom(roomPos + Vector2Int.right);
            }
            _dictionaryIndex++;
        }
    }

    private void CreateRoom(Vector2Int position){
        Room newRoom = new Room();

        newRoom.Doors[Vector2Int.up] = AssignDoor(position, Vector2Int.up);
        newRoom.Doors[Vector2Int.down] = AssignDoor(position, Vector2Int.down);
        newRoom.Doors[Vector2Int.left] = AssignDoor(position, Vector2Int.left);
        newRoom.Doors[Vector2Int.right] = AssignDoor(position, Vector2Int.right);

        _mapRooms.Add(position,newRoom);
    }

    private bool AssignDoor(Vector2Int originalPosition, Vector2Int direction){
        Vector2Int targetPosition = originalPosition + direction;

        if(_mapRooms.TryGetValue(targetPosition, out Room neighbor)){
            if(neighbor.Doors[-direction]){
                ConnectRooms(originalPosition,direction,targetPosition);
                return true;
            }
            return false;
        }

        if(targetPosition.y >= _upperBound){
            return false;
        }
        
        if(targetPosition.y <= -_lowerBound){
            return false;
        }

        if(targetPosition.x >= _rightBound){
            return false;
        }

        if(targetPosition.x <= -_leftBound){
            return false;
        }

        int roomCount = _mapRooms.Count;

        if(_dictionaryIndex >= roomCount){
            return false;
        }

        if(roomCount >= _maxRoomsApprox){
            return false;
        }

        return RandomBoolean();
    }

    void ConnectRooms(Vector2Int position1, Vector2Int direction, Vector2Int position2){
        //conectar salas
        Room room2 = _mapRooms[position2];
        if(room2.Neighbors.ContainsKey(-direction)){
            _mapRooms[position1].Neighbors.Add(direction,position2);
            _mapRooms[position2].Neighbors.Add(-direction,position1);
        }
    }

    void FindFinalRoom(){
        for(int i = _mapRooms.Count-1; i >= 0; i--){
            var item = _mapRooms.ElementAt(i);
            Room room = item.Value;

            int doorCount = room.Doors.Count(doors => doors.Value == true);

            if(doorCount == 1 && room.Type != RoomType.StartRoom){
                _mapRooms.ElementAt(i).Value.Type = RoomType.FinalRoom;
                break;
            }
        }
    }

    bool IsBigEnough(){
        return _mapRooms.Count >= _minRooms;
    }

    private bool RandomBoolean(){
        if (Random.value >= 0.5){
            return true;
        }
        return false;
    }
}

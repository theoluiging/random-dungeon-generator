using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomRenderer : MonoBehaviour
{
    [SerializeField] private GameObject DoorUp;
    [SerializeField] private GameObject DoorDown;
    [SerializeField] private GameObject DoorLeft;
    [SerializeField] private GameObject DoorRight;


    public void OpenDoor(Vector2Int direction, bool open){
        if(direction == Vector2Int.up){
            DoorUp.SetActive(open);
        }
        if(direction == Vector2Int.down){
            DoorDown.SetActive(open);
        }
        if(direction == Vector2Int.left){
            DoorLeft.SetActive(open);
        }
        if(direction == Vector2Int.right){
            DoorRight.SetActive(open);
        }
    }

    public bool IsDoorOpen(Vector2Int direction){
        if(direction == Vector2Int.up){
            return DoorUp.activeSelf;
        }
        if(direction == Vector2Int.down){
            return DoorDown.activeSelf;
        }
        if(direction == Vector2Int.left){
            return DoorLeft.activeSelf;
        }
        if(direction == Vector2Int.right){
            return DoorRight.activeSelf;
        }
        return false;
    }
}

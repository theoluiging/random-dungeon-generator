using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _roomCountTxt;
    [SerializeField] private RoomSpawner _spawner;
    private Transform _camTransform;

    private void Start() {
        _camTransform = Camera.main.transform;
        _spawner.OnGenerationFinished += UpdateRoomCount;
    }

    private void UpdateRoomCount(int count){
        _roomCountTxt.SetText("Salas: " + count.ToString());
    }

    public void ResetCam(){
        _camTransform.position = new Vector3(0f,0f,-10f);
    }
}

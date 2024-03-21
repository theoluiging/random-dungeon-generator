using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GenerationSettings : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _upperBoundTxt;
    [SerializeField] private TextMeshProUGUI _lowerBoundTxt;
    [SerializeField] private TextMeshProUGUI _leftBoundTxt;
    [SerializeField] private TextMeshProUGUI _rightBoundTxt;

    [Space(10)]
    [SerializeField] private Slider _upperBoundSlider;
    [SerializeField] private Slider _lowerBoundSlider;
    [SerializeField] private Slider _leftBoundSlider;
    [SerializeField] private Slider _rightBoundSlider;

    [Space(10)]
    [SerializeField] private RoomRenderer _startRoom;
    [SerializeField] private RoomSpawner _spawner;

    private bool _isOpen;

    private GenerationParams generationParams;

    private void Awake() {
        gameObject.SetActive(false);

        generationParams = new GenerationParams();
    }

    private void Start() {
        UpToggle(true);
        DownToggle(true);
        LeftToggle(true);
        RightToggle(true);
    }

    public void ToggleOpen(){
        if(_isOpen){
            gameObject.SetActive(false);
            _isOpen = false;
        } else {
            gameObject.SetActive(true);
            _isOpen = true;
        }
    }
    
    public void AssignMaxRooms(string input){
        try{
            generationParams.MaxRoomsApprox = int.Parse(input);
        } catch (FormatException){
            generationParams.MaxRoomsApprox = 15;
        }
    }
    public void AssignMinRooms(string input){
        try{
            generationParams.MinRooms = int.Parse(input);
        } catch (FormatException){
            generationParams.MinRooms = 5;
        }
    }
    public void AssignSeed(string input){
        try{
            generationParams.Seed = int.Parse(input);
            generationParams.UseSeed = true;
        } catch (FormatException){
            generationParams.UseSeed = false;
        }
        
    }

    public void ChangeUpperBound(float value){
        _upperBoundTxt.SetText(value.ToString());

        generationParams.UpperBound = (int)value;
    }
    public void ChangeLowerBound(float value){
        _lowerBoundTxt.SetText(value.ToString());

        generationParams.LowerBound = (int)value;
    }
    public void ChangeLeftBound(float value){
        _leftBoundTxt.SetText(value.ToString());

        generationParams.LeftBound = (int)value;
    }
    public void ChangeRightBound(float value){
        _rightBoundTxt.SetText(value.ToString());

        generationParams.RightBound = (int)value;
    }

    public void UpToggle(bool value){
        _startRoom.OpenDoor(Vector2Int.up, value);

        if(_startRoom.IsDoorOpen(Vector2Int.up)){
            _upperBoundSlider.minValue = 1;
            ChangeUpperBound(_upperBoundSlider.value);
        } else {
            _upperBoundSlider.minValue = 0;
            ChangeUpperBound(_upperBoundSlider.value); 
        }
    }
    public void DownToggle(bool value){
        _startRoom.OpenDoor(Vector2Int.down, value);

        if(_startRoom.IsDoorOpen(Vector2Int.down)){
            _lowerBoundSlider.minValue = 1;
            ChangeLowerBound(_lowerBoundSlider.value);
        } else {
            _lowerBoundSlider.minValue = 0;
            ChangeLowerBound(_lowerBoundSlider.value); 
        }
    }
    public void LeftToggle(bool value){
        _startRoom.OpenDoor(Vector2Int.left, value);

        if(_startRoom.IsDoorOpen(Vector2Int.left)){
            _leftBoundSlider.minValue = 1;
            ChangeLeftBound(_leftBoundSlider.value);
        } else {
            _leftBoundSlider.minValue = 0;
            ChangeLeftBound(_leftBoundSlider.value); 
        }
    }
    public void RightToggle(bool value){
        _startRoom.OpenDoor(Vector2Int.right, value);

        if(_startRoom.IsDoorOpen(Vector2Int.right)){
            _rightBoundSlider.minValue = 1;
            ChangeRightBound(_rightBoundSlider.value);
        } else {
            _rightBoundSlider.minValue = 0;
            ChangeRightBound(_rightBoundSlider.value); 
        }
    }

    public void GenerateButton(){
        if(generationParams.MinRooms >= generationParams.MaxRoomsApprox){
            throw new InvalidOperationException("Mínimo de Salas deve ser menor que Máximo de Salas (Aproximado)");
        }
        if(!_startRoom.IsDoorOpen(Vector2Int.up) && !_startRoom.IsDoorOpen(Vector2Int.down) && !_startRoom.IsDoorOpen(Vector2Int.left) && !_startRoom.IsDoorOpen(Vector2Int.right)){
           throw new InvalidOperationException("A Sala Inicial deve ter pelo menos uma porta"); 
        }
        if(generationParams.UpperBound == 0 && generationParams.LowerBound == 0){
            throw new InvalidOperationException("Limites Inválidos"); 
        }
        if(generationParams.LeftBound == 0 && generationParams.RightBound == 0){
            throw new InvalidOperationException("Limites Inválidos"); 
        }

        generationParams.StartRoom = CreateParamsStartRoom();

        _spawner.StartGeneration(generationParams);
    }

    private Room CreateParamsStartRoom(){
        Room room = new Room(RoomType.StartRoom);

        if(_startRoom.IsDoorOpen(Vector2Int.up)){
            room.Doors[Vector2Int.up] = true;
        }
        if(_startRoom.IsDoorOpen(Vector2Int.down)){
            room.Doors[Vector2Int.down] = true;
        }
        if(_startRoom.IsDoorOpen(Vector2Int.left)){
            room.Doors[Vector2Int.left] = true;
        }
        if(_startRoom.IsDoorOpen(Vector2Int.right)){
            room.Doors[Vector2Int.right] = true;
        }

        return room;
    }
}

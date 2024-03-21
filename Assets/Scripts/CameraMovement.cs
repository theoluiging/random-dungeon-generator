using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 8f;
    [SerializeField] private float _zoomSpeed;

    //Vetores de movimentação
    private Vector2 _moveVelocity;
    private Vector2 _moveInput;

    private Camera _camera;

    private void Start() {
        _camera = Camera.main;
    }

    void Update()
    {
        GetInput();
    }

    void GetInput()
    {

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        _moveInput = new Vector2(moveX, moveY);
        _moveVelocity = _moveInput.normalized * _moveSpeed;

        transform.Translate(_moveVelocity * Time.deltaTime);

        if(Input.GetKey(KeyCode.Space)){
            if(_camera.orthographicSize >= 1f){
                _camera.orthographicSize -= _zoomSpeed * Time.deltaTime;
            }
        }
        if(Input.GetKey(KeyCode.LeftShift)){
            _camera.orthographicSize += _zoomSpeed * Time.deltaTime;
        }
    }
}

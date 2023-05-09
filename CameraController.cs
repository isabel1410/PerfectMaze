using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    [Tooltip("Speed the camera will move")] [SerializeField] private float _cameraSpeed = 50f;
    [Tooltip("The speed the camera will zoom")] [SerializeField] private float _cameraZoomSpeed = 5f;
    [Tooltip("The position of the camera before recognizing the input")] private Vector2 _cameraPosition;
    [Tooltip("The camera to move")] [SerializeField] private Camera _camera;

    [Header("Input")]
    [Tooltip("Input from user used to move camera")] [SerializeField] private Input _input;

    void Update()
    {
        // Move camera with mouse input
        _cameraPosition = new Vector2(transform.position.x, transform.position.y);
        Vector2 cameraMovement = new Vector2(_input.MoveInput.x, _input.MoveInput.y).normalized * _cameraSpeed * Time.deltaTime;
        Vector2 newCameraPosition = _cameraPosition + cameraMovement;

        // Zoom camera with mouse scroll wheel
        float zoomInput = _input.ScrollInput;
        float cameraZoom = _camera.orthographicSize + zoomInput * _cameraZoomSpeed * Time.deltaTime;
        _camera.orthographicSize = cameraZoom;

        // Sets camera to new position
        transform.position = new Vector3(newCameraPosition.x, newCameraPosition.y, _camera.orthographicSize);
    }
}

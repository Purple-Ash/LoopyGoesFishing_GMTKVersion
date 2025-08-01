using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private Camera _mainCamera;
    [SerializeField] private float _positionZ = -10f;
    [SerializeField] private float _lerpStrength;
    [SerializeField] private float _minZoom = 6;
    [SerializeField] public float _maxZoom = 12;
    [SerializeField] private float _zoomSpeed = 0.5f;
    [SerializeField] private float _zoomIncrement = 1;
    private float _targetZoom;
    private GameObject _boat;

    void Start()
    {
        _mainCamera = GetComponent<Camera>();
        _targetZoom = _mainCamera.orthographicSize;
        _boat = GameObject.FindGameObjectWithTag("Boat");
        if(_boat == null)
        {
            Debug.LogError("Zabij sie");
        }
    }

    private void updateCameraPosition()
    {
        float lerpModifier = _mainCamera.orthographicSize / Mathf.Pow(_maxZoom,1.4f);
        transform.position = new Vector3(
            Mathf.Lerp(
                _boat.transform.position.x,
                transform.position.x,
                Mathf.Pow(_lerpStrength* lerpModifier, Time.fixedDeltaTime)),
            Mathf.Lerp(
                _boat.transform.position.y,
                transform.position.y,
                Mathf.Pow(_lerpStrength* lerpModifier, Time.fixedDeltaTime)),
            _positionZ
            );
    }

    private void updateCameraZoom()
    {
        float currentZoom = _mainCamera.orthographicSize;
        float newZoom = Mathf.Lerp(
            currentZoom,
            _targetZoom,
            _zoomSpeed * Time.fixedDeltaTime);
        _mainCamera.orthographicSize = newZoom;
    }

    private void FixedUpdate()
    {
        updateCameraPosition();
        updateCameraZoom();
    }

    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") < 0f) // zoom in
        {
            _targetZoom += _zoomIncrement;
            if(_targetZoom > _maxZoom)
                _targetZoom = _maxZoom;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0f) // zoom out
        {
            _targetZoom -= _zoomIncrement;
            if(_targetZoom < _minZoom)
                _targetZoom = _minZoom;
        }
    }
}

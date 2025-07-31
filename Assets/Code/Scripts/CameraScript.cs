using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] private float _positionZ = -10f;
    [SerializeField] private float _lerpStrength;
    private GameObject _boat;

    void Start()
    {
        _boat = GameObject.FindGameObjectWithTag("Boat");
        if(_boat == null)
        {
            Debug.LogError("Zabij sie");
        }
    }

    private void updateCameraPosition()
    {
        transform.position = new Vector3(
            Mathf.Lerp(
                _boat.transform.position.x,
                transform.position.x,
                Mathf.Pow(_lerpStrength, Time.fixedDeltaTime)),
            Mathf.Lerp(
                _boat.transform.position.y,
                transform.position.y,
                Mathf.Pow(_lerpStrength, Time.fixedDeltaTime)),
            _positionZ
            );
    }

    private void FixedUpdate()
    {
        updateCameraPosition();
    }

    void Update()
    {

    }
}

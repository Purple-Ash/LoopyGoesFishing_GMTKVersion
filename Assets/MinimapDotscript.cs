using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapDotscript : MonoBehaviour
{
    private GameObject _boat;
    private Vector2 _position;
    [SerializeField] private Vector2 _scale;
    [SerializeField] private Vector2 _offset;
    // Start is called before the first frame update
    void Start()
    {
        _boat = GameObject.FindGameObjectWithTag("Boat");

    }

    // Update is called once per frame
    void Update()
    {
        _position = (Vector2)_boat.transform.position * _scale + _offset;
        GetComponent<RectTransform>().localPosition = _position; 
        if (transform.localPosition.x > 160)
        {
            transform.localPosition = new Vector3(160, transform.localPosition.y, transform.localPosition.z);
        }
        if (transform.localPosition.x < -160)
        {
            transform.localPosition = new Vector3(-160, transform.localPosition.y, transform.localPosition.z);
        }
        if (transform.localPosition.y > 160)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, 160, transform.localPosition.z);
        }
        if (transform.localPosition.y < -160)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, -160, transform.localPosition.z);
        }
    }
}

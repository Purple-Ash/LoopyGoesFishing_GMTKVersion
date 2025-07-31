using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetExtension : MonoBehaviour
{
    [SerializeField] public GameObject followedPoint;
    [SerializeField] public float length = 2.0f; // Length of the net extension
    protected LineRenderer lineRenderer; // LineRenderer component for drawing the net
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, followedPoint.transform.position);

        Vector3 direction = (followedPoint.transform.position - transform.position).normalized;
        //transform.rotation = Quaternion.LookRotation(direction);
        float distance = Vector3.Distance(transform.position, followedPoint.transform.position);
        if (distance > length)
        {
            transform.position = followedPoint.transform.position - direction * length;
        }
    }
}

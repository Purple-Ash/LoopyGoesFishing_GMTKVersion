using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    public Vector3 targetPosition;

    private void Update()
    {
        Vector2 direction = targetPosition - transform.parent.position;
        float angle = Mathf.Atan2(-direction.x, direction.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle); 
    }
}

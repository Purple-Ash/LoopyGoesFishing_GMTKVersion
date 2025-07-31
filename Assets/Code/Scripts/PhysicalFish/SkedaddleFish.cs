using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : FishScript
{
    protected override void MoveTowards(Vector2 direction)
    {
        Vector2 normalised = direction.normalized;
        //transform.root.LookAt(normalised);
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(_velocity.y, _velocity.x) * 180f / Mathf.PI);
        _velocity += normalised * _maxAcceleration * Time.fixedDeltaTime;
        float speedLimit = _velocity.magnitude / _maxAcceleration;
        if (speedLimit > 1.0f)
        {
            _velocity /= speedLimit;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SharkScript : FishScript
{
    protected override void OnCollisionStay2D(Collision2D collision)
    {
    }

    protected override void MoveTowards(Vector2 direction, float speedMultiplier)
    {
        Vector2 normalised;
        Vector2 acceleration;
        Vector2 boatTransformPosition = new Vector2(
                boat.transform.position.x,
                boat.transform.position.y
            );
        Vector2 thisPosition = new Vector2(
                transform.position.x,
                transform.position.y
            );
        float actualMaxVelocity = 1;

        if ((boatTransformPosition - thisPosition).magnitude < _skedaddleRange)
        {
            Vector2 boatDirection = thisPosition - boatTransformPosition;
            normalised = boatDirection.normalized;
            acceleration = normalised * _skedaddleAcceleration * Time.fixedDeltaTime * 1 / boatDirection.magnitude * _skedaddleRange;
            actualMaxVelocity = _skedaddleVelocity;
        }
        else
        {
            normalised = direction.normalized;
            acceleration = normalised * _maxAcceleration * Time.fixedDeltaTime;
            //transform.root.LookAt(normalised);
            actualMaxVelocity = _maxVelocity;
        }
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(_velocity.y, _velocity.x) * 180f / Mathf.PI);
        _velocity += acceleration * speedMultiplier;
        float speedLimit = _velocity.magnitude / actualMaxVelocity;
        if (speedLimit > 1.0f)
        {
            _velocity /= speedLimit;
        }
    }
}

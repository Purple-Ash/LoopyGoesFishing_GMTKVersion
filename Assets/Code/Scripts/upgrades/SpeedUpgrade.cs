using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpeedUpgrade", menuName = "Upgrades/SpeedUpgrade")]
public class SpeedUpgrade : UpgradeScript
{
    [SerializeField] private float newForwardMaxSpeed = 1.0f;
    [SerializeField] private float newBackwardsMaxSpeed = 1.0f;
    [SerializeField] private float newAcceleration = 1.0f;
    [SerializeField] private float newDeceleration = 1.0f;

    public override void ApplyUpgrade()
    {
        BoatMovement boatMovement = FindObjectOfType<BoatMovement>();
        boatMovement.maxSpeedForward = newForwardMaxSpeed;
        boatMovement.acceleration = newAcceleration;
        boatMovement.deceleration = newDeceleration;
        ShowNextUpgrade();
    }
}

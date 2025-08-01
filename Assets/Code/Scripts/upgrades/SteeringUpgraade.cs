using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SteeringUpgrade", menuName = "Upgrades/SteeringUpgrade")]
public class SteeringUpgraade : UpgradeScript
{
    [SerializeField] private float newSteering = 140f;

    public override void ApplyUpgrade()
    {
        FindObjectOfType<BoatMovement>().turnSpeed = newSteering;
        ShowNextUpgrade();
    }
}

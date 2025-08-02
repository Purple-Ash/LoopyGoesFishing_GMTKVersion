using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "SpeedUpgrade", menuName = "Upgrades/SpeedUpgrade")]
public class SpeedUpgrade : UpgradeScript
{
    [SerializeField] private float newForwardMaxSpeed = 1.0f;
    [SerializeField] private float newBackwardsMaxSpeed = 1.0f;
    [SerializeField] private float newAcceleration = 1.0f;
    [SerializeField] private float newDeceleration = 1.0f;
    [SerializeField] private Sprite speedometerBottom;
    [SerializeField] private Sprite speedometerMid;
    [SerializeField] private Sprite speedometerTop;

    public override void ApplyUpgrade()
    {
        BoatMovement boatMovement = FindObjectOfType<BoatMovement>();
        boatMovement.maxSpeedForward = newForwardMaxSpeed;
        boatMovement.acceleration = newAcceleration;
        boatMovement.deceleration = newDeceleration;
        ShowNextUpgrade();
        Transform t = GameObject.FindGameObjectWithTag("Speedometer").transform;
        t.GetChild(0).GetComponent<Image>().sprite = speedometerBottom;
        t.GetChild(1).GetComponent<Image>().sprite = speedometerMid;
        t.GetChild(2).GetComponent<Image>().sprite = speedometerTop;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ZoomUpgrade", menuName = "Upgrades/ZoomUpgrade")]
public class ZoomUpgrade : UpgradeScript
{
    [SerializeField] private float maxZoom = 20f;

    public override void ApplyUpgrade()
    {
        FindObjectOfType<CameraScript>()._maxZoom = maxZoom;
    }
}

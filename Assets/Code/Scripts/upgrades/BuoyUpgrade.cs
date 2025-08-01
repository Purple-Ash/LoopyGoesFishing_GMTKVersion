using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuoyUpgrade", menuName = "Upgrades/Buoy Upgrade")]
public class BuoyUpgrade : UpgradeScript
{
    [SerializeField] private int numberOfBuoys = 25; // Number of buoys to spawn
    [SerializeField] private Color color = Color.white; // Color of the buoys

    public override void ApplyUpgrade()
    {
        BuoySpawner buoySpawner = FindObjectOfType<BuoySpawner>();
        if (buoySpawner != null)
        {
            buoySpawner.addBuoys(numberOfBuoys, color);
        }
        else
        {
            Debug.LogWarning("BuoySpawner not found in the scene.");
        }
    }
}

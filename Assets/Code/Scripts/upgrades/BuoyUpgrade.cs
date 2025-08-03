using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuoyUpgrade", menuName = "Upgrades/Buoy Upgrade")]
public class BuoyUpgrade : UpgradeScript
{
    [SerializeField] private int numberOfBuoys = 25; // Number of buoys to spawn
    [SerializeField] private Color color = Color.white; // Color of the buoys
    [SerializeField] private bool islandCatcher = false; // Flag to indicate if the buoys are for an island catcher

    public override void ApplyUpgrade()
    {
        BuoySpawner buoySpawner = FindObjectOfType<BuoySpawner>();
        if (buoySpawner != null)
        {
            buoySpawner.addBuoys(numberOfBuoys, color);
            buoySpawner.isIslandCatcher = islandCatcher; // Set the island catcher flag
        }
        else
        {
            Debug.LogWarning("BuoySpawner not found in the scene.");
        }
    }
}

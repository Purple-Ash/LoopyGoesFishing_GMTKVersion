using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RarityUpgrade", menuName = "Upgrades/RarityUpgrade")]
public class RarityUpgrade : UpgradeScript
{
    [SerializeField] public float[] rarities;

    public override void ApplyUpgrade()
    {
        for (int i = 0; i < rarities.Length; i++)
        {
            rarities[i] = rarities[i] / 100f; // Convert percentages to probabilities
        }
        GameObject.FindGameObjectWithTag("EquipementManager").GetComponent<EquipementScript>().probabilities = rarities;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RarityUpgrade", menuName = "Upgrades/RarityUpgrade")]
public class RarityUpgrade : UpgradeScript
{
    [SerializeField] public float[] rarities;

    public override void ApplyUpgrade()
    {
        GameObject.FindGameObjectWithTag("EquipementManager").GetComponent<EquipementScript>().probabilities = rarities;
    }
}

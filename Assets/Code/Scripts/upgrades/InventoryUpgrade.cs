using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryUpgrade", menuName = "Upgrades/InventoryUpgrade")]
public class InventoryUpgrade : UpgradeScript
{
    [SerializeField] private float inventoryIncrease = 10;

    public override void ApplyUpgrade()
    {
        GameObject.FindGameObjectWithTag("EquipementManager").GetComponent<EquipementScript>().capacity += inventoryIncrease;
        GameObject.FindGameObjectWithTag("EquipementManager").GetComponent<EquipementScript>().UpdateMoneyAndWeight();
        ShowNextUpgrade();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MoneyUpgrade", menuName = "Upgrades/MoneyUpgrade")]
public class MoneyUpgrade : UpgradeScript
{
    [SerializeField] private float moneyMultiplier = 1.5f;
    [SerializeField] private float extraSpace = 5f;

    public override void ApplyUpgrade()
    {
        FindObjectOfType<EquipementScript>().moneyMult = moneyMultiplier;
        FindObjectOfType<EquipementScript>().capacity += extraSpace;
    }
}

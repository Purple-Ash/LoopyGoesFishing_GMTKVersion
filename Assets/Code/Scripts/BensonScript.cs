using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BensonScript : BaseNPCScript
{

    private List<UpgradeScript> upgrades = new List<UpgradeScript>();

    public GameObject entityToSpawn;

    public Transform upgradeUIList;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void addUpgradeItem(UpgradeScript upgradeToAdd)
    {
        this.upgrades.Add(upgradeToAdd);
    }

    public override void setShopUIActive()
    {

        foreach (UpgradeScript upgrade in this.upgrades)
        {
            Instantiate(entityToSpawn, upgradeUIList.transform);
        }

        base.setShopUIActive();


    }

}

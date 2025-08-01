using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BensonScript : BaseNPCScript
{

    public List<UpgradeScript> upgrades = new List<UpgradeScript>();

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

    public override void setShopUIActive()
    {
        createEntities();
        base.setShopUIActive();
    }

    void buyItem(string upgradeName)
    {
        Debug.Log("Buying item " +  upgradeName);
        UpgradeScript upgradeToBuy = upgrades.Find(x => x.upgradeName == upgradeName);

        if (upgradeToBuy == null)
        {
            Debug.Log("Upgrade withg name <" + upgradeName + "> not found.");
        }

        GameObject.FindGameObjectWithTag("EquipementManager").GetComponent<EquipementScript>().money -= upgradeToBuy.cost;

        if (upgradeToBuy != null && upgradeToBuy.nextUpgrade != null)
        {
            upgrades.Add(upgradeToBuy.nextUpgrade);
            Debug.Log("Next Upgrade added to list");
        }

        GameObject.FindGameObjectWithTag("EquipementManager").GetComponent<EquipementScript>().UpdateMoneyAndWeight();
        upgradeToBuy.ApplyUpgrade();
        upgrades.Remove(upgradeToBuy);
        refreshUpgradeItems();
    }

    public override void setShopUIInactive()
    {
        destroyEntities();
        base.setShopUIInactive();
    }

    private void createEntities()
    {
        foreach (UpgradeScript upgrade in this.upgrades)
        {
            GameObject inst = Instantiate(entityToSpawn, upgradeUIList.transform);

            Image[] fishImages = inst.GetComponentsInChildren<Image>();

            fishImages[1].sprite = upgrade.icon;

            TMP_Text[] textFields = inst.GetComponentsInChildren<TMP_Text>();

            textFields[0].text = upgrade.upgradeName;
            textFields[1].text = upgrade.description;
            textFields[2].text = upgrade.cost.ToString() + "$";

            Button buyButton = inst.GetComponentInChildren<Button>();
            float money = GameObject.FindGameObjectWithTag("EquipementManager").GetComponent<EquipementScript>().money;

            buyButton.onClick.AddListener(delegate () { buyItem(upgrade.upgradeName); });
            if (money < upgrade.cost)
            {
                buyButton.interactable = false;
                buyButton.GetComponent<Image>().color = new Color(0.8f, 0.2f, 0.2f);
            }

        }
    }

    private void destroyEntities()
    {
        Debug.Log("Destroying Upgrade entities");
        for (int i = 0; i < upgradeUIList.childCount; i++)
        {
            Destroy(upgradeUIList.GetChild(i).gameObject);
        }
    }

    private void refreshUpgradeItems()
    {
        destroyEntities();
        createEntities();
    }

}

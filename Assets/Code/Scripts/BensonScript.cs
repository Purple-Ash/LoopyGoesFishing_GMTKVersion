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

    [Header("Sounds")]
    [SerializeField] protected AudioClip boughtSound;
    [SerializeField] float boughtMultiplier = 0.5f;
    [SerializeField] protected AudioClip hammerSound;
    [SerializeField] float hammerMultiplier = 0.8f;

    private AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        this.upgrades.Sort((x, y) => x.name.CompareTo(y.name)); // Sort upgrades by cost
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void setShopUIActive()
    {
        createEntities();
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraScript>().blockZoom();
        base.setShopUIActive();
        transform.GetChild(0).GetChild(3).GetChild(0).GetChild(0).GetChild(1).GetChild(0).position = new Vector3(transform.GetChild(0).GetChild(3).GetChild(0).GetChild(0).GetChild(1).GetChild(0).position.x, -2000, transform.GetChild(0).GetChild(3).GetChild(0).GetChild(0).GetChild(1).GetChild(0).position.z);
        TutorialScript tutorialScript = FindObjectOfType<TutorialScript>();
        if (tutorialScript != null)
        {
            tutorialScript.enterredBensonShop = true; // Set the flag to true when entering the shop
        }
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

        audioManager.PlayCenter(boughtSound, boughtMultiplier);
        audioManager.PlayCenter(hammerSound, hammerMultiplier);
    }

    public override void setShopUIInactive()
    {
        destroyEntities();
        base.setShopUIInactive();
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraScript>().unlockZoom();
        TutorialScript script = FindAnyObjectByType<TutorialScript>(); // Reset the flag when entering the shop
        if (script != null)
        {
            script.exitedShop = true; // Reset the flag when exiting the shop
        }
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
        this.upgrades.Sort((x, y) => x.name.CompareTo(y.name)); // Sort upgrades by cost

        destroyEntities();
        createEntities();
    }

}

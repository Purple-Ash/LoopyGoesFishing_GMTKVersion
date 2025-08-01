using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BarryScript : BaseNPCScript
{
    // The GameObject to instantiate.
    public GameObject entityToSpawn;

    // Content container in which GameObjects are spawned
    public Transform contentTransform;

    // The Equipment View UI object
    public GameObject imageView;
    public GameObject equipmentView;

    // A Map to store the current equipment data
    private Dictionary<NewFishData, int[]> fishDataDictionary;

    public TMPro.TMP_Text TotalPrice;
    public TMPro.TMP_Text TotalPriceSmall;
    public Button select;

    void Start()
    {
        //SpawnEntities();
        fishDataDictionary = new Dictionary<NewFishData, int[]>();
    }

    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Tab))
    //    {
    //        Debug.Log("Equipment button activated");
    //        if (imageView != null)
    //        {
    //            Debug.Log("Equipment object found");
    //            if (imageView.activeSelf)
    //            {
    //                DestroyEntities();
    //                imageView.SetActive(false);
    //                equipmentView.SetActive(false); // Hide the equipment view when the image view is closed
    //            }
    //            else
    //            {
    //                imageView.SetActive(true);
    //                equipmentView.SetActive(true); // Show the equipment view when the image view is opened
    //                SpawnEntities();
    //                UpdatePrice();
    //            }
    //        }
    //        else
    //        {
    //            Debug.Log("Equipment object not found");
    //        }
    //    }
    //}

    //public void addFishEntity(Dictionary<NewFishData, int[]> caughtFishes)
    //{
    //    //Debug.Log("Adding caught fish data");
    //    foreach (NewFishData newFishData in caughtFishes.Keys)
    //    {
    //        if (fishDataDictionary.ContainsKey(newFishData))
    //        {
    //            Debug.Log("Adding numbers...." + "\n" + fishDataDictionary[newFishData] + "\n" + caughtFishes[newFishData]);
    //            fishDataDictionary[newFishData][0] += caughtFishes[newFishData][0];
    //            fishDataDictionary[newFishData][1] += caughtFishes[newFishData][1];
    //            fishDataDictionary[newFishData][2] += caughtFishes[newFishData][2];
    //        } else
    //        {
    //            fishDataDictionary.Add(newFishData, caughtFishes[newFishData]);
    //        }
    //    }
    //}

    public override void setShopUIActive()
    {
        imageView.SetActive(true);
        equipmentView.SetActive(true); // Show the equipment view when the image view is opened
        SpawnEntities();
        UpdatePrice();
    }

    public override void setShopUIInactive()
    {
        select.GetComponent<CheckScript>().isChecked = false;
        select.GetComponent<UnityEngine.UI.Image>().color = new Color(0.8f, 0.2f, 0.2f, 1f); // Reset the color to unchecked
        select.transform.GetChild(0).GetComponent<TMP_Text>().SetText("Select All"); // Reset the text to "Select All"

        imageView.SetActive(false);
        equipmentView.SetActive(false); // Hide the equipment view when the image view is closed

        DestroyEntities();
        
        Time.timeScale = 1.0f; // Resume the game time

        GameObject.FindGameObjectWithTag("EquipementManager").GetComponent<EquipementScript>().CheckAllFish(false);
    }

    void DestroyEntities()
    {
        Debug.Log("Destroying Equipment entities");
        for (int i = 0; i < contentTransform.childCount; i++)
        {
            Debug.Log("Destroying Equipment entities");
            Destroy(contentTransform.GetChild(i).gameObject);
        }
    }

    void SpawnEntities()
    {
        float mass = 0f;
        foreach (var f in GameObject.FindGameObjectWithTag("EquipementManager").GetComponent<EquipementScript>().fishDataDictionary)
        {
            if (f.Value[0] == 0 && f.Value[1] == 0 && f.Value[2] == 0)
            {
                continue; // Skip if all counts are zero
            }
            Debug.Log("Instantiating equipment entities");
            GameObject inst = Instantiate(entityToSpawn, contentTransform.transform);


            Image[] fishImages = inst.GetComponentsInChildren<Image>();

            fishImages[1].sprite = f.Key.image;

            TMP_Text[] textFields = inst.GetComponentsInChildren<TMP_Text>();

            textFields[0].SetText(f.Key.name);
            textFields[1].SetText(f.Key.description);
            textFields[7].SetText(f.Value[2].ToString()); //XL Number
            textFields[8].SetText(f.Value[1].ToString()); //L Number
            textFields[9].SetText(f.Value[0].ToString()); //M Number
            textFields[11].SetText(f.Value[2] * f.Key.weight * 2 + " kg"); // Total weight for XL
            textFields[12].SetText(f.Value[1] * f.Key.weight * 1.5f + "kg"); // Total weight for L
            textFields[13].SetText(f.Value[0] * f.Key.weight + "kg"); // Total weight for M
            textFields[15].SetText(f.Value[2] * f.Key.price * FindObjectOfType<EquipementScript>().moneyMult * 4 + "$"); // Total price for XL
            textFields[16].SetText(f.Value[1] * f.Key.price * FindObjectOfType<EquipementScript>().moneyMult * 2 + "$"); // Total price for L
            textFields[17].SetText(f.Value[0] * f.Key.price * FindObjectOfType<EquipementScript>().moneyMult + "$"); // Total price for M
            textFields[19].SetText(((f.Key.price * f.Value[0] + f.Value[1] * f.Key.price * 2 + f.Value[2] * f.Key.price * 4) * FindObjectOfType<EquipementScript>().moneyMult).ToString() + "$");

            mass += f.Value[0] * f.Key.weight + f.Value[1] * f.Key.weight * 1.5f + f.Value[2] * f.Key.weight * 2;
        }
        
    }

    public void UpdatePrice()
    {
        float totalPrice = 0f;
        var skibidi = GameObject.FindGameObjectWithTag("EquipementManager").GetComponent<EquipementScript>().fishDataDictionary;
        foreach (var f in GameObject.FindGameObjectWithTag("EquipementManager").GetComponent<EquipementScript>().fishCheckDictionary)
        {
            for (int i = 0; i < f.Value.Length; i++)
            {
                if (f.Value[i])
                {
                    if (i == 0) // M
                    {
                        totalPrice += f.Key.price * FindObjectOfType<EquipementScript>().moneyMult * skibidi[f.Key][0];
                    }
                    else if (i == 1) // L
                    {
                        totalPrice += f.Key.price * FindObjectOfType<EquipementScript>().moneyMult * skibidi[f.Key][1] * 2;
                    }
                    else if (i == 2) // XL
                    {
                        totalPrice += f.Key.price * FindObjectOfType<EquipementScript>().moneyMult * skibidi[f.Key][2] * 4;
                    }
                }
            }
        }
        TotalPrice.SetText(totalPrice.ToString() + "$");
        TotalPriceSmall.SetText(totalPrice.ToString());
    }

    public void Sell()
    {
        GameObject.FindGameObjectWithTag("EquipementManager").GetComponent<EquipementScript>().SellFish();
        UpdateValues();
        UpdatePrice();
        select.GetComponent<CheckScript>().isChecked = false;
        select.GetComponent<UnityEngine.UI.Image>().color = new Color(0.8f, 0.2f, 0.2f, 1f); // Reset the color to unchecked
        select.transform.GetChild(0).GetComponent<TMP_Text>().SetText("Select All"); // Reset the text to "Select All"
    }

    public void UpdateValues()
    {
        DestroyEntities();
        SpawnEntities();
    }
}


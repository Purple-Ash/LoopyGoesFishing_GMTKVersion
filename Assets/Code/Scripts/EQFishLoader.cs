using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EQFishLoader : MonoBehaviour
{
    // The GameObject to instantiate.
    public GameObject entityToSpawn;

    // Content container in which GameObjects are spawned
    public Transform contentTransform;

    // An instance of the ScriptableObject defined above.
    public List<FishData> fishDataValues;

    // The Equipment View UI object
    public GameObject equipmentView;

    // A Map to store the current equipment data
    private Dictionary<NewFishData, int[]> fishDataDictionary;

    public TMPro.TMP_Text fishWeight;

    void Start()
    {
        //SpawnEntities();
        fishDataDictionary = new Dictionary<NewFishData, int[]>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (equipmentView != null)
            {
                if(equipmentView.activeSelf)
                {
                    Close();
                    GameObject.FindGameObjectWithTag("OptionsManager").GetComponent<OptionsManager>().OtherMenuClose();
                }
                else
                {
                    GameObject.FindGameObjectWithTag("OptionsManager").GetComponent<OptionsManager>().OtherMenuOpen();
                    Open();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (equipmentView.activeSelf)
            {
                Close();
                GameObject.FindGameObjectWithTag("OptionsManager").GetComponent<OptionsManager>().OtherMenuClose();
            }
        }
    }

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

    void Open()
    {
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraScript>().blockZoom();
        equipmentView.SetActive(true);
        SpawnEntities();
        transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).position = new Vector3(transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).position.x, -2000, transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).position.z);
    }

    void Close()
    {
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraScript>().unlockZoom();
        DestroyEntities();
        equipmentView.SetActive(false);
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
        foreach (var f in GetComponent<EquipementScript>().fishDataDictionary)
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
        //Debug.Log(GameObject.FindGameObjectWithTag("FishWeight").name);
        //GameObject.FindGameObjectWithTag("FishWeight").GetComponent<TMP_Text>().SetText("Capacity: " + mass + "/" + GetComponent<EquipementScript>().capacity + "kg"); // Update total weight text
    }

    public void UpdateValues()
    {
        DestroyEntities();
        SpawnEntities();
    }
}


using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FishExpertScript : BaseNPCScript
{
    // The GameObject to instantiate.
    public GameObject entityToSpawn;

    // Content container in which GameObjects are spawned
    public Transform contentTransform;

    // The Equipment View UI object
    public GameObject imageView;
    public GameObject equipmentView;
    public GameObject backPanel;

    public List<NewFishData> fishData;
    private List<bool> discoveredFish;
    private List<bool> talkedFish;

    [Header("Sounds")]
    [SerializeField] protected AudioClip discoverSound;
    [SerializeField] float discoverMultiplier = 0.5f;

    private AudioManager audioManager;

    void Start()
    {
        //SpawnEntities();
        fishData.Sort((f1, f2) => {
            if (f1.price > f2.price) return 1;
            if (f1.price == f2.price) return 0;
            return -1;
        });
        foreach (NewFishData f in fishData)
        {
            discoveredFish.Add(false); // Initialize the discoveredFish list with false values
            talkedFish.Add(false); // Initialize the talkedFish list with false values
        }
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    public override void setShopUIActive()
    {
        imageView.SetActive(true);
        equipmentView.SetActive(true); // Show the equipment view when the image view is opened
        backPanel.SetActive(true);
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraScript>().blockZoom();
        SpawnEntities();
        equipmentView.transform.GetChild(0).GetChild(0).position = new Vector3(equipmentView.transform.GetChild(0).GetChild(0).position.x, -2000, equipmentView.transform.GetChild(0).GetChild(0).position.z);
    }

    public override void setShopUIInactive()
    {
        imageView.SetActive(false);
        equipmentView.SetActive(false); // Hide the equipment view when the image view is closed
        backPanel.SetActive(false);
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraScript>().unlockZoom();

        DestroyEntities();

        Time.timeScale = 1.0f; // Resume the game time
    }

    void DestroyEntities()
    {
        for (int i = 0; i < contentTransform.childCount; i++)
        {
            Destroy(contentTransform.GetChild(i).gameObject);
        }
    }

    void SpawnEntities()
    {
        Dictionary<NewFishData, int[]> fishDataDictionary = GameObject.FindGameObjectWithTag("EquipementManager").GetComponent<EquipementScript>().fishDataDictionary;
        int caught = 0;
        foreach (NewFishData f in fishData)
        {

            GameObject inst = Instantiate(entityToSpawn, contentTransform.transform);

            Image[] fishImages = inst.GetComponentsInChildren<Image>();
            TMP_Text[] textFields = inst.GetComponentsInChildren<TMP_Text>();
            if (fishDataDictionary.ContainsKey(f))
            {
                fishImages[1].sprite = f.image;

                textFields[0].SetText(f.name);
                textFields[1].SetText(f.description);

                caught++;

                ColorUtility.TryParseHtmlString("#CFC2A1", out Color baseTextColor);

                textFields[0].color = baseTextColor;
                textFields[1].color = baseTextColor;

            }
            else
            {
                fishImages[1].sprite = f.image;
                fishImages[1].color = Color.black;

                textFields[0].SetText("???");
                textFields[1].SetText("???");

                textFields[0].color = Color.gray * 0.70f;
                textFields[1].color = Color.gray * 0.70f;

                fishImages[0].color = Color.gray;
            }
        }
    }

    public void UpdateValues()
    {
        DestroyEntities();
        SpawnEntities();
    }
}


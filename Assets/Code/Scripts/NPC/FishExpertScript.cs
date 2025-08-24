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

    public TMPro.TMP_Text text;
    public TMP_Text numText;

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
    private int studies = 0;

    void Start()
    {
        studies = 0;
        discoveredFish = new List<bool>();
        talkedFish = new List<bool>();
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
        text.SetText("Welcome back to my Lighthouse! Caught any new fish for us to study recently? Or maybe you want to pry me for some more information? I might just throw you a *red Herring*, heh heh.");
        numText.SetText("Studied: " + studies + "/" + fishData.Count);
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
        foreach (NewFishData f in fishData)
        {
            GameObject inst = Instantiate(entityToSpawn, contentTransform.transform);

            inst.GetComponent<LighthouseEntryManager>().newFishData = f;

            Image[] fishImages = inst.GetComponentsInChildren<Image>();
            TMP_Text[] textFields = inst.GetComponentsInChildren<TMP_Text>();
            if (fishDataDictionary.ContainsKey(f))
            {
                fishImages[1].sprite = f.image;

                textFields[0].SetText(f.name);
                textFields[1].SetText("???");

                ColorUtility.TryParseHtmlString("#CFC2A1", out Color baseTextColor);

                textFields[0].color = baseTextColor;
                textFields[1].color = Color.gray * 0.70f;
                inst.transform.GetChild(4).gameObject.SetActive(false);

                if (!discoveredFish[fishData.IndexOf(f)] && (fishDataDictionary[f][0] != 0 || fishDataDictionary[f][1] != 0 || fishDataDictionary[f][2] != 0))
                {
                    inst.transform.GetChild(4).gameObject.SetActive(true);
                }
                else if (discoveredFish[fishData.IndexOf(f)])
                {
                    textFields[1].SetText(f.infoDetails);
                    textFields[1].color = baseTextColor;
                }
            }
            else
            {
                fishImages[1].sprite = f.image;
                fishImages[1].color = Color.black;

                textFields[1].SetText("???");
                
                textFields[1].color = Color.gray * 0.70f;

                if (talkedFish[fishData.IndexOf(f)])
                {
                    textFields[0].SetText(f.name);
                    ColorUtility.TryParseHtmlString("#CFC2A1", out Color baseTextColor);
                    textFields[0].color = baseTextColor;
                }
                else
                {
                    textFields[0].SetText("???");
                    textFields[0].color = Color.gray * 0.70f;
                }
            }


            if (!talkedFish[fishData.IndexOf(f)])
            {
                inst.transform.GetChild(3).GetChild(0).GetComponent<TMP_Text>().SetText("Here's " + f.infoPrice + "$, Where do I find it?");
            }
            else
            {
                inst.transform.GetChild(3).GetChild(0).GetComponent<TMP_Text>().SetText("Remind me where to find them again?");
            }
        }
    }

    public bool onInfo(NewFishData fish)
    {
        int index = fishData.IndexOf(fish);
        if (!talkedFish[index])
        {
            if (FindObjectOfType<EquipementScript>().money >= fish.infoPrice)
            {
                FindObjectOfType<EquipementScript>().money -= fish.infoPrice;
                talkedFish[index] = true;
                text.SetText(fish.infoPlace);
                return true;
            }
            else
            {
                text.SetText("This seems a bit *fishy*, you don't have enough money to make that offer!");
                return false;
            }
        }
        else
        {
            text.SetText(fish.infoPlace);
            return true;
        }
    }

    public void onDiscover(NewFishData fish)
    {
        int index = fishData.IndexOf(fish);
        Dictionary<NewFishData, int[]> fishDataDictionary = GameObject.FindGameObjectWithTag("EquipementManager").GetComponent<EquipementScript>().fishDataDictionary;
        if (!discoveredFish[index])
        {
            if (fishDataDictionary[fish][2] != 0)
            {
                fishDataDictionary[fish][2] -= 1;
                discoveredFish[index] = true;
                text.SetText(fish.infoDetails);
            }
            else if (fishDataDictionary[fish][1] != 0)
            {
                fishDataDictionary[fish][1] -= 1;
                discoveredFish[index] = true;
            }
            else if (fishDataDictionary[fish][0] != 0)
            {
                fishDataDictionary[fish][0] -= 1;
                discoveredFish[index] = true;
            }
            else
            {
                Debug.LogError("Coœ siê potê¿nie zjeba³o, nie ma ¿adnych ryb w dictionary, a powinny byæ!");
            }

        }
    }

    public void UpdateValues()
    {
        DestroyEntities();
        SpawnEntities();
    }
}


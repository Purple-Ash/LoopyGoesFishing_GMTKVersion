using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GlossaryScript : MonoBehaviour
{

    [SerializeField] private GameObject entityToSpawn;

    [SerializeField] private List<NewFishData> fishData;

    [SerializeField] private Transform contentTransform;

    [SerializeField] private GameObject GlossaryPopup;
    
    [SerializeField] private TMP_Text totalText;

    private List<NewFishData> knownBeforeFish = new List<NewFishData>();

    private bool isOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        fishData.Sort((f1, f2) => {
            if (f1.price > f2.price) return 1;
            if (f1.price == f2.price) return 0;
            return -1;
        });
        contentTransform.transform.position = new Vector3(contentTransform.transform.position.x, contentTransform.transform.position.y - 2000, contentTransform.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log("Glossary view activated");
            if (isOpen)
            {
                Close();
                GameObject.FindGameObjectWithTag("OptionsManager").GetComponent<OptionsManager>().OtherMenuClose();
            }
            else
            {
                Open();
                GameObject.FindGameObjectWithTag("OptionsManager").GetComponent<OptionsManager>().OtherMenuOpen();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isOpen)
            {
                Close();
                GameObject.FindGameObjectWithTag("OptionsManager").GetComponent<OptionsManager>().OtherMenuClose();
            }
        }
    }

    public void Open()
    {
        Debug.Log("Opening Glossary");
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraScript>().blockZoom();
        SpawnEntities();
        contentTransform.transform.position = new Vector3(contentTransform.transform.position.x, contentTransform.transform.position.y - 2000, contentTransform.transform.position.z);
        GlossaryPopup.SetActive(true);
        isOpen = true;
    }

    public void Close()
    {
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraScript>().unlockZoom();
        DestroyEntities();
        GlossaryPopup.SetActive(false);
        isOpen = false;
    }

    void SpawnEntities()
    {
        Debug.Log("Instantiating glossary entities");

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

                textFields[3].SetText(f.price.ToString() + "$");
                textFields[5].SetText(f.weight.ToString() + "kg");
                caught++;

                ColorUtility.TryParseHtmlString("#CFC2A1", out Color baseTextColor);

                textFields[0].color = baseTextColor;
                textFields[1].color = baseTextColor;
                textFields[2].color = baseTextColor;
                textFields[3].color = baseTextColor;
                textFields[4].color = baseTextColor;
                textFields[5].color = baseTextColor;

                if (!knownBeforeFish.Contains(f))
                {
                    Debug.Log("New fish in glossary");
                    fishImages[0].color = Color.red + Color.white * 0.50f;
                    knownBeforeFish.Add(f);
                }

            }
            else
            {
                fishImages[1].sprite = f.image;
                fishImages[1].color = Color.black;

                textFields[0].SetText("???");
                textFields[1].SetText("???");

                textFields[3].SetText("???$");
                textFields[5].SetText("???kg");

                textFields[0].color = Color.gray * 0.70f;
                textFields[1].color = Color.gray * 0.70f;
                textFields[2].color = Color.gray * 0.70f;
                textFields[3].color = Color.gray * 0.70f;
                textFields[4].color = Color.gray * 0.70f;
                textFields[5].color = Color.gray * 0.70f;

                fishImages[0].color = Color.gray;
            }

            Debug.Log("Caught: " + caught + "/" + fishData.Count);

            totalText.SetText("Caught: " + caught + "/" + fishData.Count);

        }
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
}

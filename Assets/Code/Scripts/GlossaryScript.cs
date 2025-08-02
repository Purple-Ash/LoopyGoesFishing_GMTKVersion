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

    private bool isOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        fishData.Sort((f1, f2) => {
            if (f1.price > f2.price) return 1;
            if (f1.price == f2.price) return 0;
            return -1;
        });
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
            }
            else
            {
                Open();
            }
        }
    }

    void Open()
    {
        SpawnEntities();
        contentTransform.transform.position = new Vector3(contentTransform.transform.position.x, contentTransform.transform.position.y - 2000, contentTransform.transform.position.z);
        GlossaryPopup.SetActive(true);
        isOpen = true;
    }

    void Close()
    {
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
            }
            else
            {
                fishImages[1].sprite = f.image;
                fishImages[1].color = Color.black;

                textFields[0].SetText("???");
                textFields[1].SetText("???");

                textFields[3].SetText("???$");
                textFields[5].SetText("???kg");
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

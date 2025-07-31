using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Globalization;

public class FishInfoRetrieveScript : MonoBehaviour
{

    // The GameObject to instantiate.
    public GameObject entityToSpawn;

    public Transform scrollView;

    // An instance of the ScriptableObject defined above.
    public List<FishData> fishDataValues;

    // This will be appended to the name of the created entities and increment when each is created.
    int instanceNumber = 1;

    void Start()
    {
        SpawnEntities();
    }

    void SpawnEntities()
    {

        foreach (FishData f in fishDataValues)
        {
            Debug.Log("Instantiating");
            GameObject inst = Instantiate(entityToSpawn, scrollView.transform);
            

            Image[] fishImages = inst.GetComponentsInChildren<Image>();

            fishImages[1].sprite = f.image;

            TMP_Text[] textFields = inst.GetComponentsInChildren<TMP_Text>();

            Debug.Log("text fields retrieved: " + textFields.Length);
            textFields[0].SetText(f.name);
            textFields[1].SetText(f.description);
            textFields[5].SetText(f.numGood.ToString());
            textFields[6].SetText(f.numNormal.ToString());
            textFields[7].SetText(f.numBad.ToString());
            textFields[8].SetText(f.price.ToString("C", new CultureInfo("en-US")));

        }
    }
}

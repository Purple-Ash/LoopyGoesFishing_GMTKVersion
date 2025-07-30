using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
            GameObject inst = Instantiate(entityToSpawn);
            

            Image[] fishImage = inst.GetComponentsInChildren<Image>();

            Debug.Log("Image retrieval: " + fishImage != null);

            fishImage[2].sprite = f.image;

            TMP_Text[] textFields = inst.GetComponentsInChildren<TMP_Text>();

            Debug.Log("text fields retrieved: " + textFields.Length);
            textFields[0].SetText(f.name);
            textFields[1].SetText(f.description);
            textFields[6].SetText(f.numBad.ToString());
            textFields[7].SetText(f.numNormal.ToString());
            textFields[8].SetText(f.numGood.ToString());
            textFields[11].SetText(f.price.ToString());

            inst.transform.SetParent(scrollView);
            inst.transform.localScale = Vector2.one;

        }
    }
}

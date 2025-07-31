using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipementScript : MonoBehaviour
{
    internal Dictionary<NewFishData, int[]> fishDataDictionary = new Dictionary<NewFishData, int[]>();
    [SerializeField] internal float[] probabilities;

    internal void AddFishData(NewFishData fishName)
    {
        if (fishDataDictionary.ContainsKey(fishName))
        {
            Debug.LogWarning($"Fish data for {fishName} already exists. Overwriting.");
        }
        else
        {
            Debug.Log($"Adding fish data for {fishName}");
            fishDataDictionary.Add(fishName, new int[3] { 0, 0, 0 });
        }

        float random = Random.Range(0f, 1f);

        if (random < probabilities[0])
        {
            fishDataDictionary[fishName][0] += 1; // Bad
        }
        else if (random < probabilities[1] + probabilities[0])
        {
            fishDataDictionary[fishName][1] += 1; // Normal
        }
        else if (random < probabilities[2] + probabilities[1] + probabilities[0])
        {
            fishDataDictionary[fishName][2] += 1; // Good
        }
        else
        {
            Debug.LogWarning($"Random value {random} did not match any probability range.");
        }

        Debug.Log($"Fish data for {fishName} updated: Bad={fishDataDictionary[fishName][0]}, Normal={fishDataDictionary[fishName][1]}, Good={fishDataDictionary[fishName][2]}");

        GetComponent<EQFishLoader>().addFishEntity(fishDataDictionary);
    }
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < probabilities.Length; i++)
        {
            probabilities[i] = probabilities[i] / 100f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

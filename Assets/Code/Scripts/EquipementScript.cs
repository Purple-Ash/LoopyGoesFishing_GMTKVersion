using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipementScript : MonoBehaviour
{
    internal Dictionary<NewFishData, int[]> fishDataDictionary = new Dictionary<NewFishData, int[]>();
    [SerializeField] internal float[] probabilities;
    [SerializeField] internal float capacity;
    [SerializeField] internal float weight;
    [SerializeField] internal GameObject notification;

    internal bool AddFishData(NewFishData fishName)
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
            if (weight + fishName.weight > capacity)
            {
                Instantiate(notification, GameObject.FindGameObjectWithTag("Boat").transform.position, Quaternion.identity);
                return false;
            }
            fishDataDictionary[fishName][0] += 1; // M
            weight += fishName.weight; // Update the total weight
        }
        else if (random < probabilities[1] + probabilities[0])
        {
            if (weight + fishName.weight * 1.5f > capacity)
            {
                Instantiate(notification, GameObject.FindGameObjectWithTag("Boat").transform.position, Quaternion.identity);
                return false;
            }
            fishDataDictionary[fishName][1] += 1; // L
            weight += fishName.weight * 1.5f; // Update the total weight
        }
        else if (random < probabilities[2] + probabilities[1] + probabilities[0])
        {
            if (weight + fishName.weight * 2 > capacity)
            {
                Instantiate(notification, GameObject.FindGameObjectWithTag("Boat").transform.position, Quaternion.identity);
                return false;
            }
            fishDataDictionary[fishName][2] += 1; // XL
            weight += fishName.weight * 2; // Update the total weight
        }
        else
        {
            Debug.LogWarning($"Random value {random} did not match any probability range.");
        }

        Debug.Log($"Fish data for {fishName} updated: Bad={fishDataDictionary[fishName][0]}, Normal={fishDataDictionary[fishName][1]}, Good={fishDataDictionary[fishName][2]}");

        return true;
        //GetComponent<EQFishLoader>().addFishEntity(fishDataDictionary);
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

    public void ClearFishData(string fishName, int type)
    {
        foreach (var skibid in fishDataDictionary)
        {
            if (skibid.Key.name == fishName)
            {
                if (type == 0)
                {
                    weight -= skibid.Key.weight * skibid.Value[0];
                    skibid.Value[0] = 0;
                }
                else if (type == 1)
                {
                    weight -= skibid.Key.weight * 1.5f * skibid.Value[1];
                    skibid.Value[1] = 0;
                }
                else if (type == 2)
                {
                    weight -= skibid.Key.weight * 2 * skibid.Value[2];
                    skibid.Value[2] = 0;
                }
            }
        }
        GetComponent<EQFishLoader>().UpdateValues();
    }
}

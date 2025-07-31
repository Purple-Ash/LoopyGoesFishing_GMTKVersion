using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuoySpawner : MonoBehaviour
{

    [SerializeField] private GameObject buoyPrefab; // Prefab for the buoy
    [SerializeField] private int numberOfBuoys = 5; // Number of buoys to spawn
    [SerializeField] private float spacing = 2.0f; // Spacing between buoys
    [SerializeField] private float firstBuoyOffset = 1.0f; // Offset for the first buoy

    // Start is called before the first frame update
    void Start()
    {
        NetExtension lastBuoy = null;
        for (int i = 0; i < numberOfBuoys; i++)
        {
            // Calculate position for each buoy
            NetExtension ne; 
            if (i == 0)
            {
                ne = Instantiate(buoyPrefab, transform.position + Vector3.down * firstBuoyOffset, Quaternion.identity).GetComponent<NetExtension>();
                ne.followedPoint = this.gameObject; // Set the followed point to this spawner
                ne.length = firstBuoyOffset; // Set the length of the net extension
            }
            else
            {
                ne = Instantiate(buoyPrefab, transform.position + Vector3.down * (spacing * i + firstBuoyOffset), Quaternion.identity).GetComponent<NetExtension>();
                ne.followedPoint = lastBuoy.gameObject; // Set the followed point to the last buoy
                ne.length = spacing; // Set the length of the net extension
            }
            
            lastBuoy = ne; // Update the last buoy reference
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

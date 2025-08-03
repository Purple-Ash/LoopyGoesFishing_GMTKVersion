using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuoySpawner : MonoBehaviour
{

    [SerializeField] private GameObject buoyPrefab; // Prefab for the buoy
    [SerializeField] private int numberOfBuoys = 5; // Number of buoys to spawn
    [SerializeField] private float spacing = 2.0f; // Spacing between buoys
    [SerializeField] private float firstBuoyOffset = 1.0f; // Offset for the first buoy

    public bool isIslandCatcher = false;

    private NetExtension lastBuoy; // Reference to the last buoy spawned

    public NetExtension LastBuoy { get => lastBuoy; set => lastBuoy = value; }

    // Start is called before the first frame update
    void Start()
    {
        lastBuoy = null;
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

    public void addBuoys(int numberOfBuoys, Color color)
    {
        if (lastBuoy == null)
        {
            LastBuoy = Instantiate(buoyPrefab, transform.position + Vector3.down * firstBuoyOffset, Quaternion.identity).GetComponent<NetExtension>();
            LastBuoy.followedPoint = this.gameObject; // Set the followed point to this spawner
        }

        for (int i = 0; i < numberOfBuoys; i++)
        {
            // Calculate position for each buoy
            NetExtension ne = Instantiate(buoyPrefab, lastBuoy.transform.position, Quaternion.identity).GetComponent<NetExtension>();
            ne.followedPoint = lastBuoy.gameObject; // Set the followed point to the last buoy
            ne.length = spacing; // Set the length of the net extension
            lastBuoy = ne; // Update the last buoy reference
        }
        this.numberOfBuoys += numberOfBuoys; // Update the total number of buoys

        GameObject gameObject = lastBuoy.gameObject;
        while (gameObject.CompareTag("Buoy"))
        {
            gameObject.GetComponent<LineRenderer>().colorGradient = new Gradient
            {
                colorKeys = new GradientColorKey[] { new GradientColorKey(color, 0.0f) },
                alphaKeys = new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f) }
            };
            gameObject.GetComponent<SpriteRenderer>().color = color;
            gameObject = gameObject.GetComponent<NetExtension>().followedPoint;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

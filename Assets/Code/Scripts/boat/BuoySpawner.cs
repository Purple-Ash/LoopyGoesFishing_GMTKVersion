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

    private int totalBuoys = 0; // Total number of buoys spawned
    private int restoreNumberOfBuoys = 0; // Number of buoys to restore when the boat is restored
    private Color RestoreColor;

    // Start is called before the first frame update
    void Start()
    {
        RestoreColor = new Color(1, 1, 1, 1); // Default color for buoys if not set
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
        totalBuoys = numberOfBuoys; // Initialize total buoys spawned
    }

    public void addBuoys(int numberOfBuoys, Color color)
    {
        totalBuoys += numberOfBuoys; // Update the total number of buoys
        if (totalBuoys > 500)
        {
            if (restoreNumberOfBuoys <= 100)
                restoreNumberOfBuoys = totalBuoys - numberOfBuoys ; // Calculate how many buoys to restore
            if (RestoreColor.Equals(new Color(1, 1, 1, 1)))
            {
                Debug.LogWarning("RestoreColor is not set, using last buoy color instead.");
                RestoreColor = lastBuoy.gameObject.GetComponent<SpriteRenderer>().color; // Store the color of the last buoy
                Debug.Log("RestoreColor set to: " + RestoreColor);
            }
        }
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

    public void restoreBuoys()
    {
        totalBuoys = 0;
        while (lastBuoy != null)
        {
            GameObject toDestroy = lastBuoy.gameObject;
            lastBuoy = lastBuoy.followedPoint.GetComponent<NetExtension>();
            Destroy(toDestroy);
        }
        numberOfBuoys = restoreNumberOfBuoys; // Restore the number of buoys to the previous state
        Start(); // Reinitialize the buoys

        GameObject gameObject = lastBuoy.gameObject;
        while (gameObject.CompareTag("Buoy"))
        {
            gameObject.GetComponent<LineRenderer>().colorGradient = new Gradient
            {
                colorKeys = new GradientColorKey[] { new GradientColorKey(RestoreColor, 0.0f) },
                alphaKeys = new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f) }
            };
            gameObject.GetComponent<SpriteRenderer>().color = RestoreColor;
            gameObject = gameObject.GetComponent<NetExtension>().followedPoint;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

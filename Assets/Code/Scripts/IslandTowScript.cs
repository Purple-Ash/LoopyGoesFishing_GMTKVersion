using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandTowScript : MonoBehaviour
{
    bool isTowed = false; // Flag to check if the island is being towed
    public GameObject chrisActivator; // Reference to the Chris activator GameObject
    public int nodeNum = 100;
    public float fadeTime = 2f; // Time to fade out the island
    public float holdTime = 1f; // Time to hold the island before fading out
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<FishCatcher>() != null && !isTowed)
        {
            GetComponent<PolygonCollider2D>().enabled = false; // Disable the collider to prevent further collisions

            isTowed = true; // Set the flag to true to prevent multiple triggers

            Color color = collision.GetComponent<FishCatcher>().GetComponent<MeshRenderer>().material.color; // Get the color of the FishCatcher

            NetExtension stuff = FindObjectOfType<BuoySpawner>().LastBuoy;
            while (true)
            {
                NetExtension oldStuff = stuff;
                stuff = stuff.followedPoint.GetComponent<NetExtension>();
                Destroy(oldStuff.gameObject); // Destroy the buoy
                if (stuff == null || stuff.gameObject == null || stuff.gameObject.CompareTag("Boat"))
                {
                    break; // Exit the loop if we reach the last buoy
                }
            }

            FindAnyObjectByType<BuoySpawner>().addBuoys(nodeNum, color); // Add new buoys with the specified color
            NetExtension netExtension = FindAnyObjectByType<BuoySpawner>().LastBuoy; // Get the last buoy after adding new ones

            while (true)
            {
                netExtension.transform.position = transform.position; // Set the position of the buoy to the island's position
                netExtension = netExtension.followedPoint.GetComponent<NetExtension>(); // Traverse to the last buoy

                if (netExtension == null || netExtension.gameObject == null || netExtension.gameObject.CompareTag("Boat"))
                {
                    break; // Exit the loop if we reach the last buoy
                }
            }
            transform.SetParent(FindAnyObjectByType<BuoySpawner>().LastBuoy.gameObject.transform);
            chrisActivator.SetActive(false); // Activate the Chris activator GameObject
        }
    }

    private void Update()
    {
        if (isTowed && ((Vector2)GameObject.FindGameObjectWithTag("VillageCenter").transform.position - (Vector2)transform.position).magnitude < 40)
        {

        }
    }
}

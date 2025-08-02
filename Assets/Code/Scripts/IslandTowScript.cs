using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandTowScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<FishCatcher>() != null)
        {
            Debug.Log("Island collided with FishCatcher");
            FishCatcher fishCatcher = collision.GetComponent<FishCatcher>();
            transform.parent = fishCatcher.colliderByoy.transform; // Set the parent of the island to the last buoy

            GameObject buoy = fishCatcher.colliderByoy.GetComponent<NetExtension>().followedPoint;
            while (true)
            {
                buoy = buoy.GetComponent<NetExtension>().followedPoint; // Get the next buoy in the chain
                if (buoy.GetComponent<NetExtension>().followedPoint == null || fishCatcher.colliderByoy.GetComponent<NetExtension>().followedPoint.CompareTag("Boat"))
                {
                    break; // Exit the loop if there is no followed point
                }
            }
        }
    }
}

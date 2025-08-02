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
        }
    }
}

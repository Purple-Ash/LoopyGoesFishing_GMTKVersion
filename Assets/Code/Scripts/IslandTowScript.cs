using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IslandTowScript : MonoBehaviour
{
    bool isTowed = false; // Flag to check if the island is being towed
    public GameObject chrisActivator; // Reference to the Chris activator GameObject
    public int nodeNum = 100;
    public float fadeTime = 2f; // Time to fade out the island
    public float holdTime = 1f; // Time to hold the island before fading out
    public float fadeInTime = 2f;
    public float textFadeTime = 2f;
    bool triggerEnd = false; // Flag to check if the trigger has ended
    bool triggerHold = false; // Flag to check if the trigger is holding
    bool triggerIn = false; // Flag to check if the trigger is in
    bool triggerText = false; // Flag to check if the trigger text is activated
    bool triggered = false; // Flag to check if the trigger has been activated
    float timer = 0;
    public GameObject ending; // Reference to the ending GameObject
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
            if (!triggered)
            {
                triggered = true; // Set the triggered flag to true when the island is close to the village center
                triggerEnd = true; // Set the trigger end flag to true when close to the village center
            }
        }

        if (triggerEnd)
        {
            ending.SetActive(true); // Activate the ending GameObject
            timer += Time.deltaTime; // Increment the timer
            ending.transform.GetChild(1).GetComponent<Image>().color = new Color(1, 1, 1, timer / fadeTime); // Activate the ending GameObject
            if (timer >= fadeTime)
            {
                triggerHold = true; // Set the trigger hold flag to true after the fade time
                triggerEnd = false; // Reset the trigger end flag
                timer = 0; // Reset the timer
                ending.transform.GetChild(1).GetComponent<Image>().color = new Color(1, 1, 1, 1);
            }
        }
        if (triggerHold)
        {
            ending.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 1);
            timer += Time.deltaTime; // Increment the timer
            if (timer >= holdTime)
            {
                triggerIn = true; // Set the trigger in flag to true after the hold time
                triggerHold = false; // Reset the trigger hold flag
                timer = 0; // Reset the timer
            }
        }
        if (triggerIn)
        {
            timer += Time.deltaTime; // Increment the timer
            ending.transform.GetChild(1).GetComponent<Image>().color = new Color(1, 1, 1, 1 - (timer / fadeInTime)); // Fade out the ending GameObject
            if (timer >= fadeInTime)
            {
                triggerIn = false; // Reset the trigger in flag
                triggerText = true; // Set the trigger text flag to true after the fade in time
                timer = 0; // Reset the timer
                ending.transform.GetChild(1).GetComponent<Image>().color = new Color(1, 1, 1, 0);
            }
        }
        if (triggerText)
        {
            timer += Time.deltaTime; // Increment the timer
            ending.transform.GetChild(2).GetChild(0).GetComponent<TMP_Text>().color = ending.transform.GetChild(2).GetChild(0).GetComponent<TMP_Text>().color + new Color(0, 0, 0, timer / textFadeTime - ending.transform.GetChild(2).GetChild(0).GetComponent<TMP_Text>().color.a); // Fade in the text
            if (timer >= textFadeTime)
            {
                triggerText = false; // Reset the trigger text flag
                ending.transform.GetChild(2).GetChild(0).GetComponent<TMP_Text>().color = ending.transform.GetChild(2).GetChild(0).GetComponent<TMP_Text>().color + new Color(0, 0, 0, 1 - ending.transform.GetChild(2).GetChild(0).GetComponent<TMP_Text>().color.a); // Fade in the text
            }
        }
    }
}

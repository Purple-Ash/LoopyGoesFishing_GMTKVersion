using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBarActivator : MonoBehaviour
{

    public GameObject progressBarFiller;
    public GameObject progressBar;
    public GameObject calledEvent;

    private bool isBoatIn;
    private bool shopInVisit = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isBoatIn && progressBarFiller.transform.localScale.x <= 0.3)
        {
            Vector3 newScale = progressBarFiller.transform.localScale;
            newScale.x = newScale.x + Time.deltaTime * 0.1f;
            newScale.y = 0.05f;
            Vector3 offset = progressBarFiller.transform.localPosition;
            offset.x = offset.x + Time.deltaTime * 1.65f * 0.1f;
            offset.y = 0.634f;

            Debug.Log("Boat is in: scale modified: " + newScale);
            progressBarFiller.transform.localScale = newScale;
            progressBarFiller.transform.localPosition = offset;
        }
        else if (!isBoatIn)
        {
            progressBarFiller.transform.localScale = new Vector3(0.02f, 0.05f);
            progressBarFiller.transform.localPosition = new Vector3(-0.462f, 0.634f);
        }

        if (!shopInVisit && progressBarFiller != null && progressBarFiller.transform.localScale.x >= 0.3)
        {
            Debug.Log("Entering the shop...");
            Time.timeScale = 0;
            shopInVisit = true;
            // Some script part to Switch the shop UI on

            calledEvent.GetComponent<BaseNPCScript>().setShopUIActive();
            TutorialScript tutorialScript = FindObjectOfType<TutorialScript>();
            if (tutorialScript != null)
            {
                tutorialScript.enterredShop = true; // Set the flag to true when entering the shop
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Boat")
        {
            isBoatIn = true;
            progressBar.SetActive(true);
            progressBarFiller.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Boat")
        {
            isBoatIn = false;
            shopInVisit = false;
            progressBar.SetActive(false);
            progressBarFiller.SetActive(false);
        }
    }
}

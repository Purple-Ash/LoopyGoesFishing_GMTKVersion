using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialScript : MonoBehaviour
{
    public GameObject tutorialPanel; // Reference to the tutorial panel GameObject
    public GameObject arrow; // Reference to the button that opens the tutorial

    public List<Sprite> expressions; // List of tutorial step GameObjects
    public List<string> texts; // List of tutorial step texts

    public GameObject image; // Reference to the Image component in the tutorial panel
    public TMPro.TMP_Text text; // Reference to the Text component in the tutorial panel

    public GameObject gooberFishes;
    public GameObject barryShop;
    public GameObject BensonShop;

    public bool enterredShop = false; // Flag to check if the player has entered Barry's shop
    public bool exitedShop = false; // Flag to check if the player has exited Barry's shop

    public int currentStep = 0; // Current step in the tutorial
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ShowPanel();
        if (Input.GetMouseButtonDown(0) && expressions[currentStep] != null)
        {
            // Move to the next step when the mouse is clicked
            currentStep++;
        }

        if (currentStep == 4 && ((Vector2)FindAnyObjectByType<BoatMovement>().gameObject.transform.position - (Vector2)gooberFishes.transform.position).magnitude < 5f)
        {
            // If the player reaches step 4 and is close to the GooberFishes, skip the tutorial
            currentStep++;
        }

        if (currentStep == 7 && FindAnyObjectByType<EquipementScript>().weight > 0)
        {
            // If the player reaches step 7 and has weight, skip the tutorial
            currentStep++;
        }

        if (currentStep == 10 && enterredShop)
        {
            // If the player reaches step 10 and has entered Barry's shop, skip the tutorial
            currentStep++;
            enterredShop = false; // Reset the flag after skipping
        }

        if (currentStep == 12 && exitedShop)
        {
            // If the player reaches step 11 and has exited Barry's shop, skip the tutorial
            currentStep++;
            exitedShop = false; // Reset the flag after skipping
        }

        if (currentStep == 15 && enterredShop)
        {
            // If the player reaches step 15 and has fish data, skip the tutorial
            currentStep++;
        }

        if (currentStep == 18 && exitedShop)
        {
            currentStep++;
        }
    }

    public void ShowPanel()
    {
        if (currentStep >= expressions.Count)
        {
            // If the current step exceeds the number of expressions, reset to 0
            SkipTutorial();
        }
        else if (expressions[currentStep] != null)
        {
            tutorialPanel.SetActive(true);
            // Set the sprite for the current step
            image.GetComponent<Image>().sprite = expressions[currentStep];
            text.text = texts[currentStep];
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraScript>().blockZoom();
            Time.timeScale = 0.0f; // Pause the game time
        }
        else
        {
            tutorialPanel.SetActive(false);
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraScript>().unlockZoom();
            Time.timeScale = 1.0f; // Resume the game time if no expression is set
        }

        if (currentStep == 4)
        {
            arrow.SetActive(true); // Show the arrow when reaching step 4
            arrow.GetComponent<ArrowScript>().targetPosition = gooberFishes.transform.position; // Set the target position for the arrow
        }
        else if (currentStep == 10)
        {
            arrow.SetActive(true);
            arrow.GetComponent<ArrowScript>().targetPosition = barryShop.transform.position; // Set the target position for the arrow
        }
        else if (currentStep == 15)
        {
            arrow.SetActive(true);
            arrow.GetComponent<ArrowScript>().targetPosition = BensonShop.transform.position; // Set the target position for the arrow
        }
        else
        {
            arrow.SetActive(false); // Hide the arrow for other steps
        }
    }

    public void SkipTutorial()
    {
        Time.timeScale = 1.0f; // Resume the game time
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraScript>().unlockZoom();
        this.gameObject.SetActive(false); // Hide the tutorial script GameObject
        // Hide the tutorial panel and arrow
        tutorialPanel.SetActive(false);
        arrow.SetActive(false);
        // Optionally, you can also reset the current step
        currentStep = 0;
    }
}

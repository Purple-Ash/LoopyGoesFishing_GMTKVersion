using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChrisScript : BaseNPCScript
{
    // The Equipment View UI object
    public GameObject imageView;
    public GameObject equipmentView;

    public List<string> chitchats;
    public List<string> responses;
    public List<NewFishData> fishDataList;
    public int fishIndex;
    public int fishSize;
    public float fishPrice;

    public bool isChitchatting = true;
    public bool firstVisit = true;
    public bool isWaiting = false;
    public bool isResponding = false;

    public GameObject redButton;
    public GameObject blueButton;
    public GameObject greenButton;

    public TMPro.TMP_Text textbox;

    void Start()
    {
        fishIndex = Random.Range(0, fishDataList.Count);
        fishSize = Random.Range(0, 3); // Random size between 1 and 2
        fishPrice = fishDataList[fishIndex].price * (fishSize == 0 ? 1 : fishSize == 1 ? 2 : 4) * FindObjectOfType<EquipementScript>().moneyMult * Random.Range(4, 9) / 2f; // Price based on size
    }

    public void onBlueButton()
    {
        int randomIndex = Random.Range(0, responses.Count);
        string response = responses[randomIndex];
        blueButton.transform.GetChild(0).GetComponent<TMP_Text>().text = response;
        if(!isResponding)
        {
            isChitchatting = false;
            textbox.text = "Anyway I am, like, totally starving man. Any chance could get me any, like, " + fishDataList[fishIndex].name + " ? The " + (fishSize == 0 ? "M" : fishSize == 1 ? "L" : "XL") + " one specifically. I'll give you, like, " + fishPrice + "$ for it.";
            
        }
        else
        {
            isResponding = false;
        }

        setShopUIActive();
    }

    public void onRedButton()
    {
        isChitchatting = true;
        isWaiting = true;
        isResponding = true;
        textbox.text = "Yeah, that's like, totally cool, man. Come back in, like, a few minutes and maybe I'll get a taste for a different fish, you know? I can, like, just talk for now tho, man.";

        setShopUIActive();
    }

    public void onGreenButton()
    {
        EquipementScript equipementScript = FindAnyObjectByType<EquipementScript>();
        if (equipementScript.fishDataDictionary.ContainsKey(fishDataList[fishIndex]) && equipementScript.fishDataDictionary[fishDataList[fishIndex]][fishSize] != 0)
        {
            equipementScript.fishDataDictionary[fishDataList[fishIndex]][fishSize] -= 1;
            equipementScript.money += fishPrice; // Add the price to the player's money

            fishIndex = Random.Range(0, fishDataList.Count);
            fishSize = Random.Range(0, 3); // Random size between 1 and 2
            fishPrice = fishDataList[fishIndex].price * (fishSize == 0 ? 1 : fishSize == 1 ? 2 : 4) * FindObjectOfType<EquipementScript>().moneyMult * Random.Range(4, 9) / 2f;
            textbox.text = "Ay, man, like, totally thank you. Here's your chash, man.";
        }
        else
        {
            textbox.text = "Ay, man, that's, like, totally cool, man. No worries, you know? I'll wait here till you get it for me, man. Anyway, I can, like, totally talk about something else now.";
        }

        isResponding = true;
        isChitchatting = true;
        setShopUIActive();
    }

    public IEnumerator waitForFish()
    {
        yield return new WaitForSeconds(90f); // Wait for 5 seconds
        isWaiting = false;
        fishIndex = Random.Range(0, fishDataList.Count);
        fishSize = Random.Range(0, 3); // Random size between 1 and 2
        fishPrice = fishDataList[fishIndex].price * (fishSize == 0 ? 1 : fishSize == 1 ? 2 : 4) * FindObjectOfType<EquipementScript>().moneyMult * Random.Range(4, 9) / 2f; // Price based on size
    }

    public override void setShopUIActive()
    {
        imageView.SetActive(true);
        equipmentView.SetActive(true); // Show the equipment view when the image view is opened
        if (isChitchatting || isWaiting)
        {
            blueButton.SetActive(true);
            redButton.SetActive(false);
            greenButton.SetActive(false);
            if(!isResponding)
                textbox.text = chitchats[firstVisit ? 0 : Random.Range(0, chitchats.Count)];
        }
        else
        {
            greenButton.SetActive(true);
            redButton.SetActive(true);
            blueButton.SetActive(false);
            if (FindAnyObjectByType<EquipementScript>().fishDataDictionary.ContainsKey(fishDataList[fishIndex]) && FindAnyObjectByType<EquipementScript>().fishDataDictionary[fishDataList[fishIndex]][fishSize] != 0)
            {
                greenButton.transform.GetChild(0).GetComponent<TMP_Text>().text = "Sure, no problem!";
            }
            else
            {
                greenButton.transform.GetChild(0).GetComponent<TMP_Text>().text = "Yeah, just let me catch it.";
            }
        }
        firstVisit = false;
    }

    public override void setShopUIInactive()
    {
        imageView.SetActive(false);
        equipmentView.SetActive(false); // Hide the equipment view when the image view is closed
        Time.timeScale = 1.0f; // Resume the game time
    }
}

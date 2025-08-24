using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LighthouseEntryManager : MonoBehaviour
{
    public NewFishData newFishData;

    public void onInfo()
    {
        if (FindObjectOfType<FishExpertScript>().onInfo(newFishData))
        {
            Image[] fishImages = GetComponentsInChildren<Image>();
            TMP_Text[] textFields = GetComponentsInChildren<TMP_Text>();

            //fishImages[1].sprite = newFishData.image;
            //fishImages[1].color = Color.white;

            textFields[0].SetText(newFishData.name);

            ColorUtility.TryParseHtmlString("#CFC2A1", out Color baseTextColor);

            textFields[0].color = baseTextColor;
            transform.GetChild(3).GetChild(0).GetComponent<TMP_Text>().SetText("Remind me where to find them again?");
        }
        FindObjectOfType<EquipementScript>().UpdateMoneyAndWeight();
    }

    public void onDelivery()
    {
        FindAnyObjectByType<FishExpertScript>().onDiscover(newFishData);
        TMP_Text[] textFields = GetComponentsInChildren<TMP_Text>();
        textFields[1].SetText(newFishData.infoDetails);
        ColorUtility.TryParseHtmlString("#CFC2A1", out Color baseTextColor);
        textFields[1].color = baseTextColor;
        transform.GetChild(4).gameObject.SetActive(false);
    }
}

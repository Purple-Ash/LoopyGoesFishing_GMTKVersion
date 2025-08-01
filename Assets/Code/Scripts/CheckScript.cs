using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckScript : MonoBehaviour
{
    bool isChecked = false;
    Color checkedColor = new Color(0.2f, 0.8f, 0.2f, 1f);
    Color uncheckedColor = new Color(0.8f, 0.2f, 0.2f, 1f);

    private void Start()
    {
        // Initialize the checkbox state based on the current color
        if (isChecked == false)
        {
            GetComponent<UnityEngine.UI.Image>().color = uncheckedColor;
        }
        else
        {
            GetComponent<UnityEngine.UI.Image>().color = checkedColor;
        }
    }

    public void OnClick(int size)
    {
        if (isChecked)
        {
            isChecked = false;
            GetComponent<UnityEngine.UI.Image>().color = uncheckedColor;
        }
        else
        {
            isChecked = true;
            GetComponent<UnityEngine.UI.Image>().color = checkedColor;
        }

        string name = "";
        for (int i = 0; i < transform.parent.parent.parent.childCount; i++)
        {
            if (transform.parent.parent.parent.GetChild(i).CompareTag("FishName"))
            {
                name = transform.parent.parent.parent.GetChild(i).GetComponent<TMPro.TMP_Text>().text;
                break;
            }
        }
        GameObject.FindGameObjectWithTag("EquipementManager").GetComponent<EquipementScript>().CheckFishData(name, size);
        transform.parent.parent.parent.parent.parent.parent.parent.GetComponent<BarryScript>().UpdatePrice();
    }

    public void CheckAll()
    {
        if (isChecked)
        {
            isChecked = false;
            GetComponent<UnityEngine.UI.Image>().color = uncheckedColor;
            for (int i = 0; i < transform.parent.GetChild(0).GetChild(0).childCount; i++)
            {
                foreach (var child in transform.parent.GetChild(0).GetChild(0).GetChild(i).GetComponentsInChildren<UnityEngine.UI.Image>())
                {
                    if (child.CompareTag("CheckButton"))
                    {
                        child.color = uncheckedColor;
                        child.GetComponent<CheckScript>().isChecked = false; // Ensure the state is updated
                    }
                }
            }
            transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = "Select All"; // Update text to "Check All"
            GameObject.FindGameObjectWithTag("EquipementManager").GetComponent<EquipementScript>().CheckAllFish(false);
        }
        else
        {
            isChecked = true;
            GetComponent<UnityEngine.UI.Image>().color = checkedColor;
            for (int i = 0; i < transform.parent.GetChild(0).GetChild(0).childCount; i++)
            {
                foreach (var child in transform.parent.GetChild(0).GetChild(0).GetChild(i).GetComponentsInChildren<UnityEngine.UI.Image>())
                {
                    if (child.CompareTag("CheckButton"))
                    {
                        child.color = checkedColor;
                        child.GetComponent<CheckScript>().isChecked = true; // Ensure the state is updated
                    }
                }
            }
            transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = "Unselect All"; // Update text to "Uncheck All"
            GameObject.FindGameObjectWithTag("EquipementManager").GetComponent<EquipementScript>().CheckAllFish(true);
        }
        transform.parent.parent.GetComponent<BarryScript>().UpdatePrice();
    }
}

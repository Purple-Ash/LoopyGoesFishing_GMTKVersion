using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteScript : MonoBehaviour
{
    public void OnClick(int size)
    {
        string name = "";
        for (int i = 0; i < transform.parent.parent.parent.childCount; i++)
        {
            if (transform.parent.parent.parent.GetChild(i).CompareTag("FishName"))
            {
                name = transform.parent.parent.parent.GetChild(i).GetComponent<TMPro.TMP_Text>().text;
                break;
            }
        }
        transform.parent.parent.parent.parent.parent.parent.parent.parent.GetComponent<EquipementScript>().ClearFishData(name, size);
    }
}

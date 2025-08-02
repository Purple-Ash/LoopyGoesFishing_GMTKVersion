using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ZoomUpgrade", menuName = "Upgrades/ZoomUpgrade")]
public class ZoomUpgrade : UpgradeScript
{
    [SerializeField] private float maxZoom = 20f;
    [SerializeField] private int mapType = 1;

    public override void ApplyUpgrade()
    {
        FindObjectOfType<CameraScript>()._maxZoom = maxZoom;

        if (mapType == 1)
        {
            Debug.Log(GameObject.FindGameObjectWithTag("Maps").name);
            GameObject.FindGameObjectWithTag("Maps").transform.GetChild(0).gameObject.SetActive(false);
            GameObject.FindGameObjectWithTag("Maps").transform.GetChild(1).gameObject.SetActive(true);
        }
        else if (mapType == 2)
        {
            GameObject.FindGameObjectWithTag("Maps").transform.GetChild(1).gameObject.SetActive(false);
            GameObject.FindGameObjectWithTag("Maps").transform.GetChild(2).gameObject.SetActive(true);
        }
    }
}

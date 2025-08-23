using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FishExpertScript : BaseNPCScript
{
    // The GameObject to instantiate.
    public GameObject entityToSpawn;

    // Content container in which GameObjects are spawned
    public Transform contentTransform;

    // The Equipment View UI object
    public GameObject imageView;
    public GameObject equipmentView;
    public GameObject backPanel;

    [Header("Sounds")]
    [SerializeField] protected AudioClip discoverSound;
    [SerializeField] float discoverMultiplier = 0.5f;

    private AudioManager audioManager;

    void Start()
    {
        //SpawnEntities();
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    public override void setShopUIActive()
    {
        imageView.SetActive(true);
        equipmentView.SetActive(true); // Show the equipment view when the image view is opened
        backPanel.SetActive(true);
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraScript>().blockZoom();
        SpawnEntities();
        equipmentView.transform.GetChild(0).GetChild(0).position = new Vector3(equipmentView.transform.GetChild(0).GetChild(0).position.x, -2000, equipmentView.transform.GetChild(0).GetChild(0).position.z);
    }

    public override void setShopUIInactive()
    {
        imageView.SetActive(false);
        equipmentView.SetActive(false); // Hide the equipment view when the image view is closed
        backPanel.SetActive(false);
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraScript>().unlockZoom();

        DestroyEntities();

        Time.timeScale = 1.0f; // Resume the game time
    }

    void DestroyEntities()
    {
        Debug.Log("Destroying Equipment entities");
        for (int i = 0; i < contentTransform.childCount; i++)
        {
            Debug.Log("Destroying Equipment entities");
            Destroy(contentTransform.GetChild(i).gameObject);
        }
    }

    void SpawnEntities()
    {

    }

    public void UpdateValues()
    {
        DestroyEntities();
        SpawnEntities();
    }
}


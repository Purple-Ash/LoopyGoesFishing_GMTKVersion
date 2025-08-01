using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{

    [SerializeField] Slider volumeSlider;

    [SerializeField] GameObject optionsMenu;

    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1);
            Load();
        }
        else
        {
            Load();
        }
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name != "Main Menu")
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                ShowOptions();
            }
        }
    }

    public void ShowOptions()
    {
        optionsMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void CloseOptions() 
    {
        optionsMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
        Save();
        Debug.Log("Volume changed to " +  volumeSlider.value);
    }

    private void Load()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
    }

    private void Save()
    {
        PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
    }
}

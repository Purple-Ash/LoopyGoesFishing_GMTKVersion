using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{

    [SerializeField] Slider volumeSlider;

    [SerializeField] GameObject optionsMenu;

    [SerializeField] GameObject optionsBoard;

    [SerializeField] GameObject confirmationPopup;

    // Start is called before the first frame update
    void Start()
    {
        AudioListener.volume = 1f;
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1);
            Load();
        }
        else
        {
            Load();
        }

        SetQualityLevel(5);
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name != "Main Menu")
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                SwitchOptions();
            }
        }
    }

    public void SetQualityLevel(int level)
    {
        Debug.Log("Quality set to level" +  level);
        QualitySettings.SetQualityLevel(level);
    }

    void SwitchOptions()
    {
        if(!optionsMenu.activeSelf)
        {
            ShowOptions();
        }
        else
        {
            CloseOptions();
        }
    }

    public void ShowOptions()
    {
        CameraScript script = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraScript>();
        if (script != null)
        {
            script.blockZoom();
        }

        optionsMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void CloseOptions() 
    {
        CameraScript script = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraScript>();
        if (script != null)
        {
            script.unlockZoom();
        }
        optionsMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void ShowQuitConfirmationPopup()
    {
        if (optionsMenu.activeSelf)
        {
            confirmationPopup.SetActive(true);
            Selectable[] elements = optionsBoard.GetComponentsInChildren<Selectable>();
            foreach (Selectable el in elements)
            {
                el.interactable = false;
            }

        }
    }

    public void CancelQuitConfirmation()
    {
        confirmationPopup.SetActive(false);
        Selectable[] elements = optionsBoard.GetComponentsInChildren<Selectable>();
        foreach (Selectable el in elements)
        {
            el.interactable = true;
        }
    }

    public void QuitToMainMenu()
    {
        Debug.Log("Quitting to Main Menu");
        CloseOptions();
        SceneManager.LoadScene(PlayerPrefs.GetString("StartingMenu"));
    }


    public void ChangeVolume()
    {
        /*   
        AudioListener.volume = volumeSlider.value;
        Save();
        Debug.Log("Volume changed to " +  volumeSlider.value);*/
    }

    private void Load()
    {
        //volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
    }

    private void Save()
    {
        //PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
    }
}

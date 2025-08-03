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
    public enum Sliders
    {
        Master,
        Music,
        SFX,
        Ambient
    }

    [SerializeField] Slider _masterVolume;
    [SerializeField] Slider _musicVolume;
    [SerializeField] Slider _sfxVolume;
    [SerializeField] Slider _ambientVolume;

    [SerializeField] GameObject optionsMenu;

    [SerializeField] GameObject optionsBoard;

    [SerializeField] GameObject confirmationPopup;

    private bool isOtherMenuOpen = false;

    private string _masterVolumeKey = "MasterVolume";
    private string _musicVolumeKey = "MusicVolume";
    private string _sfxVolumeKey = "SFXVolume";
    private string _ambientVolumeKey = "AmbientVolume";

    [Header("Audio Parameters")]

    [SerializeField]  float _startMasterVolume = 0.7f;
    [SerializeField]  float _startMusicVolume = 1f;
    [SerializeField] float _startSfxVolume = 1f;
    [SerializeField]  float _startAmbientVolume = 1f;


    private AudioManager _audioManager;


    // Start is called before the first frame update
    void Start()
    {
        AudioListener.volume = _startMasterVolume;
        _audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        InitAllAudioSliders();
        LoadAllAudioSliders();
        ApplyAllSliders();

        SetQualityLevel(5);
    }

    private void InitAllAudioSliders()
    {
        if (!PlayerPrefs.HasKey(_masterVolumeKey))
        {
            PlayerPrefs.SetFloat(_masterVolumeKey, _startMasterVolume);
            AudioListener.volume = _startMasterVolume;
            _masterVolume.value = _startMasterVolume;
            Save(Sliders.Master);
            Debug.Log("NO INIT MASTER");
        }
        if (!PlayerPrefs.HasKey(_musicVolumeKey))
        {
            PlayerPrefs.SetFloat(_musicVolumeKey, _startMusicVolume);
            _audioManager.SetVolume(AudioManager.Audio.Music, _startMusicVolume);
            _musicVolume.value = _startMusicVolume;
            Save(Sliders.Music);

            Debug.Log("NO INIT MUSIC");
        }
        if (!PlayerPrefs.HasKey(_sfxVolumeKey))
        {
            PlayerPrefs.SetFloat(_sfxVolumeKey, _startSfxVolume);
            _audioManager.SetVolume(AudioManager.Audio.SFX, _startSfxVolume);
            _sfxVolume.value = _startSfxVolume;
            Save(Sliders.SFX);
            
            Debug.Log("NO INIT SFX");
        }
        if (!PlayerPrefs.HasKey(_ambientVolumeKey))
        {
            PlayerPrefs.SetFloat(_ambientVolumeKey, _startAmbientVolume);
            _audioManager.SetVolume(AudioManager.Audio.Ambient, _startAmbientVolume);
            _ambientVolume.value = _startAmbientVolume;
            Save(Sliders.Ambient);
            
            Debug.Log("NO INIT AMBIENT");
        }
    }
    private void LoadAllAudioSliders()
    {
        Load(Sliders.Master);
        Load(Sliders.Music);
        Load(Sliders.SFX);
        Load(Sliders.Ambient);
    }

    private void ApplyAllSliders()
    {
        ChangeMasterVolume();
        ChangeMusicVolume();
        ChangeSfxVolume();
        ChangeAmbientVolume();
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name != "Main Menu")
        {
            if(Input.GetKeyDown(KeyCode.Escape) && !isOtherMenuOpen)
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

    public void OtherMenuOpen()
    {
        isOtherMenuOpen = true;
    }

    public void OtherMenuClose()
    {
        isOtherMenuOpen = false;
    }

    public void ChangeMasterVolume()
    {
        AudioListener.volume = _masterVolume.value;
        Save(Sliders.Master);
        Debug.Log("Master volume changed to " +  _masterVolume.value);
    }

    public void ChangeMusicVolume()
    {

        Save(Sliders.Music);
        _audioManager.SetVolume(AudioManager.Audio.Music, _musicVolume.value);
        Debug.Log("Music volume changed to " + _musicVolume.value);
    }

    public void ChangeAmbientVolume()
    {
        Save(Sliders.Ambient);
        _audioManager.SetVolume(AudioManager.Audio.Ambient, _ambientVolume.value);
        Debug.Log("Ambient volume changed to " + _ambientVolume.value);
    }

    public void ChangeSfxVolume()
    {
        Save(Sliders.SFX);
        _audioManager.SetVolume(AudioManager.Audio.SFX, _sfxVolume.value);
        Debug.Log("Sfx volume changed to " + _sfxVolume.value);
    }

    private void Load(Sliders slider)
    {
        switch (slider)
        {
            case Sliders.Master:
                _masterVolume.value = PlayerPrefs.GetFloat(_masterVolumeKey);
                return;
            case Sliders.Music:
                _musicVolume.value = PlayerPrefs.GetFloat(_musicVolumeKey);
                return;
            case Sliders.SFX:
                _sfxVolume.value = PlayerPrefs.GetFloat(_sfxVolumeKey);
                return;
            case Sliders.Ambient:
                _ambientVolume.value = PlayerPrefs.GetFloat(_ambientVolumeKey);
                return;
        }
        
    }
    private void Save(Sliders slider)
    {
        switch (slider)
        {
            case Sliders.Master:
                PlayerPrefs.SetFloat(_masterVolumeKey, _masterVolume.value);
                return;
            case Sliders.Music:
                PlayerPrefs.SetFloat(_musicVolumeKey, _musicVolume.value);
                return;
            case Sliders.SFX:
                PlayerPrefs.SetFloat(_sfxVolumeKey, _sfxVolume.value);
                return;
            case Sliders.Ambient:
                PlayerPrefs.SetFloat(_ambientVolumeKey, _ambientVolume.value);
                return;
        }
        
    }
}

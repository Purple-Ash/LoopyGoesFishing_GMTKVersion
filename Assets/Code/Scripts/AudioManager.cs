using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public enum Audio
    {
        Music,
        SFX,
        Ambient
    }

    [Header("Audio Sources")]
    [SerializeField] private float _musicVolume = 1f;
    [SerializeField] private float _sfxVolume = 1f;
    [SerializeField] private float _ambientVolume = 1f;

    [Header("Audio Clip")]
    [SerializeField] private AudioClip _mainMusic;
    [SerializeField] private float _musicMultiplier = 1f;
    [SerializeField] private AudioClip _buttonSound;
    [SerializeField] private float _buttonVolumeMultiplier = 0.8f;

    [SerializeField] private AudioClip _seagullAmbient;
    [SerializeField] private float _seagullMultiplier = 0.2f;


    private AudioSource _musicSource;
    private List<AudioSource> _sfxSources;
    private List<AudioSource> _ambientSource;

    private float GetVolume(Audio volumeSetting)
    {
        switch (volumeSetting)
        {
            case Audio.Music: return _musicVolume;
            case Audio.SFX: return _sfxVolume;
            case Audio.Ambient: return _ambientVolume;
        }
        Debug.LogWarning("Unknown Volume Enum!");
        return _sfxVolume;
    }

    private void UpdateSFXVolume()
    {
        _sfxSources.RemoveAll(src => src == null);
        foreach (var source in _sfxSources)
        {
            source.volume = _sfxVolume;
        }
    }

    private void UpdateAmbientVolume()
    {
        _ambientSource.RemoveAll(src => src == null);
        foreach (var source in _sfxSources)
        {
            source.volume = _sfxVolume;
        }
    }

    private void UpdateMusicVolume()
    {
        _musicSource.volume = _musicVolume;
    }

    public void SetVolume(Audio volumeSetting, float newVolume)
    {
        switch (volumeSetting)
        {
            case Audio.Music:
                _musicVolume = newVolume;
                UpdateMusicVolume();
                return;
            case Audio.SFX:
                _sfxVolume = newVolume;
                UpdateSFXVolume();
                return;
            case Audio.Ambient:
                _ambientVolume = newVolume;
                UpdateAmbientVolume();
                return;
        }
        Debug.LogWarning("Unknown Volume Enum!");
    }

    public void PlayAtPosition(AudioClip clip, Vector3 position, float relativeVolume)
    {
        GameObject sourceObject = new GameObject("TempAudio");
        sourceObject.transform.position = position;
        AudioSource src = sourceObject.AddComponent<AudioSource>();
        src.clip = clip;
        src.volume = relativeVolume * _sfxVolume;
        src.spatialBlend = 1f; // 3D
        src.Play();
        _sfxSources.Add(src);
        Destroy(sourceObject, clip.length);
    }

    public void PlayCenter(AudioClip clip, float relativeVolume)
    {
        GameObject sourceObject = new GameObject("TempAudio");
        AudioSource src = sourceObject.AddComponent<AudioSource>();
        src.clip = clip;
        src.volume = relativeVolume * _sfxVolume;
        src.spatialBlend = 0f; // 2D
        src.Play();
        _sfxSources.Add(src);
        Destroy(sourceObject, clip.length);
    }

    public void SetRelativeVolume(AudioSource source, float relativeVolume)
    {
        source.volume = relativeVolume * _sfxVolume;
    }

    public void PlayButtonSound()
    {
        PlayCenter(_buttonSound, _buttonVolumeMultiplier);
    }

    public AudioSource PlayLoopAtPosition(AudioClip clip, Vector3 position, float relativeVolume)
    {
        GameObject sourceObject = new GameObject("TempAudio");
        sourceObject.transform.position = position;
        AudioSource src = sourceObject.AddComponent<AudioSource>();
        src.clip = clip;
        src.volume = relativeVolume * _sfxVolume;
        src.spatialBlend = 1f; // 3D
        src.loop = true;
        src.Play();
        _sfxSources.Add(src);
        return src;
    }

    private void PlayAmbientSound(AudioClip clip, float relativeVolume)
    {
        GameObject sourceObject = new GameObject("TempAmbient");
        AudioSource src = sourceObject.AddComponent<AudioSource>();
        src.clip = clip;
        src.volume = relativeVolume * _ambientVolume;
        src.spatialBlend = 0f; // 2D
        src.loop= true;
        src.Play();
        _ambientSource.Add(src);
        Debug.Log("Ambient Source Added");
    }

    private void PlayAmbientSounds()
    {
        PlayAmbientSound(_seagullAmbient, _seagullMultiplier);
    }

    private void Awake()
    {
        _sfxSources = new List<AudioSource>();
        _ambientSource = new List<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _musicSource = gameObject.AddComponent<AudioSource>();
        _musicSource.clip = _mainMusic;
        _musicSource.loop = true;
        _musicSource.spatialBlend = 0f; // 2D
        _musicSource.volume = _musicVolume * _musicMultiplier;
        _musicSource.Play();
        Debug.Log("Playing music");

        PlayAmbientSounds();
    }
}

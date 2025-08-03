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

    private AudioSource _musicSource;
    private List<AudioSource> _sfxSources;
    private AudioSource _ambientSource;

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
       _ambientSource.volume = _ambientVolume;
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

    private void Awake()
    {
        _sfxSources = new List<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _musicSource = gameObject.AddComponent<AudioSource>();
        _musicSource.clip = _mainMusic;
        _musicSource.loop = true;
        _musicSource.spatialBlend = 0f; // 2D
        _musicSource.volume = _musicVolume;
        _musicSource.Play();
        Debug.Log("Playing music");

        _ambientSource = gameObject.AddComponent<AudioSource>();
        _ambientSource.volume = _ambientVolume;
        _ambientSource.spatialBlend = 0f;
    }
}

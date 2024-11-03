using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundEffectsSlider;

    private void Start()
    {
        musicSlider.value = AudioManager.Instance.musicVolume;
        soundEffectsSlider.value = AudioManager.Instance.soundEffectsVolume;

        musicSlider.onValueChanged.AddListener(ChangeMusicVolume);
        soundEffectsSlider.onValueChanged.AddListener(ChangeSoundEffectsVolume);
    }

    private void ChangeMusicVolume(float volume)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetMusicVolume(volume);
            SaveVolumes();
        }
    }

    private void ChangeSoundEffectsVolume(float volume)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetSoundEffectsVolume(volume);
            SaveVolumes();
        }
    }

    private void SaveVolumes()
    {
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
        PlayerPrefs.SetFloat("SoundEffectsVolume", soundEffectsSlider.value);
        PlayerPrefs.Save();
    }

    private void LoadVolumes()
    {
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            float savedMusicVolume = PlayerPrefs.GetFloat("MusicVolume");
            musicSlider.value = savedMusicVolume;
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.SetMusicVolume(savedMusicVolume);
            }
        }

        if (PlayerPrefs.HasKey("SoundEffectsVolume"))
        {
            float savedSoundEffectsVolume = PlayerPrefs.GetFloat("SoundEffectsVolume");
            soundEffectsSlider.value = savedSoundEffectsVolume;
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.SetSoundEffectsVolume(savedSoundEffectsVolume);
            }
        }
    }

    private void OnEnable()
    {
        LoadVolumes();
    }

    private void OnDisable()
    {
        SaveVolumes();
    }
}

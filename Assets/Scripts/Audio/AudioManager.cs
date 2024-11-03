using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    const string audioDirectory = "Audio";
    const string StartGameSoundEffectFilename = "StartGameSound";
    const string GoofyDeathSoundEffectFilename = "DeathGoofySound";
    const string SceneTransitionSoundEffectFilename = "SceneTransitionSound";

    public static AudioManager Instance;

    [Range(0.0f, 1.0f)] public float musicVolume;
    [Range(0.0f, 1.0f)] public float soundEffectsVolume;

    private string songsDirectory;
    private string soundEffectsDirectory;
    private List<AudioClip> allSongs = new();
    private List<AudioClip> allSoundEffects = new();
    private AudioSource musicSource;
    private AudioSource soundEffectsSource;
    private int currentSongIndex = 0;

    private bool isPaused = false;
    private bool isAppFocused = true;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            musicVolume = PlayerPrefs.HasKey("MusicVolume") ? PlayerPrefs.GetFloat("MusicVolume") : musicVolume;
            soundEffectsVolume = PlayerPrefs.HasKey("SoundEffectsVolume") ? PlayerPrefs.GetFloat("SoundEffectsVolume") : soundEffectsVolume;

            InitAudioSourcesAndDirectories();

            musicSource.loop = true;

            ShuffleSongs();
            PlayNextSong();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (!isPaused && !musicSource.isPlaying && allSongs.Count > 0)
        {
            PlayNextSong();
        }
    }

    private void InitAudioSourcesAndDirectories()
    {
        musicSource = gameObject.AddComponent<AudioSource>();
        soundEffectsSource = gameObject.AddComponent<AudioSource>();

        musicSource.volume = musicVolume;
        soundEffectsSource.volume = soundEffectsVolume;

        songsDirectory = audioDirectory + "/songs";
        soundEffectsDirectory = audioDirectory + "/soundEffects";

        allSongs = LoadAllSongsFromDirectory(songsDirectory);
        allSoundEffects = LoadAllSongsFromDirectory(soundEffectsDirectory);
    }

    private List<AudioClip> LoadAllSongsFromDirectory(string directory)
    {
        AudioClip[] loadedSongs = Resources.LoadAll<AudioClip>(directory);

        if (loadedSongs.Length > 0)
        {
            return loadedSongs.ToList();
        }
        else
        {
            Debug.LogWarning("No se encontraron archivos de audio en el directorio: " + directory);
            return null;
        }
    }

    private void ShuffleSongs()
    {
        for (int i = 0; i < allSongs.Count; i++)
        {
            AudioClip temp = allSongs[i];
            int randomIndex = Random.Range(0, allSongs.Count);
            allSongs[i] = allSongs[randomIndex];
            allSongs[randomIndex] = temp;
        }
    }

    private void PlayNextSong()
    {
        if (allSongs.Count > 0)
        {
            musicSource.clip = allSongs[currentSongIndex];
            musicSource.Play();

            currentSongIndex = (currentSongIndex + 1) % allSongs.Count;
        }
        else
        {
            Debug.LogWarning("No hay canciones para reproducir.");
        }
    }

    private void PlaySoundEffectByName(string soundName)
    {
        AudioClip clip = allSoundEffects.FirstOrDefault(s => s.name == soundName);
        if (clip != null)
        {
            soundEffectsSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("No se encontró el sonido: " + soundName);
        }
    }

    public void PlayStartNewGameSound()
    {
        PlaySoundEffectByName(StartGameSoundEffectFilename);
    }

    public void PlayGoofyDeathSound()
    {
        PlaySoundEffectByName(GoofyDeathSoundEffectFilename);
    }

    public void PlayTransitionSceneSound()
    {
        PlaySoundEffectByName(SceneTransitionSoundEffectFilename);
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp(volume, 0f, 1f);
        musicSource.volume = musicVolume;
    }

    public void SetSoundEffectsVolume(float volume)
    {
        soundEffectsVolume = Mathf.Clamp(volume, 0f, 1f);
        soundEffectsSource.volume = soundEffectsVolume;
    }

    public void MuteAllSounds()
    {
        musicSource.Pause();
        musicSource.volume = 0f;
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        isAppFocused = hasFocus;

        // Pausar al perder el focus de la ventana, reanudar al ganarlo
        HandleAudioPause(!hasFocus);
    }

    public void HandleAudioPause(bool shouldPause)
    {
        isPaused = shouldPause;

        if (shouldPause)
        {
            musicSource.Pause();
            soundEffectsSource.Pause();
        }
        else
        {
            musicSource.UnPause();
            soundEffectsSource.UnPause();
        }
    }
}

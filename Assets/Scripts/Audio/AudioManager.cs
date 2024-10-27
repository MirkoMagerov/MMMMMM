using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private string audioDirectory = "Audio/Songs";
    private List<AudioClip> allSongs = new List<AudioClip>();
    private AudioSource audioSource;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            audioSource = gameObject.AddComponent<AudioSource>();

            LoadAllSongsFromDirectory(audioDirectory);

            ShuffleSongs();

            PlayNextSong();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadAllSongsFromDirectory(string directory)
    {
        AudioClip[] loadedSongs = Resources.LoadAll<AudioClip>(directory);

        if (loadedSongs.Length > 0)
        {
            allSongs.AddRange(loadedSongs);
        }
        else
        {
            Debug.LogWarning("No se encontraron archivos de audio en el directorio: " + directory);
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
            audioSource.clip = allSongs[0];
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("No hay canciones para reproducir.");
        }
    }
}

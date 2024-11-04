using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Levels
{
    FirstLevel = 1,
    SecondLevel = 2,
    ThirdLevel = 3
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private Dictionary<Levels, Vector3> checkpointsPerLevel = new Dictionary<Levels, Vector3>();
    private Levels currentLevel;
    private SceneTransition sceneTransition;
    private GameObject PlayerGO;
    private Vector3 activeSpawnPoint;
    private bool gravityFlipped = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (PlayerGO == null)
            {
                PlayerGO = GameObject.FindGameObjectWithTag("Player");
            }
        } 
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        currentLevel = (Levels)SceneManager.GetActiveScene().buildIndex;
        sceneTransition = gameObject.GetComponentInChildren<SceneTransition>();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (Enum.IsDefined(typeof(Levels), scene.buildIndex))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            currentLevel = (Levels)scene.buildIndex;

            SetInitialSpawnPoint();

            PlayerManager.Instance.gameObject.transform.position = activeSpawnPoint;
        }

        if (SceneManager.GetActiveScene().buildIndex == 4)
        {
            StartCreditsScene();
        }
    }

    private void SetInitialSpawnPoint()
    {
        if (Enum.IsDefined(typeof(Levels), SceneManager.GetActiveScene().buildIndex))
        {
            if (checkpointsPerLevel.ContainsKey(currentLevel))
            {
                activeSpawnPoint = checkpointsPerLevel[currentLevel];
            }
            else
            {
                activeSpawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint").transform.position;
            }
        }
    }

    public void NewCheckpoint(Vector3 newCheckpoint, bool newGravityFlipped)
    {
        activeSpawnPoint = newCheckpoint;
        gravityFlipped = newGravityFlipped;

        checkpointsPerLevel[currentLevel] = newCheckpoint;
    }

    public void EnablePlayer()
    {
        PlayerGO.SetActive(true);
    }

    public bool InPlayingLevelsScene()
    {
        return Enum.IsDefined(typeof(Levels), SceneManager.GetActiveScene().buildIndex);
    }

    private bool CheckValidSceneIndex(int indexToLoad)
    {
        if (indexToLoad < 0 || indexToLoad >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.LogWarning("El índice de escena está fuera de rango o no es correcto.");
            return false;
        }

        return true;
    }

    private void DisablePlayerControls()
    {
        UIManager.Instance.SetCanPause(false);
        PlayerManager.Instance.DisableMovementAndGravity();
    }

    private void EnablePlayerControls()
    {
        PlayerManager.Instance.EnableMovementAndGravity();
        UIManager.Instance.SetCanPause(true);
    }

    public void LoadLevel(int nextSceneIndex)
    {
        StartCoroutine(LoadNextLevelCoroutine(nextSceneIndex));
    }

    private IEnumerator LoadNextLevelCoroutine(int nextSceneIndex)
    {
        DisablePlayerControls(); 

        if (!CheckValidSceneIndex(nextSceneIndex))
        {
            yield break;
        }

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            AudioManager.Instance.PlayStartNewGameSound();
        }
        else
        {
            AudioManager.Instance.PlayTransitionSceneSound();
        }

        yield return StartCoroutine(sceneTransition.EndLevelAnimation());

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextSceneIndex);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        yield return StartCoroutine(sceneTransition.StartLevelAnimation());

        EnablePlayerControls();

        if (currentLevel == Levels.FirstLevel)
        {
            PlayerManager.Instance.DisableGravityChange();
        }
    }

    public IEnumerator ReturnToLastLevelAndActivateFlags(int lastLevelIndex)
    {
        yield return StartCoroutine(LoadNextLevelCoroutine(lastLevelIndex));
        ActivateAllFlagsInScene();
    }

    private void ActivateAllFlagsInScene()
    {
        Checkpoint[] flags = GameObject.FindGameObjectsWithTag("Checkpoint").Select(go => go.GetComponent<Checkpoint>()).ToArray();

        foreach (Checkpoint checkpoint in flags)
        {
            if (checkpoint.IsActivated())
            {
                checkpoint.ActivateFlag();
            }
        }
    }

    public Vector3 GetLastCheckpoint() { return activeSpawnPoint; }

    public bool IsGravityFlipped() { return gravityFlipped; }

    public void StartCreditsScene()
    {
        Destroy(PlayerGO);
        UIManager.Instance.SetCanPause(false);
        AudioManager.Instance.MuteAllSounds();
    }
}

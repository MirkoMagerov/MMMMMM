using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Levels
{
    FirstLevel = 1,
    SecondLevel = 2,
    ThirdLevel = 3,
    FourthLevel = 4
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            LoadSceneWithCheckpoint(Levels.FirstLevel);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            LoadSceneWithCheckpoint(Levels.SecondLevel);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (Enum.IsDefined(typeof(Levels), scene.buildIndex))
        {
            currentLevel = (Levels)scene.buildIndex;

            SetInitialSpawnPoint();

            PlayerManager.Instance.gameObject.transform.position = activeSpawnPoint;
        }
    }

    private void SetInitialSpawnPoint()
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

    public void NewCheckpoint(Vector3 newCheckpoint, bool newGravityFlipped)
    {
        activeSpawnPoint = newCheckpoint;
        gravityFlipped = newGravityFlipped;

        checkpointsPerLevel[currentLevel] = newCheckpoint;
    }

    public void LoadSceneWithCheckpoint(Levels newLevel)
    {
        checkpointsPerLevel[currentLevel] = activeSpawnPoint;

        SceneManager.LoadSceneAsync((int)newLevel);
    }

    public void EnablePlayer()
    {
        PlayerGO.SetActive(true);
    }

    public IEnumerator WaitForSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
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

        yield return StartCoroutine(sceneTransition.EndLevelAnimation());

        int indexToLoad = SceneManager.GetActiveScene().buildIndex + nextSceneIndex;

        if (!CheckValidSceneIndex(indexToLoad))
        {
            yield break;
        }

        SceneManager.LoadScene(indexToLoad);

        yield return new WaitForSeconds(0.5f);

        yield return StartCoroutine(sceneTransition.StartLevelAnimation());

        EnablePlayerControls();
    }

    public Vector3 GetLastCheckpoint() { return activeSpawnPoint; }

    public bool IsGravityFlipped() { return gravityFlipped; }
}

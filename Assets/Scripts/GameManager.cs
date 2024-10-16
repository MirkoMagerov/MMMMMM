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
    private GameObject PlayerGO;
    private Vector3 activeSpawnPoint;
    private Levels currentLevel;
    private bool gravityFlipped = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (PlayerGO == null)
            {
                GetPlayer();
            }
        } 
        else
        {
            Destroy(gameObject);
        }
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

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        currentLevel = (Levels)SceneManager.GetActiveScene().buildIndex;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        currentLevel = (Levels)scene.buildIndex;

        if (Enum.IsDefined(typeof(Levels), scene.buildIndex))
        {
            SetInitialSpawnPoint();

            PlayerManager.Instance.gameObject.transform.position = activeSpawnPoint;
        }
    }

    private void SetInitialSpawnPoint()
    {
        if (checkpointsPerLevel.ContainsKey(currentLevel))
        {
            Debug.Log(checkpointsPerLevel);
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

    public Vector3 GetLastCheckpoint() { return activeSpawnPoint; }

    public bool IsGravityFlipped() { return gravityFlipped; }

    public void StartNewGame()
    {
        PlayerGO.SetActive(true);
    }

    private void GetPlayer()
    {
        PlayerGO = GameObject.FindGameObjectWithTag("Player");
        if (PlayerGO != null) { PlayerGO.SetActive(false); }
        else { Debug.LogError("Player not found"); }
    }

    public IEnumerator WaitForSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    public bool InPlayingLevelsScene()
    {
        return Enum.IsDefined(typeof(Levels), SceneManager.GetActiveScene().buildIndex);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    private Vector3 activeSpawnPoint;
    private bool gravityFlipped = false;
    private Levels currentLevel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
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

        SetInitialSpawnPoint();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        currentLevel = (Levels)scene.buildIndex;

        SetInitialSpawnPoint();

        PlayerManager.Instance.gameObject.transform.position = activeSpawnPoint;
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

    public Vector3 GetLastCheckpoint() { return activeSpawnPoint; }

    public bool IsGravityFlipped() { return gravityFlipped; }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}

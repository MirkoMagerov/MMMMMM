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

    // ------ CAMBIAR A PRIVADA ----------
    public Dictionary<Levels, Vector3> checkpointsPerLevel = new Dictionary<Levels, Vector3>();
    private Vector3 lastActivatedCheckpoint;
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
        currentLevel = (Levels)SceneManager.GetActiveScene().buildIndex;

        if (checkpointsPerLevel.ContainsKey(currentLevel))
        {
            lastActivatedCheckpoint = checkpointsPerLevel[currentLevel];
        }
        else
        {
            lastActivatedCheckpoint = GameObject.FindGameObjectWithTag("Player").transform.position;
        }
    }

    public void NewCheckpoint(Vector3 newCheckpoint, bool newGravityFlipped)
    {
        lastActivatedCheckpoint = newCheckpoint;
        gravityFlipped = newGravityFlipped;

        checkpointsPerLevel[currentLevel] = newCheckpoint;
    }

    public void LoadSceneWithCheckpoint(Levels newLevel)
    {
        checkpointsPerLevel[currentLevel] = lastActivatedCheckpoint;

        SceneManager.LoadSceneAsync((int)newLevel);

        currentLevel = newLevel;

        if (checkpointsPerLevel.ContainsKey(currentLevel))
        {
            lastActivatedCheckpoint = checkpointsPerLevel[currentLevel];
        }
        else
        {
            lastActivatedCheckpoint = GameObject.FindGameObjectWithTag("Player").transform.position;
        }
    }

    public Vector3 GetLastCheckpoint() { return lastActivatedCheckpoint; }

    public bool IsGravityFlipped() { return gravityFlipped; }
}

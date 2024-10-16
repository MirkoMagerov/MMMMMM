using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuCanvasGO;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        pauseMenuCanvasGO.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && SceneManager.GetActiveScene().name.Equals("Menu"))
        {
            StartCoroutine(GameManager.Instance.WaitForSeconds(2));

            SceneManager.LoadSceneAsync(Levels.FirstLevel.ToString());
            GameManager.Instance.StartNewGame();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && GameManager.Instance.InPlayingLevelsScene())
        {
            if (pauseMenuCanvasGO.activeSelf)
            {
                ResumeGameLogic();
            }
            else
            {
                PauseGameLogic();
            }
        }
    }

    public void ResumeGameButtonClick()
    {
        ResumeGameLogic();
    }

    public void ExitGameButtonClick()
    {
        Debug.Log("Exiting game");
        Application.Quit();
    }

    private void ResumeGameLogic()
    {
        pauseMenuCanvasGO.SetActive(false);
        Time.timeScale = 1;
        PlayerManager.Instance.EnableMovementAndGravity();
    }

    private void PauseGameLogic()
    {
        pauseMenuCanvasGO.SetActive(true);
        Time.timeScale = 0;
        PlayerManager.Instance.DisableMovementAndGravity();
    }
}

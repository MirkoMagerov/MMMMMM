using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private GameObject pauseMenuCanvasGO;
    private bool canPause = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }

        if (SceneManager.GetActiveScene().buildIndex == 4)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        pauseMenuCanvasGO.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && GameManager.Instance.InPlayingLevelsScene() && canPause)
        {
            if (pauseMenuCanvasGO.activeSelf)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                ResumeGameLogic();
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
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
        Application.Quit();
    }

    private void PauseGameLogic()
    {
        AudioManager.Instance.HandleAudioPause(true);
        pauseMenuCanvasGO.SetActive(true);
        Time.timeScale = 0;
        PlayerManager.Instance.DisableMovementAndGravity();
    }

    private void ResumeGameLogic()
    {
        AudioManager.Instance.HandleAudioPause(false);
        pauseMenuCanvasGO.SetActive(false);
        Time.timeScale = 1;
        PlayerManager.Instance.EnableMovementAndGravity();
    }

    public void Play()
    {
        GameManager.Instance.LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
        GameManager.Instance.EnablePlayer();
    }

    public void Exit()
    {
        Application.Quit();
    }

    public bool GetCanPause() { return canPause; }

    public void SetCanPause(bool value) { canPause = value; }
}

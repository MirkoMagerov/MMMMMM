using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    private CinemachineVirtualCamera cinemachineCamera;

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

    void Start()
    {
        cinemachineCamera = GetComponentInChildren<CinemachineVirtualCamera>();

        AssignPlayerToCamera();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AssignPlayerToCamera();
    }

    private void AssignPlayerToCamera()
    {
        if (PlayerManager.Instance != null)
        {
            cinemachineCamera.Follow = PlayerManager.Instance.transform;
            cinemachineCamera.LookAt = PlayerManager.Instance.transform;

            cinemachineCamera.OnTargetObjectWarped(PlayerManager.Instance.transform, PlayerManager.Instance.transform.position - cinemachineCamera.transform.position);
        }
    }
}

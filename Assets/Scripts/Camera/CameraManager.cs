using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    private CinemachineVirtualCamera cinemachineCamera;

    [SerializeField] private float verticalOffset = 2f;

    private bool isGravityFlipped;

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

        UpdateCameraOffset();
    }

    private void Update()
    {
        isGravityFlipped = PlayerReferenceManager.Instance.playerGC.GetIsGravityFlipped();

        UpdateCameraOffset();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AssignPlayerToCamera();
    }

    private void UpdateCameraOffset()
    {
        if (cinemachineCamera != null)
        {
            CinemachineFramingTransposer framingTransposer = cinemachineCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
            if (framingTransposer != null)
            {
                framingTransposer.m_TrackedObjectOffset.y = isGravityFlipped ? -verticalOffset : verticalOffset;
            }
        }
    }

    private void AssignPlayerToCamera()
    {
        if (PlayerManager.Instance != null)
        {
            cinemachineCamera.Follow = PlayerManager.Instance.transform;
            cinemachineCamera.LookAt = PlayerManager.Instance.transform;
        }
    }
}

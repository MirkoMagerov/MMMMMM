using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    private CinemachineVirtualCamera cinemachineCamera;

    [SerializeField] private float normalOffset = 1.5f;
    [SerializeField] private float alternativeOffset = -1.5f;
    [SerializeField] private float smoothSpeed = 5f;

    private bool isGravityFlipped;
    private float currentOffset;
    private float targetOffset;
    private float verticalOffset;
    private bool isAlternativeOffset = false;

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

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        cinemachineCamera = GetComponentInChildren<CinemachineVirtualCamera>();

        AssignPlayerToCamera();

        SceneManager.sceneLoaded += OnSceneLoaded;

        verticalOffset = normalOffset;
        currentOffset = normalOffset;
        targetOffset = normalOffset;
        UpdateCameraOffset();
    }

    private void Update()
    {
        isGravityFlipped = PlayerReferenceManager.Instance.playerGC.GetIsGravityFlipped();
        targetOffset = isGravityFlipped ? -verticalOffset : verticalOffset;

        currentOffset = Mathf.Lerp(currentOffset, targetOffset, Time.deltaTime * smoothSpeed);
        UpdateCameraOffset();

        if (Input.GetKeyDown(KeyCode.V))
        {
            SwapVerticalOffset();
        }
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
                framingTransposer.m_TrackedObjectOffset.y = currentOffset;
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

    private void SwapVerticalOffset()
    {
        isAlternativeOffset = !isAlternativeOffset;

        verticalOffset = isAlternativeOffset ? alternativeOffset : normalOffset;

        targetOffset = isGravityFlipped ? -verticalOffset : verticalOffset;
    }
}

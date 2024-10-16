using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

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

    public void DisableMovementAndGravity()
    {
        PlayerReferenceManager.Instance.playerMovement.SetCanMove(false);
        PlayerReferenceManager.Instance.playerGC.SetCanChangeGravity(false);
    }

    public void EnableMovementAndGravity()
    {
        PlayerReferenceManager.Instance.playerMovement.SetCanMove(true);
        PlayerReferenceManager.Instance.playerGC.SetCanChangeGravity(true);
    }
}

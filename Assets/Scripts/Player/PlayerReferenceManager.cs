using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReferenceManager : MonoBehaviour
{
    [HideInInspector] public PlayerAnimation playerAnimation;
    [HideInInspector] public PlayerGravityController playerGC;
    [HideInInspector] public PlayerLife playerLife;
    [HideInInspector] public PlayerManager playerManager;
    [HideInInspector] public PlayerMovement playerMovement;

    void Start()
    {
        playerAnimation = GetComponent<PlayerAnimation>();
        playerGC = GetComponent<PlayerGravityController>();
        playerLife = GetComponent<PlayerLife>();
        playerManager = GetComponent<PlayerManager>();
        playerMovement = GetComponent<PlayerMovement>();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    PlayerReferenceManager playerReferenceManager;

    private void Start()
    {
        animator = GetComponent<Animator>();
        playerReferenceManager = GetComponent<PlayerReferenceManager>();
    }

    void Update()
    {
        ControlAnimations();
        FlipLocalScaleHorizontally();
    }

    void FlipLocalScaleHorizontally()
    {
        if (playerReferenceManager.playerMovement != null)
        {
            float dirX = playerReferenceManager.playerMovement.GetDirX();
            if (dirX != 0)
            {
                Vector2 localScale = transform.localScale;
                localScale.x = dirX > 0 ? 1 : -1;
                transform.localScale = localScale;
            }
        }
    }

    void ControlAnimations()
    {
        if (playerReferenceManager.playerMovement != null)
        {
            animator.SetBool("running", playerReferenceManager.playerMovement.IsMoving());
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    PlayerReferenceManager playerReferenceManager;

    private float dirX;

    private void Start()
    {
        animator = GetComponent<Animator>();
        playerReferenceManager = GetComponent<PlayerReferenceManager>();
    }

    void Update()
    {
        ControlAnimations();
        FlipLocalScaleHorizontally();

        dirX = playerReferenceManager.playerMovement.GetDirX();
    }

    private void FlipLocalScaleHorizontally()
    {
        if (playerReferenceManager.playerMovement != null)
        {
            if (dirX != 0)
            {
                Vector2 localScale = transform.localScale;
                localScale.x = dirX > 0 ? 1 : -1;
                transform.localScale = localScale;
            }
        }
    }

    private void ControlAnimations()
    {
        if (!playerReferenceManager.playerGC.IsGrounded())
        {
            animator.SetBool("falling", true);
        }
        else
        {
            animator.SetBool("falling", false);
        }

        animator.SetBool("running", playerReferenceManager.playerMovement.IsMoving());
    }
}

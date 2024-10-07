using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    private Rigidbody2D rb;
    private Animator animator;

    private float dirX;
    private bool canMove = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (canMove)
        {
            dirX = Input.GetAxisRaw("Horizontal");
            ControlAnimations();
            FlipLocalScaleHorizontally();
        }
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
        }
    }

    void FlipLocalScaleHorizontally()
    {
        if (dirX != 0)
        {
            Vector2 localScale = transform.localScale;
            localScale.x = dirX > 0 ? 1 : -1;
            transform.localScale = localScale;
        }
    }

    void ControlAnimations()
    {
        animator.SetBool("running", dirX != 0);
    }

    public void SetCanMove(bool newCanMove)
    {
        canMove = newCanMove;
    }
}

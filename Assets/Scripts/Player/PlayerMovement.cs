using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    private Rigidbody2D rb;
    private float dirX;
    private bool canMove = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (canMove)
        {
            dirX = Input.GetAxisRaw("Horizontal");
        }
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
        }
    }

    public void SetCanMove(bool newCanMove)
    {
        canMove = newCanMove;
    }

    public float GetDirX()
    {
        return dirX;
    }

    public bool IsMoving()
    {
        return dirX != 0;
    }
}

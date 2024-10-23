using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;
using UnityEngine.UIElements;

public class PlayerGravityController : MonoBehaviour
{
    [SerializeField] private float linearGravityStrength;
    [SerializeField] private float rayCastDistance;
    [SerializeField] private GameObject centerPoint;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Vector2 boxSize;

    private Rigidbody2D rb;

    private bool isGravityFlipped;
    private bool canChangeGravity = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        isGravityFlipped = false;
    }

    void Update()
    {
        if (PlayerManager.Instance.GetComponent<PlayerLife>().GetPlayerAlive())
        {
            ApplyCustomGravity();
        }

        if (canChangeGravity && IsGrounded())
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                FlipGravity();
            }
        }
    }

    public void FlipGravity()
    {
        isGravityFlipped = !isGravityFlipped;
        FlipLocalScaleVertically();
    }

    public void FlipGravity(bool newGravity)
    {
        if (isGravityFlipped != newGravity)
        {
            FlipLocalScaleVertically();
            isGravityFlipped = newGravity;
        }
    }

    private void ApplyCustomGravity()
    {
        float gravity = isGravityFlipped ? linearGravityStrength : -linearGravityStrength;
        rb.velocity = new Vector2(rb.velocity.x, gravity);
    }

    private void FlipLocalScaleVertically()
    {
        Vector2 localScale = transform.localScale;
        localScale.y *= -1;
        transform.localScale = localScale;
    }

    public bool IsGrounded()
    {
        Vector2 direction = isGravityFlipped ? Vector2.up : Vector2.down;

        return Physics2D.BoxCast(transform.position, boxSize, 0, direction, rayCastDistance, groundLayer);
    }

    public void SetCanChangeGravity(bool newCanChangeGravity)
    {
        canChangeGravity = newCanChangeGravity; 
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Vector2 direction = isGravityFlipped ? Vector2.up : Vector2.down;

        Gizmos.DrawWireCube(transform.position + (Vector3)direction * rayCastDistance, boxSize);
    }
}

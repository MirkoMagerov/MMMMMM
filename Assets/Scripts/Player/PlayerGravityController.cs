using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class PlayerGravityController : MonoBehaviour
{
    [SerializeField] private float linearGravityStrength;

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
        if (canChangeGravity)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                FlipGravity();
            }
            ApplyCustomGravity();
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

    void ApplyCustomGravity()
    {
        float gravity = isGravityFlipped ? linearGravityStrength : -linearGravityStrength;
        rb.velocity = new Vector2(rb.velocity.x, gravity);
    }

    void FlipLocalScaleVertically()
    {
        Vector2 localScale = transform.localScale;
        localScale.y *= -1;
        transform.localScale = localScale;
    }
    //public bool GetFlippedGravity() { return isGravityFlipped; }

    public void SetCanChangeGravity(bool newCanChangeGravity) { canChangeGravity = newCanChangeGravity; }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SpikeHead : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private GameObject pointA, pointB;

    private Transform currentPoint;
    private Animator animator;
    private Rigidbody2D rb;
    private bool canMove = true;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        currentPoint = pointB.transform;
    }

    private void Update()
    {
        if (canMove)
        {
            float direction = currentPoint == pointB.transform ? 1 : -1;
            rb.velocity = new Vector2(moveSpeed * direction, 0);

            if (Vector2.Distance(transform.position, currentPoint.position) < 0.05f && currentPoint == pointB.transform)
            {
                currentPoint = pointA.transform;
            }
            if (Vector2.Distance(transform.position, currentPoint.position) < 0.05f && currentPoint == pointA.transform)
            {
                currentPoint = pointB.transform;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.SetBool("Blink", true);
            canMove = false;
            rb.velocity = Vector2.zero;
            StartCoroutine(EnableMovement());
            animator.SetBool("Blink", false);
        }
    }

    private IEnumerator EnableMovement()
    {
        yield return new WaitForSeconds(1.5f);
        canMove = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(pointA.transform.position, 0.2f);
        Gizmos.DrawWireSphere(pointB.transform.position, 0.2f);
        Gizmos.DrawLine(pointA.transform.position, pointB.transform.position);
    }
}

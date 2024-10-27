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
            Vector2 direction = (currentPoint.position - transform.position).normalized;

            rb.velocity = direction * moveSpeed;

            if (Vector2.Distance(transform.position, currentPoint.position) < 0.1f)
            {
                currentPoint = currentPoint == pointB.transform ? pointA.transform : pointB.transform;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            canMove = false;
            rb.velocity = Vector2.zero;
            animator.SetTrigger("Blink");
            StartCoroutine(EnableMovement());
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
        Gizmos.DrawWireSphere(pointA.transform.position, 0.15f);
        Gizmos.DrawWireSphere(pointB.transform.position, 0.15f);
        Gizmos.DrawLine(pointA.transform.position, pointB.transform.position);
    }
}

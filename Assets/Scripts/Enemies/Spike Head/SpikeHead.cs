using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeHead : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private GameObject firstPatrolPoint, secondPatrolPoint;

    private Animator animator;
    private bool changedDirection;

    private void Start()
    {
        animator = GetComponent<Animator>();
        firstPatrolPoint = transform.GetChild(0).gameObject;
        secondPatrolPoint = transform.GetChild(1).gameObject;
    }

    private void Update()
    {
        if (changedDirection)
        {
            transform.position = Vector2.MoveTowards(transform.position, secondPatrolPoint.transform.position, 2 * Time.deltaTime);
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, firstPatrolPoint.transform.position, 2 * Time.deltaTime);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private bool gravityFlipped;

    private Animator anim;

    private bool isActive = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isActive)
        {
            anim.SetBool("Activated", true);

            isActive = true;

            GameManager.Instance.NewCheckpoint(transform.position, gravityFlipped);
        }
    }
}

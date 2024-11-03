using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private bool gravityFlipped;
    [SerializeField] private bool lastCheckpoint;

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

            if (lastCheckpoint)
            {
                GameManager.Instance.LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
            }

            isActive = true;

            if (!lastCheckpoint)
            {
                GameManager.Instance.NewCheckpoint(transform.position, gravityFlipped);
            }
        }
    }

    public void ActivateFlag()
    {
        isActive = true;
        anim.Play("CheckpointFlagIdle", 0, 1f);
    }

    public bool IsActivated() { return isActive; }
}

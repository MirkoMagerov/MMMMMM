using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
    public static event Action OnPlayerDeath;

    [SerializeField] private float waitOnDeathSeconds;

    private bool playerAlive = true;
    private Rigidbody2D rb;
    private Animator anim;
    private PlayerGravityController gravityController;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gravityController = GetComponent<PlayerGravityController>();
        anim = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Death>() && playerAlive) KillPlayer();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Death>() && playerAlive) KillPlayer();
    }

    void KillPlayer()
    {
        OnPlayerDeath?.Invoke();
        StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        PlayerManager.Instance.DisableMovementAndGravity();
        rb.velocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static;

        AudioManager.Instance.PlayGoofyDeathSound();

        playerAlive = false;

        anim.SetTrigger("dead");

        yield return new WaitForSeconds(waitOnDeathSeconds);

        gravityController.FlipGravity(GameManager.Instance.IsGravityFlipped());

        transform.position = GameManager.Instance.GetLastCheckpoint();

        anim.SetTrigger("respawn");
        yield return new WaitForSeconds(0.1f);

        playerAlive = true;
        rb.bodyType = RigidbodyType2D.Dynamic;

        PlayerManager.Instance.EnableMovementAndGravity();
    }

    public bool GetPlayerAlive() { return playerAlive; }
}

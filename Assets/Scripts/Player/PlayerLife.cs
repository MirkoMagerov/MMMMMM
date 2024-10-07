using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
    public bool playerAlive = true;

    [SerializeField] private float waitOnDeathSeconds;

    private Rigidbody2D rb;
    private Animator anim;
    private PlayerGravityController gravityController;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gravityController = GetComponent<PlayerGravityController>();
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Death>()) KillPlayer();
    }

    void KillPlayer()
    {
        StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        GetComponent<PlayerMovement>().SetCanMove(false);
        GetComponent<PlayerGravityController>().SetCanChangeGravity(false);

        anim.SetTrigger("dead");

        rb.velocity = Vector2.zero;

        yield return new WaitForSeconds(waitOnDeathSeconds);

        gravityController.FlipGravity(GameManager.Instance.IsGravityFlipped());

        transform.position = GameManager.Instance.GetLastCheckpoint();

        anim.SetTrigger("respawn");
        yield return new WaitForSeconds(0.1f);

        GetComponent<PlayerMovement>().SetCanMove(true);
        GetComponent<PlayerGravityController>().SetCanChangeGravity(true);
    }
}

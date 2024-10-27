using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class ShooterPlant : MonoBehaviour
{
    public enum ShootingDirection
    {
        Horizontal,
        Vertical
    }

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private ShootingDirection shootingDirection;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float shootingCooldown;
    [SerializeField] private bool leftDirection;
    [SerializeField] private bool upDirection;
    [SerializeField] private int stackSize;

    private Animator animator;
    private bool canShoot = true;
    private bool playerInRange = false;
    private Stack<GameObject> bulletStack;

    private void Start()
    {
        animator = GetComponent<Animator>();
        InitializeStack();
    }

    private void Update()
    {
        if (playerInRange && canShoot && PlayerReferenceManager.Instance.playerLife.GetPlayerAlive())
        {
            canShoot = false;
            animator.SetTrigger("Shoot");
            StartCoroutine(CooldownCoroutine());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    private IEnumerator CooldownCoroutine()
    {
        yield return new WaitForSeconds(shootingCooldown);
        canShoot = true;
    }

    public void ShootBullet()
    {
        GameObject bullet = GetBulletFromPool();
        if (bullet != null)
        {
            bullet.transform.position = bulletSpawnPoint.position;
            bullet.SetActive(true);

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.velocity = Vector2.zero;
            Vector2 direction = Vector2.zero;

            if (shootingDirection == ShootingDirection.Horizontal)
            {
                direction = leftDirection ? Vector2.left : Vector2.right;
            }
            else if (shootingDirection == ShootingDirection.Vertical)
            {
                direction = upDirection ? Vector2.up : Vector2.down;
            }

            rb.AddForce(direction * bulletSpeed, ForceMode2D.Impulse);
        }
    }

    private void InitializeStack()
    {
        bulletStack = new Stack<GameObject>();
        
        for (int i = 0; i < stackSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.transform.SetParent(transform);
            bullet.GetComponent<Bullet>().SetShooterPlant(this);
            bulletStack.Push(bullet);
            bullet.SetActive(false);
        }
    }

    private GameObject GetBulletFromPool()
    {
        if (bulletStack.Count > 0)
        {
            return bulletStack.Pop();
        }
        return null;
    }

    public void ReturnBulletToPool(GameObject bullet)
    {
        bulletStack.Push(bullet);
        bullet.SetActive(false);
    }
}

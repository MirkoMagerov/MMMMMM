using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private ShooterPlant shooterPlant;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        shooterPlant.ReturnBulletToPool(gameObject);
    }

    private void OnDisable()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    public void SetShooterPlant(ShooterPlant shooterPlant)
    {
        this.shooterPlant = shooterPlant;
    }
}

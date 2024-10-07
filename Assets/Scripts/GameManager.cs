using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private Vector3 lastActivatedCheckpoint;
    private bool gravityFlipped = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        } else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        lastActivatedCheckpoint = GameObject.FindGameObjectWithTag("Player").transform.position;
    }

    public void NewCheckpoint(Vector3 newCheckpoint, bool newGravityFlipped)
    {
        lastActivatedCheckpoint = newCheckpoint;
        gravityFlipped = newGravityFlipped;
    }

    public Vector3 GetLastCheckpoint() { return lastActivatedCheckpoint; }

    public bool IsGravityFlipped() { return gravityFlipped; }
}

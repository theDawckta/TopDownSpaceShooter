using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float Acceleration = 10.0f;
    public float MaxSpeed = 10.0f;
    public float RotateSpeed = 100.0f;
    [HideInInspector]
    public GameObject Player;

    private System.Random random = new System.Random(System.DateTime.Now.Ticks.GetHashCode());
    private Rigidbody2D enemyRigidbody;
    private int rotateDirection;

    void Start()
    {
        rotateDirection = (random.NextDouble() > 0.5) ? 1 : -1;
        enemyRigidbody = transform.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        float xVel = transform.InverseTransformDirection(enemyRigidbody.velocity).x;
        float yVel = transform.InverseTransformDirection(enemyRigidbody.velocity).y;
        if ((xVel + yVel) < MaxSpeed)
        {
            enemyRigidbody.AddForce(transform.up * Acceleration);
        }

        transform.Rotate(Vector3.forward * (Time.deltaTime * RotateSpeed * rotateDirection));
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("PlayerBullet"))
        {
            Destroy(gameObject);
        }
    }
}

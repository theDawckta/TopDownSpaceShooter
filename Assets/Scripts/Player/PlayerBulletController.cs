using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletController : MonoBehaviour
{
    public float Acceleration = 50.0f;
    public float MaxSpeed = 50.0f;
    public float Range = 10.0f;

    private Vector3 originalPosition;
    private Rigidbody2D playerBulletRigidbody;

    void Start()
    {
        originalPosition = transform.position;
        playerBulletRigidbody = transform.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        float xVel = transform.InverseTransformDirection(playerBulletRigidbody.velocity).x;
        float yVel = transform.InverseTransformDirection(playerBulletRigidbody.velocity).y;

        if ((xVel + yVel) < MaxSpeed)
        {
            playerBulletRigidbody.AddForce(transform.up * Acceleration);
        }

        if ((originalPosition - transform.position).magnitude > Range)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletController : MonoBehaviour
{
    public float Speed = 5.0f;
    public float Range = 10.0f;
    public ParticleSystem HitSparks;

    private Vector3 originalPosition;
    private Rigidbody2D playerBulletRigidbody;
    private MeshRenderer playerBulletMeshRenderer;

    void Start()
    {
        originalPosition = transform.position;
        playerBulletRigidbody = transform.GetComponent<Rigidbody2D>();
        playerBulletMeshRenderer = transform.GetComponent<MeshRenderer>();
        playerBulletRigidbody.AddForce(transform.up * Speed, ForceMode2D.Impulse);
    }

    void FixedUpdate()
    {
        if ((originalPosition - transform.position).magnitude > Range)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            playerBulletRigidbody.collisionDetectionMode = CollisionDetectionMode2D.Discrete;
            playerBulletMeshRenderer.enabled = false;
            gameObject.GetComponent<Collider2D>().enabled = false;
            
            HitSparks.Play();
            StartCoroutine("Dead");
        }
    }

    IEnumerator Dead()
    {
        yield return new WaitForSeconds(0.4f);
        Destroy(gameObject);
    }
}
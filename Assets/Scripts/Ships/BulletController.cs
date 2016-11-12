using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float Speed = 5.0f;
    public float Range = 10.0f;
    public ParticleSystem HitSparks;
    [HideInInspector]
    public GameObject Shooter;

    private Vector3 originalPosition;
    private Rigidbody2D bulletRigidbody;
    private MeshRenderer bulletMeshRenderer;

    void Start()
    {
        originalPosition = transform.position;
        bulletRigidbody = transform.GetComponent<Rigidbody2D>();
        bulletMeshRenderer = transform.GetComponent<MeshRenderer>();
        bulletRigidbody.AddForce(transform.up * Speed, ForceMode2D.Impulse);
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
        if (collision.collider.gameObject.layer != gameObject.layer)
        {
            bulletRigidbody.collisionDetectionMode = CollisionDetectionMode2D.Discrete;
            bulletMeshRenderer.enabled = false;
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

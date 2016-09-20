using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float Acceleration = 10.0f;
    public float MaxSpeed = 10.0f;
    public float RotateSpeed = 100.0f;
    public float GunCoolDown = 1.0f;
    public GameObject Nose;
    public GameObject PlayerBullet;

    private Rigidbody2D shipRigidbody;
    private bool firing = false;

    void Start()
    {
        shipRigidbody = transform.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        float xVel = transform.InverseTransformDirection(shipRigidbody.velocity).x;
        float yVel = transform.InverseTransformDirection(shipRigidbody.velocity).y;

        if ((xVel + yVel) < MaxSpeed)
        {
            if (Input.GetAxisRaw("Vertical") > 0)
            {
                shipRigidbody.AddForce(transform.up * Acceleration);
            }
            else if (Input.GetAxisRaw("Vertical") < 0)
            {
                shipRigidbody.AddForce(-transform.up * Acceleration);
            }
        }

        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            transform.Rotate(-Vector3.forward * (Time.deltaTime * RotateSpeed));
        }
        else if (Input.GetAxisRaw("Horizontal") < 0)
        {
            transform.Rotate(Vector3.forward * (Time.deltaTime * RotateSpeed));
        }
    }

    void Update()
    {
        if (Input.GetButton("Jump") && !firing)
        {
            StartCoroutine("FireGun");
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Destroy(gameObject);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    IEnumerator FireGun()
    {
        firing = true;
        GameObject playerBullet = Instantiate<GameObject>(PlayerBullet, Nose.transform.position, Nose.transform.rotation);
        playerBullet.transform.parent = transform.root;
        
        float timePassed = 0.0f;
        while ((timePassed / GunCoolDown) <= 1)
        {
            timePassed = timePassed + Time.deltaTime;
            yield return null;
        }

        firing = false;
    }
}
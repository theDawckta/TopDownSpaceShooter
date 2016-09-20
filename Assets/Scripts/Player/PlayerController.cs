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
    public float RollSpeed = 1.0f;
    public float TurnSpeed = 10.0f;
    public GameObject Nose;
    public GameObject PlayerBullet;

    private Rigidbody2D shipRigidbody;
    private bool firing = false;

    void Start()
    {
        shipRigidbody = transform.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetButton("Fire1") && !firing)
        {
            StartCoroutine("FireGun");
        }
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

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                                                                           Input.mousePosition.y,
                                                                          -Camera.main.transform.position.z));
	
		Quaternion rotate = Quaternion.FromToRotation(Vector3.up, mousePosition - transform.position);
        transform.localRotation = Quaternion.RotateTowards(transform.localRotation, rotate, TurnSpeed);
		transform.localEulerAngles = new Vector3(0.0f, 0.0f, transform.localEulerAngles.z);
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

        float timePassed = 0.0f;
        while ((timePassed / GunCoolDown) <= 1)
        {
            timePassed = timePassed + Time.deltaTime;
            yield return null;
        }

        firing = false;
    }
}
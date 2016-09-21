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
    public GameObject RollRotation;
    public GameObject Nose;
    public GameObject PlayerBullet;

    private Rigidbody2D playerRigidbody;
    private bool firing = false;

    void Start()
    {
        playerRigidbody = transform.GetComponent<Rigidbody2D>();
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
        Vector3 velocityAngle;
        Vector2 shipDirection;
        float direction;
        Quaternion rotate;
        Vector3 mousePosition;
        float xVel = transform.InverseTransformDirection(playerRigidbody.velocity).x;
        float yVel = transform.InverseTransformDirection(playerRigidbody.velocity).y;

        if ((xVel + yVel) < MaxSpeed)
        {
            if (Input.GetAxisRaw("Vertical") > 0)
            {
                playerRigidbody.AddForce(transform.up * Acceleration);
            }
            else if (Input.GetAxisRaw("Vertical") < 0)
            {
                playerRigidbody.AddForce(-transform.up * Acceleration);
            }
        }

        mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                                                                           Input.mousePosition.y,
                                                                          -Camera.main.transform.position.z));

        rotate = Quaternion.FromToRotation(Vector3.up, mousePosition - transform.position);
        transform.localRotation = Quaternion.RotateTowards(transform.localRotation, rotate, TurnSpeed);
        transform.localEulerAngles = new Vector3(0.0f, 0.0f, transform.localEulerAngles.z);

        velocityAngle = playerRigidbody.velocity.normalized;
        shipDirection = transform.position - mousePosition;
        direction = AngleFromAToB(velocityAngle, shipDirection);
        if ((direction > 0.0f && direction < 180.0f) && direction != 90.0f)
        {
            RollRotation.transform.localEulerAngles = new Vector3(0.0f, -(180 - Mathf.Abs(direction)), 0.0f);
        }
        else if ((direction < 0.0f && direction > -180.0f) && direction != 90.0f)
        {
            RollRotation.transform.localEulerAngles = new Vector3(0.0f, 180 - Mathf.Abs(direction), 0.0f);
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

    IEnumerator HandleRoll()
    {

        yield return null;
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

    float AngleFromAToB(Vector3 angleA, Vector3 angleB)
    {
        Vector3 axis = new Vector3(0, 0, 1);
        float angle = Vector3.Angle(angleA, angleB);
        float sign = Mathf.Sign(Vector3.Dot(axis, Vector3.Cross(angleA, angleB)));

        // angle in [-179,180]
        float signed_angle = angle * sign;
        return signed_angle;
    }
}
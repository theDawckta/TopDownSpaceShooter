using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float Acceleration = 10.0f;
    public float MaxSpeed = 10.0f;
    public float RotateSpeed = 100.0f;
    public GameObject RollRotation;
    [HideInInspector]
    public GameObject Player;
    [HideInInspector]
    public bool Alive = true;

    private System.Random random = new System.Random(System.DateTime.Now.Ticks.GetHashCode());
    private float turnInterval;
    private bool turnCycler = true;
    private Rigidbody2D enemyRigidbody;
    private int rotateDirection;

    void Start()
    {
        rotateDirection = (random.NextDouble() > 0.5) ? 1 : -1;
        enemyRigidbody = transform.GetComponent<Rigidbody2D>();
        turnInterval = random.Next(2, 4);
        InvokeRepeating("TurnTimer", 0.0f, turnInterval);
    }

    void FixedUpdate()
    {
        Vector3 velocityAngle;
        Vector2 shipDirection;
        float direction;
        float xVel = transform.InverseTransformDirection(enemyRigidbody.velocity).x;
        float yVel = transform.InverseTransformDirection(enemyRigidbody.velocity).y;
        if ((xVel + yVel) < MaxSpeed)
        {
            enemyRigidbody.AddForce(transform.up * Acceleration);
        }

        if(turnCycler)
            transform.Rotate(Vector3.forward * (Time.deltaTime * RotateSpeed * -rotateDirection));
        else
            transform.Rotate(Vector3.forward * (Time.deltaTime * RotateSpeed * rotateDirection));

        velocityAngle = enemyRigidbody.velocity.normalized;
        shipDirection = transform.up;
        direction = AngleFromAToB(velocityAngle, shipDirection);
        if ((direction > 0.0f && direction < 180.0f))
        {
            RollRotation.transform.localEulerAngles = new Vector3(0.0f, -(180 - Mathf.Abs(direction)), 0.0f);
        }
        else if ((direction < 0.0f && direction > -180.0f))
        {
            RollRotation.transform.localEulerAngles = new Vector3(0.0f, (180 - Mathf.Abs(direction)), 0.0f);
        }
    }

    void TurnTimer()
    {
        turnCycler = !turnCycler;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("PlayerBullet"))
        {
            Alive = false;
        }
    }

    float AngleFromAToB(Vector3 angleA, Vector3 angleB)
    {
        if (angleA == Vector3.zero || angleB == Vector3.zero)
            return 0.0f;
        Vector3 axis = new Vector3(0, 0, 1);
        float angle = Vector3.Angle(angleA, angleB);
        float sign = Mathf.Sign(Vector3.Dot(axis, Vector3.Cross(angleA, angleB)));

        // angle in [-179,180]
        float signed_angle = angle * sign;
        return signed_angle;
    }
}

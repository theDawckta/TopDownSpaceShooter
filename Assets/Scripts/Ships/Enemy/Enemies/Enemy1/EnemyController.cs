using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : StarShip
{
    [HideInInspector]
    public GameObject Player;

    private System.Random random = new System.Random(System.DateTime.Now.Ticks.GetHashCode());
    private float turnInterval;
    private bool turnCycler = true;
    private int rotateDirection;

    void Start()
    {
        rotateDirection = (random.NextDouble() > 0.5) ? 1 : -1;
        turnInterval = random.Next(2, 4);
        InvokeRepeating("TurnTimer", 0.0f, turnInterval);
        transform.LookAt(Player.transform);
        this.Target = Player.transform.position;
    }

    void FixedUpdate()
    {
        Vector3 distance;

        distance = this.Target - transform.position;

        if (distance.magnitude < 3)
        {
            this.MoveTarget(1.0f, Player.transform.position);
        }

        this.AddThrust(this.transform.up);
        if(turnCycler)
            this.AddThrust(this.transform.right);
        else
            this.AddThrust(-this.transform.right);
        base.FixedUpdate();
    }

    void TurnTimer()
    {
        turnCycler = !turnCycler;
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

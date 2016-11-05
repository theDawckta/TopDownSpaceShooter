using UnityEngine;
using System.Collections;

public class PatrolState : IEnemyState
{
    private readonly StatePatternEnemy enemy;
    private int nextWayPoint;

    public PatrolState(StatePatternEnemy statePatternEnemy)
    {
        enemy = statePatternEnemy;
    }

    public void UpdateState()
    {
        Look();
        Patrol();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            ToAlertState();
    }

    public void ToPatrolState()
    {
        Debug.Log("Can't transition to same state");
    }

    public void ToAlertState()
    {
        enemy.currentState = enemy.alertState;
    }

    public void ToChaseState()
    {
        enemy.currentState = enemy.chaseState;
    }

    private void Look()
    {
        RaycastHit2D hit = Physics2D.Raycast((Vector2)enemy.Eyes.transform.position, (Vector2)enemy.Eyes.transform.forward, 20); Debug.Log(hit);
        if (hit.collider)
        {
            Debug.Log("hit");
            enemy.chaseTarget = hit.transform;
            ToChaseState();
        }
        Debug.DrawRay(enemy.Eyes.transform.position, enemy.Eyes.transform.forward * enemy.SightRange, Color.red);
    }

    void Patrol()
    {
        //enemy.enemyRigidbody.AddForce(enemy.transform.up * enemy.Acceleration);
        enemy.MeshRendererFlag.material.color = Color.green;

        Vector3 distance;
        Vector3 velocityAngle;
        Vector2 shipDirection;
        float direction;
        Quaternion rotate;
        float xVel = enemy.transform.InverseTransformDirection(enemy.enemyRigidbody.velocity).x;
        float yVel = enemy.transform.InverseTransformDirection(enemy.enemyRigidbody.velocity).y;

        if ((xVel + yVel) < enemy.MaxSpeed)
        {
            if (Input.GetAxisRaw("Vertical") > 0)
            {
                enemy.enemyRigidbody.AddForce(enemy.transform.up * enemy.Acceleration);
            }
            else if (Input.GetAxisRaw("Vertical") < 0)
            {
                enemy.enemyRigidbody.AddForce(-enemy.transform.up * enemy.Acceleration);
            }
        }

        rotate = Quaternion.FromToRotation(Vector3.up, enemy.WayPoints[nextWayPoint].position - enemy.transform.position);
        enemy.transform.localRotation = Quaternion.RotateTowards(enemy.transform.localRotation, rotate, enemy.TurnSpeed);
        enemy.transform.localEulerAngles = new Vector3(0.0f, 0.0f, enemy.transform.localEulerAngles.z);

        velocityAngle = enemy.enemyRigidbody.velocity.normalized;
        shipDirection = enemy.transform.position - enemy.WayPoints[nextWayPoint].position;
        direction = AngleFromAToB(velocityAngle, shipDirection);
        if ((direction > 0.0f && direction < 180.0f))
        {
            enemy.RollRotation.transform.localEulerAngles = new Vector3(0.0f, -(180 - Mathf.Abs(direction)), 0.0f);
        }
        else if ((direction < 0.0f && direction > -180.0f))
        {
            enemy.RollRotation.transform.localEulerAngles = new Vector3(0.0f, (180 - Mathf.Abs(direction)), 0.0f);
        }

        Debug.Log("direction: " + direction + "     velocityAngle" + velocityAngle + "     angle:" + enemy.RollRotation.transform.localEulerAngles.y);

        distance = enemy.WayPoints[nextWayPoint].position - enemy.transform.position;
        Debug.Log(distance);
        if (distance.magnitude < 2)
        {
            nextWayPoint = (nextWayPoint + 1) % enemy.WayPoints.Length;
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
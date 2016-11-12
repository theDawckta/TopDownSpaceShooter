using UnityEngine;
using System.Collections;

public class PatrolState : IEnemyState
{
    public float PatrolDistance;

    private readonly StatePatternEnemy enemy;
    private int currentWayPoint;

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
    }

    public void ToPatrolState()
    {
        Debug.Log("Can't transition to same state");
    }

    public void ToEvadeState()
    {

    }

    public void ToAttackState()
    {
        enemy.currentState = enemy.attackState;
    }

    public void ToChaseState()
    {
        enemy.currentState = enemy.chaseState;
    }

    private void Look()
    {
        RaycastHit2D hit = Physics2D.Raycast((Vector2)enemy.Eyes.transform.position, (Vector2)enemy.Eyes.transform.forward, 20);
       
        if (hit)
        {
			if(hit.collider.tag == "Player")
			{
	            enemy.chaseTarget = hit.transform;
	            ToChaseState();
            }
        }
        Debug.DrawRay(enemy.Eyes.transform.position, enemy.Eyes.transform.forward * enemy.SightRange, Color.red);
    }

    void Patrol()
    {
        Vector3 distance;

		distance = enemy.WayPoints[currentWayPoint] - enemy.transform.position;
        if (distance.magnitude < 4)
        {
            enemy.WayPoints[currentWayPoint] = enemy.Player.transform.position + (Random.insideUnitSphere * PatrolDistance);
            enemy.Target.transform.position = enemy.WayPoints[currentWayPoint];
        }

        enemy.AddThrust(enemy.transform.up);
		enemy.MeshRendererFlag.material.color = Color.green;
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
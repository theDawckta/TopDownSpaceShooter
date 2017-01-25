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

    void Patrol()
    {
        Vector3 distance;

		distance =  enemy.WayPoint - enemy.transform.position;
        if (distance.magnitude < 4)
        {
            enemy.WayPoint = enemy.WayPoint + (Random.insideUnitSphere * PatrolDistance);
        }

        enemy.AddThrust(enemy.transform.up);
		enemy.MeshRendererFlag.material.color = Color.green;
    }
}
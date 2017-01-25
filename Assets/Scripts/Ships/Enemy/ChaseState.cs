using UnityEngine;
using System.Collections;

public class ChaseState : IEnemyState
{

    private readonly StatePatternEnemy enemy;


    public ChaseState(StatePatternEnemy statePatternEnemy)
    {
        enemy = statePatternEnemy;
    }

    public void UpdateState()
    {
        Chase();
    }

    public void OnTriggerEnter(Collider other)
    {

    }

    public void ToPatrolState()
    {

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

    }

    private void Chase()
    {
        Vector3 distance;
        
        distance = enemy.Target - enemy.transform.position;
        if(distance.magnitude > 20)
        {
            enemy.AddThrust(enemy.transform.up);
        }
        else
        {
            ToAttackState();
        }
		Debug.DrawLine(enemy.chaseTarget.position, enemy.Target, Color.green);
        enemy.MeshRendererFlag.material.color = Color.yellow;
    }
}
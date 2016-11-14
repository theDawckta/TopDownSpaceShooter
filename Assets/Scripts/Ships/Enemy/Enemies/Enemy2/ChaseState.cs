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
        Look();
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

    private void Look()
    {
        //RaycastHit hit;
        //Vector3 enemyToTarget = (enemy.chaseTarget.position + enemy.Offset) - enemy.Eyes.transform.position;
        //if (Physics.Raycast(enemy.Eyes.transform.position, enemyToTarget, out hit, enemy.SightRange) && hit.collider.CompareTag("Player"))
        //{
        //    enemy.chaseTarget = hit.transform;

        //}
        //else
        //{
        //    ToAlertState();
        //}

    }

    private void Chase()
    {
        Vector3 distance;

        enemy.WayPoints.Clear();
        enemy.Target = enemy.Player.transform.position;
        distance = enemy.Player.gameObject.transform.position - enemy.transform.position;
        Debug.Log(distance.magnitude);
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
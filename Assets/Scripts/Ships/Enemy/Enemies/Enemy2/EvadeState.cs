using UnityEngine;
using System.Collections;

public class EvadeState : IEnemyState
{
    public float EvadeTime = 2.0f;
    private float timePassed = 0.0f;
    private readonly StatePatternEnemy enemy;

    public EvadeState(StatePatternEnemy statePatternEnemy)
    {
        enemy = statePatternEnemy;
    }

    public void UpdateState()
    {
        Look();
        Evade();
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

    private void Evade()
    {
        enemy.MoveTarget(EvadeTime, Quaternion.Euler(0, -45, 0) * enemy.Target.transform.position);
        if(EvadeTime < timePassed)
        {
            enemy.AddThrust(enemy.transform.up);
        }  
        else
        {
            ToAttackState();
            timePassed = 0.0f;
        }

        enemy.MeshRendererFlag.material.color = Color.white;
    }
}
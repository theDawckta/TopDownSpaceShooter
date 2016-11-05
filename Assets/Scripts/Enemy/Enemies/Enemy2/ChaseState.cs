﻿using UnityEngine;
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

    public void ToAlertState()
    {
        enemy.currentState = enemy.alertState;
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
        enemy.MeshRendererFlag.material.color = Color.red;
    }


}
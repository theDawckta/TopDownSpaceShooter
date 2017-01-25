using UnityEngine;
using System.Collections;

public class AttackState : IEnemyState
{
    private readonly StatePatternEnemy enemy;

    public AttackState(StatePatternEnemy statePatternEnemy)
    {
        enemy = statePatternEnemy;
    }

    public void UpdateState()
    {
        Attack();
    }

    public void OnTriggerEnter(Collider other)
    {

    }

    public void ToPatrolState()
    {
        enemy.currentState = enemy.patrolState;
    }

    public void ToEvadeState()
    {
        enemy.currentState = enemy.evadeState;
    }

    public void ToAttackState()
    {
        Debug.Log("Can't transition to same state");
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
            if (hit.collider.tag == "Enemy")
            {
                ToEvadeState();
            }
        }
    }

    private void Attack()
    {
        Vector3 distance;

        distance = enemy.Target - enemy.transform.position;
        if (distance.magnitude < 22)
        {
            enemy.FireGun();
        }
        else
        {
            ToChaseState();
        }

        enemy.MeshRendererFlag.material.color = Color.red;
    }
}
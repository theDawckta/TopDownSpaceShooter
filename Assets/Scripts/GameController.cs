using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour 
{
    private EnemySpawnController _enemySpawnController;
    private DropController _dropController;

    void Awake()
    {
        _enemySpawnController = gameObject.GetComponent<EnemySpawnController>();
        _dropController = gameObject.GetComponent<DropController>();
    }

    void Start ()
    {
        _enemySpawnController.OnEnemyStarShipDeathEvent += _enemySpawnController_OnEnemyStarShipDeathEvent;
	}

    private void _enemySpawnController_OnEnemyStarShipDeathEvent(StarShip deadEnemy)
    {
        _dropController.MakeDrop(deadEnemy.transform.position);
    }

    void Update () 
    {
		
	}
}

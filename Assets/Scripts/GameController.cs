using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour 
{
    public PlayerController Player;
    public UIController UI;

    private EnemySpawnController _enemySpawnController;
    private DropController _dropController;

    void Awake()
    {
        _enemySpawnController = gameObject.GetComponent<EnemySpawnController>();
        _dropController = gameObject.GetComponent<DropController>();
        
    }

    void Update () 
    {
		
	}

    public void StarButtonClicked()
    {
        UI.GameOn();
        _enemySpawnController.StartSpawn();
        Player.ShipCollider.enabled = true;
    }

    void PlayerDied(StarShip ship)
    {
        _enemySpawnController.EndSpawn();
        _dropController.RemoveAllDrops();
        UI.GameOff();
    }

    private void EnemyDied(StarShip deadEnemy)
    {
        _dropController.MakeDrop(deadEnemy.transform.position);
    }

    void OnEnable()
    {
        _enemySpawnController.OnEnemyStarShipDeathEvent += EnemyDied;
        Player.OnDeath += PlayerDied;
    }

    void OnDisable()
    {
        _enemySpawnController.OnEnemyStarShipDeathEvent += EnemyDied;
        Player.OnDeath -= PlayerDied;
    }
}

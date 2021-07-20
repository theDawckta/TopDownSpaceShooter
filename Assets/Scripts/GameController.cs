using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour 
{
    public PlayerController Player;
    public UIController UI;
    public Enemy2 Enemy2;

    private EnemySpawnController _enemySpawnController;
    private DropController _dropController;

    void Awake()
    {
        _enemySpawnController = gameObject.GetComponent<EnemySpawnController>();
        _dropController = gameObject.GetComponent<DropController>();
        
    }

    void Start()
    {
        if (Enemy2 != null)
            Enemy2.gameObject.SetActive(true);
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

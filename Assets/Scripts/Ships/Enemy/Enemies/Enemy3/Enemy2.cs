using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy2 : StarShip
{
    public StarShip CurrentTarget { get { return _currentTarget; } private set { } }

    private List<StarShip> _players = new List<StarShip>();
    private StarShip _currentTarget;

    protected override void Awake()
    {
        GameObject[] tempPlayers = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < tempPlayers.Length; i++)
        {
            _players.Add(tempPlayers[i].GetComponent<StarShip>());
        }
        if (_players.Count == 1)
        {
            _currentTarget = _players[0];
        }

        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    void EnemyDied(StarShip ship)
    {

    }

    public void OnEnable()
    {
        base.OnDeath += EnemyDied;
    }

    public void OnDisable()
    {
        base.OnDeath -= EnemyDied;
    }
}

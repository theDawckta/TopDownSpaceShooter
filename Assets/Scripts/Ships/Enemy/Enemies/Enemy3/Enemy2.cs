using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy2 : StarShip
{
    public GameObject TestTarget;
    public Animator Enemy2Animator { get { return _enemy2Animator; } private set { } }
    public NavMeshAgent Enemy2NavMeshAgent { get { return _navMeshAgent; } private set { } }
    public StarShip CurrentTarget { get { return _currentTarget; } private set { } }

    private List<StarShip> _players = new List<StarShip>();
    private StarShip _currentTarget;
    private Animator _enemy2Animator;
    private NavMeshAgent _navMeshAgent;

    protected override void Awake()
    {
        _enemy2Animator = GetComponent<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();

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

    public void GotoTarget(Vector3 position)
    {
        _navMeshAgent.SetDestination(position);
        TestTarget.transform.parent = null;
        TestTarget.transform.position = position;
    }

    public void ArrivedAtTarget()
    {
        TestTarget.transform.SetParent(transform);
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

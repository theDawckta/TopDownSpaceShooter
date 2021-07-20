using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy2 : StarShip
{
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

    protected override void OnTriggerEnter(Collider other)
    {
        if(other.transform == StarShipDestTarget.transform)
        {
            _navMeshAgent.ResetPath();
            StarShipDestTarget.SetActive(false);
            StarShipDestTarget.transform.SetParent(transform, false);
            StarShipDestTarget.transform.localScale = Vector3.one;
            _enemy2Animator.SetTrigger("ArrivedAtPathEnd");
            //Debug.Log("ARRIVED AT END OF PATH");
        }

        base.OnTriggerEnter(other);
    }

    public void GotoTarget(Vector3 newPosition)
    {
        Vector3 yNormalizedPosition = new Vector3(newPosition.x, transform.position.y, newPosition.z);

        _navMeshAgent.SetDestination(yNormalizedPosition);
        StarShipDestTarget.transform.parent = null;
        StarShipDestTarget.transform.position = yNormalizedPosition;
        StarShipDestTarget.SetActive(true);
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

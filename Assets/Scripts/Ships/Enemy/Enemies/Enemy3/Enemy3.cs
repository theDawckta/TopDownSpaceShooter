using System.Collections.Generic;
using UnityEngine;

public class Enemy3 : StarShip
{
    private List<StarShip> _players = new List<StarShip>();
    private StarShip _playerTarget;
    private FSMSystem _fsm;

    protected override void Awake()
    {
        GameObject[] tempPlayers = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < tempPlayers.Length; i++)
        {
            _players.Add(tempPlayers[i].GetComponent<StarShip>());
        }
        if(_players.Count == 1)
        {
            _playerTarget = _players[0];
        }
        base.Awake();
    }

    protected override void Start()
    {
        MakeFSM();
        base.Start();
    }

    protected override void FixedUpdate()
    {
        _fsm.CurrentState.Reason(_playerTarget, this);
        _fsm.CurrentState.Act(_playerTarget, this);
        base.FixedUpdate();
    }

    void EnemyDied(StarShip ship)
    {

    }

    public void SetTransition(Transition t)
    {
        _fsm.PerformTransition(t);
    }

    // The NPC has two states: ChasePlayer and AttackPlayer
    // If it's on the first state and SawPlayer transition is fired, it changes to ChasePlayer
    // If it's on ChasePlayerState and LostPlayer transition is fired, it returns to FollowPath
    private void MakeFSM()
    {
        ChasePlayerState chase = new ChasePlayerState();
        chase.AddTransition(Transition.PlayerInRange, StateID.AttackPlayer);

        AttackPlayerState attack = new AttackPlayerState();
        attack.AddTransition(Transition.PlayerOutOfRange, StateID.ChasePlayer);
        attack.AddTransition(Transition.PlayerInsideOfRange, StateID.RunFromPlayer);

        RunFromPlayerState run = new RunFromPlayerState();
        run.AddTransition(Transition.EnemySafe, StateID.ChasePlayer);


        _fsm = new FSMSystem();
        _fsm.AddState(chase);
        _fsm.AddState(attack);
        _fsm.AddState(run);
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
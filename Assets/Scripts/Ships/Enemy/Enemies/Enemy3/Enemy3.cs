using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Enemy3 : StarShip
{
    public Transform[] Path;
    private FSMSystem _fsm;

    public void SetTransition(Transition t) { _fsm.PerformTransition(t); }

    protected override void Start()
    {
        MakeFSM();
        base.Start();
    }

    protected override void FixedUpdate()
    {
        _fsm.CurrentState.Reason(Target, StarShip);
        _fsm.CurrentState.Act(Target, StarShip);
    }

    // The NPC has two states: FollowPath and ChasePlayer
    // If it's on the first state and SawPlayer transition is fired, it changes to ChasePlayer
    // If it's on ChasePlayerState and LostPlayer transition is fired, it returns to FollowPath
    private void MakeFSM()
    {
        FollowPathState follow = new FollowPathState(Path);
        follow.AddTransition(Transition.SawPlayer, StateID.ChasingPlayer);

        ChasePlayerState chase = new ChasePlayerState();
        chase.AddTransition(Transition.LostPlayer, StateID.FollowingPath);

        _fsm = new FSMSystem();
        _fsm.AddState(follow);
        _fsm.AddState(chase);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunFromPlayerState : FSMState
{
    public RunFromPlayerState()
    {
        stateID = StateID.RunFromPlayer;
    }

    public override void Reason(StarShip player, StarShip npc)
    {
        if (Vector3.Distance(npc.transform.position, player.transform.position) >= 40)
            npc.GetComponent<Enemy3>().SetTransition(Transition.EnemySafe);
    }

    public override void Act(StarShip player, StarShip npc)
    {
        Debug.Log("Running");
        npc.StarShipTarget.transform.position = npc.transform.up + (npc.transform.position - player.transform.position);
        npc.AddThrust(npc.transform.up);
    }
}
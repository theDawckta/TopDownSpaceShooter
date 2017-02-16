using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPlayerState : FSMState
{
    public AttackPlayerState()
    {
        stateID = StateID.AttackPlayer;
    }

    public override void Reason(StarShip player, StarShip npc)
    {
        if (Vector3.Distance(npc.transform.position, player.transform.position) >= 60)
            npc.GetComponent<Enemy3>().SetTransition(Transition.PlayerOutOfRange);
        if (Vector3.Distance(npc.transform.position, player.transform.position) <= 30)
            npc.GetComponent<Enemy3>().SetTransition(Transition.PlayerInsideOfRange);
    }

    public override void Act(StarShip player, StarShip npc)
    {
        Debug.Log("Attacking");
        npc.StarShipTarget.transform.position = player.transform.position;
        npc.FireGun();
    }
}
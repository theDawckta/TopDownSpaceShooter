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
        // If the player has gone 30 meters away from the NPC, fire PlayerOutOfRange transition
        if (Vector3.Distance(npc.transform.position, player.transform.position) >= 30)
            npc.GetComponent<Enemy3>().SetTransition(Transition.PlayerOutOfRange);
    }

    public override void Act(StarShip player, StarShip npc)
    {
        Debug.Log("Attacking");
        npc.StarShipTarget.transform.position = player.transform.position;
        npc.FireGun();
    }
}
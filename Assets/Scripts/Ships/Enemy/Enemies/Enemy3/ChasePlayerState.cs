using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasePlayerState : FSMState
{
    public ChasePlayerState()
    {
        stateID = StateID.ChasePlayer;
    }

    public override void Reason(StarShip player, StarShip npc)
    {
        // If the player is within 30 meters away from the NPC, fire PlayerInRange transition
        Debug.Log(Vector3.Distance(npc.transform.position, player.transform.position));
        if (Vector3.Distance(npc.transform.position, player.transform.position) <= 30)
            npc.GetComponent<Enemy3>().SetTransition(Transition.PlayerInRange);
    }

    public override void Act(StarShip player, StarShip npc)
    {
        Debug.Log("Chasing");
        npc.StarShipTarget.transform.position = player.transform.position + (player.transform.up * player.shipRigidbody.velocity.magnitude);
        npc.AddThrust(npc.transform.up);
    }
}

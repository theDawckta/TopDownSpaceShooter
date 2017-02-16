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
        Debug.Log(Vector3.Distance(npc.transform.position, player.transform.position));
        if (Vector3.Distance(npc.transform.position, player.transform.position) <= 40)
            npc.GetComponent<Enemy3>().SetTransition(Transition.PlayerInRange);
    }

    public override void Act(StarShip player, StarShip npc)
    {
        Debug.Log("Chasing");
        npc.StarShipTarget.transform.position = player.transform.position + (player.transform.up * player.ShipRigidbody.velocity.magnitude);
        npc.AddThrust(npc.transform.up);
    }
}

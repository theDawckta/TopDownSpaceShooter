using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPathState : FSMState
{
    private int currentWayPoint;
    private Transform[] waypoints;

    public FollowPathState(Transform[] wp)
    {
        waypoints = wp;
        currentWayPoint = 0;
        stateID = StateID.FollowingPath;
    }

    public override void Reason(GameObject player, GameObject npc)
    {
        // If the Player passes less than 15 meters away in front of the NPC
        //RaycastHit hit;
        //if (Physics.Raycast(npc.transform.position, npc.transform.forward, out hit, 15F))
        //{
        //    if (hit.transform.gameObject.tag == "Player")
        //        npc.GetComponent<Enemy3>().SetTransition(Transition.SawPlayer);
        //}
    }

    public override void Act(GameObject player, StarShip npc)
    {
        // Follow the path of waypoints
        // Find the direction of the current way point 
        Vector3 vel = npc.GetComponent<Rigidbody2D>().velocity;
        Vector3 moveDir = waypoints[currentWayPoint].position - npc.transform.position;

        if (moveDir.magnitude < 1)
        {
            currentWayPoint++;
            if (currentWayPoint >= waypoints.Length)
            {
                currentWayPoint = 0;
            }
            npc.MoveTarget(1.0f, waypoints[currentWayPoint].position);
        }
        else
        {
            vel = moveDir.normalized * 10;

            // Rotate towards the waypoint
            npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation,
                                                      Quaternion.LookRotation(moveDir),
                                                      5 * Time.deltaTime);
            npc.transform.eulerAngles = new Vector3(0, npc.transform.eulerAngles.y, 0);

        }

        // Apply the Velocity
        npc.GetComponent<Rigidbody2D>().velocity = vel;
    }

} // FollowPathState

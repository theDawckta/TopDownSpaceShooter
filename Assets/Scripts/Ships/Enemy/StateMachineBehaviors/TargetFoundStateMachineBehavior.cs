using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFoundStateMachineBehavior : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Enemy2 enemy = animator.GetComponent<Enemy2>();

        if (enemy != null && enemy.CurrentTarget != null)
        {
            RaycastHit hit;
            bool hitOccured = Physics.Raycast(enemy.transform.position, enemy.CurrentTarget.transform.position - enemy.transform.position, out hit, Mathf.Infinity);
            
            // Does the ray intersect any objects excluding the player layer
            if (hitOccured && hit.transform != enemy.CurrentTarget.transform)
            {
                animator.SetTrigger("TargetLost");
                Debug.DrawRay(enemy.transform.position, enemy.CurrentTarget.transform.position - enemy.transform.position * hit.distance, Color.yellow, 10f);
                Debug.Log("LOST LINE OF SITE");
            }
            else
            {
                Debug.DrawRay(enemy.transform.position, enemy.CurrentTarget.transform.position - enemy.transform.position * 1000, Color.white, 10f);
                Debug.Log("HAS LINE OF SITE");
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}

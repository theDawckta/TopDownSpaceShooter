using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleStateMachineBehavior : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Enemy2 enemy = animator.GetComponent<Enemy2>();

        if (enemy != null)
        {
            Debug.Log("ENEMY3 DISTANCE TO CURRENT TARGET: " + Vector3.Distance(enemy.transform.position, enemy.CurrentTarget.transform.position));
            if (Vector3.Distance(enemy.transform.position, enemy.CurrentTarget.transform.position) >= 20)
                animator.SetTrigger("TargetLost");
            if (Vector3.Distance(enemy.transform.position, enemy.CurrentTarget.transform.position) < 20)
                animator.SetTrigger("TargetFound");
        }
        else
        {
            Debug.LogError("missing enemy in IdleStateMachine");
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

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
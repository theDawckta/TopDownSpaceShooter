using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchStateMachineBehavior : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Enemy2 enemy = animator.GetComponent<Enemy2>();

        if (enemy != null && enemy.CurrentTarget != null)
        {
            enemy.GotoTarget(enemy.CurrentTarget.transform.position);
        }
        else
        {
            Debug.LogError("missing enemy or enemy StarShipTarget in SearchStateMachine");
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Enemy2 enemy = animator.GetComponent<Enemy2>();

        if (!enemy.Enemy2NavMeshAgent.pathPending)
        {
            if (enemy.Enemy2NavMeshAgent.remainingDistance <= enemy.Enemy2NavMeshAgent.stoppingDistance)
            {
                if (!enemy.Enemy2NavMeshAgent.hasPath || enemy.Enemy2NavMeshAgent.velocity.sqrMagnitude == 0f)
                {
                    Debug.Log("ARRIVED AT END OF PATH");
                    enemy.ArrivedAtTarget();
                    enemy.Enemy2Animator.SetTrigger("ArrivedAtPathEnd");
                }
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

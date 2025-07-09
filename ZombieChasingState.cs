using UnityEngine;
using UnityEngine.AI;

public class ZombieChasingState : StateMachineBehaviour
{
    private GameObject player;
    private NavMeshAgent navAgent;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player");
        navAgent = animator.GetComponent<NavMeshAgent>();

    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        navAgent.SetDestination(player.transform.position);

        float distanceBetweenPlayerAndZombie = Vector3.Distance(player.transform.position, animator.transform.position);
        Debug.Log("distanceBetweenPlayerAndZombie is " + distanceBetweenPlayerAndZombie);

        if(distanceBetweenPlayerAndZombie <= Enemy.attackDistance)
        {
            animator.SetBool("ISATTACKING", true);
        }

        if(distanceBetweenPlayerAndZombie > Enemy.stopChasingPlayerDistance)
        {
            animator.SetBool("ISCHASING", false);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        navAgent.SetDestination(animator.transform.position);
    }
}

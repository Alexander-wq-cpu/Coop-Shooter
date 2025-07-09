using UnityEngine;
using UnityEngine.AI;

public class ZombieAttackingState : StateMachineBehaviour
{
    private GameObject player;
    private NavMeshAgent navAgent;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player");
        navAgent = animator.GetComponent<NavMeshAgent>();

        //navAgent.updateRotation = false;
        animator.applyRootMotion = false;
        navAgent.SetDestination(animator.transform.position);
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        LookAtPlayer();

        float distanceBetweenPlayerAndZombie = Vector3.Distance(player.transform.position, animator.transform.position);
        if (distanceBetweenPlayerAndZombie > Enemy.attackDistance)
        {
            animator.SetBool("ISATTACKING", false);
        }
    }

    private void LookAtPlayer()
    {
        Vector3 direction = player.transform.position - navAgent.transform.position;
        navAgent.transform.rotation = Quaternion.LookRotation(direction);

        float yRotation = navAgent.transform.eulerAngles.y;
        navAgent.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //navAgent.updateRotation = true;
        animator.applyRootMotion = true;
    }
}

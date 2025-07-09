using UnityEngine;
using UnityEngine.AI;

public class ZombieIdleState : StateMachineBehaviour
{
    private GameObject player;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float distanceBetweenPlayerAndZombie = Vector3.Distance(player.transform.position, animator.transform.position);

        if(distanceBetweenPlayerAndZombie < Enemy.detectionRadius)
        {
            animator.SetBool("ISCHASING", true);
        }
    }

}

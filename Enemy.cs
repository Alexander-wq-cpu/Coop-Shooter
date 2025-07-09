using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int health;
    public static int enemyDamage = 10;
    Animator animator;
    private NavMeshAgent navAgent;
    private bool isDead = false;
    public static int detectionRadius = 10;
    public static float attackDistance = 3.7f;
    public static float stopChasingPlayerDistance = 11;
    

    private void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }
    public void TakeDamage(int amount)
    {
        if (isDead == false)
        {
            health -= amount;

            if (health <= 0)
            {
                animator.SetTrigger("ISDEAD");
                isDead = true;
                /*Collider[] colliders = GetComponentsInChildren<Collider>();
                foreach(Collider col in colliders)
                {
                    col.enabled = false;
                }*/
            }
            else
            {
                animator.SetTrigger("ISHIT");
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color  = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, stopChasingPlayerDistance);
    }
}

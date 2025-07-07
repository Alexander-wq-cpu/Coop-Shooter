using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int health;
    Animator animator;
    private NavMeshAgent navAgent;
    private bool isDead = false;
    public static int detectionRadius = 10;
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

    private void Update()
    {
        if(navAgent.velocity.magnitude > 0.1f)
        {
            animator.SetBool("ISWALKING", true);
        }
        else
        {
            animator.SetBool("ISWALKING", false);
        }
    }

    private void OnDrawGizmos()
    {
        Color color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}

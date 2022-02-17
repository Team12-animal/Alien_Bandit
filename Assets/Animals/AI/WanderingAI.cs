using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class WanderingAI : MonoBehaviour
{

    public float wanderRadius;  //漫步範圍
    public float wanderTimer;   //漫步時間

    private Vector3 targerPoint;  //目標點
    private Transform target;  //目標位置
    private NavMeshAgent agent;  
    private float timer;  //經過時間
    private Animator animator;  

    // Use this for initialization
    void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        timer = wanderTimer;
    }

    private void Start()
    {
        targerPoint = RandomNavSphere(transform.position, wanderRadius, -1);
        StartCoroutine("StartWander");
    }

    // Update is called once per frame
    void Update()
    {
        ChangeAnimatiom();
        WanderAngin();
    }

    IEnumerator StartWander()
    {
        yield return new WaitForSeconds(8f);
        agent.SetDestination(targerPoint);
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, wanderRadius);
    }

    void ChangeAnimatiom()
    {
        if (agent.remainingDistance == 0)
        {
            animator.SetInteger("State", 0);
        }
        else
        {
            animator.SetInteger("State", 1);
            animator.applyRootMotion = false;
        }
    }

    void WanderAngin()
    {
        if (transform.position == targerPoint)
        {
            timer += Time.deltaTime;

            if (timer >= wanderTimer)
            {
                targerPoint = RandomNavSphere(transform.position, wanderRadius, -1);
                agent.SetDestination(targerPoint);
                timer = 0;
            }
        }
    }
}

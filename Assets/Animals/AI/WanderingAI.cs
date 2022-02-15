using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class WanderingAI : MonoBehaviour
{

    public float wanderRadius;  //���B�d��
    public float wanderTimer;   //���B�ɶ�

    private Vector3 targerPoint;  //�ؼ��I
    private Transform target;  //�ؼЦ�m
    private NavMeshAgent agent;  
    private float timer;  //�g�L�ɶ�
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

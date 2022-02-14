using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class WanderingAI : MonoBehaviour
{

    public float wanderRadius;
    public float wanderTimer;

    private Vector3 targerPoint;
    private Transform target;
    private NavMeshAgent agent;
    private float timer;

    // Use this for initialization
    void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer;
    }

    private void Start()
    {
        targerPoint = RandomNavSphere(transform.position, wanderRadius, -1);
        agent.SetDestination(targerPoint);
    }

    // Update is called once per frame
    void Update()
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
        Gizmos.DrawWireSphere(transform.position,wanderRadius);
    }
}

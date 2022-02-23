using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ElephantAI : MonoBehaviour
{
    public enum ElephantState
    {
        Idle,
        Wander
    }

    private ElephantState currentState;
    private float currentTime;
    private float stateTime;
    private GameObject currentTarget;
    private Vector3 currentPos;
    private Animator animator;
    private NavMeshAgent agent;
    private Quaternion rotate;

    void Start()
    {
        currentState = ElephantState.Idle;
        currentTarget = null;
        currentPos = new Vector3(0, 0, 0);
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
    }
    void Update()
    {
        switch (currentState)
        {
            case ElephantState.Idle:
                IdleState();
                break;

            case ElephantState.Wander:
                WanderState();
                break;
        }
    }

    private void IdleState()
    {

        stateTime = Random.Range(3f, 5f);
        currentTime += Time.deltaTime;
        if (currentTime > stateTime)
        {
            currentTime = 0;
            currentState = ElephantState.Wander;
            animator.SetInteger("State", 1);
            currentPos = RandomNavSphere(transform.position, 50, -1);
            agent.SetDestination(currentPos);
            Vector3 dist =  currentPos - transform.position;
            rotate = Quaternion.LookRotation(dist);
        }
    }
    private void WanderState()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, rotate, 0.001f);
        float dist = (transform.position - currentPos).magnitude;

        if (dist < 1f)
        {
            currentTime = 0;
            currentState = ElephantState.Idle;
            animator.SetInteger("State", 0);
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
}

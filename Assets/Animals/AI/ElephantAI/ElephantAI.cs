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
    [SerializeField]
    private RabbitAIData data;
    private float currentTime;
    private float stateTime;
    private GameObject currentTarget;
    private Animator animator;
    private Rigidbody currentRigi;
    void Start()
    {
        currentRigi = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
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

        stateTime = Random.Range(3f,5f);
        currentTime += Time.deltaTime;
        if (currentTime > stateTime)
        {
            currentTime = 0;
            currentState = ElephantState.Wander;
            animator.SetInteger("State", 1);
            data.m_vTarget = RandomNavSphere(transform.position, data.m_fSight, -1);
            Debug.LogError(data.m_vTarget);
        }
    }
    private void WanderState()
    {
        data.agent.SetDestination(data.m_vTarget);
        float dist = (transform.position - data.m_vTarget).magnitude;

        if (dist < 1f)
        {
            currentTime = 0;
            currentState = ElephantState.Idle;
            animator.SetInteger("State",0);
        }
    }

    private Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {

        Vector3 randDirection = Random.insideUnitSphere * (dist + 1);

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }
}

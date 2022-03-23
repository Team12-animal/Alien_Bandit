using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BornState : IState
{
    private RabbitFSM manager;

    private FSMData data;


    public BornState(RabbitFSM manager)
    {
        this.manager = manager;
        this.data = manager.data;
    }

    public void OnEnter()
    {

    }

    public void OnUpdate()
    {
        if (data.animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            manager.TransitionState(StateType.Idle);
        }
    }

    public void OnExit()
    {
        data.animator.applyRootMotion = false;
    }
}

public class IdleState : IState
{
    private RabbitFSM manager;

    private FSMData data;

    private float timer;

    public IdleState(RabbitFSM manager)
    {
        this.manager = manager;
        this.data = manager.data;
    }

    public void OnEnter()
    {
        data.animator.SetInteger("State", 0);
        data.IdleTime = Random.Range(0.5f, 3f);
    }

    public void OnUpdate()
    {
        timer += Time.deltaTime;
        if (timer > data.IdleTime)
        {
            manager.TransitionState(StateType.Wander);
        }
    }

    public void OnExit()
    {
        timer = 0;
    }


}
public class WanderState : IState
{
    private RabbitFSM manager;

    private FSMData data;

    private float timer;

    private float dist;

    public WanderState(RabbitFSM manager)
    {
        this.manager = manager;
        this.data = manager.data;
    }

    public void OnEnter()
    {
        data.animator.SetInteger("State", 1);
        data.WanderTime = Random.Range(4.0f, 6.0f);
        data.TargetPoint = RandomNavSphere(manager.currentPos, data.Sight, -1);
    }
    public void OnUpdate()
    {
        data.agent.SetDestination(data.TargetPoint);

        dist = (data.TargetPoint - manager.currentPos).magnitude;
        timer += Time.deltaTime;
        if (dist<0.3f || timer > data.WanderTime)
        {
            manager.TransitionState(StateType.Idle);
        }
    }

    public void OnExit()
    {
        timer = 0;
        data.agent.SetDestination(manager.currentPos);
    }

    public Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {

        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        if (navHit.distance == Mathf.Infinity)
        {
            return manager.gameObject.transform.position;
        }
        return navHit.position;
    }
}
public class ChaseState : IState
{
    private RabbitFSM manager;

    private FSMData data;

    public ChaseState(RabbitFSM manager)
    {
        this.manager = manager;
        this.data = manager.data;
    }

    public void OnEnter()
    {

    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {

    }
}
public class AttackState : IState
{
    private RabbitFSM manager;

    private FSMData data;

    public AttackState(RabbitFSM manager)
    {
        this.manager = manager;
        this.data = manager.data;
    }

    public void OnEnter()
    {

    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {

    }
}
public class GoHomeState : IState
{
    private RabbitFSM manager;

    private FSMData data;

    public GoHomeState(RabbitFSM manager)
    {
        this.manager = manager;
        this.data = manager.data;
    }

    public void OnEnter()
    {

    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {

    }
}

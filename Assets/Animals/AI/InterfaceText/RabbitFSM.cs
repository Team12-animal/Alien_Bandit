using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum StateType
{
    Born,
    Idle,
    Wander,
    Chase,
    Attack,
    GoHome
}
public class RabbitFSM : MonoBehaviour
{
    //當前狀態
    private IState currentState;
    //所有狀態聲明
    private Dictionary<StateType, IState> states = new Dictionary<StateType, IState>();

    public FSMData data;

    public Vector3 currentPos;
    void Start()
    {
        states.Add(StateType.Born, new BornState(this));
        states.Add(StateType.Idle, new IdleState(this));
        states.Add(StateType.Wander, new WanderState(this));
        states.Add(StateType.Chase, new ChaseState(this));
        states.Add(StateType.Attack, new AttackState(this));
        states.Add(StateType.GoHome, new GoHomeState(this));

        TransitionState(StateType.Born);

        data.animator = GetComponent<Animator>();
        data.agent = GetComponent<NavMeshAgent>();
        currentPos = transform.position;
    }

    void Update()
    {
        currentPos = transform.position;
        currentState.OnUpdate();
    }

    public void TransitionState(StateType type)
    {
        if (currentState != null)
        {
            currentState.OnExit();
        }
        currentState = states[type];
        currentState.OnEnter();
    }

    private void OnDrawGizmos()
    {

    }

}

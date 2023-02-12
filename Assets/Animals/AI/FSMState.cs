using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum eFSMTransition
{
    NullTransition = 0,
    Go_Idle,  //待命
    Go_Wander,  //漫步
    Go_Chase,  //被吸引
    Go_Attack,  //攻擊
    Go_Dead,  //死亡
    Go_Runaway,  //逃跑
   Go_MoveTo, //移動到
}


public enum eFSMStateID
{
    NullStateID = 0,
    IdleStateID,
    WanderStateID,
    ChaseStateID,
    AttackStateID,
    DeadStateID,
    RunawayStateID,
    MoveToStateID,
}

public class FSMState
{
    public eFSMStateID m_StateID;  //目前狀態
    public Dictionary<eFSMTransition, FSMState> m_Map;  
    public float m_fCurrentTime;  

    public FSMState()
    {
        m_StateID = eFSMStateID.NullStateID;
        m_fCurrentTime = 0.0f;
        m_Map = new Dictionary<eFSMTransition, FSMState>();
    }
    
    /// <summary>
    /// 狀態流向
    /// </summary>
    /// <param name="trans"></param>
    /// <param name="toState"></param>
    public void AddTransition(eFSMTransition trans, FSMState toState)
    {
        if(m_Map.ContainsKey(trans))
        {
            return;
        }

        m_Map.Add(trans, toState);
    }
    public void DelTransition(eFSMTransition trans)
    {
        if (m_Map.ContainsKey(trans))
        {
            m_Map.Remove(trans);
        }

    }
    /// <summary>
    /// 要做的下個狀態
    /// </summary>
    /// <param name="trans"></param>
    /// <returns></returns>
    public FSMState TransitionTo(eFSMTransition trans)
    {
        if (m_Map.ContainsKey(trans) == false)
        {
            return null;
        }
        return m_Map[trans];
    }

    public virtual void DoBeforeEnter(RabbitAIData data)
    {

    }

    public virtual void DoBeforeLeave(RabbitAIData data)
    {

    }

    public virtual void Do(RabbitAIData data)
    {

    }

    public virtual void CheckCondition(RabbitAIData data)
    {
        
    }
}


public class FSMIdleState : FSMState
{

    private float m_fIdleTim;
  

    public FSMIdleState()
    {
        m_StateID = eFSMStateID.IdleStateID;
        m_fIdleTim = Random.Range(1.0f, 3.0f);
        
    }


    public override void DoBeforeEnter(RabbitAIData data)
    {
        m_fCurrentTime = 0.0f;
        m_fIdleTim = Random.Range(1.0f, 3.0f);
    }

    public override void DoBeforeLeave(RabbitAIData data)
    {

    }

    public override void Do(RabbitAIData data)
    {
        m_fCurrentTime += Time.deltaTime;
    }

    public override void CheckCondition(RabbitAIData data)
    {
        bool bAttack = false;
        GameObject go = RabbitAIFunction.CheckEnemyInSight(data, ref bAttack);
        if (go != null)
        {
            data.m_TargetObject = go;
            if (bAttack)
            {
                data.m_FSMSystem.PerformTransition(eFSMTransition.Go_Attack);
            }
            else
            {
                data.m_FSMSystem.PerformTransition(eFSMTransition.Go_Chase);
            }
            return;
        }
        if (m_fCurrentTime > m_fIdleTim)
        {
            

            data.m_FSMSystem.PerformTransition(eFSMTransition.Go_MoveTo);
        }
    }
}

public class FSMMoveToState : FSMState
{
    private int m_iCurrentWanderPt;
    private GameObject[] m_WanderPoints;
    public FSMMoveToState()
    {
        m_StateID = eFSMStateID.MoveToStateID;
        m_iCurrentWanderPt = -1;
        m_WanderPoints = GameObject.FindGameObjectsWithTag("WanderPoint");
    }


    public override void DoBeforeEnter(RabbitAIData data)
    {
        int iNewPt = Random.Range(0, m_WanderPoints.Length);
        if (m_iCurrentWanderPt == iNewPt)
        {
            return;
        }
        m_iCurrentWanderPt = iNewPt;
        data.m_vTarget = m_WanderPoints[m_iCurrentWanderPt].transform.position;
        data.m_bMove = true;
    }

    public override void DoBeforeLeave(RabbitAIData data)
    {

    }

    public override void Do(RabbitAIData data)
    {
        if (SteeringBehavior.CollisionAvoid(data) == false)
        {
            SteeringBehavior.Seek(data);
        }

        SteeringBehavior.Move(data);
    }

    public override void CheckCondition(RabbitAIData data)
    {
        bool bAttack = false;
        GameObject go = RabbitAIFunction.CheckEnemyInSight(data, ref bAttack);
        if (go != null)
        {
            data.m_TargetObject = go;
            if (bAttack)
            {
                data.m_FSMSystem.PerformTransition(eFSMTransition.Go_Attack);
            }
            else
            {
                data.m_FSMSystem.PerformTransition(eFSMTransition.Go_Chase);
            }
            return;
        }

        if (data.m_bMove == false)
        {
            data.m_FSMSystem.PerformTransition(eFSMTransition.Go_Idle);
        }
    }
}

public class FSMChaseState : FSMState
{
    public FSMChaseState()
    {
        m_StateID = eFSMStateID.ChaseStateID;
    }


    public override void DoBeforeEnter(RabbitAIData data)
    {

    }

    public override void DoBeforeLeave(RabbitAIData data)
    {

    }

    public override void Do(RabbitAIData data)
    {
        data.m_vTarget = data.m_TargetObject.transform.position;
        if (SteeringBehavior.CollisionAvoid(data) == false)
        {
            SteeringBehavior.Seek(data);
        }

        SteeringBehavior.Move(data);
    }

    public override void CheckCondition(RabbitAIData data)
    {
        bool bAttack = false;
        bool bCheck = RabbitAIFunction.CheckTargetEnemyInSight(data, data.m_TargetObject, ref bAttack);
        if (bCheck == false)
        {
            data.m_FSMSystem.PerformTransition(eFSMTransition.Go_Idle);
            return;
        }
        if (bAttack)
        {
            data.m_FSMSystem.PerformTransition(eFSMTransition.Go_Attack);
        }
    }
}


public class FSMAttackState : FSMState
{
    private float fAttackTime = 0.0f;
    public FSMAttackState()
    {
        m_StateID = eFSMStateID.AttackStateID;
    }


    public override void DoBeforeEnter(RabbitAIData data)
    {
        fAttackTime = Random.Range(1.0f, 3.0f);
        m_fCurrentTime = 0.0f;
    }

    public override void DoBeforeLeave(RabbitAIData data)
    {

    }


    public override void Do(RabbitAIData data)
    {
        // Check Animation complete.
        //...

        if (m_fCurrentTime > fAttackTime)
        {
            m_fCurrentTime = 0.0f;
            // Do attack.
        }
        m_fCurrentTime += Time.deltaTime;
    }

    public override void CheckCondition(RabbitAIData data)
    {
        bool bAttack = false;
        bool bCheck = RabbitAIFunction.CheckTargetEnemyInSight(data, data.m_TargetObject, ref bAttack);
        if (bCheck == false)
        {
            data.m_FSMSystem.PerformTransition(eFSMTransition.Go_Idle);
            return;
        }
        if (bAttack == false)
        {
            data.m_FSMSystem.PerformTransition(eFSMTransition.Go_Chase);
            return;
        }
    }
}

public class FSMWander : FSMState
{
    private float wanderTimer = 5f;
    private Vector3 targerPoint;
    private float timer;

    public FSMWander()
    {
        m_StateID = eFSMStateID.WanderStateID;
    }

    public override void DoBeforeEnter(RabbitAIData data)
    {
        wanderTimer = Random.Range(3.0f, 5.0f);
        m_fCurrentTime = 0.0f;
        targerPoint = RandomNavSphere(data.m_vCurrentVector, data.m_fSight, -1);
    }

    public override void DoBeforeLeave(RabbitAIData data)
    {

    }

    public override void Do(RabbitAIData data)
    {
        data.agent.SetDestination(targerPoint);
        timer += Time.deltaTime;

        if (timer >= wanderTimer)
        {
            targerPoint = RandomNavSphere(data.m_vCurrentVector, data.m_fSight, -1);
            data.agent.SetDestination(targerPoint);
            timer = 0;
        }
    }

    public override void CheckCondition(RabbitAIData data)
    {

    }
    public  Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }

}

public class FSMDeadState : FSMState
{
    public FSMDeadState()
    {
        m_StateID = eFSMStateID.DeadStateID;
    }


    public override void DoBeforeEnter(RabbitAIData data)
    {

    }

    public override void DoBeforeLeave(RabbitAIData data)
    {

    }

    public override void Do(RabbitAIData data)
    {
        Debug.Log("Do Dead State");
    }

    public override void CheckCondition(RabbitAIData data)
    {

    }
}
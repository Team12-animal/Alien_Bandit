using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RaccoonBadyAI : MonoBehaviour
{
    enum CurrentState
    {
        Idle,
        Walk,
        Run,
        Attack
    }
    private CurrentState currentState;
    public RabbitAIData m_Data;         //AI資料
    private float m_fCurrentTime;       //當前狀態經過時間
    private float m_fIdleTime;          //狀態時間
    private Animator m_Am;              //AI的動畫狀態機
    private List<GameObject> players;
    Vector3 lastPos;

    // Use this for initialization
    public void Start()
    {
        currentState = CurrentState.Idle;
        m_fCurrentTime = 0.0f;
        m_fIdleTime = Random.Range(0.5f, 3.0f);
        m_Am = GetComponent<Animator>();
        players = AIMain.m_Instance.GetPlayerList();
    }

    /// <summary>
    /// 檢查玩家是否進入AI的視線範圍內
    /// </summary>
    /// <param name="bAttack">當玩家進入警戒範圍</param>
    /// <returns>進入視線範圍最近的玩家</returns>
    private GameObject CheckEnemyInSight(ref bool bAttack)
    {
        float currentDist = m_Data.m_fSight + 1f;
        GameObject play = null;
        foreach (var v in players) //所有玩家和AI距離
        {
            float dist = (v.transform.position - m_Data.m_Go.transform.position).magnitude;
            if (currentDist > dist)
            {
                currentDist = dist;
                play = v;
            }
        }
        if (currentDist < m_Data.m_fAttackRange)  //如果小於警戒範圍
        {
            bAttack = true;
            return play;
        }
        else if (currentDist < m_Data.m_fSight)  //如果進入視線範圍
        {
            bAttack = false;
            return play;
        }
        return null;
    }

    /// <summary>
    /// 檢查物件是否還在視線範圍內
    /// </summary>
    /// <param name="target">目標物件</param>
    /// <param name="bAttack">是否進入警戒</param>
    /// <returns>返回是否還在視線內</returns>
    private bool CheckTargetEnemyInSight(GameObject target, ref bool bAttack)
    {
        GameObject go = target;
        Vector3 v = go.transform.position - this.transform.position;
        float fDist = v.magnitude;
        if (fDist < m_Data.m_fAttackRange)
        {
            bAttack = true;
            return true;
        }
        else if (fDist < m_Data.m_fSight)
        {
            bAttack = false;
            return true;
        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        m_Data.arriveDist = m_Data.m_Speed + 0.001f;
        //Debug.LogError("Current State " + m_eCurrentState);  //印出當前狀態
        if (currentState == CurrentState.Idle)
        {
            m_Am.SetInteger("State", 0);
            // Wait to move.           
            if (m_fCurrentTime > m_fIdleTime)  //當當前經過時間大於停留時間，進入漫步
            {
                m_Data.agent.enabled = true;
                m_fCurrentTime = 0.0f;
                m_fIdleTime = 0.5f;
                m_Data.m_vTarget = RandomNavSphere(transform.position, m_Data.m_fSight, -1);  //在視野範圍內隨機位置
                currentState = CurrentState.Walk;
                m_Am.applyRootMotion = false;
                lastPos = transform.position;
            }
            else
            {
                m_fCurrentTime += Time.deltaTime;
            }
        }
        else if (currentState == CurrentState.Walk)
        {
            m_fIdleTime = Random.Range(3.0f, 6.0f);  //漫步停留時間為隨機3～4秒
            if (!(lastPos == transform.position))
            {
                m_Am.SetInteger("State", 1);
            }
            lastPos = transform.position;
            m_Data.agent.enabled = true;
            m_Data.agent.updateRotation = true;
            m_Data.agent.SetDestination(m_Data.m_vTarget);  //AI移動到隨機目標點
            Vector3 newPos = (m_Data.m_vTarget - transform.position); //到目標點的向量
            float dis = newPos.magnitude;  //距離長度
            if (dis < 0.3f || (m_fCurrentTime > m_fIdleTime))  //若小於0.1f便回到IDLE狀態 (到達) Or 若超過停留時間便中斷並進入IDLE狀態 (未到達)
            {
                m_Data.agent.updateRotation = false;
                m_Data.agent.SetDestination(transform.position);  //將位置調整為當前位置(避免平移)
                currentState = CurrentState.Idle;
                m_fCurrentTime = 0.0f;
                m_fIdleTime = Random.Range(1.0f, 3.0f);
                m_Data.m_bMove = false;
                m_Am.SetInteger("State", 0);
            }
            else
            {
                m_fCurrentTime += Time.deltaTime;
            }
        }
        else if (currentState == CurrentState.Attack)
        {
        }
    }

    public Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {

        Vector3 randDirection = Random.insideUnitSphere * (dist + 5);

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        if (navHit.distance == Mathf.Infinity)
        {
            NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);
        }
        return navHit.position;
    }

    private void OnDrawGizmos()
    {
        if (m_Data == null)
        {
            return;
        }
        Gizmos.color = Color.black;
        Gizmos.DrawLine(this.transform.position, this.transform.position + this.transform.forward * 2.0f);
        if (currentState == CurrentState.Idle)
        {
            Gizmos.color = Color.white;
        }
        else if (currentState == CurrentState.Walk)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(this.transform.position, m_Data.m_vTarget);
        }
        else if (currentState == CurrentState.Run)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(this.transform.position, m_Data.m_vTarget);
        }
    }
}

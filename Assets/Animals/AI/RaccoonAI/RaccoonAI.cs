using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RaccoonAI : MonoBehaviour
{
    public enum eFSMState
    {
        Idle,        //狀態機State:0
        Wander,      //狀態機State:1
        Chase,       //狀態機State:2
        MoveToTarget,//狀態機State:2
    }

    private eFSMState m_eCurrentState;      //當前狀態
    public RabbitAIData m_Data;             //AI資料
    private float m_fCurrentTime;           //當前狀態經過時間
    private float m_fIdleTime;              //狀態時間
    private GameObject m_CurrentEnemyTarget;//當前偵測目標
    private GameObject m_WanderPoints;    //兔子窩的位置物件
    private Animator m_Am;                  //AI的動畫狀態機
    private List<GameObject> players;
    private Vector3 lastPos;
    public Collider currentCollider;
    public SkinnedMeshRenderer skinnedMesh;

    public void Start()
    {
        m_CurrentEnemyTarget = null;
        m_eCurrentState = eFSMState.Idle;
        m_fCurrentTime = 0.0f;
        m_fIdleTime = Random.Range(0.5f, 3.0f);
        m_Am = GetComponent<Animator>();
        players = AIMain.m_Instance.GetPlayerList();
        m_WanderPoints = GameObject.Find("HousePoint");
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
        else if (m_eCurrentState == eFSMState.Chase && fDist < m_Data.m_fSight + 1.5f)
        {
            bAttack = false;
            return true;
        }
        return false;
    }


    private void CheckPlayerInSight()
    {
        bool bAttack = false;
        m_CurrentEnemyTarget = CheckEnemyInSight(ref bAttack);
        if (m_CurrentEnemyTarget != null) //偵測範圍內有玩家
        {
            m_Data.m_TargetObject = m_CurrentEnemyTarget;
            if (bAttack)  //在警戒範圍內
            {
                m_Data.agent.enabled = true;
                m_Data.agent.updateRotation = true;
                m_Data.m_vTarget = m_WanderPoints.transform.position;
                m_Am.SetInteger("State", 3);
                m_eCurrentState = eFSMState.MoveToTarget;
            }
            else
            {
                m_Data.agent.enabled = false;
                m_Data.m_fMaxSpeed = 0.1f;
                m_eCurrentState = eFSMState.Chase;   //逃跑
                m_Am.SetInteger("State", 3);
                m_Data.m_vTarget = m_Data.m_TargetObject.transform.position;
                if (SteeringBehavior.CollisionAvoid(m_Data) == false)
                {
                    SteeringBehavior.Flee2(m_Data);
                }
                SteeringBehavior.Move(m_Data);
            }
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        m_Data.arriveDist = m_Data.m_Speed + 0.001f;
        if (m_Data.isBited || m_Data.isCatched)
        {
            m_Am.SetInteger("State", 0);
            m_eCurrentState = eFSMState.Idle;
            m_Data.agent.enabled = false;
            currentCollider.enabled = false;
            this.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);

            if (m_Data.isCatched
                )
            {
                this.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
            }
            else
            {
                this.transform.localPosition = new Vector3(0.092f, 0.0f, -0.412f);
            }

            if (m_Data.isCatched)
            {
                skinnedMesh.enabled = false;
            }
        }
        else
        {
            skinnedMesh.enabled = true;
            currentCollider.enabled = true;
            //Debug.LogError("Current State " + m_eCurrentState);  //印出當前狀態
            if (m_eCurrentState == eFSMState.Idle)
            {
                m_Data.m_fMaxSpeed = 0.05f;
                m_Am.SetInteger("State", 0);
                CheckPlayerInSight();

                // Wait to move.           
                if (m_fCurrentTime > m_fIdleTime)  //當當前經過時間大於停留時間，進入漫步
                {
                    m_Data.agent.enabled = true;
                    m_fCurrentTime = 0.0f;
                    m_fIdleTime = 0.5f;
                    m_Data.m_vTarget = RandomNavSphere(m_WanderPoints.transform.position, m_Data.m_fSight, -1);  //在視野範圍內隨機位置
                    m_eCurrentState = eFSMState.Wander;
                    m_Am.applyRootMotion = false;
                    lastPos = transform.position;
                }
                else
                {
                    m_fCurrentTime += Time.deltaTime;
                }
            }
            else if (m_eCurrentState == eFSMState.Wander)
            {
                m_fIdleTime = Random.Range(4.0f, 5.0f);  //漫步停留時間為隨機3～4秒
                CheckPlayerInSight();
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
                    m_eCurrentState = eFSMState.Idle;
                    m_fCurrentTime = 0.0f;
                    m_fIdleTime = Random.Range(1.0f, 3.0f);
                    m_Data.m_bMove = false;
                }
                else
                {
                    m_fCurrentTime += Time.deltaTime;
                }
            }
            else if (m_eCurrentState == eFSMState.MoveToTarget)
            {
                m_Am.SetInteger("State", 2);
                m_Data.agent.SetDestination(m_Data.m_vTarget);
                m_Data.agent.speed = 5f;
                Vector3 newPos = (m_Data.m_vTarget - transform.position);
                float dis = newPos.magnitude;
                if (dis < 1.5f)
                {
                    m_Am.SetInteger("State", 0);
                    m_eCurrentState = eFSMState.Idle;
                    m_fCurrentTime = 0.0f;
                    m_fIdleTime = Random.Range(2.0f, 3.0f);
                    return;
                }
            }
            else if (m_eCurrentState == eFSMState.Chase)
            {
                bool bAttack = false;
                bool bCheck = CheckTargetEnemyInSight(m_CurrentEnemyTarget, ref bAttack);
                m_Data.m_fMaxSpeed = 0.1f;
                m_Data.agent.enabled = false;
                if (bCheck == false)
                {
                    m_Am.SetInteger("State", 0);
                    m_eCurrentState = eFSMState.Idle;
                    m_fCurrentTime = 0.0f;
                    m_fIdleTime = Random.Range(2.0f, 3.0f);
                    return;
                }
                if (bAttack)
                {
                    m_Data.agent.enabled = true;
                    m_Data.agent.updateRotation = true;
                    m_Data.m_vTarget = m_WanderPoints.transform.position;
                    m_Am.SetInteger("State", 2);
                    m_eCurrentState = eFSMState.MoveToTarget;
                }
                else
                {
                    m_Data.m_vTarget = m_Data.m_TargetObject.transform.position;
                    if (SteeringBehavior.CollisionAvoid(m_Data) == false)
                    {
                        SteeringBehavior.Flee2(m_Data);
                    }
                    SteeringBehavior.Move(m_Data);
                    m_Am.SetInteger("State", 2);
                }

            }
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
        if (m_eCurrentState == eFSMState.Idle)
        {
            Gizmos.color = Color.white;
        }
        else if (m_eCurrentState == eFSMState.MoveToTarget)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(this.transform.position, m_Data.m_vTarget);
        }
        else if (m_eCurrentState == eFSMState.Chase)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(this.transform.position, m_Data.m_vTarget);
        }
        else if (m_eCurrentState == eFSMState.Wander)
        {
            Gizmos.color = Color.cyan;
        }

        Gizmos.DrawWireSphere(this.transform.position, m_Data.m_fSight);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, m_Data.m_fAttackRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(this.transform.position, m_Data.m_fRadius);
        Gizmos.DrawLine(this.transform.position, this.transform.position + this.transform.forward * this.m_Data.m_fProbeLength);
        Gizmos.color = Color.yellow;
        Vector3 vLeftStart = this.transform.position - this.transform.right * m_Data.m_fRadius;
        Vector3 vLeftEnd = vLeftStart + this.transform.forward * m_Data.m_fProbeLength;
        Gizmos.DrawLine(vLeftStart, vLeftEnd);
        Vector3 vRightStart = this.transform.position + this.transform.right * m_Data.m_fRadius;
        Vector3 vRightEnd = vRightStart + this.transform.forward * m_Data.m_fProbeLength;
        Gizmos.DrawLine(vRightStart, vRightEnd);
        Gizmos.DrawLine(vLeftEnd, vRightEnd);

    }


}

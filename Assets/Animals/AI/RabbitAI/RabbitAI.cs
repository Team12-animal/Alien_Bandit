using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RabbitAI : MonoBehaviour
{
    public enum eFSMState
    {
        NONE = -1,
        Idle,  //狀態機State:0
        Wander,//狀態機State:1
        Attack,//狀態機State:2
        Chase,//狀態機State:3
        MoveToTarget,//狀態機State:3 到兔子洞State:4
        Runaway,
        Dead,
    }

    private eFSMState m_eCurrentState;  //當前狀態
    public RabbitAIData m_Data;         //AI資料
    private float m_fCurrentTime;       //當前狀態經過時間
    private float m_fIdleTime;          //狀態時間
    private GameObject m_CurrentEnemyTarget;//當前偵測目標
    private GameObject[] m_WanderPoints;//兔子窩的位置物件
    private Animator m_Am;              //AI的動畫狀態機
    private List<GameObject> players;
    private GameObject attackWood;
    // Use this for initialization
    public void Start()
    {
        m_CurrentEnemyTarget = null;
        m_eCurrentState = eFSMState.Idle;
        m_fCurrentTime = 0.0f;
        m_fIdleTime = Random.Range(3.0f, 5.0f);
        m_WanderPoints = GameObject.FindGameObjectsWithTag("WanderPoint");  //找到場景上的所有兔子窩
        m_Am = GetComponent<Animator>();
        players = AIMain.m_Instance.GetPlayerList();
    }

    /// <summary>
    /// 檢查離兔子最近的那個兔子窩
    /// </summary>
    /// <returns>返回離兔子最近的那個兔子窩</returns>
    private GameObject CheckCloseHole()
    {
        List<float> diss = new List<float>();  //存放距離的清單
        int index = 0;
        foreach (var v in m_WanderPoints) //所有兔子窩和AI距離
        {
            Vector3 dis = v.transform.position - m_Data.m_Go.transform.position;  //兔子和兔子窩距離位置
            diss.Add(dis.magnitude);  //距離長度
        }
        if (diss.Count != 1)  //如果數量為一的話不用查找
        {
            for (int i = 0; i < diss.Count - 1; i++)  //找出距離最近得並排序到最後一個
            {
                float temp = 0;
                GameObject gtemp = null;
                if (diss[i] < diss[i + 1])
                {
                    temp = diss[i];
                    diss[i] = diss[i + 1];
                    diss[i + 1] = temp;

                    gtemp = m_WanderPoints[i];
                    m_WanderPoints[i] = m_WanderPoints[i + 1];
                    m_WanderPoints[i + 1] = gtemp;
                }
            }
            index = diss.Count - 1;  //最後一個陣列的參數(總數量減一)
            return m_WanderPoints[index];
        }
        return m_WanderPoints[0];
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
                m_Data.m_vTarget = CheckCloseHole().transform.position;
                m_Am.SetInteger("State", 3);
                m_eCurrentState = eFSMState.MoveToTarget;
            }
            else
            {
                m_Data.agent.enabled = false;
                m_eCurrentState = eFSMState.Chase;   //逃跑
                m_Am.SetInteger("State", 3);
                m_Data.m_vTarget = m_Data.m_TargetObject.transform.position;
                if (SteeringBehavior.CollisionAvoid(m_Data) == false)
                {
                    SteeringBehavior.Flee(m_Data);
                }
                SteeringBehavior.Move(m_Data);
            }
            return;
        }
    }
    // Update is called once per frame
    void Update()
    {
        //Debug.LogError("Current State " + m_eCurrentState);  //印出當前狀態
        if (m_eCurrentState == eFSMState.Idle)
        {

            if (attackWood != null)
            {
                m_eCurrentState = eFSMState.Attack;
                return;
            }
            m_Data.m_fMaxSpeed = 0.09f;
            m_Am.SetInteger("State", 0);
            CheckPlayerInSight();
            // Wait to move.           
            if (m_fCurrentTime > m_fIdleTime)  //當當前經過時間大於停留時間，進入漫步
            {
                m_Data.agent.enabled = true;
                m_fCurrentTime = 0.0f;
                m_fIdleTime = 0.5f;
                m_Data.m_vTarget = RandomNavSphere(transform.position, m_Data.m_fSight, -1);  //在視野範圍內隨機位置
                m_eCurrentState = eFSMState.Wander;
                m_Am.applyRootMotion = false;
                m_Am.SetInteger("State", 1);
            }
            else
            {
                m_fCurrentTime += Time.deltaTime;
            }
        }
        else if (m_eCurrentState == eFSMState.Wander)
        {
            if (attackWood != null)
            {
                m_eCurrentState = eFSMState.Attack;
                return;
            }
            m_fIdleTime = Random.Range(2.0f, 3.0f);  //漫步停留時間為隨機3～4秒
            CheckPlayerInSight();
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
                m_fIdleTime = Random.Range(2.0f, 3.0f);
                m_Data.m_bMove = false;

            }
            else
            {
                m_fCurrentTime += Time.deltaTime;
            }
        }
        else if (m_eCurrentState == eFSMState.MoveToTarget)
        {
            m_Am.SetInteger("State", 3);
            m_Data.agent.SetDestination(m_Data.m_vTarget);
            m_Data.agent.speed = 5f;
            Vector3 newPos = (m_Data.m_vTarget - transform.position);
            float dis = newPos.magnitude;
            if (dis < 1.5f)
            {
                m_Am.applyRootMotion = true;
                m_Am.SetInteger("State", 4);
            }
        }
        else if (m_eCurrentState == eFSMState.Chase)
        {
            bool bAttack = false;
            bool bCheck = CheckTargetEnemyInSight(m_CurrentEnemyTarget, ref bAttack);

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
                m_Data.m_vTarget = CheckCloseHole().transform.position;
                m_Am.SetInteger("State", 3);
                m_eCurrentState = eFSMState.MoveToTarget;
            }
            else
            {
                m_Data.m_vTarget = m_Data.m_TargetObject.transform.position;
                if (SteeringBehavior.CollisionAvoid(m_Data) == false)
                {
                    SteeringBehavior.Flee(m_Data);
                }
                SteeringBehavior.Move(m_Data);
                m_Am.SetInteger("State", 3);
            }

        }
        else if (m_eCurrentState == eFSMState.Attack)
        {
            CheckPlayerInSight();
            if (attackWood == null)
            {
                m_eCurrentState = eFSMState.Idle;
                return;
            }
            else
            {
                m_Data.m_fMaxSpeed = 0.01f;
                transform.rotation = Quaternion.Lerp(this.transform.rotation, attackWood.transform.rotation, 0.05f);
                float dist = (transform.position - m_Data.m_vTarget).magnitude;
                if (dist < 1.5f)
                {
                    transform.forward = attackWood.transform.position;
                    m_Am.SetInteger("State", 2);
                }
                else
                {
                    m_Data.m_vTarget = attackWood.transform.position - new Vector3(0f, 0f, 1f);
                    SteeringBehavior.Seek(m_Data);
                    SteeringBehavior.Move(m_Data);
                }
            }
        }
    }

    public Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {

        Vector3 randDirection = Random.insideUnitSphere * (dist + 1);

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        if(navHit.distance == Mathf.Infinity)
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
        else if (m_eCurrentState == eFSMState.Attack)
        {
            Gizmos.color = Color.magenta;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Wood")
        {
            if (m_eCurrentState == eFSMState.Wander || m_eCurrentState == eFSMState.Idle)
            {
                attackWood = other.gameObject;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Wood")
        {
            attackWood = null;
        }
    }

    public void EnterHole()
    {
        AIMain.m_Instance.RemoveRabbit(this.gameObject);
    }
}

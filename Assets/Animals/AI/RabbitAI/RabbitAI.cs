using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RabbitAI : MonoBehaviour
{
    public enum eFSMState
    {
        NONE = -1,
        Idle,
        Wander,
        Chase,
        Attack,
        Dead,
        Runaway,
        MoveToTarget,
    }

    private eFSMState m_eCurrentState;
    public RabbitAIData m_Data;
    private float m_fCurrentTime;
    private float m_fIdleTime;
    private GameObject m_CurrentEnemyTarget;
    private int m_iCurrentWanderPt;
    private GameObject[] m_WanderPoints;
    private Animator m_Am;

    // Use this for initialization
    public void Start()
    {
        m_CurrentEnemyTarget = null;
        m_eCurrentState = eFSMState.Idle;
        m_fCurrentTime = 0.0f;
        m_fIdleTime = Random.Range(5.0f, 8.0f);
        m_iCurrentWanderPt = -1;
        m_WanderPoints = GameObject.FindGameObjectsWithTag("WanderPoint");
        m_Am = GetComponent<Animator>();
    }

    private GameObject CheckCloseHole()
    {
        List<float> diss = new List<float>();
        int index = 0;
        foreach (var v in m_WanderPoints) //所有玩家和AI距離
        {
            Vector3 dis = v.transform.position - m_Data.m_Go.transform.position;  //距離位置
            diss.Add(dis.magnitude);  //距離長度
        }
        if (diss.Count != 1)
        {
            for (int i = 0; i < diss.Count - 1; i++)  //找出距離最近得
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
            index = diss.Count - 1;
            return m_WanderPoints[index];
        }
        return m_WanderPoints[0];
    }

    private GameObject CheckEnemyInSight(ref bool bAttack)
    {
        List<float> diss = new List<float>();
        List<GameObject> go = AIMain.m_Instance.GetPlayer();  //找到玩家
        int index = 0;
        foreach (var v in go) //所有玩家和AI距離
        {
            Vector3 dis = v.transform.position - m_Data.m_Go.transform.position;  //距離位置
            diss.Add(dis.magnitude);  //距離長度
        }
        if (diss.Count != 1)
        {
            for (int i = 0; i < diss.Count - 1; i++)  //找出距離最近得
            {
                float temp = 0;
                GameObject gtemp = null;
                if (diss[i] < diss[i + 1])
                {
                    temp = diss[i];
                    diss[i] = diss[i + 1];
                    diss[i + 1] = temp;

                    gtemp = go[i];
                    go[i] = go[i + 1];
                    go[i + 1] = gtemp;
                }
            }
            index = diss.Count - 1;
        }

        if (diss[index] < m_Data.m_fAttackRange)  //如果小於攻擊範圍
        {
            bAttack = true;
            return go[index];
        }
        else if (diss[index] < m_Data.m_fSight)
        {
            bAttack = false;
            return go[index];
        }
        return null;
    }

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
        Debug.LogError("Current State " + m_eCurrentState);
        if (m_eCurrentState == eFSMState.Idle)
        {
            bool bAttack = false;
            m_CurrentEnemyTarget = CheckEnemyInSight(ref bAttack);
            if (m_CurrentEnemyTarget != null) //偵測範圍內有玩家
            {
                Debug.LogError(m_CurrentEnemyTarget.gameObject+"玩家靠近");
                m_Data.m_TargetObject = m_CurrentEnemyTarget;
                if (bAttack)  //在警戒範圍內
                {
                    m_Data.m_vTarget = CheckCloseHole().transform.position;
                    m_Am.SetInteger("State", 3);
                    m_eCurrentState = eFSMState.MoveToTarget;
                }
                else
                {
                    m_eCurrentState = eFSMState.Chase;   //逃跑
                    m_Am.SetInteger("State", 3);
                }
                return;
            }  
            
            // Wait to move.           
            if (m_fCurrentTime > m_fIdleTime)
            {
                m_fCurrentTime = 0.0f;
                m_fIdleTime = 0.5f;
                m_Data.m_vTarget = RandomNavSphere(transform.position, m_Data.m_fSight, -1);
                m_eCurrentState = eFSMState.Wander;
                m_Am.SetInteger("State", 1);
                m_Data.m_bMove = true;
            }
            else
            {
                m_fCurrentTime += Time.deltaTime;
                m_Am.SetInteger("State", 0);
            }
        }
        else if (m_eCurrentState == eFSMState.Wander)
        {
            // Check Dead.            
            bool bAttack = false;
            m_Data.agent.updateRotation = true;
            m_fIdleTime = Random.Range(2.0f, 3.0f);
            m_CurrentEnemyTarget = CheckEnemyInSight(ref bAttack);
            if (m_CurrentEnemyTarget != null) //偵測範圍內有玩家
            {
                m_Data.m_TargetObject = m_CurrentEnemyTarget;
                if (bAttack)  //在警戒範圍內
                {
                    m_Data.m_vTarget = CheckCloseHole().transform.position;
                    m_Am.SetInteger("State", 3);
                    m_eCurrentState = eFSMState.MoveToTarget;
                }
                else
                {
                    m_eCurrentState = eFSMState.Chase;   //逃跑
                    m_Am.SetInteger("State", 3);
                }
                return;
            }
            m_Data.agent.SetDestination(m_Data.m_vTarget);
            Vector3 newPos = (m_Data.m_vTarget - transform.position);
            float dis = newPos.magnitude;
            Debug.LogError("目標位置在" + newPos + "離目標位置還有"+dis);
            if (dis < 0.1f)
            {
                m_Data.agent.updateRotation = false;
                m_Data.agent.SetDestination(transform.position);
                m_eCurrentState = eFSMState.Idle;
                m_fCurrentTime = 0.0f;
                m_fIdleTime = Random.Range(2.0f, 3.0f);
                m_Data.m_bMove = false;
            }
            else if (m_fCurrentTime > m_fIdleTime)
            {
                m_Data.agent.updateRotation = false;
                m_Data.agent.SetDestination(transform.position);
                m_eCurrentState = eFSMState.Idle;
                m_fCurrentTime = 0.0f;
                m_fIdleTime = Random.Range(2.0f, 3.0f);
                m_Data.m_bMove = false;
            }
            else
            {
                m_fCurrentTime += Time.deltaTime;
            }
            //if (m_Data.m_bMove == false)
            //{
            //    m_eCurrentState = eFSMState.Idle;
            //    m_fCurrentTime = 0.0f;
            //    m_fIdleTime = Random.Range(3.0f, 5.0f);
            //    m_Am.SetInteger("State", 0);
            //}
        }
        else if (m_eCurrentState == eFSMState.MoveToTarget)
        {
            m_Am.SetInteger("State", 3);
            m_Data.agent.SetDestination(m_Data.m_vTarget);
            Vector3 newPos = (m_Data.m_vTarget - transform.position);
            float dis = newPos.magnitude;
            if (dis < 1.5f)
            {
                m_Am.SetInteger("State",4);
            }
        }
        else if (m_eCurrentState == eFSMState.Chase)
        {
            // Check Dead.

            bool bAttack = false;
            bool bCheck = CheckTargetEnemyInSight(m_CurrentEnemyTarget, ref bAttack);

            if (bCheck == false)
            {
                Debug.LogError("不用逃跑了");
                m_eCurrentState = eFSMState.Idle;
                m_fCurrentTime = 0.0f;
                m_fIdleTime = Random.Range(3.0f, 5.0f);
                m_Am.SetInteger("State", 1);
                return;
            }
            if (bAttack)
            {
                m_Data.m_vTarget = CheckCloseHole().transform.position;
                m_Am.SetInteger("State", 3);
                m_eCurrentState = eFSMState.MoveToTarget;
            }
            else
            {
                Debug.LogError("還在視線範圍內");
                m_Data.m_vTarget = m_Data.m_TargetObject.transform.position;
                if (SteeringBehavior.CollisionAvoid(m_Data) == false)
                {
                    SteeringBehavior.Flee(m_Data);
                }
                SteeringBehavior.Move(m_Data);
                m_Am.SetInteger("State",3);
            }

        }
        else if (m_eCurrentState == eFSMState.Attack)
        {
            // Check Dead.

            // Check Animation complete.
            //...


            if (m_Am.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                // Check enemy damage.

                return;
            }

            if (m_Am.IsInTransition(0))
            {
                return;
            }


            bool bAttack = false;
            bool bCheck = CheckTargetEnemyInSight(m_CurrentEnemyTarget, ref bAttack);
            if (bCheck == false)
            {
                m_eCurrentState = eFSMState.Idle;
                m_fCurrentTime = 0.0f;
                m_fIdleTime = Random.Range(3.0f, 5.0f);
                m_Am.SetInteger("State", 1);
                return;
            }
            if (bAttack == false)
            {
                m_Data.m_TargetObject = m_CurrentEnemyTarget;
                m_eCurrentState = eFSMState.Chase;
                m_Am.SetInteger("State", 3);
                return;
            }
            if (m_fCurrentTime > m_Data.m_fAttackTime)
            {
                m_fCurrentTime = 0.0f;
                // Do attack.
                if (m_Am.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                {
                    m_Am.SetInteger("State", 2);
                }

            }
            m_fCurrentTime += Time.deltaTime;
        }
    }

    public Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {


        Vector3 randDirection = Random.insideUnitCircle * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);


        Debug.LogError("新的目標點"+navHit.position);
        return navHit.position;
    }

    private void OnDrawGizmos()
    {
        if (m_Data == null)
        {
            return;
        }
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(this.transform.position, this.transform.position + this.transform.forward * 2.0f);
        if (m_eCurrentState == eFSMState.Idle)
        {
            Gizmos.color = Color.green;
        }
        else if (m_eCurrentState == eFSMState.MoveToTarget)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(this.transform.position, m_Data.m_vTarget);
        }
        else if (m_eCurrentState == eFSMState.Chase)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(this.transform.position, m_Data.m_vTarget);
        }
        else if (m_eCurrentState == eFSMState.Attack)
        {
            Gizmos.color = Color.red;
        }
        else if (m_eCurrentState == eFSMState.Dead)
        {
            Gizmos.color = Color.gray;
        }
        Gizmos.DrawWireSphere(this.transform.position, m_Data.m_fSight);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, m_Data.m_fAttackRange);
    }
}

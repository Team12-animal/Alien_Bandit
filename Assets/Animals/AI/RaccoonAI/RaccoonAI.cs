using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RaccoonAI : MonoBehaviour
{
    public enum eFSMState
    {
        Idle,        //���A��State:0
        Wander,      //���A��State:1
        Chase,       //���A��State:2
        MoveToTarget,//���A��State:2
    }

    private eFSMState m_eCurrentState;      //��e���A
    public RabbitAIData m_Data;             //AI���
    private float m_fCurrentTime;           //��e���A�g�L�ɶ�
    private float m_fIdleTime;              //���A�ɶ�
    private GameObject m_CurrentEnemyTarget;//��e�����ؼ�
    private GameObject m_WanderPoints;    //�ߤl�۪���m����
    private Animator m_Am;                  //AI���ʵe���A��
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
    /// �ˬd���a�O�_�i�JAI�����u�d��
    /// </summary>
    /// <param name="bAttack">���a�i�Jĵ�ٽd��</param>
    /// <returns>�i�J���u�d��̪񪺪��a</returns>
    private GameObject CheckEnemyInSight(ref bool bAttack)
    {
        float currentDist = m_Data.m_fSight + 1f;
        GameObject play = null;
        foreach (var v in players) //�Ҧ����a�MAI�Z��
        {
            float dist = (v.transform.position - m_Data.m_Go.transform.position).magnitude;
            if (currentDist > dist)
            {
                currentDist = dist;
                play = v;
            }
        }
        if (currentDist < m_Data.m_fAttackRange)  //�p�G�p��ĵ�ٽd��
        {
            bAttack = true;
            return play;
        }
        else if (currentDist < m_Data.m_fSight)  //�p�G�i�J���u�d��
        {
            bAttack = false;
            return play;
        }

        return null;
    }

    /// <summary>
    /// �ˬd����O�_�٦b���u�d��
    /// </summary>
    /// <param name="target">�ؼЪ���</param>
    /// <param name="bAttack">�O�_�i�Jĵ��</param>
    /// <returns>��^�O�_�٦b���u��</returns>
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
        if (m_CurrentEnemyTarget != null) //�����d�򤺦����a
        {
            m_Data.m_TargetObject = m_CurrentEnemyTarget;
            if (bAttack)  //�bĵ�ٽd��
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
                m_eCurrentState = eFSMState.Chase;   //�k�]
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
            //Debug.LogError("Current State " + m_eCurrentState);  //�L�X��e���A
            if (m_eCurrentState == eFSMState.Idle)
            {
                m_Data.m_fMaxSpeed = 0.05f;
                m_Am.SetInteger("State", 0);
                CheckPlayerInSight();

                // Wait to move.           
                if (m_fCurrentTime > m_fIdleTime)  //���e�g�L�ɶ��j�󰱯d�ɶ��A�i�J���B
                {
                    m_Data.agent.enabled = true;
                    m_fCurrentTime = 0.0f;
                    m_fIdleTime = 0.5f;
                    m_Data.m_vTarget = RandomNavSphere(m_WanderPoints.transform.position, m_Data.m_fSight, -1);  //�b�����d���H����m
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
                m_fIdleTime = Random.Range(4.0f, 5.0f);  //���B���d�ɶ����H��3��4��
                CheckPlayerInSight();
                if (!(lastPos == transform.position))
                {
                    m_Am.SetInteger("State", 1);
                }
                lastPos = transform.position;
                m_Data.agent.enabled = true;
                m_Data.agent.updateRotation = true;
                m_Data.agent.SetDestination(m_Data.m_vTarget);  //AI���ʨ��H���ؼ��I
                Vector3 newPos = (m_Data.m_vTarget - transform.position); //��ؼ��I���V�q
                float dis = newPos.magnitude;  //�Z������
                if (dis < 0.3f || (m_fCurrentTime > m_fIdleTime))  //�Y�p��0.1f�K�^��IDLE���A (��F) Or �Y�W�L���d�ɶ��K���_�öi�JIDLE���A (����F)
                {
                    m_Data.agent.updateRotation = false;
                    m_Data.agent.SetDestination(transform.position);  //�N��m�վ㬰��e��m(�קK����)
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

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
    public RabbitAIData m_Data;         //AI���
    private float m_fCurrentTime;       //��e���A�g�L�ɶ�
    private float m_fIdleTime;          //���A�ɶ�
    private Animator m_Am;              //AI���ʵe���A��
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
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        m_Data.arriveDist = m_Data.m_Speed + 0.001f;
        //Debug.LogError("Current State " + m_eCurrentState);  //�L�X��e���A
        if (currentState == CurrentState.Idle)
        {
            m_Am.SetInteger("State", 0);
            // Wait to move.           
            if (m_fCurrentTime > m_fIdleTime)  //���e�g�L�ɶ��j�󰱯d�ɶ��A�i�J���B
            {
                m_Data.agent.enabled = true;
                m_fCurrentTime = 0.0f;
                m_fIdleTime = 0.5f;
                m_Data.m_vTarget = RandomNavSphere(transform.position, m_Data.m_fSight, -1);  //�b�����d���H����m
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
            m_fIdleTime = Random.Range(3.0f, 6.0f);  //���B���d�ɶ����H��3��4��
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

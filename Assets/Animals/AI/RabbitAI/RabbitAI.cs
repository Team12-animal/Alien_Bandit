using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RabbitAI : MonoBehaviour
{
    public enum eFSMState
    {
        NONE = -1,
        Idle,  //���A��State:0
        Wander,//���A��State:1
        Attack,//���A��State:2
        Chase,//���A��State:3
        MoveToTarget,//���A��State:3 ��ߤl�}State:4
        Runaway,
        Dead,
    }

    private eFSMState m_eCurrentState;  //���e���A
    public RabbitAIData m_Data;         //AI���
    private float m_fCurrentTime;       //���e���A�g�L�ɶ�
    private float m_fIdleTime;          //���A�ɶ�
    private GameObject m_CurrentEnemyTarget;//���e�����ؼ�
    private GameObject[] m_WanderPoints;//�ߤl�۪���m����
    private Animator m_Am;              //AI���ʵe���A��
    private List<GameObject> players;
    // Use this for initialization
    public void Start()
    {
        m_CurrentEnemyTarget = null;
        m_eCurrentState = eFSMState.Idle;
        m_fCurrentTime = 0.0f;
        m_fIdleTime = Random.Range(3.0f, 5.0f);
        m_WanderPoints = GameObject.FindGameObjectsWithTag("WanderPoint");  //�������W���Ҧ��ߤl��
        m_Am = GetComponent<Animator>();
        players = AIMain.m_Instance.GetPlayerList();
    }

    /// <summary>
    /// �ˬd���ߤl�̪񪺨��Өߤl��
    /// </summary>
    /// <returns>��^���ߤl�̪񪺨��Өߤl��</returns>
    private GameObject CheckCloseHole()
    {
        List<float> diss = new List<float>();  //�s��Z�����M��
        int index = 0;
        foreach (var v in m_WanderPoints) //�Ҧ��ߤl�۩MAI�Z��
        {
            Vector3 dis = v.transform.position - m_Data.m_Go.transform.position;  //�ߤl�M�ߤl�۶Z����m
            diss.Add(dis.magnitude);  //�Z������
        }
        if (diss.Count != 1)  //�p�G�ƶq���@���ܤ��άd��
        {
            for (int i = 0; i < diss.Count - 1; i++)  //��X�Z���̪�o�ñƧǨ�̫�@��
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
            index = diss.Count - 1;  //�̫�@�Ӱ}�C���Ѽ�(�`�ƶq��@)
            return m_WanderPoints[index];
        }
        return m_WanderPoints[0];
    }


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
    /// �ˬd���a�O�_�i�JAI�����u�d��
    /// </summary>
    /// <param name="bAttack">�����a�i�Jĵ�ٽd��</param>
    /// <returns>�i�J���u�d��̪񪺪��a</returns>
    //private GameObject CheckEnemyInSight(ref bool bAttack)
    //{
    //    List<float> diss = new List<float>();
    //    List<GameObject> go = AIMain.m_Instance.GetPlayerList();  //��쪱�a
    //    int index = 0;
    //    foreach (var v in go) //�Ҧ����a�MAI�Z��
    //    {
    //        Vector3 dis = v.transform.position - m_Data.m_Go.transform.position;  //�Z����m
    //        diss.Add(dis.magnitude);  //�Z������
    //    }
    //    if (diss.Count != 1)
    //    {
    //        for (int i = 0; i < diss.Count - 1; i++)  //��X�Z���̪�o
    //        {
    //            float temp = 0;
    //            GameObject gtemp = null;
    //            if (diss[i] < diss[i + 1])
    //            {
    //                temp = diss[i];
    //                diss[i] = diss[i + 1];
    //                diss[i + 1] = temp;

    //                gtemp = go[i];
    //                go[i] = go[i + 1];
    //                go[i + 1] = gtemp;
    //            }
    //        }
    //        index = diss.Count - 1;
    //    }
    //    if (diss[index] < m_Data.m_fAttackRange)  //�p�G�p��ĵ�ٽd��
    //    {
    //        bAttack = true;
    //        return go[index];
    //    }
    //    else if (diss[index] < m_Data.m_fSight)  //�p�G�i�J���u�d��
    //    {
    //        bAttack = false;
    //        return go[index];
    //    }
    //    return null;
    //}

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
        //Debug.LogError("Current State " + m_eCurrentState);  //�L�X���e���A
        if (m_eCurrentState == eFSMState.Idle)
        {
            m_Am.SetInteger("State", 0);
            bool bAttack = false;
            m_CurrentEnemyTarget = CheckEnemyInSight(ref bAttack);  //�����d�򤺦����a
            if (m_CurrentEnemyTarget != null && m_CurrentEnemyTarget.tag == "Players")   //�p�G�����a����
            {
                Debug.LogError("IDLE���a�J�I");
                m_Data.m_TargetObject = m_CurrentEnemyTarget;  //�ǵ�AIData�ؼЪ�
                if (bAttack)  //�bĵ�ٽd�򤺫K�|���ʨ�ߤl�}
                {
                    m_Data.m_vTarget = CheckCloseHole().transform.position;  //�N�ؼЦ�m�אּ�̪�ߤl�}
                    m_Am.SetInteger("State", 3);
                    m_eCurrentState = eFSMState.MoveToTarget;
                }
                else
                {
                    Debug.LogError("IDLE�ର�k�]");
                    m_eCurrentState = eFSMState.Chase;   //�k�]
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
            // Wait to move.           
            if (m_fCurrentTime > m_fIdleTime)  //�����e�g�L�ɶ��j�󰱯d�ɶ��A�i�J���B
            {
                m_fCurrentTime = 0.0f;
                m_fIdleTime = 0.5f;
                m_Data.m_vTarget = RandomNavSphere(transform.position, m_Data.m_fSight, -1);  //�b�����d���H����m
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
            bool bAttack = false;
            m_Data.agent.updateRotation = true;
            m_fIdleTime = Random.Range(2.0f, 3.0f);  //���B���d�ɶ����H��2��3��
            m_CurrentEnemyTarget = CheckEnemyInSight(ref bAttack);
            if (m_CurrentEnemyTarget != null) //�����d�򤺦����a
            {
                m_Data.m_TargetObject = m_CurrentEnemyTarget;
                if (bAttack)  //�bĵ�ٽd��
                {
                    m_Data.m_vTarget = CheckCloseHole().transform.position;
                    m_Am.SetInteger("State", 3);
                    m_eCurrentState = eFSMState.MoveToTarget;
                }
                else
                {
                    m_eCurrentState = eFSMState.Chase;   //�k�]
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
            m_Data.agent.SetDestination(m_Data.m_vTarget);  //AI���ʨ��H���ؼ��I
            Vector3 newPos = (m_Data.m_vTarget - transform.position); //��ؼ��I���V�q
            float dis = newPos.magnitude;  //�Z������
            if (dis < 0.1f || (m_fCurrentTime > m_fIdleTime))  //�Y�p��0.1f�K�^��IDLE���A (��F) Or �Y�W�L���d�ɶ��K���_�öi�JIDLE���A (����F)
            {
                m_Data.agent.updateRotation = false;
                m_Data.agent.SetDestination(transform.position);  //�N��m�վ㬰���e��m(�קK����)
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
                m_eCurrentState = eFSMState.Idle;
                m_fCurrentTime = 0.0f;
                m_fIdleTime = Random.Range(2.0f, 3.0f);
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
            Gizmos.color = Color.red;
        }
        else if (m_eCurrentState == eFSMState.Wander)
        {
            Gizmos.color = Color.cyan;
        }

        Gizmos.DrawWireSphere(this.transform.position, m_Data.m_fSight);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, m_Data.m_fAttackRange);
    }
}
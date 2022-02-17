using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class RabbitAIData
{
    public float m_fRadius;  //�����b�|
    public float m_fProbeLength;  //���w����
    public float m_Speed;  //���ʳt��
    public float m_fMaxSpeed;  //�̤j���ʳt��
    public float m_fRot;  //����t��
    public float m_fMaxRot;  //�̤j����t��
    public GameObject m_Go;  //Self


    public float m_fSight;  //�����Z��
    public float m_fAttackRange;  //�����Z��


    public float m_fAttackTime;  //�����ɶ�

    [HideInInspector]
    public GameObject m_TargetObject;  //ĵ�٥ؼ�

    [HideInInspector]
    public Vector3 m_vTarget;  //�n���ʨ쪺�ؼЦ�m
    [HideInInspector]
    public Vector3 m_vCurrentVector;  //��e��m
    [HideInInspector]
    public float m_fTempTurnForce;  //�I�[����t��
    [HideInInspector]
    public float m_fMoveForce;  //�I�[���ʳt��
    [HideInInspector]
    public bool m_bMove;  //�O�_�i�H����

    [HideInInspector]
    public bool m_bCol;  //�O�_����ê��

    [HideInInspector]
    public FSMSystem m_FSMSystem;

    public BT.cBTSystem m_BTSystem;

    public NavMeshAgent agent;

    public bool iscatched;
}



public class RabbitAIFunction
{
    /// <summary>
    /// �d�䪱�a�MAI�Z���O�_�b�����d�򤺡A�ê�^GameObject
    /// </summary>
    /// <param name="data"></param>
    /// <param name="bAttack"></param>
    /// <returns></returns>
    public static GameObject CheckEnemyInSight(RabbitAIData data, ref bool bAttack)
    {
        List<float> diss = new List<float>();
        List<GameObject> go = AIMain.m_Instance.GetPlayerList();  //��쪱�a
        foreach (var v in go) //�Ҧ����a�MAI�Z��
        {
            Vector3 dis = v.transform.position - data.m_Go.transform.position;  //�Z����m
            diss.Add(dis.magnitude);  //�Z������
        }

        for (int i = 0; i < diss.Count - 2; i++)  //��X�Z���̪�o
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
        int index = diss.Count - 1;
        if (diss[index] < data.m_fAttackRange)  //�p�G�p������d��
        {
            bAttack = true;
            return go[index];
        }
        else if (diss[index] < data.m_fSight)
        {
            bAttack = false;
            return go[index];
        }
        return null;
    }
    /// <summary>
    /// �d��S�w����MAI�Z���O�_�b�����d�򤺨ê�^�O�_���\���L
    /// </summary>
    /// <param name="data"></param>
    /// <param name="target"></param>
    /// <param name="bAttack"></param>
    /// <returns></returns>
    public static bool CheckTargetEnemyInSight(RabbitAIData data, GameObject target, ref bool bAttack)
    {
        GameObject go = target;
        Vector3 v = go.transform.position - data.m_Go.transform.position;
        float fDist = v.magnitude;
        if (fDist < data.m_fAttackRange)
        {
            bAttack = true;
            return true;
        }
        else if (fDist < data.m_fSight)
        {
            bAttack = false;
            return true;
        }
        return false;
    }


}

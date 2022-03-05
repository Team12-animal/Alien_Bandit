using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class RabbitAIData : NpcAIData
{
    public float m_fSight;  //視野距離
    public float m_fAttackRange;  //攻擊距離


    public float m_fAttackTime;  //攻擊時間

    [HideInInspector]
    public GameObject m_TargetObject;  //警戒目標

    [HideInInspector]
    public FSMSystem m_FSMSystem;

    public BT.cBTSystem m_BTSystem;

    public NavMeshAgent agent;

    public bool isCatched = false;

    public bool isBited = false;

    public bool isTargeted = false;
}



public class RabbitAIFunction
{
    /// <summary>
    /// 查找玩家和AI距離是否在攻擊範圍內，並返回GameObject
    /// </summary>
    /// <param name="data"></param>
    /// <param name="bAttack"></param>
    /// <returns></returns>
    public static GameObject CheckEnemyInSight(RabbitAIData data, ref bool bAttack)
    {
        List<float> diss = new List<float>();
        List<GameObject> go = AIMain.m_Instance.GetPlayerList();  //找到玩家
        foreach (var v in go) //所有玩家和AI距離
        {
            Vector3 dis = v.transform.position - data.m_Go.transform.position;  //距離位置
            diss.Add(dis.magnitude);  //距離長度
        }

        for (int i = 0; i < diss.Count - 2; i++)  //找出距離最近得
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
        if (diss[index] < data.m_fAttackRange)  //如果小於攻擊範圍
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
    /// 查找特定物件和AI距離是否在攻擊範圍內並返回是否成功布林
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

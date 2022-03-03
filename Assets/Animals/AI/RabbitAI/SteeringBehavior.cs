using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringBehavior
{
    static public void Move(NpcAIData data)
    {
        if (data.m_bMove == false)
        {
            return;
        }
        Transform t = data.m_Go.transform;
        Vector3 cPos = data.m_Go.transform.position;
        Vector3 vR = t.right;
        Vector3 vOriF = t.forward;
        Vector3 vF = data.m_vCurrentVector;

        if (data.m_fTempTurnForce > data.m_fMaxRot)
        {
            data.m_fTempTurnForce = data.m_fMaxRot;
        }
        else if (data.m_fTempTurnForce < -data.m_fMaxRot)
        {
            data.m_fTempTurnForce = -data.m_fMaxRot;
        }

        vF = vF + vR * data.m_fTempTurnForce;
        vF.Normalize();
        t.forward = vF;


        data.m_Speed = data.m_Speed + data.m_fMoveForce * Time.deltaTime;
        if (data.m_Speed < 0.01f)
        {
            data.m_Speed = 0.01f;
        }
        else if (data.m_Speed > data.m_fMaxSpeed)
        {
            data.m_Speed = data.m_fMaxSpeed;
        }

        if (data.m_bCol == false)
        {
            Debug.Log("CheckCollision");
            if (SteeringBehavior.CheckCollision(data))
            {
                Debug.Log("CheckCollision true");
                t.forward = vOriF;
            }
            else
            {
                Debug.Log("CheckCollision true");
            }
        }
        else
        {
            if (data.m_Speed < 0.02f)
            {
                if (data.m_fTempTurnForce > 0)
                {
                    t.forward = vR;
                }
                else
                {
                    t.forward = -vR;
                }

            }
        }
        cPos = cPos + t.forward * data.m_Speed;
        t.position = cPos;
    }

    static public bool CheckCollision(NpcAIData data)
    {
        List<Obstacle> m_AvoidTargets = AIMain.m_Instance.GetObstacles();
        if (m_AvoidTargets == null)
        {
            Debug.LogError("沒有要避免物");
            return false;
        }
        Transform ct = data.m_Go.transform;
        Vector3 cPos = ct.position;
        Vector3 cForward = ct.forward;
        Vector3 vec;

        float fDist = 0.0f;
        float fDot = 0.0f;
        int iCount = m_AvoidTargets.Count;
        for (int i = 0; i < iCount; i++)
        {
            vec = m_AvoidTargets[i].transform.position - cPos;
            vec.y = 0.0f;
            fDist = vec.magnitude;
            if (fDist > data.m_fProbeLength + m_AvoidTargets[i].m_fRadius)
            {
                m_AvoidTargets[i].m_eState = Obstacle.eState.OUTSIDE_TEST;
                continue;
            }

            vec.Normalize();
            fDot = Vector3.Dot(vec, cForward);
            if (fDot < 0)
            {
                m_AvoidTargets[i].m_eState = Obstacle.eState.OUTSIDE_TEST;
                continue;
            }
            m_AvoidTargets[i].m_eState = Obstacle.eState.INSIDE_TEST;
            float fProjDist = fDist * fDot;
            float fDotDist = Mathf.Sqrt(fDist * fDist - fProjDist * fProjDist);
            if (fDotDist > m_AvoidTargets[i].m_fRadius + data.m_fRadius)
            {
                continue;
            }

            return true;


        }
        return false;
    }


    static public bool CollisionAvoid(NpcAIData data)
    {
        List<Obstacle> m_AvoidTargets = AIMain.m_Instance.GetObstacles();
        Transform ct = data.m_Go.transform;  //AI的位置向量
        Vector3 cPos = ct.position;    //AI的位置
        Vector3 cForward = ct.forward;  //AI的朝向
        data.m_vCurrentVector = cForward;  
        Vector3 vec;
        float fFinalDotDist;
        float fFinalProjDist;
        Vector3 vFinalVec = Vector3.forward; //(0,0,1)
        Obstacle oFinal = null;
        float fDist = 0.0f;
        float fDot = 0.0f;
        float fFinalDot = 0.0f;
        int iCount = m_AvoidTargets.Count;

        float fMinDist = 10000.0f;
        for (int i = 0; i < iCount; i++)
        {
            Debug.LogWarning(m_AvoidTargets[i]+ "-" + cPos);
            vec = m_AvoidTargets[i].transform.position - cPos;
            vec.y = 0.0f;
            fDist = vec.magnitude;
            if (fDist > data.m_fProbeLength + m_AvoidTargets[i].m_fRadius)
            {
                m_AvoidTargets[i].m_eState = Obstacle.eState.OUTSIDE_TEST;
                continue;
            }

            vec.Normalize();
            fDot = Vector3.Dot(vec, cForward);
            if (fDot < 0)
            {
                m_AvoidTargets[i].m_eState = Obstacle.eState.OUTSIDE_TEST;
                continue;
            }
            else if (fDot > 1.0f)
            {
                fDot = 1.0f;
            }
            m_AvoidTargets[i].m_eState = Obstacle.eState.INSIDE_TEST;
            float fProjDist = fDist * fDot;
            float fDotDist = Mathf.Sqrt(fDist * fDist - fProjDist * fProjDist);
            if (fDotDist > m_AvoidTargets[i].m_fRadius + data.m_fRadius)
            {
                continue;
            }
            if (fDist < fMinDist)
            {
                fMinDist = fDist;
                fFinalDotDist = fDotDist;
                fFinalProjDist = fProjDist;
                vFinalVec = vec;
                oFinal = m_AvoidTargets[i];
                fFinalDot = fDot;
            }
        }

        if (oFinal != null)
        {
            Vector3 vCross = Vector3.Cross(cForward, vFinalVec);
            float fTurnMag = Mathf.Sqrt(1.0f - fFinalDot * fFinalDot);
            if (vCross.y > 0.0f)
            {
                fTurnMag = -fTurnMag;
            }
            data.m_fTempTurnForce = fTurnMag;

            float fTotalLen = data.m_fProbeLength + oFinal.m_fRadius;
            float fRatio = fMinDist / fTotalLen;
            if (fRatio > 1.0f)
            {
                fRatio = 1.0f;
            }
            fRatio = 1.0f - fRatio;
            data.m_fMoveForce = -fRatio;
            oFinal.m_eState = Obstacle.eState.COL_TEST;
            data.m_bCol = true;
            data.m_bMove = true;
            return true;
        }
        data.m_bCol = false;
        return false;
    }

    static public bool Flee(NpcAIData data)
    {
        Vector3 cPos = data.m_Go.transform.position;  //AI目前位置
        Vector3 vec = data.m_vTarget - cPos;  //下一個要到的位置
        vec.y = 0.0f;
        float fDist = vec.magnitude;  //下一個要到的位置長度
        data.m_fTempTurnForce = 0.0f;  //AI轉向強制力
        if (data.m_fProbeLength < fDist)
        {
            if (data.m_Speed > 0.01f)
            {
                data.m_fMoveForce = -1.0f;
            }
            data.m_bMove = true;
            return false;
        }
        Vector3 vf = data.m_Go.transform.forward;  //AI的朝向
        Vector3 vr = data.m_Go.transform.right;  //AI的右邊
        data.m_vCurrentVector = vf;
        vec.Normalize();
        float fDotF = Vector3.Dot(vf, vec);  //算出目標在前方或後方(反向-1、同向1、垂直0)

        if (fDotF < -0.96f)
        {

            fDotF = -1.0f;
            data.m_vCurrentVector = -vec * Random.Range(0.1f, 5f);
            data.m_fTempTurnForce = 0.0f;
            data.m_fRot = 0.0f;

        }
        else
        {
            if (fDotF > 1.0f)
            {
                fDotF = 1.0f;
            }
            float fDotR = Vector3.Dot(vr, vec);//要左轉或右轉

            if (fDotF > 0.0f)
            {
                if (fDotR > 0.0f)
                {
                    fDotR = 1.0f;
                }
                else
                {
                    fDotR = -1.0f;
                }
            }
            data.m_fTempTurnForce = -fDotR;
        }
        data.m_fMoveForce = -fDotF;
        data.m_bMove = true;
        return true;
    }

    static public bool Flee2(NpcAIData data)
    {
        Vector3 cPos = data.m_Go.transform.position;  //AI目前位置
        Vector3 vec = data.m_vTarget - cPos;  //下一個要到的位置
        vec.y = 0.0f;
        float fDist = vec.magnitude;  //下一個要到的位置長度
        data.m_fTempTurnForce = 0.0f;  //AI轉向強制力
       
        if (data.m_fProbeLength+2.5f < fDist)
        {
            if (data.m_Speed > 0.01f)
            {
                data.m_fMoveForce = -1.0f;
            }
            data.m_bMove = true;
            return false;
        }
       
        Vector3 vf = data.m_Go.transform.forward;  //AI的朝向
        Vector3 vr = data.m_Go.transform.right;  //AI的右邊
        data.m_vCurrentVector = vf;
        vec.Normalize();
        float fDotF = Vector3.Dot(vf, vec);  //算出目標在前方或後方(反向-1、同向1、垂直0)

        if (fDotF < -0.96f)
        {
            fDotF = -1.0f;
            //data.m_vCurrentVector = -vec * Random.Range(0.1f, 5f);
            data.m_vCurrentVector = -vec;
            data.m_fTempTurnForce = 0.0f;
            data.m_fRot = 0.0f;

        }
        else
        {
            if (fDotF > 1.0f)
            {
                fDotF = 1.0f;
            }
            float fDotR = Vector3.Dot(vr, vec);//要左轉或右轉

            if (fDotF > 0.0f)
            {
                if (fDotR > 0.0f)
                {
                    fDotR = 1.0f;
                }
                else
                {
                    fDotR = -1.0f;
                }
            }
            data.m_fTempTurnForce = -fDotR * Random.Range(0.1f, 5f);
            //data.m_fTempTurnForce = -fDotR;
        }
        data.m_fMoveForce = -fDotF *1.5f;
        data.m_bMove = true;
        return true;
    }

    static public bool Seek(NpcAIData data)
    {
        Vector3 cPos = data.m_Go.transform.position;
        Vector3 vec = data.m_vTarget - cPos;
        Debug.Log("astar seek" + data.m_vTarget + vec);
        vec.y = 0.0f;
        float fDist = vec.magnitude;
        if (fDist < data.arriveDist)
        {
            if(data.m_Go.tag == "Rabbit")
            {
                Vector3 vFinal = data.m_vTarget;
                vFinal.y = cPos.y;
                data.m_Go.transform.position = vFinal;
                data.m_fMoveForce = 0.0f;
                data.m_fTempTurnForce = 0.0f;
                data.m_Speed = 0.0f;
                data.m_bMove = false;
            }

            if(data.m_Go.tag == "Fox")
            {
                data.m_fMoveForce = 0.0f;
                data.m_fTempTurnForce = 0.0f;
                data.m_Speed = 0.0f;
                data.m_bMove = false;
            }
            return true;
        }
        Vector3 vf = data.m_Go.transform.forward;
        Vector3 vr = data.m_Go.transform.right;
        data.m_vCurrentVector = vf;
        vec.Normalize();
        float fDotF = Vector3.Dot(vf, vec);
        if (fDotF > 0.96f)
        {
            fDotF = 1.0f;
            data.m_vCurrentVector = vec;
            data.m_fTempTurnForce = 0.0f;
            data.m_fRot = 0.0f;
        }
        else
        {
            if (fDotF < -1.0f)
            {
                fDotF = -1.0f;
            }
            float fDotR = Vector3.Dot(vr, vec);

            if (fDotF < 0.0f)
            {

                if (fDotR > 0.0f)
                {
                    fDotR = 1.0f;
                }
                else
                {
                    fDotR = -1.0f;
                }

            }
            if (fDist < 3.0f)
            {
                fDotR *= (fDist / 3.0f + 1.0f);
            }
            data.m_fTempTurnForce = fDotR;

        }

        if (fDist < 3.0f)
        {
            Debug.Log(data.m_Speed);
            if (data.m_Speed > 0.1f)
            {
                data.m_fMoveForce = -(1.0f - fDist / 3.0f) * 5.0f;
            }
            else
            {
                data.m_fMoveForce = fDotF * 100.0f;
            }

        }
        else
        {
            data.m_fMoveForce = 100.0f;
        }
        data.m_bMove = true;
        return false;
    }


    static public bool PlayerAvoid(NpcAIData data, List<GameObject> players)
    {
        List<GameObject> m_AvoidTargets = players;
        float radius = 2.0f;
        float probe = data.m_fRadius;
        Transform ct = data.m_Go.transform;  //AI的位置向量
        Vector3 cPos = ct.position;    //AI的位置
        Vector3 cForward = ct.forward;  //AI的朝向
        data.m_vCurrentVector = cForward;
        Vector3 vec;
        float fFinalDotDist;
        float fFinalProjDist;
        Vector3 vFinalVec = Vector3.forward; //(0,0,1)
        GameObject oFinal = null;
        float fDist = 0.0f;
        float fDot = 0.0f;
        float fFinalDot = 0.0f;
        int iCount = m_AvoidTargets.Count;

        float fMinDist = 10000.0f;
        for (int i = 0; i < iCount; i++)
        {
            vec = m_AvoidTargets[i].transform.position - cPos;
            vec.y = 0.0f;
            fDist = vec.magnitude;
            if (fDist > probe + radius)
            {
                continue;
            }

            vec.Normalize();
            fDot = Vector3.Dot(vec, cForward);
            if (fDot < 0)
            {
                continue;
            }
            else if (fDot > 1.0f)
            {
                fDot = 1.0f;
            }

            float fProjDist = fDist * fDot;
            float fDotDist = Mathf.Sqrt(fDist * fDist - fProjDist * fProjDist);
            if (fDotDist > radius + data.m_fRadius)
            {
                continue;
            }
            if (fDist < fMinDist)
            {
                fMinDist = fDist;
                fFinalDotDist = fDotDist;
                fFinalProjDist = fProjDist;
                vFinalVec = vec;
                oFinal = m_AvoidTargets[i];
                fFinalDot = fDot;
            }
        }

        if (oFinal != null)
        {
            Vector3 vCross = Vector3.Cross(cForward, vFinalVec);
            float fTurnMag = Mathf.Sqrt(1.0f - fFinalDot * fFinalDot);
            if (vCross.y > 0.0f)
            {
                fTurnMag = -fTurnMag;
            }
            data.m_fTempTurnForce = fTurnMag;

            float fTotalLen = probe + radius;
            float fRatio = fMinDist / fTotalLen;
            if (fRatio > 1.0f)
            {
                fRatio = 1.0f;
            }
            fRatio = 1.0f - fRatio;
            data.m_fMoveForce = -fRatio;
            data.m_bCol = true;
            data.m_bMove = true;
            return true;
        }
        data.m_bCol = false;
        return false;
    }
}


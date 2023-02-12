using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringBehavoirTest
{
    private const float nearDistance = 3.0f;


    static public void Move(AIData_WOLF aIData)
    {
        if (aIData.isMoved == false)
            return;
        Vector3 aiSelfRightVec = aIData.self.transform.right;
        Vector3 aiSelfForwardVec = aIData.self.transform.forward;

        if (aIData.tempTurnForce > aIData.maxRotationForce)
        {
            aIData.tempTurnForce = aIData.maxRotationForce;
        }
        else if (aIData.tempTurnForce < -aIData.maxRotationForce)
        {
            aIData.tempTurnForce = -aIData.maxRotationForce;
        }

        aIData.currentVector = aIData.currentVector + aiSelfRightVec * aIData.tempTurnForce;
        aIData.currentVector.Normalize();
        aIData.self.transform.forward = aIData.currentVector;

        aIData.speed = aIData.speed + aIData.moveForce * Time.deltaTime;
        if (aIData.speed < 0.01f)
            aIData.speed = 0.01f;
        else if (aIData.speed > aIData.maxSpeed)
            aIData.speed = aIData.maxSpeed;

        aIData.self.transform.position = aIData.self.transform.position + aIData.self.transform.forward * aIData.speed;

    }

    static public bool Seek(AIData_WOLF aIData,GameObject seekTarget)
    {
        aIData.target.transform.position = seekTarget.transform.position;
        Vector3 selfPosition = aIData.self.transform.position;
        Vector3 attractiveVector = aIData.target.transform.position - aIData.self.transform.position;
        attractiveVector.y = 0.0f;
        float distance = attractiveVector.magnitude;
        if (distance < aIData.speed + 0.001f)
        {
            Vector3 finalPosition = aIData.target.transform.position;
            finalPosition.y = aIData.self.transform.position.y;
            aIData.self.transform.position = finalPosition;
            aIData.moveForce = 0.0f;
            aIData.tempTurnForce = 0.0f;
            aIData.speed = 0.0f;
            aIData.isMoved = false;
            return false;
        }
        Vector3 aiForwardVector = aIData.self.transform.forward;
        Vector3 aiRightVector = aIData.self.transform.right;
        aIData.currentVector = aiForwardVector;
        aiForwardVector.Normalize();
        float dotFromAttarctForceAndForwardVec = Vector3.Dot(aiForwardVector, attractiveVector);
        if (dotFromAttarctForceAndForwardVec > 0.96f)
        {
            dotFromAttarctForceAndForwardVec = 1.0f;
            aIData.currentVector = attractiveVector;
            aIData.tempTurnForce = 0.0f;
            aIData.rotationForce = 0.0f;
        }
        else
        {
            if (dotFromAttarctForceAndForwardVec < -1.0f)
                dotFromAttarctForceAndForwardVec = -1.0f;

            float dotFromAttarctForceAndRightVec = Vector3.Dot(aiRightVector, attractiveVector);
            if (dotFromAttarctForceAndForwardVec < 0.0f)
            {
                if (dotFromAttarctForceAndRightVec > 0.0f)//¦b¥k«á¤è
                {
                    dotFromAttarctForceAndRightVec = 1.0f;
                }
                else
                {
                    dotFromAttarctForceAndRightVec = -1.0f;
                }
            }
            if (distance < nearDistance)
            {
                dotFromAttarctForceAndRightVec *= (distance / nearDistance + 1.0f);
            }
            aIData.tempTurnForce = dotFromAttarctForceAndRightVec;
        }

        if (distance < nearDistance)
        {
            if (aIData.speed > 0.1f)
            {
                aIData.moveForce = -(1.0f - distance / nearDistance) * 5.0f;
            }
            else
            {
                aIData.moveForce = dotFromAttarctForceAndForwardVec * 100.0f;
            }
        }
        else
        {
            aIData.moveForce = 100.0f;
        }

        aIData.isMoved = true;
        return true;
    }

    static public bool CheckCollision(AIData data)
    {
        List<Obstacle> m_AvoidTargets = Main.m_Instance.GetObstacles();
        if (m_AvoidTargets == null)
        {
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


    static public bool CollisionAvoid(AIData data)
    {
        List<Obstacle> m_AvoidTargets = Main.m_Instance.GetObstacles();
        Transform ct = data.m_Go.transform;
        Vector3 cPos = ct.position;
        Vector3 cForward = ct.forward;
        data.m_vCurrentVector = cForward;
        Vector3 vec;
        float fFinalDotDist;
        float fFinalProjDist;
        Vector3 vFinalVec = Vector3.forward;
        Obstacle oFinal = null;
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



}

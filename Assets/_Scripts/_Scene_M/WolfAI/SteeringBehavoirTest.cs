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


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PigAIData : NpcAIData
{
    public GameObject birthPos;
    public GameObject homePos;

    public PigStatus Status;
    [HideInInspector]
    public int status = (int)PigStatus.Safe;

    private float oriMaxSpeed = 0.0f;
    private float oriMaxRot = 0.0f;
    public float runSpeedTimes;


    public enum PigStatus
    {
        Safe,
        Alert,
        Flee,
        Catched
    }

    public void UpdateStatus(int newStatus)
    {
        status = newStatus;
        Status = (PigStatus)status;

        ChangeSpeed(status);
    }

    public void SetTarget(Vector3 target)
    {
        m_vTarget = target;
    }

    private void ChangeSpeed(int status)
    {
        if (status == (int)PigStatus.Safe && oriMaxSpeed == 0.0f && oriMaxRot == 0.0f)
        {
            oriMaxSpeed = m_fMaxSpeed;
            oriMaxRot = m_fMaxRot;
        }

        if (status == (int)PigStatus.Flee || status == (int)PigStatus.Alert)
        {
            m_fMaxSpeed = oriMaxSpeed * runSpeedTimes;
            m_fMaxRot = oriMaxRot * runSpeedTimes;
        }
        else
        {
            m_fMaxSpeed = oriMaxSpeed;
            m_fMaxRot = oriMaxRot;
        }
    }
}

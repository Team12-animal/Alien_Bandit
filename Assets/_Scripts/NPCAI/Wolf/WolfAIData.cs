using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WolfAIData : NpcAIData
{
    //init data
    public GameObject birthPos;
    public GameObject homePos;
    public List<GameObject> preys;
    public GameObject target;

    public float alertDist;

    //jump
    public List<GameObject> jumpPs;

    //for rabbit and raccoon
    public GameObject catchedTarget;

    public WolfStatus Status;
    [HideInInspector]
    public int status = (int)WolfStatus.Safe;
   
    public enum WolfStatus
    {
        Safe, // do mission
        Alert, // player enter alert area
        Attacked, // attacked by player
        GoHome // back to homePos
    }

    public void UpdateStatus(int newStatus)
    {
        status = newStatus;
        Status = (WolfStatus)status;
    }

    public void SetTarget(Vector3 target)
    {
        m_vTarget = target;
    }
}

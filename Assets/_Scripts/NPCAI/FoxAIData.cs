using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FoxAIData : NpcAIData
{
    //birth data
    public GameObject target;
    public GameObject BirthPos;

    //speed for special movement
    public float jumpSpeed;

    //dist to player to enter alert status
    private float alertDist;

    //fox status for behaviour tree
    public FoxStatus Status = FoxStatus.Safe;
    [HideInInspector]
    public int status = (int)FoxStatus.Safe;

    public enum FoxStatus
    {
        Safe,
        Alert,
        Attacked,
        Home
    }

    public void UpdateStatus()
    {
        Status = (FoxStatus)status;
    }
}

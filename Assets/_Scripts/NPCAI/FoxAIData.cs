using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FoxAIData : NpcAIData
{
    //birth data
    public GameObject target;
    public GameObject birthPos;

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
        Safe, // do mission
        Alert, // player enter alert area
        AvoidAttack, // nearest player throw out the rock
        Attacked, // hit by rock or box
        Home // mission complete, back to birthPos
    }

    public void UpdateStatus(int newStatus)
    {
        status = newStatus;
        Status = (FoxStatus)status;
    }
}

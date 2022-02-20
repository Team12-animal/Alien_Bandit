using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxAIData : NpcAIData
{
    //dist to player to enter alert status
    public float alertDist;
    public GameObject target;

    //fox status for behaviour tree
    public int status = (int)FoxStatus.Safe;   
    public enum FoxStatus
    {
        Safe,
        Alert,
        Attacked,
        Home
    }
}

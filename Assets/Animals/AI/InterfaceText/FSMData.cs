using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class FSMData
{
    public float Sight;  //視野距離

    public float IdleTime;  //待命時間

    public float WanderTime;  //漫步時間

    public float AttackRange;  //攻擊距離

    public float AttackTime;  //攻擊時間

    public GameObject TargetObject;

    public Vector3 TargetPoint;

    public NavMeshAgent agent;

    public Animator animator;

    public bool isCatched = false;

    public bool isBited = false;

    public bool isTargeted = false;
}

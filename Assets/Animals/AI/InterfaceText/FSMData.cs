using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class FSMData
{
    public float Sight;  //�����Z��

    public float IdleTime;  //�ݩR�ɶ�

    public float WanderTime;  //���B�ɶ�

    public float AttackRange;  //�����Z��

    public float AttackTime;  //�����ɶ�

    public GameObject TargetObject;

    public Vector3 TargetPoint;

    public NavMeshAgent agent;

    public Animator animator;

    public bool isCatched = false;

    public bool isBited = false;

    public bool isTargeted = false;
}

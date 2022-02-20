using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NpcAIData
{
    public float m_fRadius;  //偵測半徑
    public float m_fProbeLength;  //探針長度
    public float m_Speed;  //移動速度
    public float m_fMaxSpeed;  //最大移動速度
    public float m_fRot;  //旋轉速度
    public float m_fMaxRot;  //最大旋轉速度
    public GameObject m_Go;  //Self

    [HideInInspector]
    public Vector3 m_vTarget;  //要移動到的目標位置
    [HideInInspector]
    public Vector3 m_vCurrentVector;  //當前位置
    [HideInInspector]
    public float m_fTempTurnForce;  //施加旋轉速度
    [HideInInspector]
    public float m_fMoveForce;  //施加移動速度
    [HideInInspector]
    public bool m_bMove;  //是否可以移動

    [HideInInspector]
    public bool m_bCol;  //是否有障礙物
}


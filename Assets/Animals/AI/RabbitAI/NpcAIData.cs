using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NpcAIData
{
    public float m_fRadius;  //�����b�|
    public float m_fProbeLength;  //���w����
    public float m_Speed;  //���ʳt��
    public float m_fMaxSpeed;  //�̤j���ʳt��
    public float m_fRot;  //����t��
    public float m_fMaxRot;  //�̤j����t��
    public GameObject m_Go;  //Self

    [HideInInspector]
    public Vector3 m_vTarget;  //�n���ʨ쪺�ؼЦ�m
    [HideInInspector]
    public Vector3 m_vCurrentVector;  //��e��m
    [HideInInspector]
    public float m_fTempTurnForce;  //�I�[����t��
    [HideInInspector]
    public float m_fMoveForce;  //�I�[���ʳt��
    [HideInInspector]
    public bool m_bMove;  //�O�_�i�H����

    [HideInInspector]
    public bool m_bCol;  //�O�_����ê��
}


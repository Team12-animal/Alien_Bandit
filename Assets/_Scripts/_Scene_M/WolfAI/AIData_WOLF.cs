using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AIData_WOLF
{
    public float radius;//�����b�|
    public float proeLenth;//���w�d��
    public float speed;
    public float maxSpeed;
    public float rotationForce;//��V����
    public float maxRotationForce;
    public GameObject self;
    public GameObject target;

    public float moveForce;
    public float tempTurnForce;

    public bool isMoved;

    public Vector3 currentVector;


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    //movement
    public float maxSpeed;
    public float maxRotate;
    public float setDashTime = 0.24f;
    public float dashSpeed;
    public float lerpAmt;

    //use item
    public GameObject item;
    public GameObject animal;

    public bool inTree;
}

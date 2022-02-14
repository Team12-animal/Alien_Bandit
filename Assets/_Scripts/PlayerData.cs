using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public int pid;
    //movement
    public float maxSpeed = 6f;
    private float reduceSpeed = 0.5f;
    public float realSpeed;
    public float maxRotate;
    private float reduceRotate = 0.5f;
    public float realRotate;
    public float setDashTime = 0.24f;
    public float dashSpeed;
    public float lerpAmt;

    //use item
    public GameObject item;
    public int catchedAmt;
    public GameObject tree;

    public bool inTree;

    private void Awake()
    {
        realSpeed = maxSpeed;
    }

    private void Update()
    {
        AdjustSpeed();
        BoxStatusReturn();
    }

    private void AdjustSpeed()
    {
        if(item != null && item.tag != "Chop")
        {
            realSpeed = maxSpeed * reduceSpeed;
            realRotate = maxRotate * reduceRotate;
        }
        else
        {
            realSpeed = maxSpeed;
            realRotate = maxRotate;
        }
    }

    private void BoxStatusReturn()
    {
        if(item != null && item.tag == "Box")
        {
            BoxController bc = item.GetComponent<BoxController>();
            bc.beUsing = true;
            bc.user = this.gameObject;
        }
    }
}

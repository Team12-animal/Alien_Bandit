using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float maxSpeed;
    public float maxRotate;
    public float transAmt;
    public float rotAmt;
    public float setDashTime;
    public float dashSpeed;

    private Vector3 fVec;
    private Vector3 rVec;
    private Vector3 dir;

    // Start is called before the first frame update
    void Start()
    {
        if(this.gameObject.name == "player")
        {
            Debug.Log("found");

        }
        else
        {
            Debug.Log("not found");
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Move(float transAmt, float rotAmt)
    {
        this.transAmt = transAmt;
        this.rotAmt = rotAmt;
        Debug.Log("MoveAndRotate");
        Debug.Log("h2" + transAmt);
        Debug.Log("v2" + rotAmt);

        rVec = this.transform.right * rotAmt * maxRotate * Time.deltaTime;
        fVec = this.transform.forward * transAmt * maxSpeed * Time.deltaTime;

        //move
        dir = rVec + fVec;
        this.transform.position += dir;

        Debug.Log("newPos" + this.transform.position);
    }

    public void Rotate(float transAmt, float rotAmt)
    {
        Vector3 currentR = this.transform.right;
        Vector3 currentF = this.transform.forward;

        if(this.transAmt == 0)
        {
            if(rotAmt > 0 && currentF != currentR)
            {
                this.transform.forward = this.transform.forward;
            }
        }
        else
        {
            rVec = this.transform.right * rotAmt * maxRotate * Time.deltaTime;
            this.transform.forward += rVec;
        }
    }

    public float Dash(float dashTime)
    {
        this.transform.position += this.transform.forward * dashSpeed * Time.deltaTime;
        dashTime -= Time.deltaTime;
        Debug.Log("dash");
        return dashTime;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(this.transform.position, this.transform.position + this.transform.forward * 2);
    }

}

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
    public float lerpAmt;

    public Camera cam;

    private Vector3 fVec;
    private Vector3 rVec;
    private Vector3 dir;

    // Start is called before the first frame update
    void Start()
    {
        if(this.gameObject.name == "Player1")
        {
            Debug.Log("found");

        }
        else
        {
            Debug.Log("not found");
        }

        rVec  = cam.transform.right;
        fVec = GenNewForward();
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

        Vector3 vMoveR = rVec * rotAmt;
        Vector3 vMoveF = fVec * transAmt;

        dir = vMoveR + vMoveF;
        this.transform.position += dir * maxSpeed * Time.deltaTime;

        Debug.Log("newPos" + this.transform.position);
    }

    public void Rotate(float transAmt, float rotAmt)
    {
        Vector3 target = (rVec * rotAmt) + (fVec * transAmt);
        this.transform.forward = Vector3.Lerp(this.transform.forward, (this.transform.forward + target) * maxRotate * Time.deltaTime, lerpAmt);
    }

    public Vector3 GenNewForward()
    {
        Vector3 tempV = cam.transform.forward;
        tempV.y = 0;
        tempV.Normalize();
    
        return tempV;
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

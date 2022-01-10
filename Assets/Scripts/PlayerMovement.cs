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

    public Camera cam;

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

        rVec = cam.transform.right * rotAmt * maxRotate * Time.deltaTime;
        fVec = cam.transform.forward * transAmt * maxSpeed * Time.deltaTime;

        dir = rVec + fVec;
        this.transform.position += dir;

        Debug.Log("newPos" + this.transform.position);
    }

    public void Rotate(float transAmt, float rotAmt)
    {
        Vector3 target = dir.normalized;
        Vector3 camF = cam.transform.forward;
        Vector3 oriF = this.transform.forward;

        Vector3 cCrossO = Vector3.Cross(camF, oriF);
        Vector3 cCrosst = Vector3.Cross(camF, target);
        float tDotC = Vector3.Dot(target, camF);
        float oDotC = Vector3.Dot(oriF, camF);

        float angle = 0.0f;
        float arc;

        if(cCrossO.y * cCrosst.y > 0)
        {
            //?CamF???????????????
            arc = Mathf.Abs(tDotC - oDotC);
            angle = Mathf.Acos(arc) * Mathf.Rad2Deg;

            if(tDotC - oDotC > 0)
            {
                angle = -angle;
            }

            Debug.Log("same side");
        }

        if(cCrossO.y * cCrosst.y < 0)
        {
            //?CamF????????????????
            arc = Mathf.Abs(tDotC) + Mathf.Abs(oDotC);
            angle = Mathf.Acos(arc) * Mathf.Rad2Deg;

            if(cCrossO.y < 0)
            {
                //?????
                angle = -angle;
            }
            Debug.Log("not same side");
        }

        if(cCrossO.y * cCrosst.y == 0)
        {
            Debug.Log("side: same dir");
            //??????CamF??
            if (oDotC * tDotC == 1)
            {
                //???????????
                angle = 0.0f;
            }

            if(oDotC * tDotC == -1)
            {
                //???????????
                angle = 180.0f;
            }

            if(Mathf.Abs(oDotC) != 1)
            {
                //?????CamF??
                this.transform.forward = camF * tDotC;
            }

            if(Mathf.Abs(tDotC) != 1)
            {
                //?????CamF??
                arc = Mathf.Abs(tDotC);
                angle = Mathf.Acos(arc) * Mathf.Rad2Deg;

                if (cCrosst.y < 0)
                {
                    //???????
                    angle = -angle;
                }
            }
        }
        Debug.Log("angle " + angle);
        this.transform.Rotate(0.0f, angle, 0.0f);
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

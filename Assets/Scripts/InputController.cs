using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    private PlayerMovement pm;
    private float transAmt;
    private float rotAmt;
    Vector3 currentPos;
    Vector3 targetPos;
    public bool isDash = false;
    public float setDashTime;
    private float remainDashTime;

    private void Awake()
    {
        pm = GetComponent<PlayerMovement>();
    }
    // Start is called before the first frame update
    void Start()
    {
        setDashTime = pm.setDashTime;
    }

    // Update is called once per frame

    void Update()
    {
        Debug.Log("Update" + 0);

        if (isDash == false || Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            Debug.Log("h" + Input.GetAxis("Horizontal"));
            Debug.Log("v" + Input.GetAxis("Vertical"));

            transAmt = Input.GetAxis("Vertical");
            rotAmt = Input.GetAxis("Horizontal");

            //pm.Rotate(transAmt, rotAmt);
            pm.Move(transAmt, rotAmt);
        }

        if (Input.GetButtonDown("Dash") && isDash == false)
        {
            Debug.Log("dash");
            if (isDash == false)
            {
                isDash = true;
                remainDashTime = setDashTime;
                Debug.Log("dash" + isDash);
            }
        }

        if(remainDashTime <= 0)
        {
            isDash = false;
            Debug.Log("dash" + isDash);
        }
        else
        {
            remainDashTime = pm.Dash(remainDashTime);
        }

    }
}

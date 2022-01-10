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
    public bool inDash = false;

    private void Awake()
    {
        pm = GetComponent<PlayerMovement>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame

    void Update()
    {
        Debug.Log("Update" + 0);

        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            Debug.Log("h" + Input.GetAxis("Horizontal"));
            Debug.Log("v" + Input.GetAxis("Vertical"));

            transAmt = Input.GetAxis("Vertical");
            rotAmt = Input.GetAxis("Horizontal");

            //pm.Rotate(transAmt, rotAmt);
            pm.Move(transAmt, rotAmt);
        }

        if (Input.GetButtonDown("Dash"))
        {
            Debug.Log("dash");
            if (inDash == false)
            {
                inDash = true;
                Debug.Log("dash" + inDash);
                inDash = pm.Dash();
            }

            Debug.Log("dash" + inDash);
        }
    }
}

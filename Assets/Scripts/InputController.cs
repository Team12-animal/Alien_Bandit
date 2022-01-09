using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    private PlayerMovement pm;
    private float transAmt;
    private float rotAmt;
    private Animator animator;

    private void Awake()
    {
        pm = GetComponent<PlayerMovement>();
    }
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Debug.Log("Update" + 0);

        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            //Debug.Log("h" + Input.GetAxis("Horizontal"));
            //Debug.Log("v" + Input.GetAxis("Vertical"));

            transAmt = Input.GetAxis("Vertical");
            rotAmt = Input.GetAxis("Horizontal");

            pm.MoveAndRotate(transAmt, rotAmt);
            animator.SetInteger("AnimationPar", 1);
        }
        else
        {
            animator.SetInteger("AnimationPar", 0);
        }
    }
}

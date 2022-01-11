using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Rigidbody rb;
    private AnimatorController anim;
    [SerializeField] float speed = 3.0f;
    [SerializeField] float rotateSpeed = 3.0f;
    [SerializeField] float t = 0.06f;
    float horizotalInput;
    float verticalInput;

    bool pressedCtrl;
    bool pressedShift;

    private void Awake()
    {
        anim = GetComponent<AnimatorController>();
        anim.animator = GetComponent<Animator>();
        anim.Init();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        ControlMoveMent();
        pressedCtrl = Input.GetButtonDown("LeftCtrl");
        pressedShift = Input.GetButton("LeftShift");
    }

    public void ControlMoveMent()
    {
        horizotalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        bool moved = (horizotalInput != 0 || verticalInput != 0);
        Vector3 movementDirection = new Vector3(horizotalInput, 0.0f, verticalInput);
        movementDirection.Normalize();
        float tempTime = 0;
        tempTime += Time.deltaTime;
        if (tempTime > 0.03f)
            transform.Translate(movementDirection * speed * Time.deltaTime, Space.World);
        if (!anim.animator.GetBool(anim.animPickedHash) && moved)
            anim.ChangeAnimationState(anim.Player_Run, horizotalInput, verticalInput);

        if (anim.animator.GetBool(anim.animPickedHash) && anim.animator.GetCurrentAnimatorStateInfo(0).IsName(anim.Player_OverHeadWalk) && moved)
            anim.ChangeAnimationState(anim.Player_OverHeadWalk, horizotalInput, verticalInput);
        if (anim.animator.GetBool(anim.animPickedHash) && anim.animator.GetCurrentAnimatorStateInfo(0).IsName(anim.Player_OverHeadWalk) && pressedCtrl)
        {
            anim.ChangeAnimationState(anim.Player_PutDown);
        }
        if (anim.animator.GetBool(anim.animPickedHash) && anim.animator.GetCurrentAnimatorStateInfo(0).IsName(anim.Player_TakeLeafWalk) && moved)
            anim.ChangeAnimationState(anim.Player_TakeLeafWalk, horizotalInput, verticalInput);


        if (movementDirection != Vector3.zero)
        {
            transform.forward = Vector3.Slerp(transform.forward, (transform.forward + movementDirection) * rotateSpeed * Time.deltaTime, t);
        }

        if (pressedShift)
        {
            transform.Translate(movementDirection * speed * 2f * Time.deltaTime, Space.World);
            anim.ChangeAnimationState(anim.Player_RunFaster);
        }

    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Rock" && pressedCtrl)
        {
            anim.ChangeAnimationState(anim.Player_PickUpOverHead);
        }
        else if (collision.gameObject.tag == "Tree" && pressedCtrl)
        {
            anim.ChangeAnimationState(anim.Player_Chop);
        }
        else if (collision.gameObject.tag == "Leaf" && pressedCtrl)
        {
            anim.ChangeAnimationState(anim.Player_TakeLeaf);
        }
        else if (collision.gameObject.tag == "WorkingTable" && pressedCtrl)
        {
            anim.ChangeAnimationState(anim.Player_UsingTable);
        }
        else if (collision.gameObject.tag == "Chop" && pressedCtrl)
        {
            anim.ChangeAnimationState(anim.Player_PickUpChop);
        }
    }
}

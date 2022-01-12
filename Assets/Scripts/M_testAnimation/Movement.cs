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


    [Header("動畫設定")]
    [SerializeField] float speedRunDistance = 100.0f;
    bool crtlPressed;
    bool shiftPressed;
    bool spacePressed;
    bool crtlCancel;

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
        crtlPressed = Input.GetButtonDown("LeftCtrl");
        crtlCancel = Input.GetButtonUp("LeftCtrl");
        shiftPressed = Input.GetButtonDown("LeftShift");
        spacePressed = Input.GetKeyDown(KeyCode.Space);
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

        if (anim.animator.GetBool(anim.animPickedHash) && anim.animator.GetCurrentAnimatorStateInfo(0).IsName(anim.Player_HoldRockWalk) && moved)
            anim.ChangeAnimationState(anim.Player_HoldRockWalk, horizotalInput, verticalInput);
        if (anim.animator.GetBool(anim.animPickedHash) && anim.animator.GetCurrentAnimatorStateInfo(0).IsName(anim.Player_HoldRockWalk) && crtlPressed)
            anim.ChangeAnimationState(anim.Player_PutDownRock);

        if (anim.animator.GetBool(anim.animPickedHash) && anim.animator.GetCurrentAnimatorStateInfo(0).IsName(anim.Player_HoldWoodWalk) && moved)
            anim.ChangeAnimationState(anim.Player_HoldWoodWalk, horizotalInput, verticalInput);
        if (anim.animator.GetBool(anim.animPickedHash) && anim.animator.GetCurrentAnimatorStateInfo(0).IsName(anim.Player_HoldWoodWalk) && crtlPressed)
            anim.ChangeAnimationState(anim.Player_PutDownWood);

        if (anim.animator.GetBool(anim.animPickedHash) && anim.animator.GetCurrentAnimatorStateInfo(0).IsName(anim.Player_HoldChopWalk) && moved)
            anim.ChangeAnimationState(anim.Player_HoldChopWalk, horizotalInput, verticalInput);
        if (anim.animator.GetBool(anim.animPickedHash) && anim.animator.GetCurrentAnimatorStateInfo(0).IsName(anim.Player_HoldChopWalk) && crtlPressed)
            anim.ChangeAnimationState(anim.Player_PutDownChop);

        if (movementDirection != Vector3.zero)
            transform.forward = Vector3.Slerp(transform.forward, (transform.forward + movementDirection) * rotateSpeed * Time.deltaTime, t);

        if (shiftPressed)
        {
            transform.Translate(transform.forward * speed  * Time.deltaTime, Space.World);
            anim.ChangeAnimationState(anim.Player_SpeedRun, horizotalInput, verticalInput);
        }

        if (spacePressed)
            anim.ChangeAnimationState(anim.Player_ThrowRock);
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Rock" && crtlPressed)
        {
            anim.ChangeAnimationState(anim.Player_PickUpRock);
        }
        else if (other.gameObject.tag == "Tree" && crtlPressed)
        {
            anim.ChangeAnimationState(anim.Player_Chopping);
        }
        else if (other.gameObject.tag == "Wood" && crtlPressed)
        {
            anim.ChangeAnimationState(anim.Player_PickWood);
        }
        else if (other.gameObject.tag == "WorkingTable" && crtlPressed)
        {
            anim.ChangeAnimationState(anim.Player_UsingTable);
        }
        else if (other.gameObject.tag == "Chop" && crtlPressed)
        {
            anim.ChangeAnimationState(anim.Player_PickUpChop);
        }
    }
}

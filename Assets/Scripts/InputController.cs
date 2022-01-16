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
    [SerializeField] private bool isDash = false;
    [SerializeField] private float setDashTime;
    [SerializeField] private float remainDashTime;

    AnimatorController anim;
    bool crtlPressed;
    bool crtlPressUp;
    float holdDownStartTime = 0f;
    [SerializeField] float pressedTime = 1.0f;

    private void Awake()
    {
        anim = GetComponent<AnimatorController>();
        anim.animator = GetComponent<Animator>();
        anim.Init();
        pm = GetComponent<PlayerMovement>();
    }
    void Start()
    {
        setDashTime = pm.setDashTime;
    }

    void Update()
    {
        rotAmt = Input.GetAxis("Horizontal");
        transAmt = Input.GetAxis("Vertical");
        bool moved = (rotAmt != 0 || transAmt != 0);
        Vector3 movementDirection = new Vector3(rotAmt, 0.0f, transAmt);
        movementDirection.Normalize();


        if (isDash == false && moved)
        {
            transAmt = Input.GetAxis("Vertical");
            rotAmt = Input.GetAxis("Horizontal");

            pm.Move(transAmt, rotAmt);
            pm.Rotate(transAmt, rotAmt);
        }

        if (Input.GetButtonDown("Dash") && isDash == false)
        {
            if (isDash == false)
            {
                isDash = true;
                remainDashTime = setDashTime;
            }
        }

        if (remainDashTime <= 0)
        {
            isDash = false;
        }
        else
        {
            remainDashTime = pm.Dash(remainDashTime);
        }

        ChangeAnimationState(moved, movementDirection);
    }

    private void ChangeAnimationState(bool moved, Vector3 movementDirection)
    {

        if (!anim.animator.GetBool(anim.animPickedHash) && moved)
        {
            if (isDash)
                anim.ChangeAnimationState(anim.Player_SpeedRun, rotAmt, transAmt);
            else if(!isDash && !anim.animator.GetCurrentAnimatorStateInfo(0).IsName(anim.Player_SpeedRun))
                anim.ChangeAnimationState(anim.Player_Run, rotAmt, transAmt);
        }

        if (anim.animator.GetBool(anim.animPickedHash) && anim.animator.GetCurrentAnimatorStateInfo(0).IsName(anim.Player_HoldRockWalk) && moved)
            anim.ChangeAnimationState(anim.Player_HoldRockWalk, rotAmt, transAmt);
        if (crtlPressed)
            transAmt = Time.time;
        if (anim.animator.GetBool(anim.animPickedHash) && anim.animator.GetCurrentAnimatorStateInfo(0).IsName(anim.Player_HoldRockWalk) && crtlPressUp)
        {
            float temp = pressedTime;
            float holdDownTime = Time.time - transAmt;
            if (holdDownTime >= pressedTime)
                anim.ChangeAnimationState(anim.Player_ThrowRock);
            else if (holdDownTime > 0.03f)
                anim.ChangeAnimationState(anim.Player_PutDownRock);
        }

        if (anim.animator.GetBool(anim.animPickedHash) && anim.animator.GetCurrentAnimatorStateInfo(0).IsName(anim.Player_HoldWoodWalk) && moved)
            anim.ChangeAnimationState(anim.Player_HoldWoodWalk, rotAmt, transAmt);
        if (anim.animator.GetBool(anim.animPickedHash) && anim.animator.GetCurrentAnimatorStateInfo(0).IsName(anim.Player_HoldWoodWalk) && crtlPressed)
            anim.ChangeAnimationState(anim.Player_PutDownWood);

        if (anim.animator.GetBool(anim.animPickedHash) && anim.animator.GetCurrentAnimatorStateInfo(0).IsName(anim.Player_HoldChopWalk) && moved)
            anim.ChangeAnimationState(anim.Player_HoldChopWalk, rotAmt, transAmt);
        if (anim.animator.GetBool(anim.animPickedHash) && anim.animator.GetCurrentAnimatorStateInfo(0).IsName(anim.Player_HoldChopWalk) && crtlPressed)
            anim.ChangeAnimationState(anim.Player_PutDownChop);

        if (!anim.animator.GetBool(anim.animPickedHash) && isDash)
            anim.ChangeAnimationState(anim.Player_SpeedRun, rotAmt, transAmt);
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
            anim.ChangeAnimationState(anim.Player_PickUpWood);
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Attack")
        {
            anim.ChangeAnimationState(anim.Player_Fear);
        }
    }
}

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
    [SerializeField] private bool holdingThing = false;
    [SerializeField] private float setDashTime;
    [SerializeField] private float remainDashTime;

    AnimatorController anim;
    bool crtlPressed;
    bool crtlPressUp;
    float holdDownStartTime = 0f;
    [SerializeField] float pressedTime = 1.0f;
    [SerializeField] bool inTreeArea = false;

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
        crtlPressed = Input.GetButtonDown("LeftCtrl");
        crtlPressUp = Input.GetButtonUp("LeftCtrl");
        bool moved = (rotAmt != 0 || transAmt != 0);
        Vector3 movementDirection = new Vector3(rotAmt, 0.0f, transAmt);
        movementDirection.Normalize();

        //aim to be removed
        bool allowMove =
               !anim.animator.GetCurrentAnimatorStateInfo(0).IsName(anim.Player_UsingTable)
            && !anim.animator.GetCurrentAnimatorStateInfo(0).IsName(anim.Player_Chopping)
            && !anim.animator.GetCurrentAnimatorStateInfo(0).IsName(anim.Player_PickUpChop)
            && !anim.animator.GetCurrentAnimatorStateInfo(0).IsName(anim.Player_PickUpRock)
            && !anim.animator.GetCurrentAnimatorStateInfo(0).IsName(anim.Player_PickUpWood)
            && !anim.animator.GetCurrentAnimatorStateInfo(0).IsName(anim.Player_Fear)
            && !anim.animator.GetCurrentAnimatorStateInfo(0).IsName(anim.Player_ChopFinished)
            && !anim.animator.GetCurrentAnimatorStateInfo(0).IsName(anim.Player_ChopIdle)
            && !anim.animator.GetCurrentAnimatorStateInfo(0).IsName(anim.Player_PutDownChop)
            && !anim.animator.GetCurrentAnimatorStateInfo(0).IsName(anim.Player_PutDownRock)
            && !anim.animator.GetCurrentAnimatorStateInfo(0).IsName(anim.Player_PutDownWood)
            && !anim.animator.GetCurrentAnimatorStateInfo(0).IsName(anim.Player_ThrowRock)
            && !anim.animator.GetCurrentAnimatorStateInfo(0).IsName(anim.Player_Idle)
            && !anim.animator.GetCurrentAnimatorStateInfo(0).IsName(anim.Player_Cheer)
            && !anim.animator.GetCurrentAnimatorStateInfo(0).IsName(anim.Player_SpeedRun);

        if (isDash == false && moved)
        {
            if (allowMove)
            {
                anim.animator.SetFloat(anim.animHorizontalHash, 0.0f);
                anim.animator.SetFloat(anim.animVerticalHash, 0.0f);
                transAmt = Input.GetAxis("Vertical");
                rotAmt = Input.GetAxis("Horizontal");
                pm.MoveAndRotate(transAmt, rotAmt);
                
                //expect code
                //anim.ChangeAnimationState(anim.Player_Run);
            }
        }

        if(Input.GetButtonDown("Use") && isDash == false)
        {
            if(holdingThing == false)
            {
                pm.Pick();
                holdingThing = true;
                anim.ChangeAnimationState(anim.Player_Run);
            }
            else
            {
                pm.Drop();
                holdingThing = false;
            }
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
            bool allowSpeedRun =
                !anim.animator.GetBool(anim.animPickedHash) && isDash
                && !anim.animator.GetCurrentAnimatorStateInfo(0).IsName(anim.Player_UsingTable)
                && !anim.animator.GetCurrentAnimatorStateInfo(0).IsName(anim.Player_Cheer);
            if (allowSpeedRun)
                remainDashTime = pm.Dash(remainDashTime);
            else
                isDash = false;
        }

        CheckAndPlayAnimation(moved);
    }

    //play the animation(for AnimatorController call)
    private void CheckAndPlayAnimation(bool moved)
    {
        bool allowExitTable = !anim.animator.GetCurrentAnimatorStateInfo(0).IsName(anim.Player_UsingTable) && !anim.animator.GetCurrentAnimatorStateInfo(0).IsName(anim.Player_Cheer);
        if (crtlPressed)
            holdDownStartTime = Time.time;
        if (!anim.animator.GetBool(anim.animPickedHash) && moved && allowExitTable)
        {
            if (isDash)
            {
                anim.ChangeAnimationState(anim.Player_SpeedRun, rotAmt, transAmt);
            }
            else if (!isDash && !anim.animator.GetCurrentAnimatorStateInfo(0).IsName(anim.Player_SpeedRun))
            {
                anim.ChangeAnimationState(anim.Player_Run, rotAmt, transAmt);
                GetComponent<PlayerMovement>().maxSpeed = 6;
                anim.animator.applyRootMotion = false;
            }
        }
        if (anim.animator.GetBool(anim.animPickedHash) && anim.animator.GetCurrentAnimatorStateInfo(0).IsName(anim.Player_HoldRockWalk) && moved)
        {
            anim.ChangeAnimationState(anim.Player_HoldRockWalk, rotAmt, transAmt);
            GetComponent<PlayerMovement>().maxSpeed = 1;
        }
        if (anim.animator.GetBool(anim.animPickedHash) && anim.animator.GetCurrentAnimatorStateInfo(0).IsName(anim.Player_HoldRockWalk) && crtlPressUp)
        {
            float temp = pressedTime;
            float holdDownTime = Time.time - holdDownStartTime;
            if (holdDownTime >= pressedTime)
                anim.ChangeAnimationState(anim.Player_ThrowRock);
            else if (holdDownTime > 0.03f)
                anim.ChangeAnimationState(anim.Player_PutDownRock);
            GetComponent<PlayerMovement>().maxSpeed = 6;
        }
        if (anim.animator.GetBool(anim.animPickedHash) && anim.animator.GetCurrentAnimatorStateInfo(0).IsName(anim.Player_HoldWoodWalk) && moved)
        {
            anim.ChangeAnimationState(anim.Player_HoldWoodWalk, rotAmt, transAmt);
            GetComponent<PlayerMovement>().maxSpeed = 2;
        }
        if (anim.animator.GetBool(anim.animPickedHash) && anim.animator.GetCurrentAnimatorStateInfo(0).IsName(anim.Player_HoldWoodWalk) && crtlPressed)
        {
            anim.ChangeAnimationState(anim.Player_PutDownWood);
            GetComponent<PlayerMovement>().maxSpeed = 6;
        }
        if (anim.animator.GetBool(anim.animPickedHash) && anim.animator.GetCurrentAnimatorStateInfo(0).IsName(anim.Player_HoldChopWalk) && moved)
        {
            anim.ChangeAnimationState(anim.Player_HoldChopWalk, rotAmt, transAmt);
            GetComponent<PlayerMovement>().maxSpeed = 4;
        }
        if (anim.animator.GetBool(anim.animPickedHash) && anim.animator.GetCurrentAnimatorStateInfo(0).IsName(anim.Player_HoldChopWalk) && crtlPressed && !inTreeArea)
        {
            anim.ChangeAnimationState(anim.Player_PutDownChop);
            GetComponent<PlayerMovement>().maxSpeed = 6;
        }
        bool allowSpeedRun =
            !anim.animator.GetBool(anim.animPickedHash) && isDash
            && (!anim.animator.GetCurrentAnimatorStateInfo(0).IsName(anim.Player_HoldChopWalk)
            || !anim.animator.GetCurrentAnimatorStateInfo(0).IsName(anim.Player_HoldRockWalk)
            || !anim.animator.GetCurrentAnimatorStateInfo(0).IsName(anim.Player_HoldWoodWalk));
        if (allowSpeedRun)
        {
            anim.ChangeAnimationState(anim.Player_SpeedRun, rotAmt, transAmt);
            anim.animator.SetFloat(anim.animHorizontalHash, 0.0f);
            anim.animator.SetFloat(anim.animVerticalHash, 0.0f);
            anim.animator.applyRootMotion = true;
            GetComponent<PlayerMovement>().maxSpeed = 8;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        bool allowUsingTable = other.gameObject.tag == "WorkingTable" && crtlPressed && !anim.animator.GetBool(anim.animHoldChop) && !anim.animator.GetBool(anim.animPickedHash);
        bool allowChopping = other.gameObject.tag == "Tree";
        bool allowPickUpRock = other.gameObject.tag == "Rock" && crtlPressed && !anim.animator.GetBool(anim.animPickedHash);
        bool allowPickUpWood = other.gameObject.tag == "Wood" && crtlPressed && !anim.animator.GetBool(anim.animPickedHash);
        bool allowPickUpChop = other.gameObject.tag == "Chop" && crtlPressed && !anim.animator.GetBool(anim.animPickedHash);

        if (allowPickUpRock)
        {
            anim.ChangeAnimationState(anim.Player_PickUpRock);
        }
        else if (allowChopping)
        {
            inTreeArea = true;
            if (anim.animator.GetBool(anim.animHoldChop) & crtlPressed)
                anim.ChangeAnimationState(anim.Player_Chopping);
        }
        else if (allowPickUpWood)
        {
            anim.ChangeAnimationState(anim.Player_PickUpWood);
        }
        else if (allowUsingTable)
        {
            anim.ChangeAnimationState(anim.Player_UsingTable);
        }
        else if (allowPickUpChop)
        {
            anim.ChangeAnimationState(anim.Player_PickUpChop);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Tree")
            inTreeArea = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Attack")
        {
            anim.ChangeAnimationState(anim.Player_Fear);
        }
    }
}
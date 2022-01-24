//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Movement : MonoBehaviour
//{
//    private Rigidbody rb;
//    private AnimatorController anim;
//    [SerializeField] float speed = 3.0f;
//    [SerializeField] float rotateSpeed = 3.0f;
//    [Tooltip("面向旋轉速度")] [SerializeField] float t = 0.06f;
//    float horizotalInput;
//    float verticalInput;


//    [Header("動畫設定")]
//    [Tooltip("尚未設定相關內容")] [SerializeField] float speedRunDistance = 100.0f;
//    [Tooltip("這個數值控制玩家按下crtl多久後才撥丟出去的動畫")] [SerializeField] float pressedTime = 1.0f;
//    bool crtlPressed;
//    bool shiftPressed;
//    bool crtlPressUp;
//    float holdDownStartTime = 0f;

//    private void Awake()
//    {
//        anim = GetComponent<AnimatorController>();
//        anim.animator = GetComponent<Animator>();
//        anim.Init();
//    }

//    private void Start()
//    {
//        rb = GetComponent<Rigidbody>();
//    }

//    private void Update()
//    {
//        ControlMoveMent();
//        crtlPressed = Input.GetButtonDown("LeftCtrl");
//        shiftPressed = Input.GetButtonDown("LeftShift");
//        crtlPressUp = Input.GetButtonUp("LeftCtrl");
//    }

//    public void ControlMoveMent()
//    {
//        horizotalInput = Input.GetAxis("Horizontal");
//        verticalInput = Input.GetAxis("Vertical");
//        bool moved = (horizotalInput != 0 || verticalInput != 0);
//        Vector3 movementDirection = new Vector3(horizotalInput, 0.0f, verticalInput);
//        movementDirection.Normalize();
//        float tempTime = 0;
//        tempTime += Time.deltaTime;
//        if (tempTime > 0.03f)
//            transform.Translate(movementDirection * speed * Time.deltaTime, Space.World);
//        if (movementDirection != Vector3.zero)
//            transform.forward = Vector3.Slerp(transform.forward, (transform.forward + movementDirection) * rotateSpeed * Time.deltaTime, t);

//        ChangeAnimationState(moved, movementDirection);
//    }

//    private void ChangeAnimationState(bool moved, Vector3 movementDirection)
//    {
//        if (!anim.animator.GetBool(anim.animPickedHash) && moved)
//            anim.ChangeAnimationState(anim.Player_Run, horizotalInput, verticalInput);

//        if (anim.animator.GetBool(anim.animPickedHash) && anim.animator.GetCurrentAnimatorStateInfo(0).IsName(anim.Player_HoldRockWalk) && moved)
//            anim.ChangeAnimationState(anim.Player_HoldRockWalk, horizotalInput, verticalInput);
//        if (crtlPressed)
//            holdDownStartTime = Time.time;
//        if (anim.animator.GetBool(anim.animPickedHash) && anim.animator.GetCurrentAnimatorStateInfo(0).IsName(anim.Player_HoldRockWalk) && crtlPressUp)
//        {
//            float temp = pressedTime;
//            float holdDownTime = Time.time - holdDownStartTime;
//            if (holdDownTime >= pressedTime)
//                anim.ChangeAnimationState(anim.Player_ThrowRock);
//            else if (holdDownTime > 0.03f)
//                anim.ChangeAnimationState(anim.Player_PutDownRock);
//        }

//        if (anim.animator.GetBool(anim.animPickedHash) && anim.animator.GetCurrentAnimatorStateInfo(0).IsName(anim.Player_HoldWoodWalk) && moved)
//            anim.ChangeAnimationState(anim.Player_HoldWoodWalk, horizotalInput, verticalInput);
//        if (anim.animator.GetBool(anim.animPickedHash) && anim.animator.GetCurrentAnimatorStateInfo(0).IsName(anim.Player_HoldWoodWalk) && crtlPressed)
//            anim.ChangeAnimationState(anim.Player_PutDownWood);

//        if (anim.animator.GetBool(anim.animPickedHash) && anim.animator.GetCurrentAnimatorStateInfo(0).IsName(anim.Player_HoldChopWalk) && moved)
//            anim.ChangeAnimationState(anim.Player_HoldChopWalk, horizotalInput, verticalInput);
//        if (anim.animator.GetBool(anim.animPickedHash) && anim.animator.GetCurrentAnimatorStateInfo(0).IsName(anim.Player_HoldChopWalk) && crtlPressed)
//            anim.ChangeAnimationState(anim.Player_PutDownChop);

//        //缺一個移動的距離
//        if (!anim.animator.GetBool(anim.animPickedHash) && shiftPressed)
//        {
//            anim.ChangeAnimationState(anim.Player_SpeedRun, horizotalInput, verticalInput);
//        }
//    }

//    private void OnTriggerStay(Collider other)
//    {
//        if (other.gameObject.tag == "Rock" && crtlPressed)
//        {
//            anim.ChangeAnimationState(anim.Player_PickUpRock);
//        }
//        else if (other.gameObject.tag == "Tree" && crtlPressed)
//        {
//            anim.ChangeAnimationState(anim.Player_Chopping);
//        }
//        else if (other.gameObject.tag == "Wood" && crtlPressed)
//        {
//            anim.ChangeAnimationState(anim.Player_PickUpWood);
//        }
//        else if (other.gameObject.tag == "WorkingTable" && crtlPressed)
//        {
//            anim.ChangeAnimationState(anim.Player_UsingTable);
//        }
//        else if (other.gameObject.tag == "Chop" && crtlPressed)
//        {
//            anim.ChangeAnimationState(anim.Player_PickUpChop);
//        }
//    }

//    private void OnTriggerEnter(Collider other)
//    {
//        if (other.gameObject.tag == "Attack")
//        {
//            anim.ChangeAnimationState(anim.Player_Fear);
//        }
//    }
//}

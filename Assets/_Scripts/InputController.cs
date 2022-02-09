using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    private PlayerMovement pm;
    private PlayerData data;

    private int pid;

    public float transAmt;
    public float rotAmt;
    Vector3 currentPos;
    Vector3 targetPos;
    [SerializeField] private bool isDash = false;
    [SerializeField] public bool holdingItem = false;
    [SerializeField] private float setDashTime;
    [SerializeField] private float remainDashTime;

    AnimatorController anim;
    bool takePressed;
    bool takePressUp;
    float holdDownStartTime = 0f;
    [SerializeField] float pressedTime = 1.0f;
    [SerializeField] bool inTreeArea = false;

    private void Awake()
    {
        anim = GetComponent<AnimatorController>();
        anim.animator = GetComponent<Animator>();
        anim.Init();
        pm = GetComponent<PlayerMovement>();
        data = GetComponent<PlayerData>();
    }
    void Start()
    {
        setDashTime = pm.setDashTime;
        pid = data.pid;
    }

    void Update()
    {
        string aniClip = "none";

        //rotAmt = Input.GetAxis("Horizontal" + pid);
        //transAmt = Input.GetAxis("Vertical" + pid);
        takePressed = Input.GetButtonDown("Take" + pid);
        takePressUp = Input.GetButtonUp("Take" + pid);
        //bool moved = (rotAmt != 0 || transAmt != 0);
        //Vector3 movementDirection = new Vector3(rotAmt, 0.0f, transAmt);
        //movementDirection.Normalize();

        bool allowMove = CheckAniPlayingOrNot();
        if (isDash == false && (Input.GetAxis("Vertical" + pid)!=0 || Input.GetAxis("Horizontal" + pid)!=0))
        {
            Debug.LogWarning(allowMove);
            if (allowMove)
            {
                Debug.LogWarning("JoyStick");
                anim.animator.SetFloat(anim.animHorizontalHash, 0.0f);
                anim.animator.SetFloat(anim.animVerticalHash, 0.0f);
                transAmt = Input.GetAxis("Vertical" + pid);
                rotAmt = Input.GetAxis("Horizontal" + pid);

                if (transAmt <= 0.2f && transAmt >= -0.2f)
                {
                    transAmt = 0.0f;
                    anim.animator.SetFloat(anim.animVerticalHash, 0.0f);
                }

                if (rotAmt <= 0.2f && rotAmt >= -0.2f)
                {
                    rotAmt = 0.0f;
                    anim.animator.SetFloat(anim.animHorizontalHash, 0.0f);
                }

                if (transAmt != 0 || rotAmt != 0)
                {
                    aniClip = pm.MoveAndRotate(transAmt, rotAmt);
                }
                
                Debug.Log("Move1" + aniClip);
                anim.ChangeAnimationState(aniClip, transAmt, rotAmt);
            }
        }
        //else if (!moved)
        //{
        //    anim.animator.SetFloat(anim.animHorizontalHash, 0.0f);
        //    anim.animator.SetFloat(anim.animVerticalHash, 0.0f);
        //}

        if(Input.GetAxis("Vertical" + pid)==0 && Input.GetAxis("Horizontal" + pid)==0)
        {
            anim.animator.SetFloat(anim.animHorizontalHash, 0.0f);
            anim.animator.SetFloat(anim.animVerticalHash, 0.0f);
        }

        if (Input.GetButtonDown("Take" + pid) && isDash == false)
        {
            if(data.item != null)
            {
                holdingItem = true;
            }
            else
            {
                holdingItem = false;
            }

            if (holdingItem == false)
            {
                aniClip = pm.Take();

                if (aniClip == "none")
                {
                    return;
                }

                Debug.Log(aniClip);
                anim.ChangeAnimationState(aniClip, 0, 0);
            }
            else
            {
                string itemInhand = data.item.gameObject.tag;

                if (itemInhand == "Chop")
                {
                    aniClip = pm.UseChop();
                }
                else if (itemInhand == "Bucket")
                {
                    //aniName = pm.UseBucket();
                }
                else if (itemInhand == "RockModel")
                {
                    float endPress = 0.0f;
                    float startPress = 0.0f;

                    if (takePressed)
                    {
                        startPress = Time.time;
                    }

                    if (takePressUp)
                    {
                        endPress = Time.time;
                    }

                    float pressTime = endPress - startPress;

                    if (pressedTime >= 3.0f)
                    {
                        aniClip = pm.Throw(pressTime);
                    }
                    else
                    {
                        aniClip = pm.Drop();
                    }
                }
                else
                {
                    aniClip = pm.Drop();
                }
            }

            Debug.Log(aniClip);
            anim.ChangeAnimationState(aniClip, 0, 0);
        }

        if (Input.GetButtonDown("Dash" + pid) && isDash == false && anim.animator.GetBool(anim.animRoling) == false)
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
            {
                remainDashTime = pm.Dash(remainDashTime);
            }
            else
            {
                isDash = false;
            }
                
        }

        CheckAndPlayAnimation(Input.GetAxis("Vertical" + pid)!=0 || Input.GetAxis("Horizontal" + pid)!=0);
    }

    //play the animation(for AnimatorController call)
    private void CheckAndPlayAnimation(bool moving)
    {
        bool allowExitTable = !anim.animator.GetCurrentAnimatorStateInfo(0).IsName(anim.Player_UsingTable) && !anim.animator.GetCurrentAnimatorStateInfo(0).IsName(anim.Player_Cheer);
        

        if (takePressed)
        {
            holdDownStartTime = Time.time;
        }
            
        if (!anim.animator.GetBool(anim.animPickedHash) && moving && allowExitTable)
        {
            if (isDash)
            {
                anim.ChangeAnimationState(anim.Player_SpeedRun, rotAmt, transAmt);
            }
            else if (!isDash && !anim.animator.GetCurrentAnimatorStateInfo(0).IsName(anim.Player_SpeedRun))
            {
                anim.ChangeAnimationState(anim.Player_Run, rotAmt, transAmt);
                //GetComponent<PlayerData>().maxSpeed = 6;
                anim.animator.applyRootMotion = false;
            }
        }
        if (anim.animator.GetBool(anim.animPickedHash) && anim.animator.GetCurrentAnimatorStateInfo(0).IsName(anim.Player_HoldRockWalk) && moving)
        {
            anim.ChangeAnimationState(anim.Player_HoldRockWalk, rotAmt, transAmt);
            //GetComponent<PlayerData>().maxSpeed = 1;
        }
        if (anim.animator.GetBool(anim.animPickedHash) && anim.animator.GetCurrentAnimatorStateInfo(0).IsName(anim.Player_HoldRockWalk) && takePressUp)
        {
            float temp = pressedTime;
            float holdDownTime = Time.time - holdDownStartTime;
            if (holdDownTime >= pressedTime)
                anim.ChangeAnimaEventState(anim.Player_ThrowRock);
            else if (holdDownTime > 0.03f)
                anim.ChangeAnimaEventState(anim.Player_PutDownRock);
            //GetComponent<PlayerData>().maxSpeed = 6;
        }
        if (anim.animator.GetBool(anim.animPickedHash) && anim.animator.GetCurrentAnimatorStateInfo(0).IsName(anim.Player_HoldWoodWalk) && moving)
        {
            anim.ChangeAnimationState(anim.Player_HoldWoodWalk, rotAmt, transAmt);
            //GetComponent<PlayerData>().maxSpeed = 2;
        }
        if (anim.animator.GetBool(anim.animPickedHash) && anim.animator.GetCurrentAnimatorStateInfo(0).IsName(anim.Player_HoldWoodWalk) && takePressed)
        {
            anim.ChangeAnimaEventState(anim.Player_PutDownWood);
           //GetComponent<PlayerData>().maxSpeed = 6;
        }
        if (anim.animator.GetBool(anim.animPickedHash) && anim.animator.GetCurrentAnimatorStateInfo(0).IsName(anim.Player_HoldChopWalk) && moving)
        {
            anim.ChangeAnimationState(anim.Player_HoldChopWalk, rotAmt, transAmt);
            //GetComponent<PlayerData>().maxSpeed = 4;
        }
        if (anim.animator.GetBool(anim.animPickedHash) && anim.animator.GetCurrentAnimatorStateInfo(0).IsName(anim.Player_HoldChopWalk) && takePressed && !inTreeArea)
        {
            anim.ChangeAnimaEventState(anim.Player_PutDownChop);
            //GetComponent<PlayerData>().maxSpeed = 6;
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

            //if(!pm.raydect)
            //{
            //    anim.animator.applyRootMotion = true;
            //}

            GetComponent<PlayerData>().maxSpeed = 8;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        bool allowUsingTable = other.gameObject.tag == "WorkingTable" && takePressed && !anim.animator.GetBool(anim.animHoldChop) && !anim.animator.GetBool(anim.animPickedHash);
        bool allowChopping = other.gameObject.tag == "Tree";
        bool allowPickUpRock = other.gameObject.tag == "Rock" && takePressed && !anim.animator.GetBool(anim.animPickedHash);
        bool allowPickUpWood = other.gameObject.tag == "Wood" && takePressed && !anim.animator.GetBool(anim.animPickedHash);
        bool allowPickUpChop = other.gameObject.tag == "Chop" && takePressed && !anim.animator.GetBool(anim.animPickedHash);

        if (allowPickUpRock)
        {
            anim.ChangeAnimaEventState(anim.Player_PickUpRock);
        }
        else if (allowChopping)
        {
            inTreeArea = true;
            if (anim.animator.GetBool(anim.animHoldChop) & takePressed)
                anim.ChangeAnimaEventState(anim.Player_Chopping);
        }
        else if (allowPickUpWood)
        {
            anim.ChangeAnimaEventState(anim.Player_PickUpWood);
        }
        else if (allowUsingTable)
        {
            anim.ChangeAnimaEventState(anim.Player_UsingTable);
        }
        else if (allowPickUpChop)
        {
            anim.ChangeAnimaEventState(anim.Player_PickUpChop);
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
            anim.ChangeAnimaEventState(anim.Player_Fear);
        }
    }

    private bool CheckAniPlayingOrNot()
    {
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

        return allowMove;
    }
}
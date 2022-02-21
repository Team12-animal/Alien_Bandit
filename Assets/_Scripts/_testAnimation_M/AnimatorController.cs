using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    [SerializeField]
    private GameObject hand;

    [HideInInspector]
    public Animator animator;
    float horizotalInput;
    float verticalInput;
    bool Roling;
    int pid;

    //crafting 
    GameObject hammer;

    [Header("�վ�ʵe����ɶ�")]
    public float delay = 2.1f;
    private string currentState;
    public int animHorizontalHash { get; set; }
    public int animVerticalHash { get; set; }
    public int animPickedHash { get; private set; }
    public int animHoldChop { get; private set; }

    public int animRoling { get; private set; }

    public string Player_Idle { get; private set; } = "Idle";
    public string Player_Run { get; private set; } = "Run";
    public string Player_SpeedRun { get; private set; } = "SpeedRun";
    public string Player_EndSpeedRun { get; private set; } = "SpeedRunToRun";
    public string Player_PickUpRock { get; private set; } = "PickUpRock";
    public string Player_PutDownRock { get; private set; } = "PutDownRock";
    public string Player_HoldRockWalk { get; private set; } = "HoldRockWalk";
    public string Player_ThrowRock { get; private set; } = "ThrowRock";
    public string Player_PickUpChop { get; private set; } = "PickUpChop";
    public string Player_HoldChopWalk { get; private set; } = "HoldChopWalk";
    public string Player_Chopping { get; private set; } = "Chopping";
    public string Player_ChopFinished { get; private set; } = "Chop-Finish";
    public string Player_ChopIdle { get; private set; } = "ChopIdle";
    public string Player_PutDownChop { get; private set; } = "PutDownChop";
    public string Player_PickUpWood { get; private set; } = "PickWood";
    public string Player_HoldWoodWalk { get; private set; } = "HoldWoodWalk";
    public string Player_PutDownWood { get; private set; } = "PutDownWood";
    public string Player_UsingTable { get; private set; } = "UsingTable";
    public string Player_MixUsingTableToWalk { get; private set; } = "MixTabletoWalk";
    public string Player_Cheer { get; private set; } = "Cheer";
    public string Player_StandUp { get; private set; } = "StandUp";
    public string Player_Fear { get; private set; } = "Fear";
    public string Player_DanceType18 { get; private set; } = "Dance18";
    public string Player_Test { get; private set; } = "Test";

    private void Start()
    {
        pid = this.GetComponent<PlayerData>().pid;
       
    }

    private void Update()
    {
        horizotalInput = Input.GetAxis("Horizontal" + pid);
        verticalInput = Input.GetAxis("Vertical" + pid);
    }

    public void Init()
    {
        animHorizontalHash = Animator.StringToHash("Horizontal");
        animVerticalHash = Animator.StringToHash("Vertical");
        animPickedHash = Animator.StringToHash("Picked");
        animHoldChop = Animator.StringToHash("HoldChop");
        animRoling = Animator.StringToHash("Roling");
    }

    //change stay but didn't play( play => InputController.ChangeAnimationState)
    public void ChangeAnimaEventState(string newState)
    {
        if (currentState == newState)
        {
            return;
        }

        animator.Play(newState);
        currentState = newState;

        if (newState == Player_PickUpChop)
        {
            animator.SetBool(animHoldChop, true);
            animator.SetBool(animPickedHash, true);
        }

        if (newState == Player_PickUpRock || newState == Player_PickUpWood)
        {
            animator.SetBool(animPickedHash, true);
        }

        if (newState == Player_PutDownRock || newState == Player_PutDownWood || newState == Player_ThrowRock)
        {
            CheckPlayerInput();
        }

        if (newState == Player_PutDownChop)
        {
            animator.SetBool(animHoldChop, false);
            CheckPlayerInput();
        }

        if (newState == Player_Chopping || newState == Player_UsingTable)
        {
            animator.SetFloat(animHorizontalHash, 0);
            animator.SetFloat(animVerticalHash, 0);
        }
    }

    private void CheckPlayerInput()
    {
        if (horizotalInput != 0 || verticalInput != 0)
            StartCoroutine(DelayAnim(animator.GetCurrentAnimatorStateInfo(0).length * delay));
        else
            StartCoroutine(DelayAnim(animator.GetCurrentAnimatorStateInfo(0).length));
    }

    /// <summary>
    /// set horizontal and veritfal value to animator
    /// </summary>
    /// <param name="newState"></param>
    /// <param name="horizontal"></param>
    /// <param name="vertical"></param>
    public void ChangeAnimationState(string newState, float horizontal, float vertical)
    {
        if (newState == Player_Run || newState == Player_HoldRockWalk || newState == Player_HoldWoodWalk || newState == Player_HoldChopWalk || newState == Player_SpeedRun)
        {
            Debug.Log("anima set float");
            animator.SetFloat(animHorizontalHash, horizontal);
            animator.SetFloat(animVerticalHash, vertical);
        }

        Debug.Log("Move2" + newState + horizontal + "/" + vertical);
        ChangeAnimaEventState(newState);
    }

    public IEnumerator DelayAnim(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        animator.SetBool(animPickedHash, false);
    }

    #region Unity AnimationEvent
    public void AnimaEvent_PickUpOverHeadToOverHeadWalk()
    {
        ChangeAnimaEventState(Player_HoldRockWalk);
    }
    public void AnimaEventPutDownToIdle()
    {
        animator.SetFloat(animHorizontalHash, 0);
        animator.SetFloat(animVerticalHash, 0);
        ChangeAnimaEventState(Player_Run);
    }
    public void AnimationEventTakeWoodToWoodWalk()
    {
        ChangeAnimaEventState(Player_HoldWoodWalk);
    }
    public void AnimaEventPickUpToRun()
    {
        ChangeAnimaEventState(Player_HoldChopWalk);
    }
    public void AnimaEventChoppingToChopIdle()
    {
        ChangeAnimaEventState(Player_ChopIdle);
    }
    public void AnimaEventPutDownChopToRun()
    {
        animator.SetBool(animPickedHash, false);
    }
    public void AnimaEventPutDownRockToRun()
    {
        int random = Random.Range(0, 6);
        if (random > 0)
            animator.SetBool(animPickedHash, false);
        else
            ChangeAnimaEventState(Player_Idle);
    }
    public void AnimaEventMixUsingTableToWalk()
    {
        ChangeAnimaEventState(Player_Cheer);

        //int random = Random.Range(0, 6);
        //if (random > 1)
        //    ChangeAnimaEventState(Player_MixUsingTableToWalk);
        //else
        //    ChangeAnimaEventState(Player_Cheer);
    }

    public void AnimaEventChopFinished()
    {
        ChangeAnimaEventState(Player_ChopFinished);
    }

    public void AnimaEventSpeedRunToRun()
    {
        animator.SetFloat(animHorizontalHash, 0.0f);
        animator.SetFloat(animVerticalHash, 0.0f);
        ChangeAnimaEventState(Player_Run);
    }

    public void AnimaEventEndThrow()
    {
        animator.SetBool(animPickedHash, false);
    }

    /// <summary>
    /// ���ե�
    /// </summary>
    public void Test()
    {
        ChangeAnimaEventState(Player_Cheer);
    }

    public void AnimaEventStartAsTrue()
    {
        animator.SetBool(animRoling, true);
    }

    public void AnimaEventEndAsFalse()
    {
        animator.SetBool(animRoling, false);
    }


    bool holdingHammer = false;
    Vector3 oriPos;
    Quaternion oriRot;
    GameObject workingTable;

    public void AnimaEventHammerInhandToggle()
    {
        if(hammer == null || workingTable == null)
        {
            hammer = GameObject.Find("Hammer");
            oriPos = hammer.transform.position;
            oriRot = hammer.transform.rotation;
            workingTable = GameObject.Find("WorkBench");
        }
       
        Debug.Log("hammerInhand");
        if(hammer != null && hand != null)
        {
            if (holdingHammer == false)
            {
                hammer.transform.position = hand.transform.position;
                hammer.transform.forward = hand.transform.forward;
                hammer.transform.Rotate(new Vector3(0.0f, 180.0f, 90.0f));
                hammer.transform.parent = hand.transform;
                holdingHammer = true;
            }
            else
            {
                hammer.transform.parent = workingTable.transform;
                hammer.transform.position = oriPos;
                hammer.transform.rotation = oriRot;
                holdingHammer = false;
            }

        }
    }

    //using table effect activtor toggle
    public GameObject usingTableEffect;
    private ParticleSystem system;
    public void AnimaEventUsingEffectActivator()
    {
        if(usingTableEffect == null)
        {
            usingTableEffect = GameObject.Find("UsingTableEffect");
            system = usingTableEffect.GetComponent<ParticleSystem>();
        }

        if (usingTableEffect != null)
        {
            if (system.isPlaying == true)
            {
                system.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

            }
            else
            {
                system.Play(true);

            }
        }
    }

    //if throwing or not > for npcAI
    private bool throwing = false;

    public void AnimaEventThrowingToggle()
    {
        if (!throwing)
        {
            throwing = true;
        }
        else
        {
            throwing = false;
        }
    }

    public bool ThorowingOrNot()
    {
        return throwing;
    }

    #endregion

    public bool CheckAniPlayingOrNot()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(Player_PutDownRock))
        {
            float time = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            Debug.Log("putdown" + time);
        }
        
        bool allowMove =
            StartIdelEnded()
            &&!animator.GetCurrentAnimatorStateInfo(0).IsName(Player_UsingTable)
            && !animator.GetCurrentAnimatorStateInfo(0).IsName(Player_Chopping)
            && !animator.GetCurrentAnimatorStateInfo(0).IsName(Player_PickUpChop)
            && !animator.GetCurrentAnimatorStateInfo(0).IsName(Player_PickUpRock)
            && !animator.GetCurrentAnimatorStateInfo(0).IsName(Player_PickUpWood)
            && !animator.GetCurrentAnimatorStateInfo(0).IsName(Player_Fear)
            && !animator.GetCurrentAnimatorStateInfo(0).IsName(Player_ChopFinished)
            && !animator.GetCurrentAnimatorStateInfo(0).IsName(Player_ChopIdle)
            && !(animator.GetCurrentAnimatorStateInfo(0).IsName(Player_PutDownChop) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            && !(animator.GetCurrentAnimatorStateInfo(0).IsName(Player_PutDownRock) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            && !(animator.GetCurrentAnimatorStateInfo(0).IsName(Player_PutDownWood) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            && !(animator.GetCurrentAnimatorStateInfo(0).IsName(Player_ThrowRock) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            && !animator.GetCurrentAnimatorStateInfo(0).IsName(Player_Cheer)
            && !animator.GetCurrentAnimatorStateInfo(0).IsName(Player_SpeedRun);

        Debug.Log("allow move" + allowMove);
        return allowMove;
    }


    /// <summary>
    /// lock animation to idel while game start
    /// </summary>
    bool startIdelEnd = false;
    private bool StartIdelEnded()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(Player_Idle) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.2f)
        {
            startIdelEnd = true;
        }

        return startIdelEnd;
    }
}
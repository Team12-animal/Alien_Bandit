using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    [HideInInspector]
    public Animator animator;
    float horizotalInput;
    float verticalInput;
    bool Roling;
    int pid;

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
    /// �u�n�򲾰ʬ����ݭn�h��J���Input
    /// </summary>
    /// <param name="newState"></param>
    /// <param name="horizontal"></param>
    /// <param name="vertical"></param>
    public void ChangeAnimationState(string newState, float horizontal, float vertical)
    {
        if (newState == Player_Run || newState == Player_HoldRockWalk || newState == Player_HoldWoodWalk || newState == Player_HoldChopWalk)
        {
            animator.SetFloat(animHorizontalHash, horizontal);
            animator.SetFloat(animVerticalHash, vertical);
        }
        Debug.Log("Move2" + newState);
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
        int random = Random.Range(0, 6);
        if (random > 1)
            ChangeAnimaEventState(Player_MixUsingTableToWalk);
        else
            ChangeAnimaEventState(Player_Cheer);
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
    
    #endregion

}
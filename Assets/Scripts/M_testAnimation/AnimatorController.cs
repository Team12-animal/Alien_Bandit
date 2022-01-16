using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    [HideInInspector]
    public Animator animator;
    float horizotalInput;
    float verticalInput;

    [Header("調整動畫延遲時間")]
    public float delay = 2.1f;
    private string currentState;
    public int animHorizontalHash { get; private set; }
    public int animVerticalHash { get; private set; }
    public int animPickedHash { get; private set; }


    public string Player_Idle { get; private set; } = "Idle";
    public string Player_Run { get; private set; } = "Run";
    public string Player_SpeedRun { get; private set; } = "SpeedRun";
    public string Player_PickUpRock { get; private set; } = "PickUpRock";
    public string Player_PutDownRock { get; private set; } = "PutDownRock";
    public string Player_HoldRockWalk { get; private set; } = "HoldRockWalk";
    public string Player_ThrowRock { get; private set; } = "ThrowRock";
    public string Player_PickUpChop { get; private set; } = "PickUpChop";
    public string Player_HoldChopWalk { get; private set; } = "HoldChopWalk";
    public string Player_Chopping { get; private set; } = "Chopping";
    public string Player_ChopIdle { get; private set; } = "ChopIdle";
    public string Player_PutDownChop { get; private set; } = "PutDownChop";
    public string Player_PickUpWood { get; private set; } = "PickWood";
    public string Player_HoldWoodWalk { get; private set; } = "HoldWoodWalk";
    public string Player_PutDownWood { get; private set; } = "PutDownWood";
    public string Player_UsingTable { get; private set; } = "UsingTable";
    public string Player_MixUsingTableToWlak { get; private set; } = "MixTabletoWalk";
    public string Player_Cheer { get; private set; } = "Cheer";
    public string Player_StandUp { get; private set; } = "StandUp";
    public string Player_Fear { get; private set; } = "Fear";


    public string Player_Test { get; private set; } = "Test";



    private void Update()
    {
        horizotalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }

    public void Init()
    {
        animHorizontalHash = Animator.StringToHash("Horizontal");
        animVerticalHash = Animator.StringToHash("Vertical");
        animPickedHash = Animator.StringToHash("Picked");
    }
    public void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;
        animator.Play(newState);
        currentState = newState;
        if (newState == Player_PickUpRock || newState == Player_PickUpWood || newState == Player_PickUpChop)
            animator.SetBool(animPickedHash, true);
        if (newState == Player_PutDownRock || newState == Player_PutDownWood || newState == Player_ThrowRock || newState == Player_PutDownChop)
        {
            if (horizotalInput != 0 || verticalInput != 0)
                StartCoroutine(DelayAnim(animator.GetCurrentAnimatorStateInfo(0).length * delay));
            else
                StartCoroutine(DelayAnim(animator.GetCurrentAnimatorStateInfo(0).length));
        }
    }

    /// <summary>
    /// 只要跟移動相關需要多輸入兩個Input
    /// </summary>
    /// <param name="newState"></param>
    /// <param name="horizontal"></param>
    /// <param name="vertical"></param>
    public void ChangeAnimationState(string newState, float horizontal, float vertical)
    {
        if (newState == Player_Run || newState == Player_HoldRockWalk || newState == Player_HoldWoodWalk || newState == Player_HoldChopWalk || newState == Player_SpeedRun)
        {
            animator.SetFloat(animHorizontalHash, horizontal);
            animator.SetFloat(animVerticalHash, vertical);
        }
        ChangeAnimationState(newState);
    }

    public IEnumerator DelayAnim(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        animator.SetBool(animPickedHash, false);
    }
    #region Unity AnimationEvent
    public void AnimaEvent_PickUpOverHeadToOverHeadWalk()
    {
        ChangeAnimationState(Player_HoldRockWalk);
    }
    public void AnimaEventPutDownToIdle()
    {
        animator.SetFloat(animHorizontalHash, 0);
        animator.SetFloat(animVerticalHash, 0);
        ChangeAnimationState(Player_Run);
    }
    public void AnimationEventTakeWoodToWoodWalk()
    {
        ChangeAnimationState(Player_HoldWoodWalk);
    }
    public void AnimaEventPickUpToRun()
    {
        ChangeAnimationState(Player_HoldChopWalk);
    }
    public void AnimaEventChoppingToChopIdle()
    {
        ChangeAnimationState(Player_ChopIdle);
    }
    public void AnimaEventPutDownChopToRun()
    {
        animator.SetBool(animPickedHash, false);
    }
    public void AnimaEventMixUsingTableToWalk()
    {
        ChangeAnimationState(Player_MixUsingTableToWlak);
    }

    public void AnimaEventSpeedRunToRun()
    {
        ChangeAnimationState(Player_Run);
    }

    public void AnimaEventIdle()
    {
        Test();
    }
    public void Test()
    {
        if (horizotalInput != 0 || verticalInput != 0)
        {
            animator.SetFloat(animHorizontalHash, horizotalInput);
            animator.SetFloat(animVerticalHash, verticalInput);
            ChangeAnimationState(Player_Test);
        }
        else if (horizotalInput == 0 && verticalInput == 0)
        {
            animator.SetFloat(animHorizontalHash, 0);
            animator.SetFloat(animVerticalHash, 0);
            ChangeAnimationState(Player_Idle);
        }
        ChangeAnimationState(Player_Idle);
    }
    #endregion
}

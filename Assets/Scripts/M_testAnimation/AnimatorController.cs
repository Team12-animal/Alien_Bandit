using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    [HideInInspector]
    public Animator animator;
    private string currentState;
    public int animHorizontalHash { get; private set; }
    public int animVerticalHash { get; private set; }
    public int animPickedHash { get; private set; }


    public string Player_Idle { get; private set; } = "Idle";
    public string Player_Run { get; private set; } = "Run";
    public string Player_RunFaster { get; private set; } = "RunFast";
    public string Player_PickUpOverHead { get; private set; } = "PickUpOverHead";
    public string Player_PutDown { get; private set; } = "PutDown";
    public string Player_OverHeadWalk { get; private set; } = "OverHeadWalk";
    public string Player_PickUpChop { get; private set; } = "PickUpChop";
    public string Player_Chop { get; private set; } = "Chop";
    public string Player_TakeLeaf { get; private set; } = "TakeWood";
    public string Player_TakeLeafWalk { get; private set; } = "TakeWoodWalk";
    public string Player_HandOff { get; private set; } = "WoodHandOff";
    public string Player_UsingTable { get; private set; } = "UsingTable";



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
        if (newState == Player_PickUpOverHead || newState == Player_TakeLeaf || newState == Player_PickUpChop)
            animator.SetBool(animPickedHash, true);
        if (newState == Player_PutDown)
            animator.SetBool(animPickedHash, false);
    }

    /// <summary>
    /// �u�n�򲾰ʬ����ݭn�h��J���Input
    /// </summary>
    /// <param name="newState"></param>
    /// <param name="horizontal"></param>
    /// <param name="vertical"></param>
    public void ChangeAnimationState(string newState, float horizontal, float vertical)
    {
        if (newState == Player_Run || newState == Player_OverHeadWalk || newState == Player_TakeLeafWalk)
        {
            animator.SetFloat(animHorizontalHash, horizontal);
            animator.SetFloat(animVerticalHash, vertical);
        }
        ChangeAnimationState(newState);
    }



    public void ChangeToOverHeadWalk()
    {
        ChangeAnimationState(Player_OverHeadWalk);
    }
    public void ChangeToIdle()
    {
        ChangeAnimationState(Player_Idle);
    }
    public void ChangeToLeafWalk()
    {
        ChangeAnimationState(Player_TakeLeafWalk);
    }


}

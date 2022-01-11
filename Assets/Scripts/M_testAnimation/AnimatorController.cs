using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController
{
    public Animator animator;
    private string currentState;
    float horizontal;
    int animHorizontalHash;
    float vertical;
    int animVerticalHash;
    int animPickedHash;
    int animFinishedHash;


    const string Player_Idle = "Idle";

    const string Player_Run = "Run";
    const string Player_RunFaster = "RunFast";

    const string Player_PickUpOverHead = "PickUpOverHead";
    const string Player_PutDown = "PutDown";
    const string Player_OverHeadWalk = "OverHeadWalk";

    public const string Player_Chop = "Chop";

    const string Player_TakeLeaf = "TakeLeaf";

    const string Player_UsingTable = "UsingTable";

    const string Empty = "Empty";


    public void Init()
    {
        animHorizontalHash = Animator.StringToHash("Horizontal");
        animVerticalHash = Animator.StringToHash("Vertical");
        animPickedHash = Animator.StringToHash("Picked");
        animFinishedHash = Animator.StringToHash("Finished");
    }

    public void PlayerIdle()
    {
        ChangeAnimationState(Player_Idle);
    }

    public void PlayerRun(float horizontal, float vertical)
    {
        this.horizontal = horizontal;
        this.vertical = vertical;
        if (this.horizontal != 0 || this.vertical != 0)
        {
            ChangeAnimationState(Player_Run);
            animator.SetFloat(animHorizontalHash, horizontal);
            animator.SetFloat(animVerticalHash, vertical);
        }
    }

    public void PlayerSpeedRun(bool shiftKeyCode)
    {
        if (!shiftKeyCode) return;
        else
            ChangeAnimationState(Player_RunFaster);
    }

    public void PlayerPickUpOverHead(bool crtlKeyCode)
    {
        if (!crtlKeyCode) return;
        else
        {
            ChangeAnimationState(Player_PickUpOverHead);
            animator.SetBool(animPickedHash, true);
        }
    }

    public void PlayerPutDown(bool crtlKeyCode)
    {
        if (!crtlKeyCode) return;
        else
        {
            ChangeAnimationState(Player_PutDown);
            animator.SetBool(animPickedHash, false);
        }
    }

    public void PlayerOverHeadWalk()
    {
            ChangeAnimationState(Player_OverHeadWalk);
    }

    public void PlayerCutTree(bool crtlKeyCode)
    {
        if (!crtlKeyCode) return;
        else
            ChangeAnimationState(Player_Chop);
    }

    public void PlayerTakeLeaf(bool crtlKeyCode)
    {
        if (!crtlKeyCode) return;
        else
            ChangeAnimationState(Player_TakeLeaf);
    }

    public void PlayerUseTable(bool crtlKeyCode)
    {
        if (!crtlKeyCode) return;
        else
            ChangeAnimationState(Player_UsingTable);
    }

    public void DoEmpty()
    {
        ChangeAnimationState(Empty);
    }

    public bool CheckFinish()
    {
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            return false;
        }
        return true;
    }


    public void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;
        animator.Play(newState);
        currentState = newState;
    }
}

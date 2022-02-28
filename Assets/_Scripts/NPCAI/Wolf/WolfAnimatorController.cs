using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfAnimatorController : MonoBehaviour
{
    [HideInInspector]
    public Animator animator;

    //parameters
    [HideInInspector] int turnForceHash;
    [HideInInspector] int moveForceHash;
    [HideInInspector] public string runTrigger = "Run";
    [HideInInspector] public string jumpTrigger = "Jump";

    //animation name
    [HideInInspector] public string attacked = "Attacked";
    [HideInInspector] public string breakBox = "BreakBox";
    [HideInInspector] public string catchT = "Catch";
    [HideInInspector] public string jump = "Jump";

    private string currentState;

    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
        turnForceHash = Animator.StringToHash("turnForce");
        moveForceHash = Animator.StringToHash("moveForce");
    }

    public void ChangeAndPlayAnimation(string state, float turnForce, float moveForce)
    {
        Debug.Log("npc play animation" + state + turnForce + " / " + moveForce);

        if (state == currentState)
        {
            animator.SetFloat(turnForceHash, turnForce);
            animator.SetFloat(moveForceHash, moveForce);
            return;
        }

        currentState = state;
        
        if (state == runTrigger || state == jumpTrigger)
        {
            animator.SetFloat(turnForceHash, turnForce);
            animator.SetFloat(moveForceHash, moveForce);
            animator.SetTrigger(runTrigger);
        }

        if (state == attacked || state == breakBox || state == catchT)
        {
            animator.SetFloat(turnForceHash, turnForce);
            animator.SetFloat(moveForceHash, moveForce);
            animator.Play(state);
        }
    }

    private bool AllowToChange()
    {
        AnimatorStateInfo nowPlaying = animator.GetCurrentAnimatorStateInfo(0);

        bool aniLock = !nowPlaying.IsName(attacked) && !nowPlaying.IsName(breakBox) &&
                       !nowPlaying.IsName(catchT) && !nowPlaying.IsName(jump);

        return aniLock;
    }

    #region ANIMA EVENT
    public void AnimaEventToRun()
    {
        Debug.Log("npc back to run");
        ChangeAndPlayAnimation(runTrigger, 1, 1);
    }

    public bool BreakingOrNot()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(catchT) || animator.GetCurrentAnimatorStateInfo(0).IsName(breakBox))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion
}

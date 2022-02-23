using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxAnimatorController : MonoBehaviour
{
    [HideInInspector]
    public Animator animator;

    //parameters
    [HideInInspector] int turnForceHash;
    [HideInInspector] int moveForceHash;
    [HideInInspector] public string trotTrigger = "Trot";
    [HideInInspector] public string runTrigger = "Run";
    [HideInInspector] public string jumpTrigger = "Jump";


    //animation name
    [HideInInspector] public string breaking = "Fox_Dig";
    [HideInInspector] public string attacked = "Fox_Damaged";
    [HideInInspector] public string stun = "Fox_Stun";
    [HideInInspector] public string jump = "Fox_Jump_InPlace";
    
    private string currentState;

    public GameObject meshes;

    private void Start()
    {
        animator = this.GetComponent<Animator>();
        turnForceHash = Animator.StringToHash("turnForce");
        moveForceHash = Animator.StringToHash("moveForce");

        Debug.Log("npc start" + turnForceHash + " / " + moveForceHash);
    }

    public void ChangeAndPlayAnimation(string state, float turnForce, float moveForce)
    {
        Debug.Log("npc play animation" + turnForce + " / " + moveForce);
        if(state == currentState)
        {
            return;
        }

        currentState = state;
       
        if (state == trotTrigger)
        {
            animator.SetTrigger(trotTrigger);
            animator.SetFloat(turnForceHash, turnForce);
            animator.SetFloat(moveForceHash, moveForce);

        }

        if (state == runTrigger)
        {
            animator.SetTrigger(runTrigger);
            animator.SetFloat(turnForceHash, turnForce);
            animator.SetFloat(moveForceHash, moveForce);

        }

        if (state == jumpTrigger)
        {
            animator.SetTrigger(jumpTrigger);
            animator.SetFloat(turnForceHash, turnForce);
            animator.SetFloat(moveForceHash, moveForce);
        }

        if (state == breaking || state == attacked)
        {
            animator.Play(state);
            animator.SetFloat(turnForceHash, turnForce);
            animator.SetFloat(moveForceHash, moveForce);
        }
    }

    public bool AllowToChange()
    {
        AnimatorStateInfo nowPlaying = animator.GetCurrentAnimatorStateInfo(0);

        if (!AllowToMove())
        {
            return false;
        }

        if (nowPlaying.IsName(attacked) || nowPlaying.IsName(stun) || nowPlaying.IsName(jump))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    //jump end or not
    public bool AvoidAttactEnd()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(jump))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private bool CheckAnimaPlayingOrNot()
    {
        bool animaPlaying = animator.GetCurrentAnimatorStateInfo(0).IsName("Fox Crawl")
                            || animator.GetCurrentAnimatorStateInfo(0).IsName(breaking)
                            || animator.GetCurrentAnimatorStateInfo(0).IsName(attacked)
                            || animator.GetCurrentAnimatorStateInfo(0).IsName(stun);

        return animaPlaying;
    }
    

    public bool AllowToMove()
    {
        if (CheckAnimaPlayingOrNot() == true)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void AnimaEventToRun()
    {
        Debug.Log("npc back to run");
        ChangeAndPlayAnimation(runTrigger, 1, 1);
    }

    public bool BreakingOrNot()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(breaking))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OpenMeshToggle()
    {
        if (meshes.activeSelf == true)
        {
            meshes.SetActive(false);
        }
        else
        {
            meshes.SetActive(true);
        }
    }
}

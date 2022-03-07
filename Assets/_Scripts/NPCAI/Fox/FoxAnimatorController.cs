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
    [HideInInspector] public string homeTrigger = "Home";

    //animation name
    [HideInInspector] public string breaking = "Fox_Dig";
    [HideInInspector] public string attacked = "Fox_Damaged";
    [HideInInspector] public string stun = "Fox_Stun";
    [HideInInspector] public string jump = "Fox_Jump_InPlace";
    [HideInInspector] public string idle = "Idle";
    [HideInInspector] public string run = "RunBlendTree";


    private string currentState;

    public GameObject meshes;

    private void Start()
    {
        animator = this.GetComponent<Animator>();
        turnForceHash = Animator.StringToHash("turnForce");
        moveForceHash = Animator.StringToHash("moveForce");
    }

    public void ChangeAndPlayAnimation(string state, float turnForce, float moveForce)
    {
        Debug.Log("fox play animation" +state + turnForce + " / " + moveForce + $"current{animator.GetCurrentAnimatorStateInfo(0).IsName(state)}");
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(state) == true)
        {
            Debug.Log($"fox animator stare{state} cuuent state-{currentState}");
            animator.SetFloat(turnForceHash, turnForce);
            animator.SetFloat(moveForceHash, moveForce);
            return;
        }
        else if (state == attacked && currentState == state)
        {
            animator.SetFloat(turnForceHash, turnForce);
            animator.SetFloat(moveForceHash, moveForce);
            return;
        }

        currentState = state;
       
        if (state == trotTrigger)
        {
            animator.SetFloat(turnForceHash, turnForce);
            animator.SetFloat(moveForceHash, moveForce);
            animator.SetTrigger(trotTrigger);
        }

        if (state == runTrigger)
        {
            animator.SetFloat(turnForceHash, turnForce);
            animator.SetFloat(moveForceHash, moveForce);
            animator.SetTrigger(runTrigger);
        }

      

        if (state == breaking || state == attacked || state == idle)
        {
            animator.SetFloat(turnForceHash, turnForce);
            animator.SetFloat(moveForceHash, moveForce);
            animator.Play(state);
        }

        if (state == homeTrigger)
        {
            animator.SetFloat(turnForceHash, turnForce);
            animator.SetFloat(moveForceHash, moveForce);
            animator.SetTrigger(homeTrigger);
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

    private bool PlayingIdle()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(idle))
        {
            return true;
        }
        else
        {
            return false;
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

    private void AnimaEventInactivate()
    {
        if (this.gameObject.activeSelf == true)
        {
            this.gameObject.SetActive(false);
        }
    }
}

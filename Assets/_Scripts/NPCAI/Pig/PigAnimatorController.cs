using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigAnimatorController : MonoBehaviour
{
    [HideInInspector]
    public Animator animator;

    //parameters
    [HideInInspector] int turnForceHash;
    [HideInInspector] int moveForceHash;
    [HideInInspector] public string runTrigger = "Run";
    [HideInInspector] public string walkTrigger = "Walk";

    //animation name
    [HideInInspector] public string spin = "Spin";
    [HideInInspector] public string idle = "Idle";
    [HideInInspector] public string shake = "SmallShake";

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
        Debug.Log("pig play animation" + state + turnForce + " / " + moveForce);

        if (animator.GetCurrentAnimatorStateInfo(0).IsName(state))
        {
            animator.SetFloat(turnForceHash, turnForce);
            animator.SetFloat(moveForceHash, moveForce);
            return;
        }

        currentState = state;

        if (state == runTrigger || state == walkTrigger)
        {
            animator.SetFloat(turnForceHash, turnForce);
            animator.SetFloat(moveForceHash, moveForce);
            animator.SetTrigger(state);
        }

        if (state == spin || state == idle || state == shake)
        {
            animator.SetFloat(turnForceHash, turnForce);
            animator.SetFloat(moveForceHash, moveForce);
            animator.Play(state);
        }
    }
}

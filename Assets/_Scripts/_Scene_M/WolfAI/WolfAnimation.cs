using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfAnimation : MonoBehaviour
{
    Animator animator;
    Rigidbody rigidbody;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
    }

    public void PlaySneak()
    {
        animator.Play("Wolfr_Sneak_Forward 0");
    }

    public void ApplyRootMotion()
    {
        animator.applyRootMotion = true;
    }

    public void CancelRooMotion()
    {
        animator.applyRootMotion = false;
    }

    public void AddForce()
    {
        Vector3 go = transform.forward * 9.0f + transform.up * 12.0f;
        rigidbody.AddForce(go, ForceMode.Impulse);
    }

    public void UpdateNewPosition()
    {
        animator.rootPosition = transform.position;
    }

    //void OnAnimatorMove()
    //{
    //    Animator animator = GetComponent<Animator>();

    //    if (animator.GetCurrentAnimatorStateInfo(0).IsName("Wolfr_Sneak_Forward 0"))
    //    {
    //        animator.applyRootMotion = true;
    //    }
    //}
}

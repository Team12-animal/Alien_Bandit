using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="UIAnimatorEvent")]
public class UIAnimationControl : ScriptableObject
{
    Animator animator;

    public void StopAnim(Animator animator)
    {
        this.animator = animator;
        animator.SetTrigger("StopAnim");
    }
}

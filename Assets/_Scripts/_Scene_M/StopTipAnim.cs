using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopTipAnim : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] UIAnimationControl animationControl;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Use1"))
        {
            animationControl.StopAnim(animator);
        }
    }

}

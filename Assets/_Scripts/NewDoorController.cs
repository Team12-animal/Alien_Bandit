using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewDoorController : MonoBehaviour
{
    Animator animator;
    private DoorState currentState;

    public bool IsDoorOpen
    {
        get
        {
            return CurrentState == DoorState.Open;
        }
    }

    public bool IsDoorClosed
    {
        get
        {
            return CurrentState == DoorState.Closed;
        }
    }

    public enum DoorState
    {
        Open,
        Closed
    }

    public DoorState CurrentState
    {
        get
        {
            return currentState;
        }
        set
        {
            currentState = value;
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("door animator init failed.");
            return;
        }
    }

    private void Start()
    {
        currentState = DoorState.Closed;
        var clip = GetCurrentAnimation();
        animator.speed = 9999;
        animator.Play(clip);
    }

    public void ToggleDoor()
    {
        if (IsDoorOpen)
        {
            CloseDoor();
        }
        else
        {
            OpenDoor();
        }
    }

    public void OpenDoor()
    {
        if (IsDoorOpen)
        {
            return;
        }

        CurrentState = DoorState.Open;
        var clip = GetCurrentAnimation();
        animator.speed = 1;
        animator.Play(clip);
    }

    public void CloseDoor()
    {
        if (IsDoorClosed)
        {
            return;
        }

        CurrentState = DoorState.Closed;
        var clip = GetCurrentAnimation();
        animator.speed = 1;
        animator.Play(clip);
    }

    private string GetCurrentAnimation()
    {
        return CurrentState.ToString();
    }
}

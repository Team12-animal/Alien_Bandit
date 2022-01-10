using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Rigidbody rb;
    private Animator animator;
    [SerializeField] float speed = 3.0f;
    [SerializeField] float rotateSpeed = 3.0f;
    [SerializeField] float t = 0.06f;
    float horizotalInput;
    float verticalInput;

    [Header("Animation")]
    private string currentState;
    int animHorizontalHash;
    int animVerticalHash;

    const string Player_RunFaster = "RunFast";
    const string Player_Run = "Run";
    const string Player_Idle = "Idle";

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        animHorizontalHash = Animator.StringToHash("Horizontal");
        animVerticalHash = Animator.StringToHash("Vertical");

    }

    private void Update()
    {
        ControlMoveMent();

        AnimationController();
    }

    public void ControlMoveMent()
    {
        horizotalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        Vector3 movementDirection = new Vector3(horizotalInput, 0.0f, verticalInput);
        movementDirection.Normalize();
        float tempTime = 0;
        tempTime += Time.deltaTime;
        if (tempTime > 0.2f)
            transform.Translate(movementDirection * speed * Time.deltaTime, Space.World);

        if (movementDirection != Vector3.zero)
        {
            transform.forward = Vector3.Slerp(transform.forward, (transform.forward + movementDirection) * rotateSpeed * Time.deltaTime, t);
        }
    }

    public void AnimationController()
    {
        bool pressed = horizotalInput != 0 || verticalInput != 0;
        float pressedShift = Input.GetAxis("LeftShift");

        if (pressed && pressedShift == 0)
        {
            ChangeAnimationState(Player_Run);
            animator.SetFloat(animHorizontalHash, horizotalInput);
            animator.SetFloat(animVerticalHash, verticalInput);
        }
        else if (pressedShift != 0)
            ChangeAnimationState(Player_RunFaster);
        else
        {
            float tempTime = 0;
            tempTime += Time.deltaTime;
            if (tempTime > 0.2f)
                ChangeAnimationState(Player_Idle);
        }
    }

    public void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;
        animator.Play(newState);
        currentState = newState;
    }
}

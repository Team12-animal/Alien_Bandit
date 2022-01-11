using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Rigidbody rb;
    private AnimatorController animatorController;
    [SerializeField] float speed = 3.0f;
    [SerializeField] float rotateSpeed = 3.0f;
    [SerializeField] float t = 0.06f;
    float horizotalInput;
    float verticalInput;

    bool pressedCtrl;
    bool pressedShift;
    private void Awake()
    {
        animatorController = new AnimatorController();
        animatorController.animator = GetComponent<Animator>();
        animatorController.Init();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        ControlMoveMent();
        pressedCtrl = Input.GetButtonDown("LeftCtrl");
        pressedShift = Input.GetButtonDown("LeftShift");
    }

    public void ControlMoveMent()
    {
        horizotalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        Vector3 movementDirection = new Vector3(horizotalInput, 0.0f, verticalInput);
        movementDirection.Normalize();
        float tempTime = 0;
        tempTime += Time.deltaTime;
        if (tempTime > 0.03f)
            transform.Translate(movementDirection * speed * Time.deltaTime, Space.World);
        if (!animatorController.animator.GetBool("Picked"))
            animatorController.PlayerRun(horizotalInput, verticalInput);

        if (movementDirection != Vector3.zero)
        {
            transform.forward = Vector3.Slerp(transform.forward, (transform.forward + movementDirection) * rotateSpeed * Time.deltaTime, t);
        }

        if (pressedShift)
        {
            transform.Translate(movementDirection * speed * 2f * Time.deltaTime, Space.World);
            animatorController.PlayerSpeedRun(pressedShift);
        }

    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Rock" && pressedCtrl)
        {
            animatorController.PlayerPickUpOverHead(pressedCtrl);
        }
        else if (collision.gameObject.tag == "Tree" && pressedCtrl)
        {
            animatorController.PlayerCutTree(pressedCtrl);
        }
        else if (collision.gameObject.tag == "Leaf" && pressedCtrl)
        {
            animatorController.PlayerTakeLeaf(pressedCtrl);
        }
        else if (collision.gameObject.tag == "WorkingTable" && pressedCtrl)
        {
            animatorController.PlayerUseTable(pressedCtrl);
        }
        else if (collision.gameObject.tag == "Tarrain" && !animatorController.animator.GetBool("Picked"))
        {
            animatorController.PlayerRun(horizotalInput, verticalInput);
        }
        else if (animatorController.animator.GetBool("Picked") && (horizotalInput != 0 || verticalInput != 0))
        {
            animatorController.PlayerOverHeadWalk();
        }
        else if (animatorController.animator.GetBool("Picked") && pressedCtrl)
        {
            animatorController.PlayerPutDown(pressedCtrl);
        }
    }


}

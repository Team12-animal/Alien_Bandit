using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockMovement : MonoBehaviour
{
    private Rigidbody rb;
    public float force = 4.0f;
    public bool beUsing = false;
    public bool touchingGround = false;

    private void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (beUsing == true)
        {
            TurnBeUsingToFalse();
        }
    }

    public void Move(GameObject player)
    {
        Vector3 hitStart = player.transform.position;
        Vector3 hitEnd = this.transform.position;

        Vector3 hitSt = hitEnd - hitStart;

        rb.velocity = hitSt * force;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (beUsing == false)
        {
            return;
        }

        if (other.gameObject.tag == "Fox")
        {
            other.GetComponent<Fox_BehaviourTree>().hitten = true;
        }

        if (other.gameObject.tag == "Wolf")
        {
            other.GetComponent<Wolf_BehaviourTree>().hitten = true;
        }
    }

    private void TurnBeUsingToFalse()
    {
        if (touchingGround && rb.velocity == new Vector3(0.0f, 0.0f, 0.0f))
        {
            beUsing = false;
        }
    }
}

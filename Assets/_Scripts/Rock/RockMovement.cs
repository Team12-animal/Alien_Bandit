using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockMovement : MonoBehaviour
{
    private Rigidbody rb;
    public float force = 4.0f;
    public bool beUsing = false;

    private void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    public void Move(GameObject player)
    {
        Vector3 hitStart = player.transform.position;
        Vector3 hitEnd = this.transform.position;

        Vector3 hitSt = hitEnd - hitStart;

        rb.velocity = hitSt * force;
    }
}

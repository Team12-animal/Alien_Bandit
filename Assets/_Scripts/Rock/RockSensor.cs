using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSensor : MonoBehaviour
{
    public GameObject targetRock;
    private RockMovement rm;
    private GameObject parent;
    private Collider c;

    private void Start()
    {
        parent = this.transform.parent.gameObject;
        targetRock = parent.GetComponent<RockCollider>().targetRock;
        rm = targetRock.GetComponent<RockMovement>();
        c = this.GetComponent(typeof(Collider)) as Collider;
    }

    private void Update()
    {
        //OnGroundTest();
        ColliderOpenerToggle();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            rm.Move(collision.gameObject);
        }
    }

    private void OnGroundTest()
    {
        Vector3 from = this.transform.position;
        Vector3 dir = -this.transform.up;

        if(Physics.Raycast(from, dir, 4.5f, 1 << 7))
        {
            c.enabled = true;
        }
        else
        {
            c.enabled = false;
        }
    }


    public void ColliderOpenerToggle()
    {
        if (rm.beUsing == true)
        {
            c.enabled = false;
        }
        else
        {
            c.enabled = true;
        }
    }
}

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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            rm.Move(collision.gameObject);
        }
    }

    //private void OnCollisionStay(Collision collision)
    //{
    //    if (collision.gameObject.layer == 7)
    //    {
    //        c.enabled = true;
    //    }
    //    else
    //    {
    //        c.enabled = false;
    //    }
    //}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSensor : MonoBehaviour
{
    public GameObject targetRock;
    private RockMovement rm;
    private GameObject parent;

    private void Start()
    {
        parent = this.transform.parent.gameObject;
        targetRock = parent.GetComponent<RockCollider>().targetRock;
        rm = targetRock.GetComponent<RockMovement>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            rm.Move(collision.gameObject);
        }
    }
}

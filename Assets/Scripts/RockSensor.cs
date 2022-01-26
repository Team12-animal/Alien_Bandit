using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSensor : MonoBehaviour
{
    public GameObject targetRock;
    private RockMovement rm;

    private void Start()
    {
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

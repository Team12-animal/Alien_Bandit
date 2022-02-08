using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSensor : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<PlayerData>().inTree = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerData>().inTree = false;
        }
    }
}

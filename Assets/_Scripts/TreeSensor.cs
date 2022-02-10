using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSensor : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            PlayerData data = other.GetComponent<PlayerData>();
            data.inTree = true;
            data.tree = this.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerData data = other.GetComponent<PlayerData>();
            data.inTree = false;
            data.tree = null;
        }
    }
}

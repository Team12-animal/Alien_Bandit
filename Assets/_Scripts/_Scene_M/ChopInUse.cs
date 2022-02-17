using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChopInUse : MonoBehaviour
{
    public bool used;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            used = true;
        }
        else
        {
            used = false;
        }
    }
}

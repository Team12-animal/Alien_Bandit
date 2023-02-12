using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChopInUse : MonoBehaviour
{
    public GameObject chopPos;
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

    public void BackToHome()
    {
        this.transform.position = chopPos.transform.position;
        this.transform.rotation = chopPos.transform.rotation;
    }
}

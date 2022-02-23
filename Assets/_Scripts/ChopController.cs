using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChopController : MonoBehaviour
{
    Vector3 oriPos;
    Quaternion oriRot;

    private void Start()
    {
        oriPos = this.transform.position;
        oriRot = this.transform.rotation;
    }

    public void BackToOriPlace()
    {
        this.transform.position = oriPos;
        this.transform.rotation = oriRot;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleFollowTarget : MonoBehaviour
{
    public GameObject followTarget;
    Vector3 offsetPosition = new Vector3(0f, 3f, -3f);

    void Update()
    {
        if (followTarget != null)
            transform.position = followTarget.transform.position + offsetPosition;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderSensor : MonoBehaviour
{
    public GameObject target;

    private void Start()
    {
    }

    private void LateUpdate()
    {
        FollowTarget(target);
    }
    private void FollowTarget(GameObject target)
    {
        this.transform.position = target.transform.position;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockCollider : MonoBehaviour
{
    public GameObject targetRock;

    private void LateUpdate()
    {
        FollowTarget(targetRock);
    }
    private void FollowTarget(GameObject target)
    {
        this.transform.position = target.transform.position;
    }
}

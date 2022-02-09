using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockCollider : MonoBehaviour
{
    public GameObject targetRock;

    private void Start()
    {
        DetectTarget();
    }

    private void LateUpdate()
    {
        FollowTarget(targetRock);
    }
    private void FollowTarget(GameObject target)
    {
        this.transform.position = target.transform.position;
    }

    private void DetectTarget()
    {
        Vector3 from = this.transform.position;
        from.y += 2.0f;
        Vector3 to = -this.transform.up;

        RaycastHit hit;

        if (Physics.Raycast(from, to, out hit, Mathf.Infinity, 1 << 12))
        {
            targetRock = hit.collider.gameObject;
        }
    }
}

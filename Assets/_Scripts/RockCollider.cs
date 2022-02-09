using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockCollider : MonoBehaviour
{
    public GameObject targetRock;
    private Collider childCollider;
    

    private void Start()
    {
        DetectTarget();
        childCollider = this.transform.Find("RockCollider").GetComponent(typeof(Collider)) as Collider;
    }

    private void LateUpdate()
    {
        FollowTarget(targetRock);
        DetectLeavingGroud();
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

    private void DetectLeavingGroud()
    {
        Vector3 from = this.transform.position;
        Vector3 to = -this.transform.up;

        if (Physics.Raycast(from, to, 0.8f, 1 << 7))
        {
            OpenCollider();
        }
        else
        {
            CloseCollider();
        }
    }

    public void OpenCollider()
    {
        childCollider.enabled = true;
        Debug.Log("colliderToggle" + childCollider.enabled);
    }

    public void CloseCollider()
    {
        childCollider.enabled = false;
        Debug.Log("colliderToggle" + childCollider.enabled);
    }
}

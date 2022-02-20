using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fox_AIData : MonoBehaviour
{
    public int status = (int)FoxStatus.Safe;

    public GameObject dirTarget;

    public float maxSpeed;
    public float realSpeed;
    public float maxRot;
    public float realRot;

    //dist to player to enter alert status
    public float alertDist;

    //probe for checking hit collider or not
    public float probeLength;

    public enum FoxStatus
    {
        Safe,
        Alert,
        Attacked,
        Home
    }

    public void Start()
    {
        realSpeed = maxSpeed;
        realRot = maxRot;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(this.transform.position, alertDist);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(this.transform.position, this.transform.position + this.transform.position * probeLength);
    }
}

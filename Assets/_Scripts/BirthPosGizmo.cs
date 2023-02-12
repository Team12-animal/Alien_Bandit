using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirthPosGizmo : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(this.transform.position, 1.3f);
    }
}

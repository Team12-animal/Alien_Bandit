using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigNode : MonoBehaviour
{
    public bool openGizmo;
    public List<GameObject> groupMember;

    private void OnDrawGizmos()
    {
        if (openGizmo)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(this.transform.position, 1.2f);
            if (groupMember != null && groupMember.Count > 0)
            {
                foreach (GameObject m in groupMember)
                {
                    Gizmos.color = Color.gray;
                    Gizmos.DrawSphere(m.transform.position, 1.2f);
                }
            }
        }
    }
}

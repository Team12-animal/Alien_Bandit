using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WP : MonoBehaviour
{
    public List<GameObject> neibors;

    private void OnDrawGizmos()
    {
        if (neibors != null && neibors.Count > 0)
        {
            foreach (GameObject n in neibors)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(this.transform.position, n.transform.position);
            }
        }
    }
}

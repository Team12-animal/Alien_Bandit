using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrownMovement : MonoBehaviour
{
    public bool lower = false;
    public float moveAmt;

    private float maxY;
    private float minY;

    // Start is called before the first frame update
    void Start()
    {
        maxY = this.transform.localPosition.y + 0.1f;
        minY = this.transform.localPosition.y - 0.1f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (this.transform.localPosition.y >= maxY)
        {
            lower = true;
        }
        else if (this.transform.localPosition.y <= minY)
        {
            lower = false;
        }

        if (lower)
        {
            this.transform.localPosition += new Vector3(0, -moveAmt, 0);
        }
        else
        {
            this.transform.localPosition += new Vector3(0, moveAmt, 0);
        }
    }
}

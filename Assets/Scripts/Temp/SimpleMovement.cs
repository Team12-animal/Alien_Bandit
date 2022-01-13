using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMovement : MonoBehaviour
{
    public float maxSpeed;
    public float maxRotate;

    Transform tp; // player transform
    Vector3 pos;
    Vector3 fVec;
    Vector3 rVec;

    // Start is called before the first frame update
    void Start()
    {
        tp = this.transform;
        pos = tp.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            MoveAndRotate();
        }
    }

    private void MoveAndRotate()
    {
        //Rotate
        float rot = Input.GetAxis("Horizontal") * maxRotate * Time.deltaTime;
        this.transform.Rotate(0.0f, rot, 0.0f);

        //Move
        float transAmt = Input.GetAxis("Vertical");
        fVec = tp.forward * maxSpeed * transAmt * Time.deltaTime;

        this.transform.position += fVec;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(this.transform.position, this.transform.position + this.transform.forward * 2);
    }
}

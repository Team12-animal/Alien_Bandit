using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float maxSpeed;
    public float maxRotate;

    Vector3 fVec;

    // Start is called before the first frame update
    void Start()
    {
        //if(this.gameObject.name == "player")
        //{
        //    Debug.Log("found");

        //}
        //else
        //{
        //    Debug.Log("not found");
        //}
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveAndRotate(float transAmt, float rotAmt)
    {
        //Debug.Log("MoveAndRotate");
        //Debug.Log("h2" + transAmt);
        //Debug.Log("v2" + rotAmt);

        //Rotate
        float rot = Input.GetAxis("Horizontal") * maxRotate * Time.deltaTime;
        this.transform.Rotate(0.0f, rot, 0.0f);

        //Move
        Vector3 fVec = this.transform.forward * maxSpeed * transAmt * Time.deltaTime;

        this.transform.position += fVec;
        //Debug.Log("trans" + this.transform.position);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(this.transform.position, this.transform.position + this.transform.forward * 2);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float maxSpeed;
    public float maxRotate;

    Vector3 pos;
    Vector3 dir;

    // Start is called before the first frame update
    void Start()
    {
        pos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move(float transAmt, float rotAmt)
    {
        Vector3 fVec = transform.forward * maxSpeed * transAmt * Time.deltaTime;
        Vector3 rVec = transform.right * maxRotate * rotAmt * Time.deltaTime;
        
        dir = fVec + rVec;
        dir.y = 0;

        this.transform.position += dir;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.collider.name);
    }

}

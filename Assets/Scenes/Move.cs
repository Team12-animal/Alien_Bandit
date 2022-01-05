using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float speed = 10f;
    public float turnSpeed = 20f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float H = Input.GetAxis("Horizontal");
        float V = Input.GetAxis("Vertical");
       
        if (H!=0||V != 0)
        {
            MovePlayer(H, V);
        }
        
    }

    private void MovePlayer(float H,float V)
    {
        transform.position += transform.forward * V * speed * Time.deltaTime;

        float turn = H * turnSpeed * Time.deltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        transform.rotation *= turnRotation;

    }

    private void OnCollisionEnter(Collision collision)
    {
        
        Debug.Log(collision.collider.name);
    }
}

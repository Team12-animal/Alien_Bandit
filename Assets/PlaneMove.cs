using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneMove : MonoBehaviour
{

    public Vector3[] point;
    int currentPoint = 0;
    public float speed = 0.001f;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        ChangePoint();
    }

    void ChangePoint()
    {
        if (currentPoint == 0)
        {
            transform.position = Vector3.Lerp(transform.position, point[1], speed);
            float dist = (transform.position - point[1]).magnitude;
            if (dist < 0.1f)
            {
                currentPoint = 1;
                StartCoroutine(WaitSecond());
            }        
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, point[0], speed);
            float dist = (transform.position - point[0]).magnitude;
            if (dist < 0.1f)
            {
                currentPoint = 0;
                StartCoroutine(WaitSecond());
            }
        }
    }

    IEnumerator WaitSecond()
    {
        int i = Random.Range(3,5);
        yield return new WaitForSeconds(i);
    }
}

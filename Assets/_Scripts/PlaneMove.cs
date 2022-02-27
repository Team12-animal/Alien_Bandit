using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneMove : MonoBehaviour
{

    public Vector3[] point;
    int currentPoint = 0;
    public float speed = 0.001f;
    private Vector3 LastPos;
    private Vector3 CurrentPos;
    private Vector3 DifPos;
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
            LastPos = transform.position;
            CurrentPos = Vector3.Lerp(transform.position, point[1], speed);
            transform.position = CurrentPos;
            DifPos = LastPos - CurrentPos;
            float dist = (transform.position - point[1]).magnitude;
            if (dist < 0.1f)
            {
                currentPoint = 1;
                StartCoroutine(WaitSecond());
            }
        }
        else
        {
            LastPos = transform.position;
            CurrentPos = Vector3.Lerp(transform.position, point[0], speed);
            transform.position = CurrentPos;
            DifPos = LastPos - CurrentPos;
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
        int i = Random.Range(3, 5);
        yield return new WaitForSeconds(i);
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.tag == "Player")
    //    {
    //        if (currentPoint == 0)
    //        {
    //            other.transform.position += DifPos;
    //        }
    //        else
    //        {
    //            other.transform.position += DifPos;
    //        }
    //    }
    //}
}

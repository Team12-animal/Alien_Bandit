using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneMove : MonoBehaviour
{

    public Vector3[] point;
    int currentPoint = 0;
    private float speed ;
    private Vector3 LastPos;
    private Vector3 CurrentPos;
    private Vector3 DifPos;
    public  bool isMove=false;
    private ButtonSensor buttonSensor;
    // Start is called before the first frame update
    void Start()
    {
        buttonSensor = GameObject.Find("DoorOpener").GetComponent<ButtonSensor>();
    }

    // Update is called once per frame
    void Update()
    {
        isMove = buttonSensor.GetPressedBool();
        if (currentPoint==1 || isMove)
        {
            ChangePoint();
        }       
    }

    void ChangePoint()
    {
        if (currentPoint == 0)
        {
            LastPos = transform.position;
            speed += 0.05f * Time.deltaTime;
            CurrentPos = Vector3.Lerp(transform.position, point[1], speed);
            transform.position = CurrentPos;
            DifPos = LastPos - CurrentPos;
            float dist = (transform.position - point[1]).magnitude;
            if (dist < 0.1f)
            {
                Debug.LogError("往前");
                currentPoint = 1;
                speed = 0;
                StartCoroutine(WaitThreeSecond());
            }
        }
        else
        {
            LastPos = transform.position;
            speed += 0.05f * Time.deltaTime;
            CurrentPos = Vector3.Lerp(transform.position, point[0], speed);
            transform.position = CurrentPos;
            DifPos = LastPos - CurrentPos;
            float dist = (transform.position - point[0]).magnitude;
            if (dist < 0.1f)
            {
                Debug.LogError("往後");
                currentPoint = 0;
                speed= 0;
                StartCoroutine(WaitThreeSecond());
            }
        }
    }

    IEnumerator WaitThreeSecond()
    {
        yield return new WaitForSeconds(3f);
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

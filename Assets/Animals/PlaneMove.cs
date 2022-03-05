using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneMove : MonoBehaviour
{

    public Vector3[] point;
    int currentPoint = 0;
    private float speed;
    private Vector3 LastPos;
    private Vector3 CurrentPos;
    private Vector3 DifPos;
    public bool isMove = false;
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
        if (currentPoint == 1 || isMove)
        {
            ChangePoint();
        }
    }

    void ChangePoint()
    {
        if (currentPoint == 0)
        {
            Debug.LogError("原點往終點");
            LastPos = transform.position;
            speed += 0.05f * Time.deltaTime;
            CurrentPos = Vector3.Lerp(transform.position, point[1], speed);
            transform.position = CurrentPos;
            DifPos = LastPos - CurrentPos;
            float dist = (transform.position - point[1]).magnitude;
            if (dist < 0.1f)
            {
                currentPoint = 1;
                speed = 0;
                StartCoroutine(WaitThreeSecond());
            }
        }
        else
        {
            Debug.LogError("終點往原點");
            LastPos = transform.position;
            speed += 0.05f * Time.deltaTime;
            CurrentPos = Vector3.Lerp(transform.position, point[0], speed);
            transform.position = CurrentPos;
            DifPos = LastPos - CurrentPos;
            float dist = (transform.position - point[0]).magnitude;
            if (dist < 0.1f)
            {
                currentPoint = 0;
                speed = 0;
                StartCoroutine(WaitThreeSecond());
            }
        }
    }

    IEnumerator WaitThreeSecond()
    {
        yield return new WaitForSeconds(3f);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.gameObject.transform.SetParent(transform);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Player01")
        {
            other.gameObject.transform.SetParent(GameObject.Find("PlayerBox01").transform);
        }
        else if (other.name == "Player02")
        {
            other.gameObject.transform.SetParent(GameObject.Find("PlayerBox02").transform);
        }
        else if (other.name == "Player03")
        {
            other.gameObject.transform.SetParent(GameObject.Find("PlayerBox03").transform);
        }
        else if (other.name == "Player04")
        {
            other.gameObject.transform.SetParent(GameObject.Find("PlayerBox04").transform);
        }
    }
}

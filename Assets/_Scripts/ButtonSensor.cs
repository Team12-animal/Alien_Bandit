using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSensor : MonoBehaviour
{
    public GameObject goalDoor;
    private GameObject player;
    private GameObject button;
    private NewDoorController dc;
    private Collider doorCollider;
    public GameObject goal;
    private Collider goalTrigger;
    [SerializeField]private bool pressed = false;
    private int playerOnButton = 0;

    private void Awake()
    {
        button = this.transform.Find("Button").gameObject;
        dc = goalDoor.GetComponent<NewDoorController>();
        doorCollider = goalDoor.GetComponent(typeof(Collider)) as Collider;
        goalTrigger = goal.GetComponent(typeof(Collider)) as Collider;
    }

    private void Update()
    {
        if (waiting == true)
        {
            waitingEnd = Countdown(startTime, 5);

            if (waitingEnd == true)
            {
                Vector3 bPos = button.transform.position;
                bPos.y += 0.2f;
                button.transform.position = bPos;
                dc.ToggleDoor();
                player = null;
                doorCollider.enabled = true;
                goalTrigger.enabled = false;
                pressed = false;
                waiting = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            playerOnButton += 1;

            if(pressed == false)
            {
                player = other.gameObject;
                Vector3 bPos = button.transform.position;
                bPos.y -= 0.2f;
                button.transform.position = bPos;
                POnButton();
                doorCollider.enabled = false;
                goalTrigger.enabled = true;
                pressed = true;
            }

            Debug.Log($"p on button enter{playerOnButton}");
        }
    }

    bool waiting = false;
    bool waitingEnd = false;
    float startTime;

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" && pressed == true)
        {
            playerOnButton -= 1;

            if (playerOnButton == 0)
            {
                if (waiting == false)
                {
                    waiting = true;
                    startTime = Time.time;
                }
            }
        }

        Debug.Log($"p on button exit{playerOnButton}");
    }

    private bool Countdown(float startTime, float waitTime)
    {
        if (Time.time >= (startTime + waitTime))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void POnButton()
    {
        dc.ToggleDoor();
    }

    public bool GetPressedBool()
    {
        return pressed;
    }
}

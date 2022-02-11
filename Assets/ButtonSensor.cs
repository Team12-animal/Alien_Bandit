using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BrokenVector.LowPolyFencePack;

public class ButtonSensor : MonoBehaviour
{
    public GameObject goalDoor;
    private GameObject player;
    private GameObject button;
    private DoorController dc;
    private Collider doorCollider;
    public GameObject goal;

    private void Awake()
    {
        button = this.transform.Find("Button").gameObject;
        dc = goalDoor.GetComponent<DoorController>();
        doorCollider = goalDoor.GetComponent(typeof(Collider)) as Collider;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            player = other.gameObject;
            Vector3 bPos = button.transform.position;
            bPos.y -= 0.2f;
            button.transform.position = bPos;
            POnButton();
            doorCollider.enabled = false;
            goal.SetActive(true);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Vector3 bPos = button.transform.position;
            bPos.y += 0.2f;
            button.transform.position = bPos;
            dc.ToggleDoor();
            player = null;
            doorCollider.enabled = true;
            goal.SetActive(false);
        }
    }

    private void POnButton()
    {
        dc.ToggleDoor();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Bird : MonoBehaviour
{
    private void Start()
    {
        //Vector3(326.0625, 254.77655, 0)  need to offset parent position;
        transform.position = new Vector3(890.1f, 260.73f, -1092.16f);
        //Vector3(564.048096, 5.95675659, -1092.16235) right position
    }
    void Update()
    {
        //SeagulPos();

        Vector3 mouse = Input.mousePosition;
        Vector3 offsetPosition = new Vector3(-1.0f, 0.0f, -1.0f);
        Ray castPoint = Camera.main.ScreenPointToRay(mouse);
        RaycastHit hit;
        if (Physics.Raycast(castPoint, out hit, Mathf.Infinity))
        {
            if (hit.transform.tag == "Button")
                transform.position = hit.transform.position + offsetPosition;
        }
    }
    void SeagulPos()
    {
        string current = EventSystem.current.currentSelectedGameObject.name;
        switch (current)
        {
            case "NewGame":
                transform.position = new Vector3(889f, 261.7f, -1092f);
                break;
            case "Continue":
                transform.position = new Vector3(889f, 259.6f, -1092f);
                break;
            case "Setting":
                transform.position = new Vector3(889f, 258f, -1092f);
                break;
            case "Exit":
                transform.position = new Vector3(889f, 256.8f, -1092f);
                break;
            case "":
                break;
        }
    }
}

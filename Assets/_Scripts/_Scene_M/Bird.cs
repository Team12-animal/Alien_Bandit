using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Bird : MonoBehaviour
{


    private void Start()
    {
     
    }

    void Update()
    {
        //SeagulPos();

        //Vector3 mouse = Input.mousePosition;
        //Ray castPoint = Camera.main.ScreenPointToRay(mouse);
        //RaycastHit hit;
        //if (Physics.Raycast(castPoint, out hit, Mathf.Infinity))
        //{
        //    if (hit.transform.tag == "Button")
        //self.transform.position = hit.transform.position + offsetPosition;
        //}
    }
    void SeagulPos()
    {
        string current = EventSystem.current.currentSelectedGameObject.name;
        Debug.LogError(current);
        switch (current)
        {
            case "NewGame":
                Debug.LogError("EnterNewGame");
                transform.position = new Vector3(889f, 261.7f, -1092f);
                break;
            case "Continue":
                Debug.LogError("EnterContinue");
                transform.position = new Vector3(889f, 259.6f, -1092f);
                break;
            case "Setting":
                transform.position = new Vector3(889f, 258f, -1092f);
                break;
            case "Exit":
                transform.position = new Vector3(889f, 256.8f, -1092f);
                break;
        }

    }
}

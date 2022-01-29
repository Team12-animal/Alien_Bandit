using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    Transform self;
    [SerializeField] Vector3 offsetPosition;

    private void Start()
    {
        self = GetComponent<Transform>();
    }

    void Update()
    {
        Vector3 mouse = Input.mousePosition;
        Ray castPoint = Camera.main.ScreenPointToRay(mouse);
        RaycastHit hit;
        if (Physics.Raycast(castPoint, out hit, Mathf.Infinity))
        {
            if (hit.transform.tag == "Button")
                self.transform.position = hit.transform.position + offsetPosition;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeAreaSensor : MonoBehaviour
{
    public GameObject senseCam;
    private Camera cam;
    private CamMovement cm;
    public GameObject go;

    // Start is called before the first frame update
    void Awake()
    {
        senseCam = GameObject.Find("Main Camera");
        cam = senseCam.GetComponent<Camera>();
        cm = senseCam.GetComponent<CamMovement>();

        if(cm == null)
        {
            Debug.Log("cm not found");
        }
        else
        {
            Debug.Log("cm found");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        go = other.gameObject;

        if (go != null)
        {
            cm.GetSABumper(go);
            Debug.Log("bump");
        }
    }
}

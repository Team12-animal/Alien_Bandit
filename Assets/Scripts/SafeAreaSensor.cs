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
    void Start()
    {
        senseCam = GameObject.Find("Main Camera");
        cam = senseCam.GetComponent<Camera>();
        cm = cam.GetComponent<CamMovement>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        go = other.gameObject;
        cm.GetSABumper(go);
        Debug.Log("bump");
    }
}

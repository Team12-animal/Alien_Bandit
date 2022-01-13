using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTest : MonoBehaviour
{
    public GameObject target;
    public GameObject target2;

    // Start is called before the first frame update
    void Start()
    {
        LookTest();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LookTest()
    {
        this.transform.position = target2.transform.position;
        this.transform.LookAt(target.transform);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(this.transform.position, this.transform.position + this.transform.forward * 10);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    private PlayerMovement pm;

    private void Awake()
    {
        pm = GetComponent<PlayerMovement>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float trans = Input.GetAxis("Vertical");
        float rot = Input.GetAxis("Horizontal");

        if (trans != 0 || rot != 0)
        {
            pm.Move(trans, rot);
        }        
    }
}

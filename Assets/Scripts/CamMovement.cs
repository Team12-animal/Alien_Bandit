using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMovement : MonoBehaviour
{
    private static CamMovement instance;

    public GameObject player;
    public float testDist;

    Vector3 dirToTarget;//Player指向Camera的向量
    Vector3 originPPos;
    Vector3 camPos;

    private static CamMovement Instance()
    {
        return instance;
    }

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;

        originPPos = player.transform.position;
        camPos = this.transform.position;

        dirToTarget = camPos - originPPos;
    }

    // Update is called once per frame
    void Update()
    {
        CamFollowing();
    }

    //Camera跟隨Player
    private void CamFollowing()
    {
        string tooClose = FixCamPos();
        Vector3 newPlayerPos = player.transform.position;
        camPos = newPlayerPos + dirToTarget;

        if (tooClose == "closeF")
        {
            if(camPos.z >= fixedPos)
            {
                camPos.z = fixedPos;
                Debug.Log("FixF");
            }
        }

        if (tooClose == "closeF")
        {
            if(camPos.x >= fixedPos)
            {
                camPos.x = fixedPos;
            }
        }

        if (tooClose == "closeR")
        {
            if(camPos.x <= fixedPos)
            {
                camPos.x = fixedPos;
            }
        }

        if (tooClose == "keepGoing" || tooClose == "closeB")
        { 
        }

        this.transform.position = camPos;
    }

    private float fixedPos;

    private string FixCamPos()
    {
        //Debug.Log("FixCamPos");

        Vector3 camPos = this.transform.position;
        
        Vector3 front = this.transform.forward;
        Vector3 back = -front;
        Vector3 right = this.transform.right;
        Vector3 left = -right;

        LayerMask mask = LayerMask.GetMask("OutLine");

        string result = "keepGoing";

        if (Physics.Raycast(camPos, front, testDist, mask))
        {
            fixedPos = camPos.z;
            result = "closeF";
        }

        if(Physics.Raycast(camPos, back, testDist, mask))
        {
            fixedPos = camPos.z;
            result = "closeB";
        }
        
        if(Physics.Raycast(camPos, right, testDist, mask))
        {
            fixedPos = camPos.x;
            result = "closeR";
        }
        
        if(Physics.Raycast(camPos, left, testDist, mask))
        {
            fixedPos = camPos.x;
            result = "closeL";
        }

        Debug.Log("tooclose" + result);
        return result;
    }
}

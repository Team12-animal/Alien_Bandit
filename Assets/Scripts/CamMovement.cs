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

    //Camera跟隨Player的位置

    private float fixedPos;
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

        if (tooClose == "closeR")
        {
            if(camPos.x >= fixedPos)
            {
                camPos.x = fixedPos;
                Debug.Log("FixR");
            }
        }

        if (tooClose == "closeL")
        {
            if (camPos.x <= fixedPos)
            {
                camPos.x = fixedPos;
                Debug.Log("FixL");
            }
        }

        if (tooClose == "keepGoing" || tooClose == "closeB")
        {
            Debug.Log("keep going");
        }

        this.transform.position = camPos;
    }

    //限定相機範圍
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(this.transform.position, this.transform.position + this.transform.forward * 2);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(this.transform.position, this.transform.position + this.transform.right * 2);
    }
}

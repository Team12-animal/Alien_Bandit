using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMovement : MonoBehaviour
{
    private static CamMovement instance;
    private Camera cam;

    public GameObject[] players;
    public GameObject p1;
    public float effectDist;

    private Vector3 playerPos;
    private Vector3 pScreenPos;
    private Vector3 camPos;

    private bool leaveSafeArea;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;

        cam = GetComponent<Camera>();
        camPos = this.transform.position;
        oriCamPos = camPos;
        players = GameObject.FindGameObjectsWithTag("Player");
        p1 = players[0];
        
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(players.Length == 1)
        {
            P1CamMove();
        }
        else if(players.Length > 1 || players.Length < 4)
        {
            P3CamMove();
        }
        else
        {
            Debug.Log("wrong player amount");
        }
        
    }

    //Camera跟隨Player
    Vector3 dir;
    Vector3 oriCamPos;
    bool startFollow = false;

    private void P1CamMove()
    {
        playerPos = p1.transform.position;

        //離開安全區
        if (leaveSafeArea)
        {
            if (!startFollow)
            {
                dir = camPos - playerPos;
                startFollow = true;
            }
        }
        else
        {
            //camPos = oriCamPos;
            //this.transform.position = camPos;
            SeekOriPos(this.gameObject, oriCamPos);
            startFollow = false;
        }

        Debug.Log("startFollow" + startFollow);

        //鏡頭跟隨
        if (startFollow)
        {
            camPos = playerPos + dir;
            this.transform.position = camPos;

            Debug.Log("playerPos" + playerPos);
            Debug.Log("dir" + dir);
            Debug.Log("campos" + camPos);

        }
    }

    private void SeekOriPos(GameObject go, Vector3 target)
    {
        float speed = p1.GetComponent<PlayerMovement>().maxSpeed;

        Vector3 seekerPos = go.transform.position;
        Vector3 dir = target - seekerPos;

        if (dir.magnitude > 0.5f)
        {
            Debug.Log("seeking" + dir.magnitude);
            dir.Normalize();
            go.transform.position += dir * speed * Time.deltaTime;
        }
        
    }


    private void P3CamMove()
    {
        
    }

    public GameObject GetSABumper(GameObject go)
    {
        if(leaveSafeArea == true)
        {
            leaveSafeArea = false;
        }
        else
        {
            leaveSafeArea = true;
        }

        Debug.Log("bump recived" + go.name);

        return go;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(this.transform.position, this.transform.position + this.transform.forward * 2);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(this.transform.position, this.transform.position + this.transform.right * 2);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMovement : MonoBehaviour
{
    private static CamMovement instance;
    private Camera cam;

    public GameObject[] players;
    public GameObject player;
    public float effectDist;

    private Vector3 playerPos;
    private Vector3 pScreenPos;
    private Vector3 camPos;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;

        cam = GetComponent<Camera>();
        players = GameObject.FindGameObjectsWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(players.Length == 1)
        {
            }
            P1CamMove();
        }
        if (tooClose == "closeL")
        {
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
    bool startFollow = false;

    private void P1CamMove()
    {   
        float h = Screen.height;
        float w = Screen.width;
        
        camPos = this.transform.position;
        playerPos = player.transform.position;
        pScreenPos = cam.WorldToScreenPoint(playerPos);

        Debug.Log("screen" + pScreenPos);

        //enter effect zone
        if (pScreenPos.x < effectDist || pScreenPos.x > (w - effectDist) || pScreenPos.y < effectDist || pScreenPos.y > (h - effectDist))
        {
            if (!startFollow)
            {
                dir = camPos - playerPos;
                startFollow = true;
            }
        }
        else
        {
            startFollow = false;
        }

        Debug.Log("startFollow" + startFollow);

        if (startFollow)
        {
            camPos = playerPos + dir;
            this.transform.position = camPos;

            Debug.Log("playerPos" + playerPos);
            Debug.Log("dir" + dir);
            Debug.Log("campos" + camPos);

        }
    }

    private void P3CamMove()
    {

    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(this.transform.position, this.transform.position + this.transform.forward * 2);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(this.transform.position, this.transform.position + this.transform.right * 2);
    }
}

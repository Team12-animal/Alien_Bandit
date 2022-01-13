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
        camPos = this.transform.position;
        players = GameObject.FindGameObjectsWithTag("Player");
    }

    void Start()
    {
        nodes = Node.nodes;
        FindSafeArea();
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

    private GameObject[] nodes;

    public void FindSafeArea()
    {
        float h = Screen.height;
        float w = Screen.width;

        List<Vector3> points = new List<Vector3>();

        Vector3 LDpoint = new Vector3(effectDist, effectDist, cam.nearClipPlane);
        points.Add(LDpoint);
        Vector3 LUpoint = new Vector3(effectDist, h - effectDist, cam.nearClipPlane);
        points.Add(LUpoint);
        Vector3 RUpoint = new Vector3(w - effectDist, h - effectDist, cam.nearClipPlane);
        points.Add(RUpoint);
        Vector3 RDpoint = new Vector3(w - effectDist, effectDist, cam.nearClipPlane);
        points.Add(RDpoint);
        
        for(int i = 0; i < points.Count; i++)
        {
            RaycastHit hit;
            var ray = Camera.main.ScreenPointToRay(points[i]);

            //7 = terrain Layermask
            if(Physics.Raycast(ray, out hit, Mathf.Infinity, 1<<7))
            {
                nodes[i].transform.position = hit.point;
            }
        }

        Debug.Log("safe area found");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(this.transform.position, this.transform.position + this.transform.forward * 2);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(this.transform.position, this.transform.position + this.transform.right * 2);
    }
}

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
            //P1CamMove();
            P1CamMove2();
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

    //Camera跟隨Player(world space safe area)
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

    //screen save area
    private void P1CamMove2()
    {
        float h = Screen.height;
        float w = Screen.width;

        camPos = this.transform.position;
        playerPos = players[0].transform.position;
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
            camPos = FixCamPos(camPos);
            
            this.transform.position = camPos;

            Debug.Log("playerPos" + playerPos);
            Debug.Log("dir" + dir);
            Debug.Log("campos" + camPos);

        }
    }


    private void P3CamMove()
    {
        
    }

    //leave safe arrive or not: check from wall's trigger
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

        return go;
    }

    //how far should cam stop
    public float rayProbe;
    private Vector3 fixedPos;
    private int CheckTerrainEnd()
    {   
        int result;
        //gen rays from cam to the 4 forwords
        List<Ray> camRays = new List<Ray>();
        
        Ray fR = new Ray(this.transform.position, this.transform.forward);
        camRays.Add(fR);
        Ray bR = new Ray(this.transform.position, -this.transform.forward);
        camRays.Add(bR);
        Ray rR = new Ray(this.transform.position, this.transform.right);
        camRays.Add(rR);
        Ray lR = new Ray(this.transform.position, -this.transform.right);
        camRays.Add(lR);

        for(int i = 0; i < camRays.Count; i++)
        {
            if(Physics.Raycast(camRays[i], rayProbe, 1 << 6))
            {
                fixedPos = this.transform.position;
                return result = i;
            }
        }

        return result = -1;
    }

    enum Results
    {
         keepgoing = -1,
         hitF,
         hitB,
         hitR,
         hitL
    }

    private Vector3 FixCamPos(Vector3 dirPos)
    {
        int result = CheckTerrainEnd();
        
        if(result == (int)Results.keepgoing)
        {
            //do noting
        }

        //stop moving forward
        if(result == (int)Results.hitF)
        {
            dirPos.z = Mathf.Min(dirPos.z, fixedPos.z);
        }

        //stop moving backward
        if (result == (int)Results.hitB)
        {
            dirPos.z = Mathf.Max(dirPos.z, fixedPos.z);
        }

        //stop moving right
        if (result == (int)Results.hitR)
        {
            dirPos.x = Mathf.Min(dirPos.x, fixedPos.x);
        }

        //stop moving left
        if (result == (int)Results.hitL)
        {
            dirPos.x = Mathf.Max(dirPos.x, fixedPos.x);
        }

        return dirPos;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(this.transform.position, this.transform.position + this.transform.forward * rayProbe);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(this.transform.position, this.transform.position + this.transform.right * rayProbe);
    }
}

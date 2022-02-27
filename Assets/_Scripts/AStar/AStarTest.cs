using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarTest : MonoBehaviour
{
    public string txtName;
    public string nodeName;
    //from npc
    [SerializeField]public FoxAIData data;
    public int currentPathPt;

    public GameObject target;

    //fromm cMain
    public bool astaring = false;

    Vector3 oriPos;

    // Start is called before the first frame update
    void Start()
    {
        WPTerrain wpt = new WPTerrain();
        wpt.Init(txtName, nodeName);

        AStar astar = new AStar();
        astar.Init(wpt);

        astaring = AStar.instance.PerformAStar(this.transform.position, target.transform.position);
        currentPathPt = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (GetTargetPos() == true)
        {
            astaring = AStar.instance.PerformAStar(this.transform.position, target.transform.position);
            currentPathPt = 0;
        }
        
        if (astaring)
        {
            List<Vector3> path = AStar.instance.GetPath();

            string sPath = "astar main path";

            foreach (Vector3 p in path)
            {
                sPath += p;
            }

            Debug.Log(sPath);

            int final = path.Count - 1;
            int i;
            for (i = final; i >= currentPathPt; i--)
            {
                Vector3 sPos = path[i];
                Vector3 cPos = this.transform.position;

                if (Physics.Linecast(cPos, sPos, 1 << 8))
                {
                    Debug.Log("astar linecast true");
                    continue;
                }

                currentPathPt = i;
                SetTarget(sPos);

                Debug.Log($"astar MAIN:vt {data.m_vTarget} sPos {sPos}");
                break;
            }
        }

        SteeringBehavior.Seek(data);
        SteeringBehavior.Move(data);

    }


    private bool GetTargetPos()
    {
        if (target.transform.position == oriPos)
        {
            return false;
        }
        else
        {
            oriPos = target.transform.position;
            return true;
        }
    }

    private void SetTarget(Vector3 v)
    {
        data.m_vTarget = v;
    }

    private void OnDrawGizmos()
    {
        if (astaring)
        {
            List<Vector3> path = AStar.instance.GetPath();
            Gizmos.color = Color.blue;
            int count = path.Count - 1;

            for (int i = 0; i < count; i++)
            {
                Vector3 sPos = path[i];
                sPos.y += 1.0f;
                Vector3 ePos = path[i + 1];
                ePos.y += 1.0f;
                Gizmos.DrawLine(sPos, ePos);
            }

        }
    }
}

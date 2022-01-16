using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainMarker : MonoBehaviour
{
    private Camera cam;

    private GameObject[] nodes;
    private float effectDist;

    public static TerrainMarker instance;
    private static TerrainMarker Instance()
    {
        return instance;
    }

    public TerrainMarker()
    {
        instance = this;
    }

    private List<GameObject> tContainer;

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        nodes = this.GetComponent<Node>().nodes;
        GameObject mainCam = GameObject.Find("Main Camera");
        cam = mainCam.GetComponent<Camera>();
        effectDist = mainCam.GetComponent<CamMovement>().effectDist;

        if(cam != null)
        {
            Debug.Log("safe area: cam found");
        }
        else
        {
            Debug.Log("safe area: cam not found");
        }

        LoadWallPrefab(nodes.Length);
        SpawnWallCollider();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LoadWallPrefab(int amt)
    {
        Debug.Log("LoadWallPrefab");

        tContainer = new List<GameObject>();

        for(int i = 0; i < amt; i++)
        {
            Debug.Log("enter" + i);
            var prefab = Resources.Load<GameObject>("terrainOutline");
            GameObject wall = GameObject.Instantiate(prefab) as GameObject;
            wall.SetActive(false);
            tContainer.Add(wall);

            Debug.Log("leave" + i);
        }

        Debug.Log("LoadWallPrefab");
    }

    private void SpawnWallCollider()
    {
        Debug.Log("SpawnWallCollider");

        int nodeAmt = nodes.Length;

        Debug.Log("nodeAmt: " + nodeAmt);

        GameObject start;
        GameObject end;
        GameObject wall;

        for (int i = 0; i < nodeAmt; i++)
        {
            int j;

            if (i + 1 == nodeAmt)
            {
                j = 0;
            }
            else if (i + 1 < nodeAmt)
            {
                j = i + 1;
            }
            else
            {
                break;
            }

            start = nodes[i];
            end = nodes[j];
            wall = tContainer[i];
            wall.SetActive(true);

            Vector3 targetLine = end.transform.position - start.transform.position;

            //scale adjustment data
            float width = targetLine.magnitude;
            Vector3 sAdjust = new Vector3(0.0f, 0.0f, -width * 10f);

            Debug.Log("width" + i + ":" + sAdjust);

            wall.transform.position = end.transform.position;
            wall.transform.LookAt(start.transform);
            wall.transform.localScale += sAdjust;

            Vector3 debug = wall.transform.right;
            targetLine.Normalize();
            debug.Normalize();
            Debug.Log("wall rot" + i + ":" + debug + "/" + targetLine);
        }
    }
}

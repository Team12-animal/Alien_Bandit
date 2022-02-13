using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonController : MonoBehaviour
{
    public string balloonName;
    public GameObject balloon;
    public float frequency = 5.0f;
    public float speed;
    public GameObject[,] paths;
    
    public int ropeInScene;

    public GameObject start;
    public GameObject end;
    private GameObject rope;

    public bool holdRope = true;
    public bool arrived = false;
    public bool sending = false;

    private void Start()
    {
        FindNodes();
    }

    private void FindNodes()
    {
        GameObject[] nodes = GameObject.FindGameObjectsWithTag("BalloonNode");
        int nodesAmt = nodes.Length;

        if(nodesAmt >= 2)
        {
            int pathAmt = nodesAmt / 2;
            paths = new GameObject[pathAmt, 2];

            for (int i = 0; i < paths.GetLength(0); i++)
            {

                foreach (GameObject node in nodes)
                {
                    if(node.name == "BalloonStartNode_" + i)
                    {
                        paths[i, 0] = node;
                    }
                    else if(node.name == "BalloonEndNode_" + i)
                    {
                        paths[i, 1] = node;
                    }
                }
            }

            Debug.Log("Balloon nodes found" + nodesAmt + "/" + paths.GetLength(0));

        }
        else
        {
            Debug.Log("Balloon nodes setting error.");
        }
    }

    float countTime = 0.0f;
    bool timeCounting = false;

    // Update is called once per frame
    void FixedUpdate()
    {
        ropeInScene = GameObject.FindGameObjectsWithTag("Rope").Length;

        if (ropeInScene < 3)
        {
            if (timeCounting == false)
            {
                Debug.Log("balloon time" + Time.time);

                countTime = Time.time;
                timeCounting = true;
            }

            if (balloon == null)
            {
                Debug.Log(countTime + "balloon time" + Time.time);
                if (Time.time > countTime + frequency)
                {
                    ChosePath();
                    SpawnBallon();
                    sending = true;
                    countTime += frequency;
                }
            }
        }

        if(balloon != null)
        {
            MoveToStart();

            if (arrived == true)
            {
                //if (Time.time > countTime + 0.5f)
                //{
                    DropRope();
                    countTime += 0.5f;
                //}

                //if(Time.time > countTime + 0.2f)
                //{
                GoBackAndDestory();
                timeCounting = false;
                sending = false;
                //}   
            }
        }
    }

    int pathToGo = 1;

    private void ChosePath()
    {
        //int pathAmt = paths.GetLength(0);
        //int pathToGo = Random.Range(0, pathAmt - 1);

        if(pathToGo == 0)
        {
            pathToGo = 1;
        }
        else
        {
            pathToGo = 0;
        }

        start = paths[pathToGo,0];
        end = paths[pathToGo, 1];
    }

    private void SpawnBallon()
    {
        var prefab = Resources.Load<GameObject>(balloonName);
        balloon = GameObject.Instantiate(prefab) as GameObject;
        balloon.SetActive(true);
        balloon.transform.position = start.transform.position;

        rope = balloon.transform.Find("Rope").gameObject;

        holdRope = true;
    }

    private void MoveToStart()
    {
        if (balloon != null && holdRope == true)
        {
            Vector3 dir = end.transform.position - balloon.transform.position;

            if(dir.magnitude > 0.3f)
            {
                dir.Normalize();
                balloon.transform.position += dir * speed;
            }
            else
            {
                arrived = true;
            }
        }
    }

    private void DropRope()
    {
        if(rope == null)
        {
            return;
        }

        rope.transform.parent = null;
        Rigidbody ropeRB = rope.GetComponent<Rigidbody>();
        ropeRB.useGravity = true;
        //ropeRB.isKinematic = false;

        holdRope = false;
    }

    private void GoBackAndDestory()
    {
        if(balloon != null && holdRope == false)
        {
            Vector3 dir = start.transform.position - balloon.transform.position;

            if (dir.magnitude > 0.3f)
            {
                dir.Normalize();
                balloon.transform.position += dir * speed;
            }
            else
            {
                balloon.SetActive(false);
                GameObject.Destroy(balloon);
                ClearData();

                Debug.Log("balloon destroied");
            }
        }
    }

    private void ClearData()
    {
        balloon = null;
        start = null;
        end = null;
        rope = null;
        arrived = false;
    }
}

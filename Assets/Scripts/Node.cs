using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Node : MonoBehaviour
{
    public static Node instance;
    private GameObject[] orgArr;
    public static GameObject[] nodes;

    private static Node Instance()
    {
        return instance;
    }

    private void Awake()
    {
        instance = this;

        orgArr = GameObject.FindGameObjectsWithTag("node");
        nodes = Sort(orgArr);

        if(nodes.Length > 0)
        {
            Debug.Log("node found");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        if (nodes != null)
        {
            Gizmos.color = Color.blue;

            for (int i = 0; i < nodes.Length; i++)
            {
                if (i + 1 < nodes.Length)
                {

                    Gizmos.DrawLine(nodes[i].transform.position, nodes[i + 1].transform.position);
                }
            }

            Gizmos.DrawLine(nodes[0].transform.position, nodes[nodes.Length - 1].transform.position);
        }
    }

    public GameObject[] Sort(GameObject[] arr)
    {
        GameObject[] result = new GameObject[arr.Length];
        for (int i = 0; i < arr.Length; i++)
        {
            string name = "node_" + i;

            for (int j = 0; j < arr.Length; j++)
            {
                if (orgArr[j].name == name)
                {
                    result[i] = arr[j];
                    break;
                }
            }
        }

        return result;
    }
}

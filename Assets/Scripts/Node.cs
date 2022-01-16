using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Node : MonoBehaviour
{
    public GameObject[] orgArr;
    public GameObject[] nodes;

    string tagName;

    private void Awake()
    {
        tagName = this.gameObject.tag;
        orgArr = GameObject.FindGameObjectsWithTag(tagName);
        nodes = Sort(orgArr);
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
        if (orgArr.Length >0 && nodes.Length == orgArr.Length)
        {
            Gizmos.color = Color.blue;

            for (int i = 0; i < nodes.Length; i++)
            {
                Debug.Log("gizmo" + tagName + i);
                if (i + 1 < nodes.Length)
                {

                    Gizmos.DrawLine(nodes[i].transform.position, nodes[i + 1].transform.position);
                }
            }

            Gizmos.DrawLine(nodes[0].transform.position, nodes[nodes.Length - 1].transform.position);
        }
        else
        {
            Debug.Log("gen nodes error");
        }
    }

    public GameObject[] Sort(GameObject[] arr)
    {
        GameObject[] result = new GameObject[arr.Length];
        for (int i = 0; i < arr.Length; i++)
        {
            string name = tagName + "_" + i;

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

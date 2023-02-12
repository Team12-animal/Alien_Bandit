using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolLoader : MonoBehaviour
{
    public string nodeTagName;
    public string prefabName;
    public GameObject[] nodes;

    public static PoolLoader instance;

    private PoolLoader()
    {
        instance = this;
    }

    private void Awake()
    {
        nodes = GameObject.FindGameObjectsWithTag(nodeTagName);

        LoadPrefab();
        SpawnPrefab();

    }

    private List<GameObject> container;

    private void LoadPrefab()
    {
        int amt = nodes.Length;

        container = new List<GameObject>();

        for(int i = 0; i < amt; i++)
        {
            var prefab = Resources.Load<GameObject>(prefabName);
            GameObject go = GameObject.Instantiate(prefab) as GameObject;
            go.SetActive(false);
            container.Add(go);
        }

        Debug.Log(nodeTagName + ": Prefab Loaded");
    }
    
    private void SpawnPrefab()
    {
        int nodeAmt = nodes.Length;

        GameObject pos;
        GameObject go;

        for(int i= 0; i < nodeAmt; i++)
        {
            pos = nodes[i];
            go = container[i];
            go.SetActive(true);
            go.transform.position = pos.transform.position;
        }

        Debug.Log(nodeTagName + ": Prefab Spawned");
    }
}

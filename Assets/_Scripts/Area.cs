using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area : MonoBehaviour
{
    [Header("Mesh Collider && Renderer Close:Z ")]
    [SerializeField]List<GameObject> targets;

    private void Awake()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Area");
        for (int i = 0; i < gameObjects.Length; i++)
        {
            targets.Add(gameObjects[i]);
        }
    }

    private void Update()
    {
        AreaControl();
    }

    public void AreaControl()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            for (int i = 0; i < targets.Count; i++)
            {
                MeshCollider meshColl = targets[i].GetComponent<MeshCollider>();
                MeshRenderer meshRender = targets[i].GetComponent<MeshRenderer>();
                meshColl.enabled = false;
                meshRender.enabled = false;
            }
        }
    }


}

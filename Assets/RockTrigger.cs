using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockTrigger : MonoBehaviour
{
    public GameObject targetRock;
    private GameObject parent;

    // Start is called before the first frame update
    void Start()
    {
        parent = this.transform.parent.gameObject;
        targetRock = parent.GetComponent<RockCollider>().targetRock;
    }
}
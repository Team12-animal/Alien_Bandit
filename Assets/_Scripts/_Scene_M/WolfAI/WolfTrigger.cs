using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfTrigger : MonoBehaviour
{
    [SerializeField] Transform place;
    [SerializeField] GameObject wolfPrefab;
    [SerializeField] GameObject chop;
    ChopInUse chopScript;
    public bool created;

    void Start()
    {
        created = false;
        chopScript = chop.GetComponent<ChopInUse>();
    }

    void Update()
    {
        if (!created && chopScript.used)
        {
            Instantiate(wolfPrefab, place.transform.position, Quaternion.Euler(0.0f, 180.0f, 0.0f));
            created = true;
            return;
        }
    }
}

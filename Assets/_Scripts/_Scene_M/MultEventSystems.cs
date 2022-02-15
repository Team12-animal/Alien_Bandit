using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultEventSystems : MonoBehaviour
{
    public static MultEventSystems instance;
    
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

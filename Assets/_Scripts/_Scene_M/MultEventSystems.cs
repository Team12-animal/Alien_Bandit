using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MultEventSystems : MonoBehaviour
{
    public static MultEventSystems instance;

    private void Start()
    {
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //public StandaloneInputModule GetStandaloneInputModule(int number)
    //{
    //    switch (number)
    //    {
    //        case 1:
    //            return standaloneInputModule01;
    //        case 2:
    //            return standaloneInputModule02;
    //        case 3:
    //            return standaloneInputModule03;
    //        case 4:
    //            return standaloneInputModule04;
    //    }
    //    return null;
    //}
}

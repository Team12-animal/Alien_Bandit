using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessionEvents : MonoBehaviour
{
    public static MessionEvents instance;

    private void Start()
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

    public enum SceneEvent
    {
        RainEvent,
        TorbadoEvent,
        EarthQuakeEvent,
        FireEvent,
        FloodedEvent,
        EndCounts,
    }


    public void RainEvent()
    {
        //show raining effects;
        Debug.LogWarning("StartRain");
    }

    public void TornadoEvent()
    {
        //show tornado effects;
        //tornado radias will pick items to random places;
        Debug.LogWarning("StartTornado");
    }

    public void EarthQuakeEvent()
    {
        //show earthquake effects;
        //the catching animals will run away to scene;
        Debug.LogWarning("StartEarthQuake");
    }

    public void FireEvent()
    {
        //show fire effects;
        //if fire on item , destroy item;
        Debug.LogWarning("StartFire");
    }

    public void FloodedEvent()
    {
        //show flooded effects;
        //slow down player movement;
        Debug.LogWarning("StartFlooded");
    }
}

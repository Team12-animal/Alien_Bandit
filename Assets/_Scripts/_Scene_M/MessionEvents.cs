using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessionEvents : MonoBehaviour
{
    public static MessionEvents instance;

    [SerializeField] GameObject rainWindowPrefab;
    [SerializeField] Material rainMaterial;

    private void Start()
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
        GameObject temp = Instantiate(rainWindowPrefab, Camera.main.transform.position, Camera.main.transform.rotation);
        temp.transform.position += Camera.main.transform.forward * 3f;
        rainWindowPrefab = temp;

        Debug.LogWarning("StartRain");
    }
    public void StopRainEvent()
    {
        float tempBlur = Mathf.Lerp(0.6f, 0f, 0.3f);
        rainMaterial.SetFloat("_Blur", tempBlur);
        Destroy(rainWindowPrefab);
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

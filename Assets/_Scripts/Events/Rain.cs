using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rain : MonoBehaviour
{
    [SerializeField] float stopRainEffectTime = 12.0f;
    [SerializeField] float startRain = 0;

    private void Start()
    {
        startRain = 0f;
    }

    private void Update()
    {
        startRain += Time.deltaTime;
        if (startRain >= stopRainEffectTime)
        {
            Destroy(gameObject);
        }
    }
}

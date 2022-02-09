using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainEventControl : MonoBehaviour
{
    [SerializeField] GameObject rainWindowPrefab;
    [SerializeField] Material rainMaterial;
    string blur = "_Blur";
    string distortion = "_Distortion";
    float rainTime = 0;

    void Update()
    {
        ControlRainBlurEffect();
    }

    private void ControlRainBlurEffect()
    {
        //set time to control blur;
        float tempTime = 0f;
        tempTime = Time.deltaTime;
        rainTime += tempTime;
        rainMaterial.SetFloat(blur, 0f);
        rainMaterial.SetFloat(distortion, 0f);
        if (rainTime > 9f)
        {
            float blurSpeed = Mathf.Lerp(0f, 0.6f, 3/rainTime);
            float distortionSpeed = Mathf.Lerp(0f, -6.0f, 9/rainTime);
            rainMaterial.SetFloat(blur, blurSpeed);
            rainMaterial.SetFloat(distortion, distortionSpeed);
            if (rainMaterial.GetFloat(blur) < 0.1f && rainMaterial.GetFloat(distortion) > -3.0f)
            {
                rainMaterial.SetFloat(blur, 0f);
                rainMaterial.SetFloat(distortion, 0f);
                Destroy(gameObject);
            }
        }
        else if (rainTime > 0.6f)
        {
            float blurSpeed = Mathf.Lerp(0f, 0.6f, rainTime * 0.3f);
            float distortionSpeed = Mathf.Lerp(0f, -6.0f, rainTime * 0.3f);
            rainMaterial.SetFloat(blur, blurSpeed);
            rainMaterial.SetFloat(distortion, distortionSpeed);
        }
    }
}

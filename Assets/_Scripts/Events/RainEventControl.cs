using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainEventControl : MonoBehaviour
{
    [SerializeField] Material rainMaterial;
    //control shader parameter
    string blur = "_Blur";
    //control shader parameter
    string distortion = "_Distortion";
    public float rainTime { get; private set; } = 0;

    private void Start()
    {
        rainTime = 0f;
    }

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
        //reset blur and distortion
        rainMaterial.SetFloat(blur, 0f);
        rainMaterial.SetFloat(distortion, 3f);
        //when going to stop raining ,cost down blur and distortion value;
        if (rainTime > 9f)
        {
            float blurSpeed = Mathf.Lerp(0f, 0.6f, 3/rainTime);
            //float distortionSpeed = Mathf.Lerp(0f, -6.0f, 9.6f/rainTime);
            rainMaterial.SetFloat(blur, blurSpeed);
            //rainMaterial.SetFloat(distortion, distortionSpeed);
            if (rainMaterial.GetFloat(blur) < 0.1f && rainMaterial.GetFloat(distortion) > -2.7f)
            {
                rainMaterial.SetFloat(blur, 0f);
                //rainMaterial.SetFloat(distortion, 0f);
                Destroy(gameObject);
            }
        }
        else if (rainTime > 0.6f)//when start raining, blur and distroetion are a little and keep move on more value; 
        {
            float blurSpeed = Mathf.Lerp(0f, 0.6f, rainTime * 0.3f);
            float distortionSpeed = Mathf.Lerp(0f, -6.0f, rainTime * 0.3f);
            rainMaterial.SetFloat(blur, blurSpeed);
            //rainMaterial.SetFloat(distortion, distortionSpeed);
        }
    }
}

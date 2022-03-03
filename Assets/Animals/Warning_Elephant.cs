using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Warning_Elephant : MonoBehaviour
{
    Camera cam;
    Image warningIcon;
    GameObject warningGo;
    Color temp;

    void Start()
    {
        cam = Camera.main;
        warningGo = this.gameObject;
        warningGo.transform.rotation = cam.transform.rotation;
        warningIcon = warningGo.GetComponent<Image>();
        temp = warningIcon.color;
    }

    // Update is called once per frame
    void Update()
    {
        TwinkleUI();
    }

    private float fullAlpha = 1.0f;
    private float fadeAlpha = 0.3f;
    private float transSpeed = 8.0f;

    public bool lighter = false;

    private void TwinkleUI()
    {
        if (warningIcon.color.a >= 0.98)
        {
            lighter = false;
        }
        else if (warningIcon.color.a <= 0.4)
        {
            lighter = true;
        }

        if (!lighter)
        {
            temp.a = Mathf.Lerp(warningIcon.color.a, fadeAlpha, transSpeed * Time.deltaTime);
            warningIcon.color = temp;
        }
        else
        {
            temp.a = Mathf.Lerp(warningIcon.color.a, fullAlpha, transSpeed * Time.deltaTime);
            warningIcon.color = temp;
        }
    }
}

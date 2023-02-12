using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Wolf_WarnDisplayer : MonoBehaviour
{
    Image warningIcon;
    GameObject warningGo;
    Color temp;

    public GameObject wolf;
    private Wolf_BehaviourTree wolfBT;

    Vector3 followingPos;

    // Start is called before the first frame update
    void Start()
    {
        this.transform.rotation = Camera.main.transform.rotation;
        warningIcon = this.GetComponent<Image>();
        temp = warningIcon.color;
    }

    private void Update()
    {
        if (wolf == null)
        {
            wolf = GameObject.FindGameObjectWithTag("Wolf");
            if (wolf != null)
            {
                wolfBT = wolf.GetComponent<Wolf_BehaviourTree>();
            }
        }

        TwinkleUI();
    }

    private void LateUpdate()
    {
        if (wolf != null && wolf.activeSelf == true)
        {
            followingPos = wolfBT.WarningUIDisplay();
            this.transform.position = followingPos;
        }
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

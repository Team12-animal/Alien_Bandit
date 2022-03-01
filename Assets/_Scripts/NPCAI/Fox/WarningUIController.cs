using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarningUIController : MonoBehaviour
{
    Camera cam;
    GameObject warningGo;
    Image warningIcon;

    public GameObject fox;
    private Fox_BehaviourTree foxBT;

    Vector3 followingPos;

    Color temp;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        warningGo = this.gameObject;
        warningGo.transform.rotation = cam.transform.rotation;
        warningIcon = warningGo.GetComponent<Image>();

        temp = warningIcon.color;
    }

    private void Update()
    {
        if (fox == null)
        {
            fox = GameObject.FindGameObjectWithTag("Fox");
            if (fox != null)
            {
                foxBT = fox.GetComponent<Fox_BehaviourTree>();
            }
        }

        TwinkleUI();
    }

    private void LateUpdate()
    {
        if (fox != null && fox.activeSelf == true)
        {
            followingPos = foxBT.WarningUIDisplay();
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

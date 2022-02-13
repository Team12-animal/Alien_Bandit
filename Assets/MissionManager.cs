using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionManager : MonoBehaviour
{
    [SerializeField]
    GameObject mission;
    [SerializeField]
    Image missionBar;
    public bool missionAction;

    // Start is called before the first frame update
    void Start()
    {
        missionBar.fillAmount = 1;
        missionBar.color = new Color(0.3f, 0.6f, 0.2f);
    }

    void FixedUpdate()
    {
        if (missionAction)
        {
            TimeLine();
            ChangeColor();
        }
    }

    void TimeLine()
    {
        missionBar.fillAmount -= 0.025f * Time.deltaTime; //40¬íÂk¹s
        if (missionBar.fillAmount <= 0)
        {
            mission.SetActive(false);
        }
    }

    void ChangeColor()
    {
        if (missionBar.fillAmount > 0.6f)
        {
            missionBar.color = new Color(0.3f, 0.6f, 0.2f);
        }
        else if (missionBar.fillAmount > 0.3f)
        {
            missionBar.color = new Color(0.78f, 0.58f, 0.19f);
        }
        else
        {
            missionBar.color = new Color(0.78f,0.19f,0.19f);
        }
    }
}

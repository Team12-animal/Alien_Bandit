using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionList : MonoBehaviour
{
    [SerializeField]
    GameObject mission;
    [SerializeField]
    Image missionBar;
    Animator MissionAnimator;
    public bool missionAction;
    public int id=0; //任務內容{0=兔子,1=狐狸,2=鹿...}
    MissionManager missionManager;

    // Start is called before the first frame update
    void Start()
    {
        MissionAnimator = GetComponent<Animator>();
        missionBar.fillAmount = 1;
        missionBar.color = new Color(0.3f, 0.6f, 0.2f);
        missionAction = true;
        missionManager = transform.parent.gameObject.GetComponent<MissionManager>();
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
        missionBar.fillAmount -= 0.025f * Time.deltaTime; //40秒歸零
        MissionAnimator.SetFloat("Value", missionBar.fillAmount);
        if (missionBar.fillAmount <= 0)
        {
            missionManager.RemoveMission(this.gameObject);
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
            missionBar.color = new Color(0.78f, 0.19f, 0.19f);
        }
    }
    public void DestroyMission()
    {
        Destroy(this.gameObject);
    }
}

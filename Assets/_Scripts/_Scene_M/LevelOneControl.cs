using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelOneControl : MonoBehaviour
{
    [Header("�C���}�l�ɶ��˼�")]
    [SerializeField] float waittingTime = 6.0f;
    [SerializeField] Text waittingTimeText;
    [SerializeField] Canvas timeUI;

    [Header("�C���i�����")]
    [SerializeField] float gamingTime = 30.0f;
    [SerializeField] Text gamingTimeText;

    [Header("�C������")]
    [SerializeField] Canvas gameoverUI;


    [SerializeField] List<GameObject> player = new List<GameObject>();

    private void Start()
    {
        SceneController.instance.GetPlayer(player);
        gameoverUI.gameObject.SetActive(false);
    }

    private void Update()
    {
        TimeSetting();
        GameOver();
    }

    private void TimeSetting()
    {
        if (waittingTime <= 0f)
        {
            if (SceneController.instance.selected01)
            {
                SceneController.instance.StartMove(player[0]);
            }
            if (SceneController.instance.selected02)
            {
                SceneController.instance.StartMove(player[1]);
            }
            timeUI.gameObject.SetActive(false);
            gamingTime = SetTime(gamingTime, gamingTimeText);
        }
        waittingTime = SetTime(waittingTime, waittingTimeText);
    }

    public float SetTime(float time, Text text)
    {
        time -= Time.deltaTime;
        time = Mathf.Clamp(time, 0f, Mathf.Infinity);
        text.text = string.Format("{0:00}", time);
        return time;
    }

    public void GameOver()
    {
        InputController input01 = new InputController();
        InputController input02 = new InputController();
        input01 = player[0].GetComponent<InputController>();
        input02 = player[1].GetComponent<InputController>();
        if (gamingTime <= 0.0f)
        {
            input01.enabled = false;
            input02.enabled = false;
            gameoverUI.gameObject.SetActive(true);
        }
    }
}

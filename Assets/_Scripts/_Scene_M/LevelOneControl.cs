using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelOneControl : MonoBehaviour
{
    [Header("遊戲開始時間倒數")]
    [SerializeField] float waittingTime = 6.0f;
    [SerializeField] Text waittingTimeText;
    [SerializeField] Canvas timeUI;

    [Header("遊戲進行期間")]
    [SerializeField] float gamingTime = 30.0f;
    [SerializeField] Text gamingTimeText;

    [Header("遊戲結束")]
    [SerializeField] Canvas gameoverUI;
    public bool doorDestroied = false;

    [SerializeField] List<GameObject> player = new List<GameObject>();

    private void Start()
    {
        SceneController.instance.GetPlayer(player);
        gameoverUI.gameObject.SetActive(false);
        doorDestroied = false;
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
        if (gamingTime <= 0.0f || doorDestroied)
        {
            GameOverSetting(input01, input02);
        }
    }

    private void GameOverSetting(InputController input01, InputController input02)
    {
        input01.enabled = false;
        input02.enabled = false;
        gameoverUI.gameObject.SetActive(true);
    }
}

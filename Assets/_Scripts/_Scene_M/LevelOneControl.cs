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
    [SerializeField] float gamingTime = 180.0f;
    [SerializeField] Text gamingTimeText;

    [Header("遊戲結束")]
    [SerializeField] GameObject[] gameoverUIText;
    public bool doorDestroied = false;
    InputController input01 = new InputController();
    InputController input02 = new InputController();

    [SerializeField] List<GameObject> player = new List<GameObject>();

    public bool isWin;//is player win the game?
    int stars = 0;

    private void Start()
    {
        SceneController.instance.GetPlayer(player);
        for (int i = 0; i < gameoverUIText.Length; i++)
        {
            gameoverUIText[i].gameObject.SetActive(false);
        }
        gamingTime = 180.0f;
        doorDestroied = false;
        isWin = false;
        stars = 0;
    }

    private void Update()
    {
        TimeSetting();
        GameOver();
        WinGame();
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
            if (!isWin)
            {
                gamingTime = SetTime(gamingTime, gamingTimeText);
            }
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
        input01 = player[0].GetComponent<InputController>();
        input02 = player[1].GetComponent<InputController>();
        if (gamingTime <= 0.0f || doorDestroied)
        {
            GameOverSetting(input01, input02);// can't control players;
        }
        else if (isWin)
        {
            //need to creat win UI;
            GameOverSetting(input01, input02);// can't control players;
        }
    }
    /// <summary>
    /// Get players InputManager and set them not enable;
    /// </summary>
    /// <param name="input01"></param>
    /// <param name="input02"></param>
    private void GameOverSetting(InputController input01, InputController input02)
    {
        input01.enabled = false;
        input02.enabled = false;
        for (int i = 0; i < gameoverUIText.Length; i++)
        {
            gameoverUIText[i].gameObject.SetActive(true);
        }
    }
    /// <summary>
    /// finish mession on time to win stars; 
    /// </summary>
    public void WinGame()
    {
        if (isWin == true && gamingTime > 90.0f)
        {
            stars = 3;
            SceneController.instance.levelOneStarsCounts = stars;//set stars count to manager;
            SceneController.instance.GetStars();//change star color;
            GameOver();
        }
        else if (isWin == true && gamingTime >= 60.0f)
        {
            stars = 2;
            SceneController.instance.levelOneStarsCounts = stars;
            SceneController.instance.GetStars();
            GameOver();
        }
        else if (isWin == true && gamingTime > 0.0f)
        {
            stars = 1;
            SceneController.instance.levelOneStarsCounts = stars;
            SceneController.instance.GetStars();
            GameOver();
        }
    }
}

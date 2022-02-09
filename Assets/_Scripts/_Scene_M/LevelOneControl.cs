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

    //is player win the game?
    public bool isWin;
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
        TriggerSceneEvents();
        WinGame(1);// 1  means what level one stars state;
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
            // can't control players;
            GameOverSetting(input01, input02);
        }
        else if (isWin)
        {
            //need to creat win UI;


            // can't control players;
            GameOverSetting(input01, input02);
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
    public void WinGame(int level)
    {
        Color yellow = new Color(1, 1, 1, 1);
        if (isWin == true && gamingTime > 90.0f)
        {
            //Saving data;
            SaveStarsState.instance.SaveDate(level, 3, yellow);
            GameOver();
        }
        else if (isWin == true && gamingTime >= 60.0f)
        {
            //Saving data;
            SaveStarsState.instance.SaveDate(level, 2, yellow);
            GameOver();
        }
        else if (isWin == true && gamingTime > 0.0f)
        {
            //Saving data;
            SaveStarsState.instance.SaveDate(level, 1, yellow);
            GameOver();
        }
    }

    //created a rain event or not;
    [SerializeField] bool createdRain = false;
    //check rain event start time;
    [SerializeField]float startRainTime = 120.0f;
    [SerializeField] float endRainEventTime = 60.0f;
    //created others events or not;
    [SerializeField] bool createdOthers = false;
    //check others event start time;
    [SerializeField] float startOthersEventTime = 105.0f;
    [SerializeField] float endOthersEventTime = 90.0f;

    public void TriggerSceneEvents()
    {
        int creatOrNot = Random.Range(0, 6);
        //random a event to creat;
        int sceneEvent = Random.Range((int)MessionEvents.SceneEvent.TorbadoEvent, (int)MessionEvents.SceneEvent.EndCounts);

        if (createdOthers == false && gamingTime <= startOthersEventTime && creatOrNot > 0 && gamingTime > endOthersEventTime)
        {
            createdOthers = true;
            switch (sceneEvent)
            {
                case 1:
                    MessionEvents.instance.TornadoEvent();
                    break;
                case 2:
                    MessionEvents.instance.FireEvent();
                    break;
                case 3:
                    MessionEvents.instance.EarthQuakeEvent();
                    break;
                case 4:
                    MessionEvents.instance.FloodedEvent();
                    break;
            }
        }
        else if (createdRain == false && gamingTime <= startRainTime && creatOrNot > 4 && gamingTime > endRainEventTime)
        {
            createdRain = true;
            MessionEvents.instance.RainEvent();
        }

        if (gamingTime <= endRainEventTime && createdRain == true)
        {
            //dispear event;
            Debug.LogWarning("EndEvent");
            createdRain = false;
        }
        if (gamingTime <= endOthersEventTime && createdOthers == true)
        {
            //dispear event;
            Debug.LogWarning("EndOthersEvent");
            createdOthers = false;
        }
    }
}

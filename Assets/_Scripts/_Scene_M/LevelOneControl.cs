using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelOneControl : MonoBehaviour
{
    [Header("Waitting before star game play")]
    [SerializeField] float waittingTime = 3.0f;
    [SerializeField] GameObject readyImage;
    [SerializeField] Sprite goImage;
    [SerializeField] GameObject waittingTimeUI;

    [Header("Gaming")]
    [SerializeField] float gamingTime = 180.0f;
    [SerializeField] Text gamingTimeText;

    [Header("End Game")]
    [SerializeField] GameObject[] gameOverUIText;
    [SerializeField] GameObject gameWinUI;
    public bool doorDestroied = false;
    InputController input01 = new InputController();
    InputController input02 = new InputController();
    InputController input03 = new InputController();
    InputController input04 = new InputController();

    [Header("ContinueUISetting")]
    float waittingTimeToShowContinueUI = 3.0f;
    [SerializeField] GameObject continueUI;
    GameObject[] levelOneStars;
    [SerializeField] List<GameObject> showStars;
    [SerializeField] List<GameObject> players = new List<GameObject>();
    bool player01CheckToContinue;
    bool player02CheckToContinue;
    bool player03CheckToContinue;
    bool player04CheckToContinue;
    [SerializeField] GameObject player02RawImage;
    [SerializeField] GameObject player03RawImage;
    [SerializeField] GameObject player04RawImage;
    [SerializeField] GameObject player01ReadyImage;
    [SerializeField] GameObject player02ReadyImage;
    [SerializeField] GameObject player03ReadyImage;
    [SerializeField] GameObject player04ReadyImage;

    //is player win the game?
    public bool isWin;

    private void Start()
    {
        //Setting Players who are in game
        SceneController.instance.GetPlayer(players);
        //Setting Game UI and time
        for (int i = 0; i < gameOverUIText.Length; i++)
        {
            gameOverUIText[i].gameObject.SetActive(false);
        }
        gamingTime = 180.0f;
        doorDestroied = false;
        isWin = false;
        gameWinUI.SetActive(false);
        continueUI.SetActive(false);
        waittingTimeToShowContinueUI = 3.0f;
        //Setting stars
        levelOneStars = new GameObject[3];
        UpdateStarsStates updateStarsStates = new UpdateStarsStates();
        levelOneStars[0] = GameObject.Find(updateStarsStates.star01);
        levelOneStars[1] = GameObject.Find(updateStarsStates.star02);
        levelOneStars[2] = GameObject.Find(updateStarsStates.star03);
        //Setting Continue UI
        player02RawImage.SetActive(true);
        player03RawImage.SetActive(true);
        player04RawImage.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetButtonDown(KeyCode.Escape.ToString()))
        {
            SceneController.instance.transition.SetTrigger(SceneController.instance.animEndHash);
            SceneController.instance.LoadLevel(0);
            //LevelLoader.instance.LoadLevel(0);
        }
        TimeSettingAndAllowPlayerMoving();
        GameOver();
        //TriggerSceneEvents();
        WinGame(1);//1  means what level two stars state;
    }
    /// <summary>
    /// When waitting time go up , allow player moving
    /// </summary>
    private void TimeSettingAndAllowPlayerMoving()
    {
        if(waittingTime < -1.0f)
        {
            return;
        }
        if (waittingTime <= 0.0f)
        {
            waittingTime = -0.9f;
            waittingTimeUI.gameObject.SetActive(false);
            if (SceneController.instance.selected01)
            {
                SceneController.instance.StartMove(players[0]);
            }
            if (SceneController.instance.selected02)
            {
                SceneController.instance.StartMove(players[1]);
            }
            if (SceneController.instance.selected03)
            {
                SceneController.instance.StartMove(players[2]);
            }
            if (SceneController.instance.selected04)
            {
                SceneController.instance.StartMove(players[3]);
            }
            waittingTimeUI.gameObject.SetActive(false);
            if (!isWin && !doorDestroied)
            {
                gamingTime = SetTime(gamingTime, gamingTimeText);
            }
        }
        else if (waittingTime < 1.2f)
        {
            readyImage.GetComponent<Image>().sprite = goImage;
        }
        waittingTime -= Time.deltaTime;
    }

    public float SetTime(float time, Text text)
    {
        time -= Time.deltaTime;
        time = Mathf.Clamp(time, 0f, Mathf.Infinity);
        int minute = (int)gamingTime / 60;
        int second = (int)gamingTime - minute * 60;
        text.text = string.Format("{0:D2}:{1:D2}", minute, second);

        return time;
    }

    public void GameOver()
    {
        input01 = players[0].GetComponent<InputController>();
        input02 = players[1].GetComponent<InputController>();
        input03 = players[2].GetComponent<InputController>();
        input04 = players[3].GetComponent<InputController>();
        if (gamingTime <= 0.0f || doorDestroied)
        {
            GameOverSetting(input01, input02, input03, input04);
            // can't control players;
            for (int i = 0; i < gameOverUIText.Length; i++)
            {
                gameOverUIText[i].gameObject.SetActive(true);
            }
        }
        else if (isWin)
        {
            //need to creat win UI;
            gameWinUI.SetActive(true);

            //can't control players;
            GameOverSetting(input01, input02, input03, input04);

            //Wait a little seconds to show Continue UI;
            waittingTimeToShowContinueUI -= Time.deltaTime;
            if (waittingTimeToShowContinueUI <= 0.0f)
            {
                waittingTimeToShowContinueUI = 0.0f;
                if (!continueUI.activeInHierarchy)
                {
                    for (int i = 0; i < levelOneStars.Length; i++)
                    {
                        showStars[i].GetComponent<RawImage>().color = levelOneStars[i].GetComponent<RawImage>().color;
                    }
                }
                continueUI.SetActive(true);
                //Reset Players Position to MainMenu;
                if (SceneController.instance.selected01)
                {
                    //Set player position to MainMenu position because using the same rawImage;
                    SceneController.instance.MainPlayer(SceneController.instance.player01);
                    //Change Animator to Dance Type;
                    CheckPlayer tempPlayer = new CheckPlayer();
                    tempPlayer.ChangePlayerAnimator(SceneController.instance.player01, tempPlayer.menuDance01);
                }
                if (SceneController.instance.selected02)
                {
                    SceneController.instance.MainPlayer(SceneController.instance.player02);
                    CheckPlayer tempPlayer = new CheckPlayer();
                    tempPlayer.ChangePlayerAnimator(SceneController.instance.player02, tempPlayer.menuDance02);
                }
                else
                {
                    player02RawImage.SetActive(false);
                }
                if (SceneController.instance.selected03)
                {
                    SceneController.instance.MainPlayer(SceneController.instance.player03);
                    CheckPlayer tempPlayer = new CheckPlayer();
                    tempPlayer.ChangePlayerAnimator(SceneController.instance.player03, tempPlayer.menuDance03);
                }
                else
                {
                    player03RawImage.SetActive(false);
                }
                if (SceneController.instance.selected04)
                {
                    SceneController.instance.MainPlayer(SceneController.instance.player04);
                    CheckPlayer tempPlayer = new CheckPlayer();
                    tempPlayer.ChangePlayerAnimator(SceneController.instance.player04, tempPlayer.menuDance04);
                }
                else
                {
                    player04RawImage.SetActive(false);
                }
                //Show Stars animations;

                //Wait every players pressed confirm button to continue;
                //PlayerCount =how many players in game
                int PlayerCount = Convert.ToInt32(SceneController.instance.selected01) + Convert.ToInt32(SceneController.instance.selected02) + Convert.ToInt32(SceneController.instance.selected03) + Convert.ToInt32(SceneController.instance.selected04);
                //1000; 1
                bool onePlayer1000 = SceneController.instance.selected01 && !SceneController.instance.selected02 && !SceneController.instance.selected03 && !SceneController.instance.selected04;
                //1100; 2
                bool twoPlayerType1100 = SceneController.instance.selected01 && SceneController.instance.selected02 && !SceneController.instance.selected03 && !SceneController.instance.selected04;
                //1010; 2
                bool twoPlayerType1010 = SceneController.instance.selected01 && !SceneController.instance.selected02 && SceneController.instance.selected03 && !SceneController.instance.selected04;
                //1001; 2
                bool twoPlayerTpye1001 = SceneController.instance.selected01 && !SceneController.instance.selected02 && !SceneController.instance.selected03 && SceneController.instance.selected04;
                //1110; 3
                bool threePlayerType1110 = SceneController.instance.selected01 && SceneController.instance.selected02 && SceneController.instance.selected03 && !SceneController.instance.selected04;
                //1011; 3
                bool threePlayerType1101 = SceneController.instance.selected01 && !SceneController.instance.selected02 && SceneController.instance.selected03 && SceneController.instance.selected04;
                //1111; 4
                bool fourPlayer = SceneController.instance.selected01 && SceneController.instance.selected02 && SceneController.instance.selected03 && SceneController.instance.selected04;

                CheckPlayerPressContinue(SceneController.instance.selected01, "Use1", "Take1", ref player01CheckToContinue,player01ReadyImage);
                CheckPlayerPressContinue(SceneController.instance.selected02, "Use2", "Take2", ref player02CheckToContinue,player02ReadyImage);
                CheckPlayerPressContinue(SceneController.instance.selected03, "Use3", "Take3", ref player03CheckToContinue,player03ReadyImage);
                CheckPlayerPressContinue(SceneController.instance.selected04, "Use4", "Take4", ref player04CheckToContinue,player04ReadyImage);

                switch (PlayerCount)
                {
                    case 1:
                        if (onePlayer1000 && player01CheckToContinue)
                        {
                            SceneController.instance.transition.SetTrigger(SceneController.instance.animEndHash);
                            SceneController.instance.LoadLevel(2);
                        }
                        break;
                    case 2:
                        if (twoPlayerType1100 && player01CheckToContinue && player02CheckToContinue)
                        {
                            SceneController.instance.transition.SetTrigger(SceneController.instance.animEndHash);
                            SceneController.instance.LoadLevel(2);
                        }
                        else if (twoPlayerType1010 && player01CheckToContinue && player03CheckToContinue)
                        {
                            SceneController.instance.transition.SetTrigger(SceneController.instance.animEndHash);
                            SceneController.instance.LoadLevel(2);
                        }
                        else if (twoPlayerTpye1001 && player01CheckToContinue && player04CheckToContinue)
                        {
                            SceneController.instance.transition.SetTrigger(SceneController.instance.animEndHash);
                            SceneController.instance.LoadLevel(2);
                        }
                        break;
                    case 3:
                        if (threePlayerType1110 && player01CheckToContinue && player02CheckToContinue && player03CheckToContinue)
                        {
                            SceneController.instance.transition.SetTrigger(SceneController.instance.animEndHash);
                            SceneController.instance.LoadLevel(2);
                        }
                        else if (threePlayerType1101 && player01CheckToContinue && player02CheckToContinue && player04CheckToContinue)
                        {
                            SceneController.instance.transition.SetTrigger(SceneController.instance.animEndHash);
                            SceneController.instance.LoadLevel(2);
                        }
                        break;
                    case 4:
                        if (fourPlayer && player01CheckToContinue && player02CheckToContinue && player03CheckToContinue && player04CheckToContinue)
                        {
                            SceneController.instance.transition.SetTrigger(SceneController.instance.animEndHash);
                            SceneController.instance.LoadLevel(2);
                        }
                        break;
                }
            }
        }
    }

    private void CheckPlayerPressContinue(bool playerChosed, string checkButton, string cancelButton, ref bool result,GameObject image)
    {
        if (playerChosed && Input.GetButtonDown(checkButton))
        {
            image.SetActive(true);
            result = true;
        }
        else if (playerChosed && Input.GetButtonDown(cancelButton))
        {
            image.SetActive(false);
            result = false;
        }
    }

    /// <summary>
    /// Get players InputManager and set them not enable;
    /// </summary>
    /// <param name="input01"></param>
    /// <param name="input02"></param>
    private void GameOverSetting(InputController input01, InputController input02, InputController input03, InputController input04)
    {
        input01.enabled = false;
        input02.enabled = false;
        input03.enabled = false;
        input04.enabled = false;
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
    [SerializeField] float startRainTime = 120.0f;
    [SerializeField] float endRainEventTime = 60.0f;
    //created others events or not;
    [SerializeField] bool createdOthers = false;
    //check others event start time;
    [SerializeField] float startOthersEventTime = 105.0f;
    [SerializeField] float endOthersEventTime = 90.0f;
    public void TriggerSceneEvents()
    {
        int creatOrNot = UnityEngine.Random.Range(0, 6);
        //random a event to creat;
        int sceneEvent = UnityEngine.Random.Range((int)MessionEvents.SceneEvent.TorbadoEvent, (int)MessionEvents.SceneEvent.EndCounts);

        if (gamingTime <= startOthersEventTime && creatOrNot > 0 && createdOthers == false && gamingTime > endOthersEventTime)
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
        else if (gamingTime <= startRainTime && creatOrNot > 4 && createdRain == false && gamingTime > endRainEventTime)
        {
            createdRain = true;
            MessionEvents.instance.RainEvent();
        }

        if (gamingTime <= endRainEventTime && createdRain == true)
        {
            //dispear event;
            //MessionEvents.instance.StopRainEvent();
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

    //for treeController
    public float GetGameTime()
    {
        return gamingTime;
    }

    //for treeController
    public bool WinOrNot()
    {
        return isWin;
    }
}

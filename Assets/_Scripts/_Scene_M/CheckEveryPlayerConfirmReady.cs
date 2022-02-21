using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckEveryPlayerConfirmReady : MonoBehaviour
{
    [SerializeField] GameObject continueUI;
    [SerializeField] int numberOfLevel;
    [SerializeField] GameObject playerRawImage02;
    [SerializeField] GameObject playerRawImage03;
    [SerializeField] GameObject playerRawImage04;
    [SerializeField] GameObject player01ReadyImage;
    [SerializeField] GameObject player02ReadyImage;
    [SerializeField] GameObject player03ReadyImage;
    [SerializeField] GameObject player04ReadyImage;
    bool player01CheckToContinue;
    bool player02CheckToContinue;
    bool player03CheckToContinue;
    bool player04CheckToContinue;
    bool cancelContinue;
    [SerializeField] GameObject cancelContinueImage;

    private void Start()
    {
        player01CheckToContinue = false;
        player02CheckToContinue = false;
        player03CheckToContinue = false;
        player04CheckToContinue = false;
        player01ReadyImage.SetActive(false);
        player02ReadyImage.SetActive(false);
        player03ReadyImage.SetActive(false);
        player04ReadyImage.SetActive(false);
        if (!SceneController.instance.selected02)
        {
            playerRawImage02.SetActive(false);
        }
        if (!SceneController.instance.selected03)
        {
            playerRawImage03.SetActive(false);
        }
        if (!SceneController.instance.selected04)
        {
            playerRawImage04.SetActive(false);
        }
        cancelContinue = false;
        cancelContinueImage.SetActive(false);
    }

    void Update()
    {
        CheckPlayerPressed();
    }

    public void CheckPlayerPressed()
    {
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

        CheckPlayerPressContinue(SceneController.instance.selected01, "Use1", "Take1", ref player01CheckToContinue, player01ReadyImage);
        CheckPlayerPressContinue(SceneController.instance.selected02, "Use2", "Take2", ref player02CheckToContinue, player02ReadyImage);
        CheckPlayerPressContinue(SceneController.instance.selected03, "Use3", "Take3", ref player03CheckToContinue, player03ReadyImage);
        CheckPlayerPressContinue(SceneController.instance.selected04, "Use4", "Take4", ref player04CheckToContinue, player04ReadyImage);

        switch (PlayerCount)
        {
            case 1:
                if (!player01CheckToContinue && !cancelContinue && Input.GetButtonDown("Take1"))
                {
                    cancelContinue = true;
                    cancelContinueImage.SetActive(true);
                    player01ReadyImage.SetActive(false);
                    return;
                }
                else if (cancelContinue && !player01CheckToContinue && Input.GetButtonDown("Take1"))
                {
                    SceneController.instance.transition.SetTrigger(SceneController.instance.animEndHash);
                    SceneController.instance.LoadLevel(0);
                    return;
                }
                else if (onePlayer1000 && player01CheckToContinue)
                {
                    SceneController.instance.transition.SetTrigger(SceneController.instance.animEndHash);
                    SceneController.instance.LoadLevel(numberOfLevel);
                }
                break;
            case 2:
                if (!player01CheckToContinue && !cancelContinue && Input.GetButtonDown("Take1"))
                {
                    cancelContinue = true;
                    cancelContinueImage.SetActive(true);
                    player01ReadyImage.SetActive(false);
                    return;
                }
                else if (cancelContinue && Input.GetButtonDown("Take1"))
                {
                    SceneController.instance.transition.SetTrigger(SceneController.instance.animEndHash);
                    SceneController.instance.LoadLevel(0);
                    return;
                }
                else if (twoPlayerType1100 && player01CheckToContinue && player02CheckToContinue)
                {
                    SceneController.instance.transition.SetTrigger(SceneController.instance.animEndHash);
                    SceneController.instance.LoadLevel(numberOfLevel);
                }
                else if (twoPlayerType1010 && player01CheckToContinue && player03CheckToContinue)
                {
                    SceneController.instance.transition.SetTrigger(SceneController.instance.animEndHash);
                    SceneController.instance.LoadLevel(numberOfLevel);
                }
                else if (twoPlayerTpye1001 && player01CheckToContinue && player04CheckToContinue)
                {
                    SceneController.instance.transition.SetTrigger(SceneController.instance.animEndHash);
                    SceneController.instance.LoadLevel(numberOfLevel);
                }
                break;
            case 3:
                if (!player01CheckToContinue && !cancelContinue && Input.GetButtonDown("Take1"))
                {
                    cancelContinue = true;
                    cancelContinueImage.SetActive(true);
                    player01ReadyImage.SetActive(false);
                    return;
                }
                else if (cancelContinue && Input.GetButtonDown("Take1"))
                {
                    SceneController.instance.transition.SetTrigger(SceneController.instance.animEndHash);
                    SceneController.instance.LoadLevel(0);
                    return;
                }
                if (threePlayerType1110 && !player01CheckToContinue && Input.GetButtonDown("Take1"))
                {
                    SceneController.instance.transition.SetTrigger(SceneController.instance.animEndHash);
                    SceneController.instance.LoadLevel(0);
                }
                else if (threePlayerType1110 && player01CheckToContinue && player02CheckToContinue && player03CheckToContinue)
                {
                    SceneController.instance.transition.SetTrigger(SceneController.instance.animEndHash);
                    SceneController.instance.LoadLevel(numberOfLevel);
                }
                else if (threePlayerType1101 && player01CheckToContinue && player02CheckToContinue && player04CheckToContinue)
                {
                    SceneController.instance.transition.SetTrigger(SceneController.instance.animEndHash);
                    SceneController.instance.LoadLevel(numberOfLevel);
                }
                break;
            case 4:
                if (!player01CheckToContinue && !cancelContinue && Input.GetButtonDown("Take1"))
                {
                    cancelContinue = true;
                    cancelContinueImage.SetActive(true);
                    player01ReadyImage.SetActive(false);
                    return;
                }
                else if (cancelContinue && Input.GetButtonDown("Take1"))
                {
                    SceneController.instance.transition.SetTrigger(SceneController.instance.animEndHash);
                    SceneController.instance.LoadLevel(0);
                    return;
                }
                if (fourPlayer && !player01CheckToContinue && Input.GetButtonDown("Take1"))
                {
                    SceneController.instance.transition.SetTrigger(SceneController.instance.animEndHash);
                    SceneController.instance.LoadLevel(0);
                }
                else if (fourPlayer && player01CheckToContinue && player02CheckToContinue && player03CheckToContinue && player04CheckToContinue)
                {
                    SceneController.instance.transition.SetTrigger(SceneController.instance.animEndHash);
                    SceneController.instance.LoadLevel(numberOfLevel);
                }
                break;
        }
    }

    private void CheckPlayerPressContinue(bool playerChosed, string checkButton, string cancelButton, ref bool result, GameObject image)
    {
        if (playerChosed && Input.GetButtonDown(checkButton))
        {
            if (checkButton == "Use1")
            {
                cancelContinue = false;
                cancelContinueImage.SetActive(false);
            }
            image.SetActive(true);
            result = true;
        }
        else if (playerChosed && Input.GetButtonDown(cancelButton))
        {
            image.SetActive(false);
            result = false;
        }
    }
}

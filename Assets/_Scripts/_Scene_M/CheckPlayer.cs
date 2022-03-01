using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CheckPlayer : MonoBehaviour
{
    [Header("Add ButtonImage")]
    [SerializeField] GameObject player2AddButtonImage;
    [SerializeField] GameObject player3AddButtonImage;
    [SerializeField] GameObject player4AddButtonImage;

    [Header("Change Sprite")]
    [SerializeField] Sprite normal02;
    [SerializeField] Sprite normal03;
    [SerializeField] Sprite normal04;
    [SerializeField] Sprite added02;
    [SerializeField] Sprite added03;
    [SerializeField] Sprite added04;

    [Header("ActiveUI")]
    [SerializeField] GameObject checkUI;
    [SerializeField] GameObject skinUI;
    [SerializeField] GameObject levelUI;
    [SerializeField] GameObject continueUI01;
    [SerializeField] GameObject continueUI02;
    [SerializeField] GameObject continueUI03;
    [SerializeField] GameObject mainMenuNewGameButton;
    [SerializeField] GameObject checkPlayerUIStartButton;
    [SerializeField] GameObject chooseRoleUIStartButton;
    [SerializeField] GameObject chooseLevelStartButton;
    string newGameButtonName = "NewGame";

    [Header("Roles Outer frame")]
    [SerializeField] GameObject checkSelectButton01;
    [SerializeField] GameObject checkSelectButton02;
    [SerializeField] GameObject checkSelectButton03;
    [SerializeField] GameObject checkSelectButton04;
    [SerializeField] GameObject outerFrameP1;
    [SerializeField] GameObject outerFrameP2;
    [SerializeField] GameObject outerFrameP3;
    [SerializeField] GameObject outerFrameP4;
    [SerializeField] GameObject leftTraingle01;
    [SerializeField] GameObject rightTraingle01;
    [SerializeField] GameObject leftTraingle02;
    [SerializeField] GameObject rightTraingle02;
    [SerializeField] GameObject leftTraingle03;
    [SerializeField] GameObject rightTraingle03;
    [SerializeField] GameObject leftTraingle04;
    [SerializeField] GameObject rightTraingle04;
    Color normalColor = Color.white;
    Color pressedColor = new Color(0.6f, 0.6f, 0.6f, 1.0f);

    [Header("Confirm Skin")]
    [SerializeField] List<RawImage> rawImages;
    bool confirm01 = false;
    bool confirm02 = false;
    bool confirm03 = false;
    bool confirm04 = false;
    string confirmName01 = "confirm01";
    string confirmName02 = "confirm02";
    string confirmName03 = "confirm03";
    string confirmName04 = "confirm04";

    [Header("EventSystems")]
    [SerializeField] EventSystem eventSystem01;
    [SerializeField] EventSystem eventSystem02;
    [SerializeField] EventSystem eventSystem03;
    [SerializeField] EventSystem eventSystem04;
    GameObject tempCurrentButton01;
    GameObject tempCurrentButton02;
    GameObject tempCurrentButton03;
    GameObject tempCurrentButton04;

    [Header("When select player then show dancing animation")]
    [SerializeField] GameObject player01;
    [SerializeField] GameObject player02;
    [SerializeField] GameObject player03;
    [SerializeField] GameObject player04;

    [Header("Stars")]
    [SerializeField] List<GameObject> stars;
    [HideInInspector] [SerializeField] List<GameObject> dataStarts;

    public string menuDance01 { get; private set; } = "CharacterControllerTest_Male_MainMenu01";
    public string menuDance02 { get; private set; } = "CharacterControllerTest_Male_MainMenu02";
    public string menuDance03 { get; private set; } = "CharacterControllerTest_Male_MainMenu03";
    public string menuDance04 { get; private set; } = "CharacterControllerTest_Male_MainMenu04";
    public string withItemAnimator { get; private set; } = "CharacterControllerTest_Male_withItem";

    bool onePlayer1000;
    bool twoPlayerType1100;
    bool twoPlayerType1010;
    bool twoPlayerTpye1001;
    bool threePlayerType1110;
    bool threePlayerType1101;
    bool fourPlayer;

    private void Awake()
    {
        FindPlayers();
        SetUpFirstCurrentSelectedButton();
        CloseContinueUI();
    }

    void Update()
    {
        CheckMouseClickOnCurrentSelectedButton();

        AddAndCaneclPlayer();
        ChangePlayerSkinWhenSelectButtonON();
        ChangeSelectTraingleColor();
        GetReadyInRoleUI(outerFrameP1, confirm01, eventSystem01, checkSelectButton01, "Use1", "Take1", playerOneReadyInRoleUI, ref lockPlayer01);
        GetReadyInRoleUI(outerFrameP2, confirm02, eventSystem02, checkSelectButton02, "Use2", "Take2", playerTwoReadyInRoleUI, ref lockPlayer02);
        GetReadyInRoleUI(outerFrameP3, confirm03, eventSystem03, checkSelectButton03, "Use3", "Take3", playerThreeReadyInRoleUI, ref lockPlayer03);
        GetReadyInRoleUI(outerFrameP4, confirm04, eventSystem04, checkSelectButton04, "Use4", "Take4", playerFourReadyInRoleUI, ref lockPlayer04);
    }
    private void LateUpdate()
    {
        //ResetPlayer234StartButton();
        ResetMouseCurrentSelectedButton();
    }
    #region Process scripts
    private void FindPlayers()
    {
        player01 = GameObject.Find("Player01");
        player02 = GameObject.Find("Player02");
        player03 = GameObject.Find("Player03");
        player04 = GameObject.Find("Player04");
    }
    private void SetUpFirstCurrentSelectedButton()
    {
        tempCurrentButton02 = checkSelectButton02;
        tempCurrentButton03 = checkSelectButton03;
        tempCurrentButton04 = checkSelectButton04;
        eventSystem02.SetSelectedGameObject(tempCurrentButton02);
        eventSystem03.SetSelectedGameObject(tempCurrentButton03);
        eventSystem04.SetSelectedGameObject(tempCurrentButton04);
    }
    private void AddAndCaneclPlayer()
    {
        if (Input.GetButtonDown("Use2") && checkUI.activeInHierarchy)//add player02
        {
            player2AddButtonImage.GetComponent<Image>().sprite = added02;
        }
        else if (Input.GetButtonDown("Take2") && checkUI.activeInHierarchy)//cancel player02
        {
            player2AddButtonImage.GetComponent<Image>().sprite = normal02;
        }
        if (Input.GetButtonDown("Use3") && checkUI.activeInHierarchy)//add player03
        {
            player3AddButtonImage.GetComponent<Image>().sprite = added03;
        }
        else if (Input.GetButtonDown("Take3") && checkUI.activeInHierarchy)//cancel player03
        {
            player3AddButtonImage.GetComponent<Image>().sprite = normal03;
        }
        if (Input.GetButtonDown("Use4") && checkUI.activeInHierarchy)//add player04
        {
            player4AddButtonImage.GetComponent<Image>().sprite = added04;
        }
        else if (Input.GetButtonDown("Take4") && checkUI.activeInHierarchy)//cancel player04
        {
            player4AddButtonImage.GetComponent<Image>().sprite = normal04;
        }
    }

    [Header("UseToRestRoleUIButton")]
    [SerializeField] GameObject Button_SeclectImage01UseToRest;
    [SerializeField] GameObject Button_SeclectImage02;
    [SerializeField] GameObject Button_SeclectImage03;
    [SerializeField] GameObject Button_SeclectImage04;
    public void ResetPlayer234StartButton()
    {
        if (outerFrameP2.activeInHierarchy && confirm02 && eventSystem02.currentSelectedGameObject == Button_SeclectImage01UseToRest)
        {
            float vertical = Input.GetAxisRaw("Vertical2");
            float horizontal = Input.GetAxisRaw("Horizontal2");
            if ((vertical > 0.01f || horizontal < 0.01f))
                eventSystem02.SetSelectedGameObject(Button_SeclectImage02);
        }
        if (outerFrameP3.activeInHierarchy && confirm03 && eventSystem03.currentSelectedGameObject == Button_SeclectImage01UseToRest)
        {
            float vertical = Input.GetAxis("Vertical3");
            float horizontal = Input.GetAxisRaw("Horizontal3");
            if (vertical > 0.01f || horizontal < 0.01f)
                eventSystem03.SetSelectedGameObject(Button_SeclectImage03);
        }
        if (outerFrameP4.activeInHierarchy && confirm04 && eventSystem04.currentSelectedGameObject == Button_SeclectImage01UseToRest)
        {
            float vertical = Input.GetAxisRaw("Vertical4");
            float horizontal = Input.GetAxisRaw("Horizontal4");
            if ((vertical > 0.01f || horizontal < 0.01f))
                eventSystem04.SetSelectedGameObject(Button_SeclectImage04);
        }
    }

    [Header("GetReadyImagesInRoleUI")]
    [SerializeField] GameObject playerOneReadyInRoleUI;
    [SerializeField] GameObject playerTwoReadyInRoleUI;
    [SerializeField] GameObject playerThreeReadyInRoleUI;
    [SerializeField] GameObject playerFourReadyInRoleUI;
    public bool lockPlayer01 = false;
    public bool lockPlayer02 = false;
    public bool lockPlayer03 = false;
    public bool lockPlayer04 = false;

    public void CloseRoleUIAndGoToLevelUI()
    {

        //if(lockPlayer01 && lockPlayer02)
        //{
        //    OpenLevelUI();
        //    CancelPlayerRawImages();
        //    lockPlayer01 = false;
        //    lockPlayer02 = false;
        //    lockPlayer03 = false;
        //    lockPlayer04 = false;
        //    playerOneReadyInRoleUI.SetActive(false);
        //    playerTwoReadyInRoleUI.SetActive(false);
        //    playerThreeReadyInRoleUI.SetActive(false);
        //    playerFourReadyInRoleUI.SetActive(false);
        //}
    }


    public void GetReadyInRoleUI(GameObject outerFrame, bool confirm, EventSystem eventNumber, GameObject checkSelectButton, string use, string take, GameObject playerReadyInRoleUI, ref bool lockPlayer)
    {
        if (outerFrame.activeInHierarchy && confirm && eventNumber.currentSelectedGameObject == checkSelectButton && Input.GetButtonDown(use))
        {
            playerReadyInRoleUI.SetActive(true);
            lockPlayer = true;
        }
        else if (outerFrame.activeInHierarchy && confirm && eventNumber.currentSelectedGameObject == checkSelectButton && Input.GetButtonDown(take))
        {
            playerReadyInRoleUI.SetActive(false);
            lockPlayer = false;
        }
    }

    private void ChangePlayerSkinWhenSelectButtonON()
    {
        if (outerFrameP1.activeInHierarchy && Input.GetButtonDown("Horizontal1") && confirm01 && eventSystem01.currentSelectedGameObject == checkSelectButton01 && !lockPlayer01)//Change player01 skin
        {
            SceneController.instance.player01.GetComponent<ChangeRoleSkin>().ChangeSkin("Horizontal1");
            float horizontal = Input.GetAxisRaw("Horizontal1");
            if (horizontal < 0)
            {
                leftTraingle01.GetComponent<Image>().color = pressedColor;
            }
            else if (horizontal > 0)
            {
                leftTraingle01.GetComponent<Image>().color = normalColor;
            }
            if (horizontal > 0)
            {
                rightTraingle01.GetComponent<Image>().color = pressedColor;
            }
            else if (horizontal < 0)
            {
                rightTraingle01.GetComponent<Image>().color = normalColor;
            }
        }
        if (outerFrameP2.activeInHierarchy && Input.GetButtonDown("Horizontal2") && confirm02 && eventSystem02.currentSelectedGameObject == checkSelectButton02 && !lockPlayer02)//Change player02 skin
        {
            SceneController.instance.player02.GetComponent<ChangeRoleSkin>().ChangeSkin("Horizontal2");
            float horizontal = Input.GetAxisRaw("Horizontal2");
            if (horizontal < 0)
            {
                leftTraingle02.GetComponent<Image>().color = pressedColor;
            }
            else if (horizontal > 0)
            {
                leftTraingle02.GetComponent<Image>().color = normalColor;
            }
            if (horizontal > 0)
            {
                rightTraingle02.GetComponent<Image>().color = pressedColor;
            }
            else if (horizontal < 0)
            {
                rightTraingle02.GetComponent<Image>().color = normalColor;
            }
        }
        if (outerFrameP3.activeInHierarchy && Input.GetAxisRaw("Horizontal3") != 0 && confirm03 && eventSystem03.currentSelectedGameObject == checkSelectButton03 && !lockPlayer03)//Change player03 skin
        {
            SceneController.instance.player03.GetComponent<ChangeRoleSkin>().ChangeSkin("Horizontal3");
            float horizontal = Input.GetAxisRaw("Horizontal3");
            if (horizontal < 0)
            {
                leftTraingle03.GetComponent<Image>().color = pressedColor;
            }
            else if (horizontal > 0)
            {
                leftTraingle03.GetComponent<Image>().color = normalColor;
            }
            if (horizontal > 0)
            {
                rightTraingle03.GetComponent<Image>().color = pressedColor;
            }
            else if (horizontal < 0)
            {
                rightTraingle03.GetComponent<Image>().color = normalColor;
            }
        }
        if (outerFrameP4.activeInHierarchy && Input.GetAxisRaw("Horizontal4") != 0 && confirm04 && eventSystem04.currentSelectedGameObject == checkSelectButton04 && !lockPlayer04)//Change player04 skin
        {
            SceneController.instance.player04.GetComponent<ChangeRoleSkin>().ChangeSkin("Horizontal4");
            float horizontal = Input.GetAxisRaw("Horizontal4");
            if (horizontal < 0)
            {
                leftTraingle04.GetComponent<Image>().color = pressedColor;
            }
            else if (horizontal > 0)
            {
                leftTraingle04.GetComponent<Image>().color = normalColor;
            }
            if (horizontal > 0)
            {
                rightTraingle04.GetComponent<Image>().color = pressedColor;
            }
            else if (horizontal < 0)
            {
                rightTraingle04.GetComponent<Image>().color = normalColor;
            }
        }
    }
    private void ChangeSelectTraingleColor()
    {
        if (eventSystem01.currentSelectedGameObject != checkSelectButton01)
        {
            leftTraingle01.GetComponent<Image>().color = normalColor;
            rightTraingle01.GetComponent<Image>().color = normalColor;
        }
        if (eventSystem02.currentSelectedGameObject != checkSelectButton02)
        {
            leftTraingle02.GetComponent<Image>().color = normalColor;
            rightTraingle02.GetComponent<Image>().color = normalColor;
        }
        if (eventSystem03.currentSelectedGameObject != checkSelectButton03)
        {
            leftTraingle03.GetComponent<Image>().color = normalColor;
            rightTraingle03.GetComponent<Image>().color = normalColor;
        }
        if (eventSystem04.currentSelectedGameObject != checkSelectButton04)
        {
            leftTraingle04.GetComponent<Image>().color = normalColor;
            rightTraingle04.GetComponent<Image>().color = normalColor;
        }
    }
    #endregion

    #region Unity Button Events
    /// <summary>
    /// Confirm how many player will play this game;
    /// </summary>
    public void ConfirmPlayerCount()
    {
        SceneController.instance.selected01 = true;
        checkSelectButton01.SetActive(true);
        confirm01 = true;
        if (player2AddButtonImage.GetComponent<Image>().sprite == added02)
        {
            SceneController.instance.selected02 = true;
            checkSelectButton02.SetActive(true);
            confirm02 = true;
        }
        if (player3AddButtonImage.GetComponent<Image>().sprite == added03)
        {
            SceneController.instance.selected03 = true;
            checkSelectButton03.SetActive(true);
            confirm03 = true;
        }
        if (player4AddButtonImage.GetComponent<Image>().sprite == added04)
        {
            SceneController.instance.selected04 = true;
            checkSelectButton04.SetActive(true);
            confirm04 = true;
        }
    }

    private void ResetImages()
    {
        player2AddButtonImage.GetComponent<Image>().sprite = normal02;
        player3AddButtonImage.GetComponent<Image>().sprite = normal03;
        player4AddButtonImage.GetComponent<Image>().sprite = normal04;
    }

    public void ResetBool()
    {
        confirm01 = false;
        confirm02 = false;
        confirm03 = false;
        confirm04 = false;
        SceneController.instance.selected01 = false;
        SceneController.instance.selected02 = false;
        SceneController.instance.selected03 = false;
        SceneController.instance.selected04 = false;
    }

    public void ConfirmSkin(string confirmBool)
    {
        if (confirmBool == confirmName01)
        {
            confirm01 = true;
        }
        else if (confirmBool == confirmName02)
        {
            confirm02 = true;
        }
        else if (confirmBool == confirmName03)
        {
            confirm03 = true;
        }
        else if (confirmBool == confirmName04)
        {
            confirm04 = true;
        }
    }

    public void ReChoseSkin(string confirmBool)
    {
        if (confirmBool == confirmName01)
        {
            confirm01 = false;
        }
        else if (confirmBool == confirmName02)
        {
            confirm02 = false;
            checkSelectButton02.SetActive(false);
            ChangePlayerAnimator(player02, withItemAnimator);
        }
        else if (confirmBool == confirmName03)
        {
            confirm03 = false;
            checkSelectButton03.SetActive(false);
            ChangePlayerAnimator(player03, withItemAnimator);
        }
        else if (confirmBool == confirmName04)
        {
            confirm04 = false;
            checkSelectButton04.SetActive(false);
            ChangePlayerAnimator(player04, withItemAnimator);
        }
    }

    public void OpenCheckUI()
    {
        checkUI.SetActive(true);
        eventSystem01.SetSelectedGameObject(checkPlayerUIStartButton);
    }

    public void OpenSkinUI()
    {
        skinUI.SetActive(true);
        eventSystem01.SetSelectedGameObject(chooseRoleUIStartButton);
    }

    public void OpenLevelUI()
    {
        int PlayerCount = Convert.ToInt32(SceneController.instance.selected01) + Convert.ToInt32(SceneController.instance.selected02) + Convert.ToInt32(SceneController.instance.selected03) + Convert.ToInt32(SceneController.instance.selected04);

        switch (PlayerCount)
        {
            case 1:
                if (lockPlayer01)
                {
                    levelUI.SetActive(true);
                    eventSystem01.SetSelectedGameObject(chooseLevelStartButton);
                }
                break;
            case 2:
                if ((lockPlayer01 && lockPlayer02) || (lockPlayer01 && lockPlayer03) || (lockPlayer01 && lockPlayer04))
                {
                    levelUI.SetActive(true);
                    eventSystem01.SetSelectedGameObject(chooseLevelStartButton);
                }
                break;
            case 3:
                if ((lockPlayer01 && lockPlayer02 && lockPlayer03) || (lockPlayer01 && lockPlayer03 && lockPlayer04) || (lockPlayer01 && lockPlayer02 && lockPlayer04))
                {
                    levelUI.SetActive(true);
                    eventSystem01.SetSelectedGameObject(chooseLevelStartButton);
                }
                break;
            case 4:
                if (lockPlayer01 && lockPlayer02 && lockPlayer03 && lockPlayer04)
                {
                    levelUI.SetActive(true);
                    eventSystem01.SetSelectedGameObject(chooseLevelStartButton);
                }
                break;
        }
    }

    public void OpenContinueUI(int number)
    {
        switch (number)
        {
            case 1:
                continueUI01.SetActive(true);
                break;
            case 2:
                continueUI02.SetActive(true);
                break;
            case 3:
                continueUI03.SetActive(true);
                break;
        }
    }

    public void CloseCheckPlayerUI()
    {
        checkUI.SetActive(false);
        ResetImages();
        ResetBool();
        eventSystem01.SetSelectedGameObject(mainMenuNewGameButton);
    }

    public void CloseSkinUI()
    {
        skinUI.SetActive(false);
        eventSystem01.SetSelectedGameObject(mainMenuNewGameButton);
    }

    public void CloseLevelUI()
    {
        levelUI.SetActive(false);
        eventSystem01.SetSelectedGameObject(chooseRoleUIStartButton);
    }

    public void CloseContinueUI()
    {
        continueUI01.SetActive(false);
        continueUI02.SetActive(false);
        //continueUI03.SetActive(false);
    }

    public void ShowPlayerRawImages()
    {
        for (int i = 0; i < rawImages.Count; i++)
        {
            rawImages[i].enabled = true;
        }
    }

    public void CancelPlayerRawImages()
    {
        //for (int i = 0; i < rawImages.Count; i++)
        //{
        //    rawImages[i].enabled = false;
        //}
    }

    public void LoadSecneLevel(int sceneIndex)
    {
        SceneController.instance.transition.SetTrigger(SceneController.instance.animEndHash);
        SceneController.instance.LoadLevel(sceneIndex);
        skinUI.SetActive(false);
        levelUI.SetActive(false);
    }

    public void ChangePlayerAnimationToDance(int playerID)
    {
        switch (playerID)
        {
            case 1:
                if (confirm01)
                {
                    ChangePlayerAnimator(player01, menuDance01);
                    player01.GetComponent<AnimatorController>().animator.Play("Dance");
                }
                break;
            case 2:
                if (confirm02)
                {
                    ChangePlayerAnimator(player02, menuDance02);
                    player02.GetComponent<AnimatorController>().animator.Play("Dance");
                }
                break;
            case 3:
                if (confirm03)
                {
                    ChangePlayerAnimator(player03, menuDance03);
                    player03.GetComponent<AnimatorController>().animator.Play("Dance");
                }
                break;
            case 4:
                if (confirm04)
                {
                    ChangePlayerAnimator(player04, menuDance04);
                    player04.GetComponent<AnimatorController>().animator.Play("Dance");
                }
                break;
        }
    }

    public void CancelPlayerAnimationToDance(int playerID)
    {
        switch (playerID)
        {
            case 1:
                player01.GetComponent<AnimatorController>().animator.SetTrigger("Stop");
                break;
            case 2:
                player02.GetComponent<AnimatorController>().animator.SetTrigger("Stop");
                break;
            case 3:
                player03.GetComponent<AnimatorController>().animator.SetTrigger("Stop");
                break;
            case 4:
                player04.GetComponent<AnimatorController>().animator.SetTrigger("Stop");
                break;
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    #endregion

    #region solve mouse trigger currebt event problems

    private void CheckMouseClickOnCurrentSelectedButton()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
        {
            if (eventSystem01.currentSelectedGameObject != null)
            {
                tempCurrentButton01 = eventSystem01.currentSelectedGameObject;
            }
            if (eventSystem02.currentSelectedGameObject != null)
            {
                tempCurrentButton02 = eventSystem02.currentSelectedGameObject;
            }
            if (eventSystem03.currentSelectedGameObject != null)
            {
                tempCurrentButton03 = eventSystem03.currentSelectedGameObject;
            }
            if (eventSystem04.currentSelectedGameObject != null)
            {
                tempCurrentButton04 = eventSystem04.currentSelectedGameObject;
            }
        }
    }

    private void ResetMouseCurrentSelectedButton()
    {
        if (eventSystem01.currentSelectedGameObject == null)
        {
            eventSystem01.SetSelectedGameObject(tempCurrentButton01);
        }
        if (eventSystem02.currentSelectedGameObject == null)
        {
            eventSystem02.SetSelectedGameObject(tempCurrentButton02);
        }
        if (eventSystem03.currentSelectedGameObject == null)
        {
            eventSystem03.SetSelectedGameObject(tempCurrentButton03);
        }
        if (eventSystem04.currentSelectedGameObject == null)
        {
            eventSystem04.SetSelectedGameObject(tempCurrentButton04);
        }
    }
    #endregion

    public void ChangePlayerAnimator(GameObject player, string animatorName)
    {
        AnimatorController anim02 = player.GetComponent<AnimatorController>();
        anim02.animator = player.GetComponent<Animator>();
        anim02.Init();
        anim02.animator.runtimeAnimatorController = Resources.Load(animatorName) as RuntimeAnimatorController;
    }

    public void NewGameResetStars()
    {
        Color normal = new Color(0.2f, 0.2f, 0.2f, 1.0f);
        SaveStarsState.instance.SaveDate(1, 3, normal);
        SaveStarsState.instance.SaveDate(2, 3, normal);
        SaveStarsState.instance.SaveDate(3, 3, normal);
        SaveStarsState.instance.LoadDate();
        UpdateStarsStates updateStars = new UpdateStarsStates();
        dataStarts.Add(GameObject.Find(updateStars.star01));
        dataStarts.Add(GameObject.Find(updateStars.star02));
        dataStarts.Add(GameObject.Find(updateStars.star03));
        dataStarts.Add(GameObject.Find(updateStars.star04));
        dataStarts.Add(GameObject.Find(updateStars.star05));
        dataStarts.Add(GameObject.Find(updateStars.star06));
        dataStarts.Add(GameObject.Find(updateStars.star07));
        dataStarts.Add(GameObject.Find(updateStars.star08));
        dataStarts.Add(GameObject.Find(updateStars.star09));

        for (int i = 0; i < dataStarts.Count; i++)
        {
            stars[i].GetComponent<RawImage>().color = dataStarts[i].GetComponent<RawImage>().color;
        }
    }

    public void ResetRoleConfirmImage()
    {
        lockPlayer01 = false;
        lockPlayer02 = false;
        lockPlayer03 = false;
        lockPlayer04 = false;
        playerOneReadyInRoleUI.SetActive(false);
        playerTwoReadyInRoleUI.SetActive(false);
        playerThreeReadyInRoleUI.SetActive(false);
        playerFourReadyInRoleUI.SetActive(false);
    }

}

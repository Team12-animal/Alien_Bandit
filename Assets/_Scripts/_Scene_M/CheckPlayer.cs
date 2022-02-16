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
    string menuDance01 = "CharacterControllerTest_Male_MainMenu01";
    string menuDance02 = "CharacterControllerTest_Male_MainMenu02";
    string menuDance03 = "CharacterControllerTest_Male_MainMenu03";
    string menuDance04 = "CharacterControllerTest_Male_MainMenu04";


    private void Awake()
    {
        FindPlayers();
        SetUpFirstCurrentSelectedButton();
    }

    void Update()
    {
        CheckMouseClickOnCurrentSelectedButton();

        AddAndCaneclPlayer();
        ChangePlayerSkinWhenSelectButtonON();
        ChangeSelectTraingleColor();
    }
    private void LateUpdate()
    {
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
        if (Input.GetButtonDown("Use2"))//add player02
        {
            player2AddButtonImage.GetComponent<Image>().sprite = added02;
        }
        else if (Input.GetButtonDown("Take2"))//cancel player02
        {
            player2AddButtonImage.GetComponent<Image>().sprite = normal02;
        }
        if (Input.GetButtonDown("Use3"))//add player03
        {
            player3AddButtonImage.GetComponent<Image>().sprite = added03;
        }
        else if (Input.GetButtonDown("Take3"))//cancel player03
        {
            player3AddButtonImage.GetComponent<Image>().sprite = normal03;
        }
        if (Input.GetButtonDown("Use4"))//add player04
        {
            player4AddButtonImage.GetComponent<Image>().sprite = added04;
        }
        else if (Input.GetButtonDown("Take4"))//cancel player04
        {
            player4AddButtonImage.GetComponent<Image>().sprite = normal04;
        }
    }
    private void ChangePlayerSkinWhenSelectButtonON()
    {
        if (outerFrameP1.activeInHierarchy && Input.GetButtonDown("Horizontal1") && confirm01 && eventSystem01.currentSelectedGameObject == checkSelectButton01)//Change player01 skin
        {
            SceneController.instance.player01.GetComponent<ChangeRoleSkin>().ChangeSkin("Horizontal1");
            float horizontal = Input.GetAxisRaw("Horizontal1");
            if (horizontal < 0)
            {
                leftTraingle01.GetComponent<Image>().color = Color.red;
            }
            else if (horizontal > 0)
            {
                leftTraingle01.GetComponent<Image>().color = Color.black;
            }
            if (horizontal > 0)
            {
                rightTraingle01.GetComponent<Image>().color = Color.red;
            }
            else if (horizontal < 0)
            {
                rightTraingle01.GetComponent<Image>().color = Color.black;
            }
        }
        if (outerFrameP2.activeInHierarchy && Input.GetButtonDown("Horizontal2") && confirm02 && eventSystem02.currentSelectedGameObject == checkSelectButton02)//Change player02 skin
        {
            SceneController.instance.player02.GetComponent<ChangeRoleSkin>().ChangeSkin("Horizontal2");
            float horizontal = Input.GetAxisRaw("Horizontal2");
            if (horizontal < 0)
            {
                leftTraingle02.GetComponent<Image>().color = Color.red;
            }
            else if (horizontal > 0)
            {
                leftTraingle02.GetComponent<Image>().color = Color.black;
            }
            if (horizontal > 0)
            {
                rightTraingle02.GetComponent<Image>().color = Color.red;
            }
            else if (horizontal < 0)
            {
                rightTraingle02.GetComponent<Image>().color = Color.black;
            }
        }
        if (outerFrameP3.activeInHierarchy && Input.GetButtonDown("Horizontal3") && confirm03 && eventSystem03.currentSelectedGameObject == checkSelectButton03)//Change player03 skin
        {
            SceneController.instance.player03.GetComponent<ChangeRoleSkin>().ChangeSkin("Horizontal3");
            float horizontal = Input.GetAxisRaw("Horizontal3");
            if (horizontal < 0)
            {
                leftTraingle03.GetComponent<Image>().color = Color.red;
            }
            else if (horizontal > 0)
            {
                leftTraingle03.GetComponent<Image>().color = Color.black;
            }
            if (horizontal > 0)
            {
                rightTraingle03.GetComponent<Image>().color = Color.red;
            }
            else if (horizontal < 0)
            {
                rightTraingle03.GetComponent<Image>().color = Color.black;
            }
        }
        if (outerFrameP4.activeInHierarchy && Input.GetButtonDown("Horizontal4") && confirm04 && eventSystem04.currentSelectedGameObject == checkSelectButton04)//Change player04 skin
        {
            SceneController.instance.player04.GetComponent<ChangeRoleSkin>().ChangeSkin("Horizontal4");
            float horizontal = Input.GetAxisRaw("Horizontal4");
            if (horizontal < 0)
            {
                leftTraingle04.GetComponent<Image>().color = Color.red;
            }
            else if (horizontal > 0)
            {
                leftTraingle04.GetComponent<Image>().color = Color.black;
            }
            if (horizontal > 0)
            {
                rightTraingle04.GetComponent<Image>().color = Color.red;
            }
            else if (horizontal < 0)
            {
                rightTraingle04.GetComponent<Image>().color = Color.black;
            }
        }
    }
    private void ChangeSelectTraingleColor()
    {
        if (eventSystem01.currentSelectedGameObject != checkSelectButton01)
        {
            leftTraingle01.GetComponent<Image>().color = Color.black;
            rightTraingle01.GetComponent<Image>().color = Color.black;
        }
        if (eventSystem02.currentSelectedGameObject != checkSelectButton02)
        {
            leftTraingle02.GetComponent<Image>().color = Color.black;
            rightTraingle02.GetComponent<Image>().color = Color.black;
        }
        if (eventSystem03.currentSelectedGameObject != checkSelectButton03)
        {
            leftTraingle03.GetComponent<Image>().color = Color.black;
            rightTraingle03.GetComponent<Image>().color = Color.black;
        }
        if (eventSystem04.currentSelectedGameObject != checkSelectButton04)
        {
            leftTraingle04.GetComponent<Image>().color = Color.black;
            rightTraingle04.GetComponent<Image>().color = Color.black;
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
        }
        else if (confirmBool == confirmName03)
        {
            confirm03 = false;
        }
        else if (confirmBool == confirmName04)
        {
            confirm04 = false;
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
        levelUI.SetActive(true);
        eventSystem01.SetSelectedGameObject(chooseLevelStartButton);
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

    public void ShowPlayerRawImages()
    {
        for (int i = 0; i < rawImages.Count; i++)
        {
            rawImages[i].enabled = true;
        }
    }

    public void CancelPlayerRawImages()
    {
        for (int i = 0; i < rawImages.Count; i++)
        {
            rawImages[i].enabled = false;
        }
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
                AnimatorController anim01 = player01.GetComponent<AnimatorController>();
                anim01.animator = player01.GetComponent<Animator>();
                anim01.Init();
                anim01.animator.runtimeAnimatorController = Resources.Load(menuDance01) as RuntimeAnimatorController;
                player01.GetComponent<AnimatorController>().animator.Play("Dance");
                break;
            case 2:
                AnimatorController anim02 = player02.GetComponent<AnimatorController>();
                anim02.animator = player02.GetComponent<Animator>();
                anim02.Init();
                anim02.animator.runtimeAnimatorController = Resources.Load(menuDance02) as RuntimeAnimatorController;
                player02.GetComponent<AnimatorController>().animator.Play("Dance");
                break;
            case 3:
                AnimatorController anim03 = player03.GetComponent<AnimatorController>();
                anim03.animator = player03.GetComponent<Animator>();
                anim03.Init();
                anim03.animator.runtimeAnimatorController = Resources.Load(menuDance03) as RuntimeAnimatorController;
                player03.GetComponent<AnimatorController>().animator.Play("Dance");
                break;
            case 4:
                AnimatorController anim04 = player04.GetComponent<AnimatorController>();
                anim04.animator = player04.GetComponent<Animator>();
                anim04.Init();
                anim04.animator.runtimeAnimatorController = Resources.Load(menuDance04) as RuntimeAnimatorController;
                player04.GetComponent<AnimatorController>().animator.Play("Dance");
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
}

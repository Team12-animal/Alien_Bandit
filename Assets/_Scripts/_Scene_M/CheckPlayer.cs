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
        player01 = GameObject.Find("Player01");
        player02 = GameObject.Find("Player02");
        player03 = GameObject.Find("Player03");
        player04 = GameObject.Find("Player04");
    }

    void Update()
    {
        if (outerFrameP1.activeInHierarchy && Input.GetButtonDown("Horizontal1") && !confirm01 && eventSystem01.currentSelectedGameObject == checkSelectButton01)//Change player01 skin
        {
            SceneController.instance.player01.GetComponent<ChangeRoleSkin>().ChangeSkin("Horizontal1");
        }
        if (outerFrameP2.activeInHierarchy && Input.GetButtonDown("Horizontal2") && !confirm02 && eventSystem02.currentSelectedGameObject == checkSelectButton02)//Change player02 skin
        {
            SceneController.instance.player02.GetComponent<ChangeRoleSkin>().ChangeSkin("Horizontal2");
        }
        if (outerFrameP3.activeInHierarchy && Input.GetButtonDown("Horizontal3") && !confirm03 && eventSystem03.currentSelectedGameObject == checkSelectButton03)//Change player03 skin
        {
            SceneController.instance.player03.GetComponent<ChangeRoleSkin>().ChangeSkin("Horizontal3");
        }
        if (outerFrameP4.activeInHierarchy && Input.GetButtonDown("Horizontal4") && !confirm04 && eventSystem04.currentSelectedGameObject == checkSelectButton04)//Change player04 skin
        {
            SceneController.instance.player04.GetComponent<ChangeRoleSkin>().ChangeSkin("Horizontal4");
        }
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
    /// <summary>
    /// Confirm how many player will play this game;
    /// </summary>
    public void ConfirmPlayerCount()
    {
        SceneController.instance.selected01 = true;
        checkSelectButton01.SetActive(true);
        if (player2AddButtonImage.GetComponent<Image>().sprite == added02)
        {
            SceneController.instance.selected02 = true;
            checkSelectButton02.SetActive(true);
        }
        if (player3AddButtonImage.GetComponent<Image>().sprite == added03)
        {
            SceneController.instance.selected03 = true;
            checkSelectButton03.SetActive(true);
        }
        if (player4AddButtonImage.GetComponent<Image>().sprite == added04)
        {
            SceneController.instance.selected04 = true;
            checkSelectButton04.SetActive(true);
        }
    }
    /// <summary>
    /// Cancel Check Player UI;
    /// </summary>
    public void CancelCheckPlayerUI()
    {
        ResetImages();
        ResetBool();
        eventSystem01.SetSelectedGameObject(mainMenuNewGameButton);
    }
    /// <summary>
    /// Going to Choose Role UI;
    /// </summary>
    public void InactiveCheckUI()
    {
        checkUI.SetActive(false);
        ResetImages();
        ResetBool();
    }

    public void ActiveCheckUI()
    {
        checkUI.SetActive(true);
        eventSystem01.SetSelectedGameObject(checkPlayerUIStartButton);
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
    public void LoadLevel(int sceneIndex)
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
}

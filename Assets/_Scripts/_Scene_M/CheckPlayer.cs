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
    [SerializeField] GameObject startButton;
    [SerializeField] GameObject newGameButton;
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
    [SerializeField] bool confirm01 = false;
    [SerializeField] bool confirm02 = false;
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
        EventSystem.current.SetSelectedGameObject(newGameButton);
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
        EventSystem.current.SetSelectedGameObject(startButton);
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
        EventSystem.current.SetSelectedGameObject(skinUI);
    }

    public void OpenLevelUI()
    {
        levelUI.SetActive(true);
    }

    public void CloseSkinUI()
    {
        skinUI.SetActive(false);
        EventSystem.current.SetSelectedGameObject(newGameButton);
    }

}

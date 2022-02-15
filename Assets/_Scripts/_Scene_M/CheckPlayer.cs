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
    [SerializeField] GameObject startButton;
    [SerializeField] GameObject newGameButton;

    [Header("Roles Outer frame")]
    [SerializeField] GameObject checkButton01;
    [SerializeField] GameObject checkButton02;
    [SerializeField] GameObject checkButton03;
    [SerializeField] GameObject checkButton04;
    [SerializeField] GameObject outerFrameP1;
    [SerializeField] GameObject outerFrameP2;
    [SerializeField] GameObject outerFrameP3;
    [SerializeField] GameObject outerFrameP4;
    Vector3 pos01 = new Vector3(-173.399994f - (-303.0f), 40f - (-137.0f), 0);
    Vector3 pos02 = new Vector3(-55.7999992f - (-303.0f), 40f - (-137.0f), 0);
    Vector3 pos03 = new Vector3(61.9000015f, 40f, 0);
    Vector3 pos04 = new Vector3(175.100006f, 40f, 0);


    void Update()
    {
        if (outerFrameP1.activeInHierarchy)
        {
            if (Input.GetButtonDown("Horizontal1"))
            {
                SceneController.instance.player01.GetComponent<ChangeRoleSkin>().ChangeSkin("Horizontal1");
            }
        }
        if (outerFrameP2.activeInHierarchy)
        {
            if (Input.GetButtonDown("Horizontal2"))
            {
                SceneController.instance.player02.GetComponent<ChangeRoleSkin>().ChangeSkin("Horizontal2");
            }
        }
        if (outerFrameP3.activeInHierarchy)
        {
            if (Input.GetButtonDown("Horizontal3"))
            {
                SceneController.instance.player03.GetComponent<ChangeRoleSkin>().ChangeSkin("Horizontal3");
            }
        }
        if (outerFrameP4.activeInHierarchy)
        {
            if (Input.GetButtonDown("Horizontal4"))
            {
                SceneController.instance.player04.GetComponent<ChangeRoleSkin>().ChangeSkin("Horizontal4");
            }
        }
        if (Input.GetButtonDown("Use2"))
        {
            player2AddButtonImage.GetComponent<Image>().sprite = added02;
        }
        else if (Input.GetButtonDown("Take2"))
        {
            player2AddButtonImage.GetComponent<Image>().sprite = normal02;
        }
        if (Input.GetButtonDown("Use3"))
        {
            player3AddButtonImage.GetComponent<Image>().sprite = added03;
        }
        else if (Input.GetButtonDown("Take3"))
        {
            player3AddButtonImage.GetComponent<Image>().sprite = normal03;
        }
        if (Input.GetButtonDown("Use4"))
        {
            player4AddButtonImage.GetComponent<Image>().sprite = added04;
        }
        else if (Input.GetButtonDown("Take4"))
        {
            player4AddButtonImage.GetComponent<Image>().sprite = normal04;
        }


    }

    public void ConfirmPlayerCount()
    {
        SceneController.instance.selected01 = true;
        checkButton01.SetActive(true);
        if (player2AddButtonImage.GetComponent<Image>().sprite == added02)
        {
            SceneController.instance.selected02 = true;
            checkButton02.SetActive(true);
        }
        if (player3AddButtonImage.GetComponent<Image>().sprite == added03)
        {
            SceneController.instance.selected03 = true;
            checkButton03.SetActive(true);
        }
        if (player4AddButtonImage.GetComponent<Image>().sprite == added04)
        {
            SceneController.instance.selected04 = true;
            checkButton04.SetActive(true);
        }
        ResetImages();
    }

    public void CancelCheckPlayerUI()
    {
        player2AddButtonImage.GetComponent<Image>().sprite = normal02;
        player3AddButtonImage.GetComponent<Image>().sprite = normal03;
        player4AddButtonImage.GetComponent<Image>().sprite = normal04;
        EventSystem.current.SetSelectedGameObject(newGameButton);
    }

    public void InactiveCheckUI()
    {
        checkUI.SetActive(false);
        ResetImages();
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
}

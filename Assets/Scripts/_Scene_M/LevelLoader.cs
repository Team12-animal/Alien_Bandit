using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    [Header("UI¤¶­±¶}±Ò»P§_")]
    [SerializeField] GameObject chooseRoleUI;
    [SerializeField] GameObject chooseLevelUI;
    ChangeRoleSkin changeRole;

    [SerializeField]GameObject temp01;
    [SerializeField]GameObject temp02;

    private void Start()
    {
        changeRole = new ChangeRoleSkin();
    }

    public void LoadLevel(int sceneIndex)
    {
        SceneController.instance.transition.SetTrigger(SceneController.instance.animEndHash);
        SceneController.instance.LoadLevel(sceneIndex);
        changeRole.LoadCharacter();
    }

    public void OpenChooseRoleUI(bool open)
    {
        chooseRoleUI.SetActive(open);
    }
    public void OpenChooseLevelUI(bool open)
    {
        chooseLevelUI.SetActive(open);
        changeRole.SaveCharacter();
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void CloseSelect(GameObject number)
    {
        if (number == temp01)
        {
            SceneController.instance.selected01 = false;
        }
        else if (number == temp02)
        {
            SceneController.instance.selected02 = false;
        }
        number.SetActive(false);
    }
    public void OpenSelect(GameObject number)
    {
        number.SetActive(true);
    }
}

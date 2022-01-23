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
    ChangeRoleSkin changeRole = new ChangeRoleSkin();

    [SerializeField]GameObject temp01;
    [SerializeField]GameObject temp02;


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

    public void CloseSelect(GameObject target)
    {
        if (target == temp01)
        {
            SceneController.instance.selected01 = false;
        }
        else if (target == temp02)
        {
            SceneController.instance.selected02 = false;
        }
        target.SetActive(false);
    }
    public void OpenSelect(GameObject target)
    {
        target.SetActive(true);
    }
}

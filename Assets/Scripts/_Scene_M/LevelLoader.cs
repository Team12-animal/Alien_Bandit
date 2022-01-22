using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] GameObject chooseRoleUI;
    [SerializeField] GameObject chooseLevelUI;
    ChangeRoleSkin changeRole;

    private void Start()
    {
        changeRole= new ChangeRoleSkin();
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


}

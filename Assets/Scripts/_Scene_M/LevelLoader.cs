using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{

    public void LoadLevel(int sceneIndex)
    {
            SceneController.instance.transition.SetTrigger(SceneController.instance.animEndHash);
            SceneController.instance.LoadLevel(sceneIndex);
    }

    public void LoadMainMenu()
    {
        SceneController.instance.LoadMainMenu();
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}

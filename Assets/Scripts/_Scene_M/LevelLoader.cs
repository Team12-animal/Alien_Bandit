using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public void LoadLevel(int sceneIndex)
    {
        SceneController.instance.LoadLevel(sceneIndex);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToMain : MonoBehaviour
{
    [SerializeField] GameObject levelControl;
    public void LoadLevel(int sceneIndex)//back to main menu; sceneIndex = 0; 
    {
        SceneController.instance.transition.SetTrigger(SceneController.instance.animEndHash);
        SceneController.instance.LoadLevel(sceneIndex);
        levelControl.gameObject.SetActive(false);

    }
}

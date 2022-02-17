using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChangeRoleSkin : MonoBehaviour
{
    [SerializeField] GameObject[] roleSkins;
    int currentSkin = 0;
    string selector = "selectedCharacter";

    void Update()
    {
        //ChangeSkin();
    }

    public void ChangeSkin(string buttonName)
    {
        if (Input.GetButtonDown(buttonName) || Input.GetKeyDown(KeyCode.Joystick1Button8))
        {
            roleSkins[currentSkin].SetActive(false);
            if (currentSkin < roleSkins.Length - 1)
            {
                roleSkins[currentSkin + 1].SetActive(true);
                currentSkin++;
            }
            else if (currentSkin == 18)
            {
                currentSkin = 0;
                roleSkins[currentSkin].SetActive(true);
            }
        }
    }

    public void LoadCharacter()
    {
        PlayerPrefs.GetInt(selector, currentSkin);
    }

    public void SaveCharacter()
    {
        PlayerPrefs.SetInt(selector, currentSkin);
    }
}

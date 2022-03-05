using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnlockLevels : MonoBehaviour
{
    [Header("Setting unlock level buttons")]
    [SerializeField] List<GameObject> stars;
    [SerializeField] List<Button> levelsButtons;
    [SerializeField] Selectable selectableLevel02;
    [SerializeField] Selectable selectableLevel03;


    void Update()
    {
        UnlockLevelButtons();
    }

    public void UnlockLevelButtons()
    {
        //if level star get more than one then unlock level button
        if (stars[0].GetComponent<RawImage>().color == new Color(1.0f, 1.0f, 1.0f, 1.0f))
        {
            //setting navigation;levelsButton[0] = level01 , [1] = level02;
            Navigation firstButton = levelsButtons[0].GetComponent<Button>().navigation;
            firstButton.mode = Navigation.Mode.Explicit;
            firstButton.selectOnRight = selectableLevel02;
            levelsButtons[0].GetComponent<Button>().navigation = firstButton;
            levelsButtons[1].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f);
        }
        else
        {
            Navigation firstButton = levelsButtons[0].GetComponent<Button>().navigation;
            firstButton.mode = Navigation.Mode.Explicit;
            firstButton.selectOnRight = null;
            levelsButtons[0].GetComponent<Button>().navigation = firstButton;
            levelsButtons[1].GetComponent<Image>().color = new Color(0.39f, 0.39f, 0.39f);
        }

        if (stars[3].GetComponent<RawImage>().color == new Color(1.0f, 1.0f, 1.0f, 1.0f))
        {
            //setting navigation;levelsButton[0] = level02 , [1] = level03;
            Navigation firstButton = levelsButtons[1].GetComponent<Button>().navigation;
            firstButton.mode = Navigation.Mode.Explicit;
            firstButton.selectOnRight = selectableLevel03;
            levelsButtons[1].GetComponent<Button>().navigation = firstButton;
            levelsButtons[2].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f);
        }
        else
        {
            Navigation firstButton = levelsButtons[1].GetComponent<Button>().navigation;
            firstButton.mode = Navigation.Mode.Explicit;
            firstButton.selectOnRight = null;
            levelsButtons[1].GetComponent<Button>().navigation = firstButton;
            levelsButtons[2].GetComponent<Image>().color = new Color(0.39f, 0.39f, 0.39f);
        }
    }

}

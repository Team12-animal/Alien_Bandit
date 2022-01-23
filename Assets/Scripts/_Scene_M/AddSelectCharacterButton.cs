using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddSelectCharacterButton : MonoBehaviour
{
    [SerializeField] GameObject[] rawImage1;
    [SerializeField] GameObject[] rawImage2;

    string playerBox01 = "PlayerBox01";
    string playerBox02 = "PlayerBox02";
    string player01 = "Player01";
    string player02 = "Player02";

    string menuDance01 = "CharacterControllerTest_Male_MainMenu01";
    string menuDance02 = "CharacterControllerTest_Male_MainMenu02";

    AnimatorController anim;

    public void SelectChanel(int number)
    {
        if (number == 1)
        {
            SceneController.instance.selected01 = true;
            AddCharacter(rawImage1, playerBox01);
        }
        else if (number == 2)
        {
            SceneController.instance.selected02 = true;
            AddCharacter(rawImage2, playerBox02);
        }
    }

    public void AddCharacter(GameObject[] objects, string player)
    {
        for (int i = 0; i < objects.Length; i++)
        {
            if (i == 2)
            {
                GameObject temp = GameObject.Find(player);
                if (player == playerBox01)
                {
                    GameObject temp01 = temp.transform.Find(player01).gameObject;
                    objects[i] = temp01;
                    objects[i].SetActive(true);

                    AnimatorController anim = objects[i].GetComponent<AnimatorController>();
                    anim.animator = objects[i].GetComponent<Animator>();
                    anim.Init();
                    anim.animator.runtimeAnimatorController = Resources.Load(menuDance01) as RuntimeAnimatorController;
                }
                else if (player == playerBox02)
                {
                    GameObject temp01 = temp.transform.Find(player02).gameObject;
                    objects[i] = temp01;
                    objects[i].SetActive(true);

                    AnimatorController anim = objects[i].GetComponent<AnimatorController>();
                    anim.animator = objects[i].GetComponent<Animator>();
                    anim.Init();
                    anim.animator.runtimeAnimatorController = Resources.Load(menuDance02) as RuntimeAnimatorController;
                }
            }
            objects[i].SetActive(true);
        }
        objects[objects.Length - 1].SetActive(false);
    }
}

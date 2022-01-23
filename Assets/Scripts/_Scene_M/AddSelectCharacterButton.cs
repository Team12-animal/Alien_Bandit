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
                    temp01.SetActive(true);
                }
                else if (player == playerBox02)
                {
                    GameObject temp01 = temp.transform.Find(player02).gameObject;
                    objects[i] = temp01;
                    temp01.SetActive(true);
                }
            }
            objects[i].SetActive(true);
        }
        objects[objects.Length - 1].SetActive(false);
    }
}

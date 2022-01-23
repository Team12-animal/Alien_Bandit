using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddSelectCharacterButton : MonoBehaviour
{
    [SerializeField] GameObject[] rawImage1;
    [SerializeField] GameObject[] rawImage2;

    string player01 = "Player01";
    string player02 = "Player02";

    public void SelectChanel(int number)
    {
        if (number == 1)
        {
            SceneController.instance.selected01 = true;
            AddCharacter(rawImage1, player01);
        }
        else if (number == 2)
        {
            SceneController.instance.selected02 = true;
            AddCharacter(rawImage2, player02);
        }
    }

    public void AddCharacter(GameObject[] objects, string player)
    {
        for (int i = 0; i < objects.Length; i++)
        {
            if(i == 2)
            {
                GameObject temp = GameObject.Find(player);
                objects[i] = temp;
            }
            objects[i].SetActive(true);
        }
        objects[objects.Length - 1].SetActive(false);
    }
}

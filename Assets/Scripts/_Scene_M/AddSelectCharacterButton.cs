using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddSelectCharacterButton : MonoBehaviour
{
    [SerializeField] GameObject[] rawImage1;
    [SerializeField] GameObject[] rawImage2;


    public void SelectChanel(int number)
    {
        if (number == 1)
        {
            AddCharacter(rawImage1);
        }
        else if (number == 2)
        {
            AddCharacter(rawImage2);
        }
    }

    public void AddCharacter(GameObject[] objects)
    {
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].SetActive(true);
        }
        objects[objects.Length - 1].SetActive(false);
    }
}

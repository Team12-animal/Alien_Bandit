using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckMouseOnclick : MonoBehaviour
{
    public void CheckWhichUIButton(string name)
    {
        UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name = name;
    }
}

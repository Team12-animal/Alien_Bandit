using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeRoleSkin : MonoBehaviour
{
    [SerializeField] GameObject role;
    [SerializeField] GameObject[] roleSkins;
    int currentSkin = 0;

    void Update()
    {
        ChangeSkin();
    }

    public void ChangeSkin()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            roleSkins[currentSkin].SetActive(false);
            if(currentSkin < roleSkins.Length-1)
            {
                roleSkins[currentSkin+1].SetActive(true);
                currentSkin++;
            }
            else
           // if (currentSkin == 18)
            {
                currentSkin = 0;
            }
        }
    }
}

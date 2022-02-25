using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue 
{
    [HideInInspector]
    public string[] focusItem;

    [TextArea(3, 1)]
    public string[] title;

    [TextArea(3,3)]
    public string[] sentences;

    [TextArea(3, 1)]
    public string[] buttonTip;

}

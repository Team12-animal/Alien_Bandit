using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelControl : MonoBehaviour
{
    //for treeController
    public virtual float GetGameTime()
    {
        return 700.0f;
    }

    //for treeController
    public virtual bool WinOrNot()
    {
        return false;
    }
}

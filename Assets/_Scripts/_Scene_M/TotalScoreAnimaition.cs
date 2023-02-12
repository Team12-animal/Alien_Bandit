using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotalScoreAnimaition : MonoBehaviour
{
    [SerializeField] LevelControl levelControl;

    public void UpdateTotalScore()
    {
        levelControl.TotalScoreUI();
    }
}

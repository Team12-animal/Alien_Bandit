using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetStarTest : MonoBehaviour
{
    [SerializeField] LevelOneControl levelOneControl;
    [SerializeField] LevelTwoControl levelTwoControl;
    




    private void Awake()
    {
        collectRabbits = 0;
        collectRaccoons = 0;
        collectPigs = 0;
    }
}

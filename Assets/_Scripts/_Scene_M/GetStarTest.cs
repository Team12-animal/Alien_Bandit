using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetStarTest : MonoBehaviour
{
    [SerializeField] LevelOneControl levelOneControl;
    [SerializeField] LevelTwoControl levelTwoControl;
    
    public int collectRabbits; //catched rabbit amount
    public int collectRaccoons; //catched raccoon amount
    public int collectLittleRaccoons; //
    public int collectPigs; //catched pig amount

    private void Awake()
    {
        collectRabbits = 0;
        collectRaccoons = 0;
        collectPigs = 0;
    }
}

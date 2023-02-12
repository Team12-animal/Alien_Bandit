using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetStarTest : MonoBehaviour
{
    [SerializeField] LevelOneControl levelOneControl;
    [SerializeField] LevelTwoControl levelTwoControl;

    AnimalCatcher catcher;

    public int collectRabbits; //catched rabbit amount
    public int collectRaccoons; //catched raccoon amount
    public int collectLittleRaccoons; //catched littel raccoon amount
    public int collectPigs; //catched pig amount

    private void Awake()
    {
        catcher = this.GetComponent<AnimalCatcher>();

        collectRabbits = catcher.collectRabbits;
        collectRaccoons = catcher.collectRaccoons;
        collectLittleRaccoons = catcher.collectLittleRaccoons;
        collectPigs = catcher.collectPigs;
    }

    private void Update()
    {
        collectRabbits = catcher.collectRabbits;
        collectRaccoons = catcher.collectRaccoons;
        collectLittleRaccoons = catcher.collectLittleRaccoons;
        collectPigs = catcher.collectPigs;

        if (collectRabbits >= 2)
        {
            if (levelOneControl != null)
                levelOneControl.isWin = true;
        }
    }
}

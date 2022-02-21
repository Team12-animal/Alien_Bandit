using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetStarTest : MonoBehaviour
{
    [SerializeField] LevelOneControl levelOneControl;
    [SerializeField] LevelTwoControl levelTwoControl;
    
    public int collectRabbits; //catched rabbit amount
    public int collectRaccoons; //catched raccoon amount
    public int collectPigs; //catched pig amount

    public int targetAmt;

    private void Awake()
    {
        collectRabbits = 0;
        collectRaccoons = 0;
        collectPigs = 0;
    }

    private void Update()
    {
        CheckCollectAmt();
    }

    private void CheckCollectAmt()
    {
        if(collectTargets == targetAmt)
        {
            if(levelOneControl != null)
            {
                levelOneControl.isWin = true;
            }

            if (levelTwoControl != null)
            {
                levelTwoControl.isWin = true;
            }
        }
    }


    /// <summary>
    /// for testing
    /// </summary>
    /// <param name="collision"></param>
    /// 
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.collider.CompareTag("Player"))
    //    {
    //        if (collectTargets == 1)
    //        {
    //            if (levelOneControl == null)
    //                return;
    //            else
    //                levelOneControl.isWin = true;
    //        }
    //        collectTargets++;
    //    }
    //    if (collision.collider.CompareTag("Player"))
    //    {
    //        if (collectTargets == 1)
    //        {
    //            if (levelTwoControl == null)
    //                return;
    //            else
    //                levelTwoControl.isWin = true;
    //        }
    //        collectTargets++;
    //    }
    //}

    //private void Update()
    //{
        
    //}

    //private void 
}

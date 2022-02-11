using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetStarTest : MonoBehaviour
{
    [SerializeField] LevelOneControl levelOneControl;
    [SerializeField] LevelTwoControl levelTwoControl;
    [SerializeField] public int collectTargets;

    public int targetAmt;

    private void Awake()
    {
        collectTargets = 0;
    }

    private void Update()
    {
        if(levelOneControl == null)
        {
            return;
        }
        else
        {
            CheckCollectAmt();
        }
    }

    private void CheckCollectAmt()
    {
        if(collectTargets == targetAmt)
        {
            levelOneControl.isWin = true;
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

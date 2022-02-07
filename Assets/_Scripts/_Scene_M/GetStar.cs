using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetStar : MonoBehaviour
{
    [SerializeField] LevelOneControl levelOneControl;
    [SerializeField] int collectTargets;

    private void Start()
    {
        collectTargets = 0;
    }
    /// <summary>
    /// for testing
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if (collectTargets == 5)
            {
                levelOneControl.isWin = true;
            }
            collectTargets++;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfController : MonoBehaviour
{
    [SerializeField] List<GameObject> birthPoses;
    [SerializeField] List<GameObject> preys;

    GameObject wolf;
    GameObject birthPos;

    //coroutine
    public float waitForStart;
    public float delay;

    private void Awake()
    {
        
    }

    private void LoadWolf()
    {
        var prefab = Resources.Load<GameObject>("Wolf");
        wolf = GameObject.Instantiate(prefab) as GameObject;
        wolf.SetActive(false);
    }

    private void SpawnWolf()
    {
        //SetBirthPos();
    }
}

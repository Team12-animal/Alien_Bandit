using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfController : MonoBehaviour
{
    public GameObject wolf;
    public GameObject birthPos;
    public GameObject homePos;

    [SerializeField] List<GameObject> preys;

    private WolfAIData data;

    //coroutine
    public float waitForStart;
    public float delay;

    //jump
    public List<GameObject> jumpPs;

    private void Awake()
    {
        LoadWolf();
        StartCoroutine(ActiveWolf());
    }
    IEnumerator ActiveWolf()
    {
        yield return new WaitForSecondsRealtime(waitForStart);

        while (true)
        {
            preys = FindPreys();

            if (preys.Count > 0 && wolf.activeSelf == false)
            {
                GenWolf(preys);
            }

            yield return new WaitForSecondsRealtime(delay);
        }
    }

    private void LoadWolf()
    {
        var prefab = Resources.Load<GameObject>("WolfAI");
        wolf = GameObject.Instantiate(prefab) as GameObject;

        data = wolf.GetComponent<Wolf_BehaviourTree>().data;
        data.birthPos = birthPos;
        data.homePos = homePos;
        data.jumpPs = jumpPs;

        wolf.SetActive(false);
    }

    private List<GameObject> FindPreys()
    {
        List<GameObject> preys = new List<GameObject>();
        GameObject[] rabbits = GameObject.FindGameObjectsWithTag("Rabbit");

        if (rabbits.Length > 0)
        {
            foreach (GameObject r in rabbits)
            {
                preys.Add(r);
            }
        }
        return preys;
    }

    private void GenWolf(List<GameObject> preys)
    {
        wolf.transform.position = birthPos.transform.position;
        wolf.transform.forward = birthPos.transform.forward;

        data.preys = preys;
        data.UpdateStatus(0);

        wolf.SetActive(true);
    }

    //return wolf exist or not
    public bool WolfInField()
    {
        if (wolf != null && wolf.activeSelf)
        {
            return   true;
        }
        return false;
    }

    //return rabbit catched or not
    public bool TargetCatched()
    {
        if (wolf != null && data.catchedTarget != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

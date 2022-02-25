using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxController : MonoBehaviour
{
    [SerializeField]
    List<GameObject> birthPoses;
    [SerializeField]
    List<GameObject> breakableItems;

    GameObject target;
    GameObject birthPos;

    private GameObject levelController;
    private LevelControl lv;

    // Start is called before the first frame update
    void Start()
    {
        levelController = GameObject.Find("LevelControl");


        if (levelController != null)
        {
            lv = levelController.GetComponent<LevelOneControl>();

            if (lv.isActiveAndEnabled != true)
            {
                lv = levelController.GetComponent<LevelTwoControl>();
            }
        }

        LoadFox();
        StartCoroutine(CheckBreakableItems());
    }

    private void Update()
    {
        if (fox.activeSelf == true && behaviour.IsTargetUsing())
        {
            behaviour.data.target = SetTarget();
        }
    }


    IEnumerator CheckBreakableItems()
    {
        yield return new WaitForSeconds(20);

        while (true)
        {
            Debug.Log("fox coroutine");
            breakableItems = FindBreakableItems();

            if (breakableItems.Count > 0 && fox.activeSelf == false)
            {
                GenNewFox();
            }

            yield return new WaitForSeconds(1000);
        }
    }

    private List<GameObject> FindBreakableItems()
    {
        List<GameObject> breakableItems = new List<GameObject>();
        GameObject[] boxes = GameObject.FindGameObjectsWithTag("Box");
        GameObject[] ropes = GameObject.FindGameObjectsWithTag("Rope");

        if (boxes.Length > 0)
        {
            foreach (GameObject box in boxes)
            {
                breakableItems.Add(box);
            }
        }

        if (ropes.Length > 0)
        {
            foreach (GameObject rope in ropes)
            {
                breakableItems.Add(rope);
            }
        }

        return breakableItems;
    }

    private GameObject SetTarget()
    {
        int maxI = breakableItems.Count - 1;
        int i = Random.Range(0, maxI);

        Debug.Log("setTarget" + i);

        if (breakableItems != null)
        {
            return breakableItems[i];
        }
        else
        {
            if(i + 1 <= maxI)
            {
                return breakableItems[i + 1];
            }
            else
            {
                return breakableItems[0];
            }
        }
    }

    float tempDist;
    GameObject nearestPos;

    private GameObject SetBirthPos()
    {
        tempDist = 10000.0f;
        nearestPos = null;

        foreach (GameObject pos in birthPoses)
        {
            float dist = (target.transform.position - pos.transform.position).magnitude;

            if(dist < tempDist)
            {
                nearestPos = pos;
                tempDist = dist;
            }
        }

        if(nearestPos == null)
        {
            Debug.Log("find nearest pos failed");
        }

        return nearestPos;
    }

    private void GenNewFox()
    {
        target = SetTarget();

        if (target != null)
        {
            birthPos = SetBirthPos();
        }
        else
        {
            birthPos = null;
        }

        SpawnFox(target, birthPos);
    }

    GameObject fox;
    Fox_BehaviourTree behaviour;
    FoxAIData foxData;

    private void LoadFox()
    {
        var prefab = Resources.Load<GameObject>("FoxAI");
        fox = GameObject.Instantiate(prefab) as GameObject;
       
        behaviour = fox.GetComponent<Fox_BehaviourTree>();
        foxData = behaviour.data;

        if (behaviour == null || foxData == null)
        {
            Debug.Log("behaviour or foxdata null");
        }

        fox.SetActive(false);
    }

    private void SpawnFox(GameObject target, GameObject birthPos)
    {
        fox.SetActive(true);

        fox.transform.position = birthPos.transform.position;
        fox.transform.forward = birthPos.transform.forward;

        behaviour.target = target;
        foxData.target = target;
        behaviour.birthPos = birthPos;
        foxData.UpdateStatus(0);

        behaviour.missionComplete = false;

        Debug.Log("foxspawn" + target.name + birthPos.name);
    }
}

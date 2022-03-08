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

    //coroutine
    public float waitForStart;
    public float delay;

    //destory target effect
    public GameObject effect;

    // Start is called before the first frame update
    void Awake()
    { 
        LoadFox();
        StartCoroutine(CheckBreakableItems());
    }

    IEnumerator CheckBreakableItems()
    {
        yield return new WaitForSecondsRealtime(waitForStart);

        while (true)
        {
            breakableItems = FindBreakableItems();

            if (breakableItems.Count > 0 && fox.activeSelf == false)
            {
                GenNewFox();
            }

            yield return new WaitForSecondsRealtime(delay);
        }
    }

    private List<GameObject> FindBreakableItems()
    {
        List<GameObject> breakableItems = new List<GameObject>();
        GameObject[] boxes = GameObject.FindGameObjectsWithTag("Box");
        GameObject[] ropes = GameObject.FindGameObjectsWithTag("Rope");
        GameObject[] bags = GameObject.FindGameObjectsWithTag("Bag");

        if (boxes.Length > 0)
        {
            foreach (GameObject box in boxes)
            {
                if (box.GetComponent<BoxController>().beUsing == false && box.transform.position.y >= -0.5f)
                {
                    breakableItems.Add(box);
                }
            }
        }

        if (ropes.Length > 0)
        {
            foreach (GameObject rope in ropes)
            {
                if (rope.GetComponent<RopeController>().beUsing == false && rope.transform.position.y >= -0.5f)
                {
                    breakableItems.Add(rope);
                }
            }
        }

        if (bags.Length > 0)
        {
            foreach (GameObject bag in bags)
            {
                if (bag.GetComponent<BagController>().beUsing == false && bag.transform.position.y >= -0.5f)
                {
                    breakableItems.Add(bag);
                }
            }
        }

        return breakableItems;
    }

    BoxController boxC;
    RopeController ropeC;
    BagController bagC;

    private GameObject SetTarget()
    {
        int maxI = breakableItems.Count - 1;
        int i = Random.Range(0, maxI);

        Debug.Log("setTarget" + i);

        if (breakableItems != null)
        {
            if (breakableItems[i].tag == "Box")
            {
                boxC = breakableItems[i].GetComponent<BoxController>();
            }

            if (breakableItems[i].tag == "Rope")
            {
                ropeC = breakableItems[i].GetComponent<RopeController>();
            }

            if (breakableItems[i].tag == "Bag")
            {
                bagC = breakableItems[i].GetComponent<BagController>();
            }

            return breakableItems[i];
        }
        else
        {
            if(i + 1 <= maxI)
            {
                if (breakableItems[i + 1].tag == "Box")
                {
                    boxC = breakableItems[i + 1].GetComponent<BoxController>();
                }

                if (breakableItems[i + 1].tag == "Rope")
                {
                    ropeC = breakableItems[i + 1].GetComponent<RopeController>();
                }

                if (breakableItems[i + 1].tag == "Bag")
                {
                    bagC = breakableItems[i + 1].GetComponent<BagController>();
                }

                return breakableItems[i + 1];
            }
            else
            {
                if (breakableItems[0].tag == "Box")
                {
                    boxC = breakableItems[0].GetComponent<BoxController>();
                }

                if (breakableItems[0].tag == "Rope")
                {
                    ropeC = breakableItems[0].GetComponent<RopeController>();
                }

                if (breakableItems[0].tag == "Bag")
                {
                    bagC = breakableItems[0].GetComponent<BagController>();
                }

                return breakableItems[0];
            }
        }
    }

    private GameObject SetBirthPos()
    {
        int birthAmt = birthPoses.Count;
        int index = Random.Range(0, birthAmt);

        return birthPoses[index];
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

        if (target != null && birthPos != null)
        {
            SpawnFox(target, birthPos);
        }
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

        behaviour.destroyEffect = effect;

        if (behaviour == null || foxData == null)
        {
            Debug.Log("behaviour or foxdata null");
        }

        fox.SetActive(false);
    }

    private void SpawnFox(GameObject target, GameObject birthPos)
    {
        fox.transform.position = birthPos.transform.position;
        fox.transform.forward = birthPos.transform.forward;

        foxData.target = target;
        foxData.birthPos = birthPos;
        foxData.UpdateStatus((int)FoxAIData.FoxStatus.Safe);

        fox.SetActive(true);

        Debug.Log("foxspawn" + target.name + birthPos.name);
    }

    //return fox exist or not
    public bool FoxInField()
    {
        if (fox != null)
        {
            return fox.activeSelf == true;
        }

        return false;
    }
}

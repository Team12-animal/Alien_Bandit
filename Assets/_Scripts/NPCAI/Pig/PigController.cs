using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigController : MonoBehaviour
{
    public GameObject pig;
    
    public GameObject[] birthNodes;

    private PigAIData data;

    public float waitForStart;
    public float delay;

    public GameObject bumpEndPos;

    // Start is called before the first frame update
    void Start()
    {   
        birthNodes = GameObject.FindGameObjectsWithTag("PigNode");
        LoadPig();
        StartCoroutine(ActivePig());
    }

    IEnumerator ActivePig()
    {
        yield return new WaitForSeconds(waitForStart);

        while (true)
        {
            if (birthNodes.Length > 0 && pig.activeSelf == false)
            {
                GenPig();
            }

            yield return new WaitForSeconds(delay);
        }
    }

    private void LoadPig()
    {
        var prefab = Resources.Load<GameObject>("PigAI");
        pig = Instantiate(prefab);
        pig.SetActive(false);
        data = pig.GetComponent<PigBehaviourTree>().data;
        pig.GetComponent<PigBehaviourTree>().bumpEndPos = bumpEndPos;
    }

    GameObject birthPos;
    GameObject homePos;
    private void GenPig()
    {
        birthPos = SetBirthPos();

        if (birthPos != null)
        {
            homePos = SetHomePos();
        }

        if (birthPos != null && homePos != null)
        {
            data.birthPos = birthPos;
            data.homePos = homePos;
            data.UpdateStatus(0);

            pig.transform.position = birthPos.transform.position;
            pig.GetComponent<PigBehaviourTree>().enabled = true;
            pig.SetActive(true);
        }
    }

    private GameObject SetBirthPos()
    {
        if (birthNodes.Length > 0)
        {
            int amt = birthNodes.Length;
            int index = Random.Range(0, amt);

            return birthNodes[index];
        }

        return null;
    }

    private GameObject SetHomePos()
    {
        pig.GetComponent<AudioSource>().Stop();
        List<GameObject> members = birthPos.GetComponent<PigNode>().groupMember;

        if (members.Count > 0)
        {
            int amt = members.Count;
            int index = Random.Range(0, amt);

            return members[index];
        }

        return null;
    }
}

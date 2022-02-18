using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreesController : MonoBehaviour
{
    private GameObject[] trees;
    private int oriTreeAmt;

    private GameObject levelController;
    private LevelOneControl lv1;

    private void Awake()
    {
        trees = GameObject.FindGameObjectsWithTag("Tree");
        oriTreeAmt = trees.Length;

        levelController = GameObject.Find("LevelControl");
        if(levelController != null)
        {
            lv1 = levelController.GetComponent<LevelOneControl>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GrowTrees());
    }

    private IEnumerator GrowTrees()
    {
        yield return new WaitWhile(() => { return checkTreeAmt() < oriTreeAmt; });

        while (lv1.GetGameTime() >= 0.0f && lv1.WinOrNot() == false)
        {
            Debug.Log("grow new tree");

            if (checkTreeAmt() < oriTreeAmt)
            {
                int i = Random.Range(0, oriTreeAmt - 1);

                for (int j = i; j < oriTreeAmt; j++)
                {
                    if (trees[j].activeSelf == false)
                    {
                        RemoveStump(trees[j]);
                        trees[j].SetActive(true);
                        break;
                    }
                }
            }

            yield return new WaitForSecondsRealtime(30);
        }
    }

    private int checkTreeAmt()
    {
        return GameObject.FindGameObjectsWithTag("Tree").Length;
    }

    private void RemoveStump(GameObject tree)
    {
        Debug.Log("remove stump");

        Vector3 from = tree.transform.position;
        from.y += 10.0f;
        Vector3 dir = -tree.transform.up;

        RaycastHit hit;
        if(Physics.Raycast(from, dir, out hit, Mathf.Infinity))
        {
            Debug.Log("stump delet" + hit.collider.gameObject.name);
            if (hit.collider.gameObject.tag == "Stump")
            {
                hit.collider.gameObject.SetActive(false);
                GameObject.Destroy(hit.collider.gameObject);
                
            }
        }
    }
}

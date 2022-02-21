using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxController : MonoBehaviour
{
    [SerializeField]
    List<GameObject> birthPoses;
    List<GameObject> breakableItems;

    // Start is called before the first frame update
    void Start()
    {
        breakableItems = FindBreakableItems();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private List<GameObject> FindBreakableItems()
    {
        List<GameObject> breakableItems = new List<GameObject>();
        GameObject[] boxes = GameObject.FindGameObjectsWithTag("Box");
        GameObject[] ropes = GameObject.FindGameObjectsWithTag("Rope");

        if(boxes.Length > 0)
        {
            foreach(GameObject box in boxes)
            {
                breakableItems.Add(box);
            }
        }

        if(ropes.Length > 0)
        {
            foreach(GameObject rope in ropes)
            {
                breakableItems.Add(rope);
            }
        }

        return breakableItems;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public List<Item> items;
    // Start is called before the first frame update
    void Start()
    {
        for (int i=0; i<items[2].gm.Length; i++)
        {
            Instantiate(items[2].gm[i],this.transform);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

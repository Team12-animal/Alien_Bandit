using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixManager : MonoBehaviour
{
    public List<Item> mixitems;
    // Start is called before the first frame update
    void Start()
    {        
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void CheckItemAndAdd(string tag)
    {
        foreach (var v in ItemManager.Instance.items)
        {
            if (tag == v.itemName)
            {
                mixitems.Add(v);
            }
        }
    }
    void CheckItemAndRemove(string tag)
    {
        foreach (var v in mixitems)
        {
            if (tag == v.itemName)
            {
                mixitems.Remove(v);
            }
        }
    }

    void MixItem()
    {
        int mix1 = mixitems[0].id;
        int mix2 = mixitems[1].id;

        if (mix1 == 0 && mix2 == 1)
        {
            mixitems.Clear();
            Instantiate(ItemManager.Instance.items[2].gm[0] ,transform);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Wood" || other.tag=="Leaf")
        {
            CheckItemAndAdd(other.tag);
        }

    }
    private void OnTriggerStay(Collider other)
    {

        if (other.tag == "Player")
        {
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                MixItem();
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Wood" || other.tag == "Leaf")
        {
            CheckItemAndRemove(other.tag);
        }
    }
}

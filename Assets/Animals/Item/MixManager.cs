using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixManager : MonoBehaviour
{
    public List<Item> mixitems;
    public Transform instantiate;
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
            Instantiate(ItemManager.Instance.items[3].gm[0], instantiate);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        foreach (var v in ItemManager.Instance.items)
        {
            if (other.tag == v.itemName)
            {
                CheckItemAndAdd(other.tag);
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && mixitems!=null)
        {
            MixItem();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        foreach (var v in ItemManager.Instance.items)
        {
            if (other.tag == v.itemName)
            {
                CheckItemAndRemove(other.tag);
            }
        }
    }
}

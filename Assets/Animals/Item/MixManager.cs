using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixManager : MonoBehaviour
{
    public List<Item> items;
    public List<Item> mixitems;
    public List<GameObject> mixGameObject;
    public bool isMix;
    public string mix = "";
    public Item mixItem;
    public Transform instantiate;

    void CheckItemAndAdd(string tag)
    {
        foreach (var v in items)
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
    void ClearAll()
    {
        foreach (var v in mixGameObject)
        {
            Destroy(v);
        }
        mixitems.Clear();
        mixGameObject.Clear();
        isMix = false;
        mix = "";
        mixItem = null;
    }

    void  CanMixItem()
    {
        mix = "";
        foreach (var v in mixitems)
        {
            mix += v.id;
        }
        foreach (var a in items)
        {
            if (a.mixId.Count > 0)
            {
                for (int i = 0; i < a.mixId.Count; i++)
                {
                    if (a.mixId[i] == mix)
                    {
                        isMix = true;
                        mixItem = a;
                        break;
                    }
                    else
                    {
                        isMix = false;
                    }
                }
            }
        }
    }

    void MixItem()
    {      
        Instantiate(mixItem.gm[0], instantiate);
        ClearAll();
    }

    private void OnTriggerEnter(Collider other)
    {
        foreach (var v in items)
        {
            if (v.canMix && other.tag == v.itemName)
            {
                CheckItemAndAdd(other.tag);
                mixGameObject.Add(other.gameObject);
            }
        }
        if (other.tag=="Player" && mixitems.Count>0)
        {
            CanMixItem();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKey(KeyCode.LeftControl) && isMix)
        {
            MixItem();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        foreach (var v in items)
        {
            if (other.tag == v.itemName)
            {
                CheckItemAndRemove(other.tag);
                mixGameObject.Remove(other.gameObject);
            }
        }
    }
}

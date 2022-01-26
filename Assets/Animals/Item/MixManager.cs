using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MixManager : MonoBehaviour
{
    public List<Item> items;
    public List<Item> mixitems;
    public List<GameObject> mixGameObject;
    public bool isMix;
    public string mix = "";
    public Item mixItem;
    public bool isRight=true;
    public GameObject[] slotimage;
    public Sprite[] slotsprite = new Sprite[2];
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

    void CanMixItem()
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

    void  SlotSelet(GameObject go)
    {
        Vector3 left = transform.TransformDirection(Vector3.up);
        Vector3 toOther = go.transform.position - transform.position;
        if (Vector3.Dot(left, toOther) > 0)
        {
            Debug.LogError("在右邊的位置");
            slotimage[1].GetComponent<Image>().sprite = slotsprite[1];
            slotimage[0].GetComponent<Image>().sprite = slotsprite[0];
            isRight= true;
        }
        else
        {
            Debug.LogError("在左邊的位置");
            slotimage[0].GetComponent<Image>().sprite = slotsprite[1];
            slotimage[1].GetComponent<Image>().sprite = slotsprite[0];
            isRight= false;
        }
    }
    void SetObjectPosition(int i)
    {
        if (!isRight)
        {
            mixGameObject[i].transform.position = slotimage[0].transform.position;
        }
        else
        {
            mixGameObject[i].transform.position = slotimage[1].transform.position;
        }
    }    

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            SlotSelet(other.gameObject);
        }

        foreach (var v in items)
        {
            if (v.canMix && other.tag == v.itemName)
            {
                CheckItemAndAdd(other.tag);             
                mixGameObject.Add(other.gameObject);
                SetObjectPosition(mixGameObject.Count-1);
            }
        }
        if (other.tag=="Player" && mixitems.Count>0)
        {
            CanMixItem();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            SlotSelet(other.gameObject);
        }
        if (Input.GetKey(KeyCode.LeftControl) && isMix)
        {
            MixItem();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            slotimage[0].GetComponent<Image>().sprite = slotsprite[0];
            slotimage[1].GetComponent<Image>().sprite = slotsprite[0];
        }
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

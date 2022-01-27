using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MixManager : MonoBehaviour
{
    //存放所有道具的清單
    public List<Item> items;
    //存放目前在桌上且可合成道具
    public List<Item> mixitems;
    //存放可合成道具的GameObject，合成完需消失
    public List<GameObject> mixGameObject;
    //目前有可以合成的道具
    public bool isMix;
    //可合成道具的MixID，以此來查找可合成哪種道具
    public string mix = "";
    //可合成的道具
    public Item mixItem;
    //現在玩家是否在桌子右邊
    public bool isRight=true;
    //存放桌子上的儲存格
    public GameObject[] slotimage;
    //變化是否選取到的儲存格
    public Sprite[] slotsprite = new Sprite[2];
    //儲存格是否已存放物件
    public bool[] isfull = new bool[2];
    //道具生成的定位點
    public Transform instantiate;

    /// <summary>
    /// 找到和桌子碰撞到的物件並加入可合成清單中
    /// </summary>
    /// <param name="tag">和桌子碰撞到的Tag</param>
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
    /// <summary>
    /// 找到和桌子停止碰撞的物件並移除可合成清單
    /// </summary>
    /// <param name="tag">>和桌子停止碰撞的Tag</param>
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
    /// <summary>
    /// 所有資料變為初始值
    /// </summary>
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
        isfull[0] = false;
        isfull[1] = false;
    }
    /// <summary>
    /// 找出可以合成出的道具
    /// </summary>
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
    /// <summary>
    /// 生成物件並清除所有資料
    /// </summary>
    void MixItem()
    {
        Instantiate(mixItem.gm[0], instantiate);
        ClearAll();
    }
    /// <summary>
    /// 選擇儲存格位置
    /// </summary>
    /// <param name="go">玩家目前位置</param>
    void  SlotSelet(GameObject go)
    {
        Vector3 left = transform.TransformDirection(Vector3.up);
        Vector3 toOther = go.transform.position - transform.position;
        if (Vector3.Dot(left, toOther) > 0)
        {
            slotimage[1].GetComponent<Image>().sprite = slotsprite[1];
            slotimage[0].GetComponent<Image>().sprite = slotsprite[0];
            isRight= true;
        }
        else
        {
            slotimage[0].GetComponent<Image>().sprite = slotsprite[1];
            slotimage[1].GetComponent<Image>().sprite = slotsprite[0];
            isRight= false;
        }
    }
    /// <summary>
    /// 將放入桌上物件定位至儲存格
    /// </summary>
    /// <param name="i"></param>
    void SetObjectPosition(int i)
    {
        if (!isRight)
        {
            if (!isfull[1])
            {
                mixGameObject[i].transform.position = slotimage[0].transform.position;
                isfull[1] = true;
            }
        }
        else
        {
            if (!isfull[0])
            {
                mixGameObject[i].transform.position = slotimage[1].transform.position;
                isfull[0] = true;
            }
        }
    }    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="other"></param>
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

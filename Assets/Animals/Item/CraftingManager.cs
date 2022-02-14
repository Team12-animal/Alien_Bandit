using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingManager : MonoBehaviour
{
    //Dictionary<int, Dictionary<GameObject, Item>> crafting = new Dictionary<int, Dictionary<GameObject, Item>>();

    //儲存所有Item
    [SerializeField]
    private List<Item> items = new List<Item>();
    //儲存所有加入工作台Item
    private Dictionary<int, Item> craftItems = new Dictionary<int, Item>();
    //儲存可合成Item的GameObject
    private Dictionary<int, GameObject> craftGameObjects = new Dictionary<int, GameObject>();
    //當前位置
    private bool isLeft = true;
    //存放桌子上的儲存格
    [SerializeField]
    private GameObject[] slotimage;
    //變化是否選取到的儲存格
    [SerializeField]
    private Sprite[] slotsprite = new Sprite[2];
    //是否可以合成
    private bool isCraft = false;
    //可合成Item
    private Item craftItem;
    //物件生成位置
    [SerializeField]
    private Transform instaniate;
    //已合成物件是否拿走
    public bool isTake = true;

    //縮小版物件icon
    public GameObject log0;
    public GameObject log1;
    public GameObject rope0;
    public GameObject rope1;
    private List<GameObject> logIcons;
    private List<GameObject> ropeIcons;

    private GameObject[] showingIcons;

    private void Awake()
    {
        logIcons = new List<GameObject>();
        ropeIcons = new List<GameObject>();

        //init object icon: find and put it into list
        log0 = this.transform.Find("Item").Find("Log _small").gameObject;
        logIcons.Add(log0);
        log0.SetActive(false);

        log1 = this.transform.Find("Item").Find("Log _small1").gameObject;
        logIcons.Add(log1);
        log1.SetActive(false);

        rope0 = this.transform.Find("Item").Find("Rope_small").gameObject;
        ropeIcons.Add(rope0);
        rope0.SetActive(false);

        rope1 = this.transform.Find("Item").Find("Rope_small1").gameObject;
        ropeIcons.Add(rope1);
        rope1.SetActive(false);

        showingIcons = new GameObject[2];
    }


    /// <summary>
    ///利用內積確認玩家當前位置最靠近的儲存格 
    /// </summary>
    /// <param name="player"></param>
    private void ItemPos(GameObject player)
    {
        //將自身座標轉換成世界座標的up(-1,0,0)
        Vector3 left = transform.TransformDirection(Vector3.up);
        //工作台到玩家的向量
        Vector3 toOther = player.transform.position - transform.position;
        if (Vector3.Dot(left, toOther) > 0) //Player在右邊儲存格
        {
            slotimage[1].GetComponent<Image>().sprite = slotsprite[1];
            slotimage[0].GetComponent<Image>().sprite = slotsprite[0];
            isLeft = false;
        }
        else
        {
            slotimage[0].GetComponent<Image>().sprite = slotsprite[1];
            slotimage[1].GetComponent<Image>().sprite = slotsprite[0];
            isLeft = true;
        }
    }
    /// <summary>
    /// 將物件定位到和儲存格相同位置
    /// </summary>
    private void SetGameObjectPos()
    {
        int slot = 0;
        if (!isLeft)
        {
            slot = 1;
        }
        
        //craftGameObjects[slot].transform.position = slotimage[slot].transform.position;

        if(craftGameObjects[slot].tag == "Log")
        {
            craftGameObjects[slot].SetActive(false);
            logIcons[slot].SetActive(true);
            showingIcons[slot] = logIcons[slot];
        }

        if(craftGameObjects[slot].tag == "Rope")
        {
            craftGameObjects[slot].SetActive(false);
            ropeIcons[slot].SetActive(true);
            showingIcons[slot] = logIcons[slot];
        }
    }
    /// <summary>
    /// 新增可合成Item，最多2個物件，排序法
    /// </summary>
    /// <param name="col">和工作台碰撞的可合成物體Collider</param>
    /// <param name="item">可合成物體Item</param>
    private void AddItem(Collider col, Item item)
    {
        int slotPos = 0;
        if (!isLeft)
        {
            slotPos = 1;
        }
        //如果當前已有存放物品，原本的被刪除，以新的取代
        if (craftItems.ContainsKey(slotPos))
        {
            showingIcons[slotPos].SetActive(false);

            craftGameObjects[slotPos].SetActive(true);
            //移到工作台旁邊的位置
            Vector3 move = new Vector3(transform.position.x - 3f, transform.position.y, transform.position.z);
            //移除舊的Item和GameObject
            craftItems.Remove(slotPos);
            craftGameObjects[slotPos].transform.position = move;
            craftGameObjects.Remove(slotPos);
        }
        craftItems.Add(slotPos, item);
        craftGameObjects.Add(slotPos, col.gameObject);
        //設定GameObject位置
        SetGameObjectPos();
    }
    /// <summary>
    /// 移除Item
    /// </summary>
    /// <param name="key">要移除的Dictionary value的key</param>
    private void RemoveItem(int key)
    {
        craftGameObjects.Remove(key);
        craftItems.Remove(key);
    }
    /// <summary>
    /// 是否可以合成
    /// </summary>
    private void CanMixItem()
    {
        string craft = "";
        foreach (var v in craftItems)
        {
            //將CraftItems中的Item的id相加可獲得mixId
            craft += v.Value.id;
        }
        foreach (var a in items)
        {
            //搜尋所有可合成的Item並查找mixId的數量
            if (a.mixId.Count > 0)
            {
                for (int i = 0; i < a.mixId.Count; i++)
                {
                    //若找到可合成的Item便跳出迴圈
                    if (a.mixId[i] == craft)
                    {
                        isCraft = true;
                        craftItem = a;
                        break;
                    }
                    else
                    {
                        isCraft = false;
                    }
                }
            }
        }
    }
    /// <summary>
    /// 合成Item並初始化
    /// </summary>
    public void CraftingItem()
    {
        if (isTake && isCraft)
        {
            Instantiate(craftItem.gm[0], instaniate);
            instaniate.transform.DetachChildren();
            isTake = false;
            ClearAll();
        }
    }
    /// <summary>
    /// 初始化
    /// 1.craftItems
    /// 2.craftGameObjects
    /// 3.craftItem
    /// 4.isCraft
    /// </summary>
    private void ClearAll()
    {
        for (int i = 0; i < craftGameObjects.Count; i++)
        {
            Destroy(craftGameObjects[i]);
        }
        craftItems.Clear();
        craftGameObjects.Clear();
        craftItem = null;
        isCraft = false;
    }

    public bool CheckWoodOnTable( string  itemname)
    {
        foreach (var v in craftItems)
        {
            if (v.Value.itemName == itemname)
            {
                return true;
            }
        }
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            return;
        }
        if (other.tag == "Box")
        {
            isTake = false;
            return;
        }

        Debug.Log("Craft enter" + other.name);
        //搜尋碰撞到的物體是否可合成，有便加入
        foreach (var v in items)
        {
            if (v.canMix && other.tag == v.itemName && isTake)
            {
                AddItem(other, v);
                CanMixItem();
                Debug.Log("Craft add");
                return;
            }
        }
        Vector3 move = new Vector3(transform.position.x - 3f, transform.position.y, transform.position.z);
        other.gameObject.transform.position = move;
    }

    private void OnTriggerStay(Collider other)
    {
        //判斷玩家位置
        if (other.tag == "Player")
        {
            ItemPos(other.gameObject);
            //當可以合成且玩家按下leftControl時觸發
            //if (isCraft && Input.GetKeyUp(KeyCode.LeftControl) && isTake)
            //{
            //    CraftingItem();
            //}
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Box")
        {
            isTake = true;
            return;
        }
        foreach (var v in craftGameObjects)
        {
            if (other.gameObject == v.Value)
            {
                RemoveItem(v.Key);
            }
        }
    }
}

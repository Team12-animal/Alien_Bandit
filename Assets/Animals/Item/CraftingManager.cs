using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingManager : MonoBehaviour
{
    //Dictionary<int, Dictionary<GameObject, Item>> crafting = new Dictionary<int, Dictionary<GameObject, Item>>();

    //one user on time
    public GameObject user;
    private GameObject p1;
    private GameObject p2;
    private GameObject p3;
    private GameObject p4;
    [SerializeField]
    public List<GameObject> players;

    //?x?s????Item
    [SerializeField]
    private List<Item> items = new List<Item>();
    //?x?s?????[?J?u?@?xItem virtual gameobect
    private Dictionary<int, Item> craftItems = new Dictionary<int, Item>();
    //?x?s?i?X??Item??GameObject Gameobjects on the scene
    private Dictionary<int, GameObject> craftGameObjects = new Dictionary<int, GameObject>();
    //???e???m
    public bool isLeft = true;
    //?s?????l?W???x?s??
    [SerializeField]
    public GameObject[] slotimage;
    //?????O?_?????????x?s??
    [SerializeField]
    private Sprite[] slotsprite = new Sprite[2];
    //?O?_?i?H?X?? could be crafted or not
    public bool isCraft = false;
    //?i?X??Item
    [SerializeField]
    private Item craftItem;
    //???????????m object spawn pos
    [SerializeField]
    private Transform instaniate;
    //?w?X???????O?_???? box be taken or not
    public bool isTake = true;

    //?Y?p??????icon
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

    private void Start()
    {
        PlayerInit();
    }

    private void PlayerInit()
    {
        players = new List<GameObject>();

        p1 = GameObject.Find("Player0" + 1);
        if (p1 != null)
        {
            players.Add(p1);
        }

        p2 = GameObject.Find("Player0" + 2);
        if (p2 != null)
        {
            players.Add(p2);
        }

        p3 = GameObject.Find("Player0" + 3);
        if (p3 != null)
        {
            players.Add(p3);
        }

        p4 = GameObject.Find("Player0" + 4);
        if (p1 != null)
        {
            players.Add(p4);
        }

    }

    private void Update()
    {
        FindUser();

        if (user != null)
        {
            ItemPos(user);
            userItem = data.item;

            if (pid < 1 || pid > 4 || userItem == null)
            {
                return;
            }

            //check if player's item could be add > if yes then add
            if (isTake && Input.GetButtonDown("Take" + pid) && userItem != null)
            {
                foreach (var v in items)
                {
                    if (v.canMix && userItem.tag == v.itemName && isTake)
                    {
                        AddItem(userItem, v);
                        CanMixItem();
                        return;
                    }
                }
            }
        }
    }


    /// <summary>
    ///?Q?????n?T?{???a???e???m???a?????x?s?? 
    /// </summary>
    /// <param name="player"></param>
    private void ItemPos(GameObject player)
    {
        //?N?????y?????????@???y????up(-1,0,0)
        Vector3 left = transform.TransformDirection(Vector3.up);
        //?u?@?x?????a???V?q
        Vector3 toOther = player.transform.position - transform.position;
        if (Vector3.Dot(left, toOther) > 0) //Player?b?k???x?s??
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

    private void ResetItmPos(GameObject player)
    {
        if (isLeft == false) //Player?b?k???x?s??
        {
            slotimage[1].GetComponent<Image>().sprite = slotsprite[0];
            isLeft = false;
        }
        else
        {
            slotimage[0].GetComponent<Image>().sprite = slotsprite[0];
            isLeft = false;
        }
    }

    /// <summary>
    /// ?N?????w?????M?x?s?????P???m
    /// </summary>
    private void SetGameObjectPos()
    {
        int slot = 0;
        if (!isLeft)
        {
            slot = 1;
        }
        
        //craftGameObjects[slot].transform.position = slotimage[slot].transform.position;

        if(craftGameObjects[slot].tag == "Wood")
        {
            //close item gameobject and open item icon
            craftGameObjects[slot].SetActive(false);
            logIcons[slot].SetActive(true);
            showingIcons[slot] = logIcons[slot];
        }

        if(craftGameObjects[slot].tag == "Rope")
        {
            craftGameObjects[slot].SetActive(false);
            ropeIcons[slot].SetActive(true);
            showingIcons[slot] = ropeIcons[slot];
        }

        //save the item gameobject at the top of slotimage
        Vector3 savePos = slotimage[slot].transform.position;
        savePos.y += 5.0f;
        savePos.z += 2.0f;
        craftGameObjects[slot].transform.position = savePos;
    }
    /// <summary>
    /// ?s?W?i?X??Item?A???h2???????A?????k
    /// </summary>
    /// <param name="go">?M?u?@?x?I?????i?X??????</param>
    /// <param name="item">?i?X??????Item</param>
    private void AddItem(GameObject go, Item item)
    {
        int slotPos = 0;
        if (!isLeft)
        {
            slotPos = 1;
        }

        //?p?G???e?w???s?????~?A???????Q?R???A?H?s?????N
        if (craftItems.ContainsKey(slotPos))
        {
            Debug.Log("craft remove item" + craftGameObjects[slotPos]);
            //close the item icon, and show the item
            showingIcons[slotPos].SetActive(false);
            craftGameObjects[slotPos].SetActive(true);
            
            //?????u?@?x?????????m
            //Vector3 move = new Vector3(transform.position.x - 3f, transform.position.y, transform.position.z);
            //????????Item?MGameObject
            craftItems.Remove(slotPos);
            //craftGameObjects[slotPos].transform.position = move;
            craftGameObjects.Remove(slotPos);
        }

        craftItems.Add(slotPos, item);
        craftGameObjects.Add(slotPos, go);

        //?]?wGameObject???m
        SetGameObjectPos();

        Debug.Log("craft Add item" + go.tag);
    }

    /// <summary>
    /// ????Item
    /// </summary>
    /// <param name="key">?n??????Dictionary value??key</param>
    private void RemoveItem(int key)
    {
        craftGameObjects.Remove(key);
        craftItems.Remove(key);
    }
    /// <summary>
    /// ?O?_?i?H?X??
    /// </summary>
    private void CanMixItem()
    {
        string craft = "";
        foreach (var v in craftItems)
        {
            //?NCraftItems????Item??id???[?i???omixId
            craft += v.Value.id;
        }
        foreach (var a in items)
        {
            //?j?M?????i?X????Item???d??mixId?????q
            if (a.mixId.Count > 0)
            {
                for (int i = 0; i < a.mixId.Count; i++)
                {
                    //?Y?????i?X????Item?K???X?j??
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
    /// ?X??Item?????l??
    /// </summary>
    public void CraftingItem()
    {
        if (isTake && isCraft)
        {
            CloseItemIcon();
            Instantiate(craftItem.gm[0], instaniate);
            instaniate.transform.DetachChildren();
            isTake = false;
            ClearAll();
        }
    }

    /// <summary>
    /// Close item icon
    /// </summary>
    private void CloseItemIcon()
    {
        foreach(GameObject icon in showingIcons)
        {
            icon.SetActive(false);
        }
    }

    /// <summary>
    /// ???l??
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


    private void OnTriggerEnter(Collider other)
    {
        foreach (var v in craftGameObjects)
        {
            //if (canMix)
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Box")
        {
            isTake = false;
            return;
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


    PlayerData data;
    int pid = 5;
    public GameObject userItem;

    private void UserDataReset()
    {
        user = null;
        data = null;
        pid = 5;
    }

    float dist = 10000.0f;
    GameObject tempUser;
    float tempDist;

    private void FindUser()
    {
        if (players.Count == 0)
        {
            return;
        }

        //Find the closest user
        foreach (GameObject p in players)
        {
            tempDist = (p.transform.position - this.transform.position).magnitude;

            if (tempDist <= dist && tempDist < 7.0f)
            {
                tempUser = p;
                dist = tempDist;
            }
        }

        //check if any player close enough to user workBench
        if (tempUser == null && user != null)
        {
            //no player using
            ResetItmPos(user);
            UserDataReset();
        }
        else
        {
            //get new user
            user = tempUser;
            data = user.GetComponent<PlayerData>();
            pid = data.pid;

        }

        //reset compare data
        tempUser = null;
        dist = 10000.0f;
    }
}

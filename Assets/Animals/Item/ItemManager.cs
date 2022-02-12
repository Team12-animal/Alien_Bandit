using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager:MonoBehaviour
{
    private  static ItemManager instance = null;
    public static ItemManager Instance
    {
        get
        {
            if (instance == null)
            {
                // 尝试寻找该类的实例。此处不能用GameObject.Find，因为MonoBehaviour继承自Component。
                instance = Object.FindObjectOfType(typeof(ItemManager)) as ItemManager;

                if (instance == null)  // 如果没有找到
                {
                    GameObject go = new GameObject("ItemManager"); // 创建一个新的GameObject
                    DontDestroyOnLoad(go);  // 防止被销毁
                    instance = go.AddComponent<ItemManager>(); // 将实例挂载到GameObject上
                }
            }
            return instance;
        }
    }
    public List<Item> items;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(instance.gameObject);
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

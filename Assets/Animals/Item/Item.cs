using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Item : ScriptableObject
{
    [Header("道具Prefab：")]
    public GameObject[] gm;
    [Header("道具Name：")]
    public string itemName;
    [Header("道具ID：")]
    public string id;
    [Header("可合成？")]
    public bool canMix;
    [Header("合成ID")]
    public  List<string> mixId = new List<string>();
    [Header("會消耗？")]
    public bool canDestroy;
    [Header("耐久度？")][SerializeField]
    private int durability;  
    public int Durability
    {
        get 
        {
            return canDestroy ? durability : durability = 999;
        }
    }
}

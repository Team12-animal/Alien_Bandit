using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Item : ScriptableObject
{
    [Header("道具Prefab：")]
    public GameObject[] gm;
    [Header("道具ID：")]
    public int id;
    [Header("可合成？")]
    public bool canMix;
    [Header("會消耗？")]
    public bool canDestroy;
    [Header("耐久度？")]
    public int Durability;


}

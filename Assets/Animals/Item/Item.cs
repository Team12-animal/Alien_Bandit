using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Item : ScriptableObject
{
    [Header("�D��Prefab�G")]
    public GameObject[] gm;
    [Header("�D��Name�G")]
    public string itemName;
    [Header("�D��ID�G")]
    public string id;
    [Header("�i�X���H")]
    public bool canMix;
    [Header("�X��ID")]
    public  List<string> mixId = new List<string>();
    [Header("�|���ӡH")]
    public bool canDestroy;
    [Header("�@�[�סH")][SerializeField]
    private int durability;  
    public int Durability
    {
        get 
        {
            return canDestroy ? durability : durability = 999;
        }
    }
}

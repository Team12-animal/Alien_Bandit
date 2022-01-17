using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Item : ScriptableObject
{
    [Header("�D��Prefab�G")]
    public GameObject[] gm;
    [Header("�D��ID�G")]
    public int id;
    [Header("�i�X���H")]
    public bool canMix;
    [Header("�|���ӡH")]
    public bool canDestroy;
    [Header("�@�[�סH")]
    public int Durability;


}

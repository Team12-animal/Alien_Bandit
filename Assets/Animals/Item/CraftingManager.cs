using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingManager : MonoBehaviour
{
    //Dictionary<int, Dictionary<GameObject, Item>> crafting = new Dictionary<int, Dictionary<GameObject, Item>>();

    //�x�s�Ҧ�Item
    [SerializeField]
    private List<Item> items = new List<Item>();
    //�x�s�Ҧ��[�J�u�@�xItem
    private Dictionary<int, Item> craftItems = new Dictionary<int, Item>();
    //�x�s�i�X��Item��GameObject
    private Dictionary<int, GameObject> craftGameObjects = new Dictionary<int, GameObject>();
    //���e��m
    private bool isLeft = true;
    //�s���l�W���x�s��
    [SerializeField]
    private GameObject[] slotimage;
    //�ܤƬO�_����쪺�x�s��
    [SerializeField]
    private Sprite[] slotsprite = new Sprite[2];
    //�O�_�i�H�X��
    private bool isCraft = false;
    //�i�X��Item
    private Item craftItem;
    //����ͦ���m
    [SerializeField]
    private Transform instaniate;
    //�w�X������O�_����
    public bool isTake = true;

    /// <summary>
    ///�Q�Τ��n�T�{���a���e��m�̾a���x�s�� 
    /// </summary>
    /// <param name="player"></param>
    private void ItemPos(GameObject player)
    {
        //�N�ۨ��y���ഫ���@�ɮy�Ъ�up(-1,0,0)
        Vector3 left = transform.TransformDirection(Vector3.up);
        //�u�@�x�쪱�a���V�q
        Vector3 toOther = player.transform.position - transform.position;
        if (Vector3.Dot(left, toOther) > 0) //Player�b�k���x�s��
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
    /// �N����w���M�x�s��ۦP��m
    /// </summary>
    private void SetGameObjectPos()
    {
        int slot = 0;
        if (!isLeft)
        {
            slot = 1;
        }
        craftGameObjects[slot].transform.position = slotimage[slot].transform.position;
    }
    /// <summary>
    /// �s�W�i�X��Item�A�̦h2�Ӫ���A�ƧǪk
    /// </summary>
    /// <param name="col">�M�u�@�x�I�����i�X������Collider</param>
    /// <param name="item">�i�X������Item</param>
    private void AddItem(Collider col, Item item)
    {
        int slotPos = 0;
        if (!isLeft)
        {
            slotPos = 1;
        }
        //�p�G���e�w���s�񪫫~�A�쥻���Q�R���A�H�s�����N
        if (craftItems.ContainsKey(slotPos))
        {
            //����u�@�x���䪺��m
            Vector3 move = new Vector3(transform.position.x - 3f, transform.position.y, transform.position.z);
            //�����ª�Item�MGameObject
            craftItems.Remove(slotPos);
            craftGameObjects[slotPos].transform.position = move;
            craftGameObjects.Remove(slotPos);
        }
        craftItems.Add(slotPos, item);
        craftGameObjects.Add(slotPos, col.gameObject);
        //�]�wGameObject��m
        SetGameObjectPos();
    }
    /// <summary>
    /// ����Item
    /// </summary>
    /// <param name="key">�n������Dictionary value��key</param>
    private void RemoveItem(int key)
    {
        craftGameObjects.Remove(key);
        craftItems.Remove(key);
    }
    /// <summary>
    /// �O�_�i�H�X��
    /// </summary>
    private void CanMixItem()
    {
        string craft = "";
        foreach (var v in craftItems)
        {
            //�NCraftItems����Item��id�ۥ[�i��omixId
            craft += v.Value.id;
        }
        foreach (var a in items)
        {
            //�j�M�Ҧ��i�X����Item�ìd��mixId���ƶq
            if (a.mixId.Count > 0)
            {
                for (int i = 0; i < a.mixId.Count; i++)
                {
                    //�Y���i�X����Item�K���X�j��
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
    /// �X��Item�ê�l��
    /// </summary>
    private void CraftingItem()
    {
        Instantiate(craftItem.gm[0], instaniate);
        isTake = false;
        ClearAll();
    }
    /// <summary>
    /// ��l��
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
        if (other.tag == "Player")
        {
            return;
        }
        if (other.tag == "Box")
        {
            isTake = false;
            return;
        }
        //�j�M�I���쪺����O�_�i�X���A���K�[�J
        foreach (var v in items)
        {
            if (v.canMix && other.tag == v.itemName && isTake)
            {
                AddItem(other, v);
                CanMixItem();
                return;
            }
        }
        Vector3 move = new Vector3(transform.position.x - 3f, transform.position.y, transform.position.z);
        other.gameObject.transform.position = move;
    }

    private void OnTriggerStay(Collider other)
    {
        //�P�_���a��m
        if (other.tag == "Player")
        {
            ItemPos(other.gameObject);
            //���i�H�X���B���a���UleftControl��Ĳ�o
            if (isCraft && Input.GetKeyUp(KeyCode.LeftControl) && isTake)
            {
                CraftingItem();
            }
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
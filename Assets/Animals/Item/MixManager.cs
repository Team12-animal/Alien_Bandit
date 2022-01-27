using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MixManager : MonoBehaviour
{
    //�s��Ҧ��D�㪺�M��
    public List<Item> items;
    //�s��ثe�b��W�B�i�X���D��
    public List<Item> mixitems;
    //�s��i�X���D�㪺GameObject�A�X�����ݮ���
    public List<GameObject> mixGameObject;
    //�ثe���i�H�X�����D��
    public bool isMix;
    //�i�X���D�㪺MixID�A�H���Ӭd��i�X�����عD��
    public string mix = "";
    //�i�X�����D��
    public Item mixItem;
    //�{�b���a�O�_�b��l�k��
    public bool isRight=true;
    //�s���l�W���x�s��
    public GameObject[] slotimage;
    //�ܤƬO�_����쪺�x�s��
    public Sprite[] slotsprite = new Sprite[2];
    //�x�s��O�_�w�s�񪫥�
    public bool[] isfull = new bool[2];
    //�D��ͦ����w���I
    public Transform instantiate;

    /// <summary>
    /// ���M��l�I���쪺����å[�J�i�X���M�椤
    /// </summary>
    /// <param name="tag">�M��l�I���쪺Tag</param>
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
    /// ���M��l����I��������ò����i�X���M��
    /// </summary>
    /// <param name="tag">>�M��l����I����Tag</param>
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
    /// �Ҧ�����ܬ���l��
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
    /// ��X�i�H�X���X���D��
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
    /// �ͦ�����òM���Ҧ����
    /// </summary>
    void MixItem()
    {
        Instantiate(mixItem.gm[0], instantiate);
        ClearAll();
    }
    /// <summary>
    /// ����x�s���m
    /// </summary>
    /// <param name="go">���a�ثe��m</param>
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
    /// �N��J��W����w����x�s��
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

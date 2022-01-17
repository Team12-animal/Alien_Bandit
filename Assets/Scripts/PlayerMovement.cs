using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerData data;
    float maxSpeed;
    public float maxRotate;
    public float setDashTime;
    public float dashSpeed;
    public float lerpAmt;
    public GameObject cam;

    private Vector3 fVec;
    private Vector3 rVec;

    void Start()
    {
        cam = GameObject.Find("Main Camera");
        data = this.gameObject.GetComponent<PlayerData>();
        InitPlayerData(data);

        rVec = cam.transform.right;
        fVec = GenNewBaseForward();
    }

    private void InitPlayerData(PlayerData data)
    {
        maxSpeed = data.maxSpeed;
        maxRotate = data.maxRotate;
        setDashTime = data.setDashTime;
        dashSpeed = data.dashSpeed;
        lerpAmt = data.lerpAmt;
        itemInhand = data.itemInhand;
    }

    public void MoveAndRotate(float transAmt, float rotAmt)
    {
        Vector3 dir = (rVec * rotAmt) + (fVec * transAmt);

        this.transform.forward = Vector3.Slerp(this.transform.forward, dir * maxRotate * Time.deltaTime, lerpAmt);
        
        float moveAmt = dir.magnitude;

        this.transform.position += this.transform.forward * moveAmt * maxSpeed * Time.deltaTime;
    }

    public Vector3 GenNewBaseForward()
    {
        Vector3 tempV = cam.transform.forward;
        tempV.y = 0;
        tempV.Normalize();
        return tempV;
    }

    public float Dash(float dashTime)
    {
        this.transform.position += this.transform.forward * dashSpeed * Time.deltaTime;
        dashTime -= Time.deltaTime;
        return dashTime;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(this.transform.position, this.transform.position + this.transform.forward * 2);
    }

    public static class ItemId
    {
        public static int workingTable = 0;
        public static int tree = 1;
        public static int rock = 2;
        public static int wood = 3;
        public static int chop = 4;
        public static int bucket = 5;
    }


    private GameObject targetItem;
    private int recentItem = -1;//-1 = no item
    private int itemInhand = -1;

    //check what to pick
    private void OnTriggerStay(Collider other)
    {
        targetItem = other.gameObject;
        string tagName = targetItem.gameObject.tag;
        GetItemId(tagName);
    }

    private void GetItemId(string tagName)
    {
        //not holding item
        int itemId = -1;

        if (tagName == "WorkingTable")
        {
            itemId = ItemId.workingTable;
        }

        if (tagName == "Tree")
        {
            itemId = ItemId.tree;
        }

        if (tagName == "Rock")
        {
            itemId = ItemId.rock;
        }

        if (tagName == "Wood")
        {
            itemId = ItemId.wood;
        }

        if (tagName == "Chop")
        {
            itemId = ItemId.chop;
        }

        if (tagName == "Bucket")
        {
            itemId = ItemId.bucket;
        }

        recentItem = itemId;
    }
    
    public void Pick()
    {  
        //update what item in hand
        itemInhand = recentItem;

        //look at targetItem
        this.transform.LookAt(targetItem.transform.position);

    }

    public void Drop()
    {

    }
}

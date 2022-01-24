using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerData data;
    public Camera cam;

    //movement
    [HideInInspector]
    float maxSpeed;
    [HideInInspector]
    public float maxRotate;
    [HideInInspector]
    public float setDashTime;
    [HideInInspector]
    public float dashSpeed;
    [HideInInspector]
    public float lerpAmt;

    private Vector3 fVec;
    private Vector3 rVec;

    //use
    public GameObject itemInhand;
    private GameObject animalCatched;

    //animation
    private Transform holdingPos;

    void Start()
    {
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        data = this.gameObject.GetComponent<PlayerData>();
        InitPlayerData(data);

        rVec = cam.transform.right;
        fVec = GenNewBaseForward();

        holdingPos = FindChildT("HoldingPos");
    }

    private void InitPlayerData(PlayerData data)
    {
        maxSpeed = data.maxSpeed;
        maxRotate = data.maxRotate;
        setDashTime = data.setDashTime;
        dashSpeed = data.dashSpeed;
        lerpAmt = data.lerpAmt;
    }

    private void UpdatePlayerData()
    {
        data.item = itemInhand;
        data.animal = animalCatched;
    }

    private Transform FindChildT(string cName)
    {
        Transform trans = this.gameObject.transform;
        Transform childT = trans.Find(cName);

        return childT;
    }

    public string MoveAndRotate(float transAmt, float rotAmt)
    {
        string aniClip;

        Vector3 dir = (rVec * rotAmt) + (fVec * transAmt);

        this.transform.forward = Vector3.Slerp(this.transform.forward, dir * maxRotate * Time.deltaTime, lerpAmt);
        
        float moveAmt = dir.magnitude;

        this.transform.position += this.transform.forward * moveAmt * maxSpeed * Time.deltaTime;

        //Get Animator Name
        if(data.item != null)
        {
            aniClip = GetMoveAniName(data.item);
        }
        else
        {
            aniClip = "Run";
        }

        return aniClip;
    }

    public Vector3 GenNewBaseForward()
    {
        Vector3 tempV = cam.transform.forward;
        tempV.y = 0;
        tempV.Normalize();
        return tempV;
    }

    private string GetMoveAniName(GameObject item)
    {
        string aniName = "Run";
        string tagName = item.gameObject.tag;

        if (tagName == "Rock")
        {
            aniName = "HoldRockWalk";
        }

        if (tagName == "Wood")
        {
            aniName = "HoldWoodWalk";
        }

        if (tagName == "Chop")
        {
            aniName = "HoldChopWalk";
        }

        if (tagName == "Bucket")
        {
            aniName = "HoldBucketWalk";
        }

        return aniName;
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

    public GameObject targetItem;
    public GameObject triggerItem;
    //check what to pick
    private void OnTriggerStay(Collider other)
    {
        triggerItem = other.gameObject;
    }
    
    public string Pick()
    {
        targetItem = triggerItem;
        
        string aniClip;

        //update what item in hand
        if (targetItem != null)
        {
            FaceTarget(targetItem);

            string tagName = targetItem.gameObject.tag;

            if (tagName == "WorkingTable")
            {
                //Wait for Pei
                //UseTable();
                
                //if using Table success
                aniClip = "UsingTable";
            }
            else
            {
                HoldItem(targetItem);
                itemInhand = targetItem;
            }

            aniClip = GetUseAniName(tagName);
            UpdatePlayerData();
            targetItem = null;

            Debug.Log("pick end");
        }
        else
        {
            Debug.Log("unable to use");
            aniClip = "Idle";
        }

        return aniClip;
    }

    private string GetUseAniName(string tagName)
    {
        string aniName = "None";

        if (tagName == "Rock")
        {
            aniName = "PickUpRock";
        }

        if (tagName == "Wood")
        {
            aniName = "PickWood";
        }

        if (tagName == "Chop")
        {
            aniName = "PickUpChop";
        }

        if (tagName == "Bucket")
        {
            aniName = "PickUpBucket";
        }

        return aniName;
    }

    public string UseChop()
    {
        if (triggerItem != null)
        {
            targetItem = triggerItem;
        }            

        string aniClip;

        if (targetItem != null && targetItem.tag == "Tree")
        {
            aniClip = ChopTree();
        }
        else
        {
            aniClip = Drop();
        }

        return aniClip;
    }

    private string ChopTree()
    {
        string aniName;

        GameObject tree;
        tree = targetItem;

        Vector3 spawnPos;
        Vector3 treePos = tree.transform.position;
        Vector3 spawnDir = -tree.transform.up;
        RaycastHit hit;

        //hide tree
        tree.SetActive(false);

        //spawn stump and log
        if (Physics.Raycast(treePos, spawnDir, out hit, Mathf.Infinity, 1 << 7))
        {
            spawnPos = hit.point;

            //change tree prefeb to stump
            GameObject stump = (GameObject)Resources.Load("TreeStump");
            stump.SetActive(true);
            stump.transform.position = spawnPos;

            //spawn log
            GameObject log = (GameObject)Resources.Load("Log");
            log.SetActive(true);
            log.transform.position = spawnPos + (-spawnDir) * 4.0f;
        }

        aniName = "Chopping";
        UpdatePlayerData();
        targetItem = null;

        return aniName;
    }


    //holding bucket > ctrl press1: get water; press2: pourwater; press3: drop 
    //bool bucketFilled = false;
    //public string UseBucket()
    //{
     //if (triggerItem != null)
     //   {
     //       targetItem = triggerItem;
     //   }

//    string aniClip;

//    if (bucketFilled == false)
//    {
//        if(targetItem != null && targetItem.tag == "Water")
//        {
//            aniClip = GetWater();
//        }
//        else
//        {
//            aniClip = Drop();
//        }
//    }
//    else
//    {
//        aniClip = PourWater();
//    }

//    return aniClip;
//}

//private string GetWater()
//{
//    string aniName = "GetWater";
//    bucketFilled = true;

//    //add put out fire function

//    return aniName;
//}

//private string PourWater()
//{
//    string aniName = "PourWater";
//    bucketFilled = false;
//    return aniName;
//}

public string Drop()
    {
        string aniClip = "none";

        aniClip = GetDropAniName(itemInhand.tag);

        itemInhand = null;
        UpdatePlayerData();

        //check animation status
        //remove child
        Debug.Log("Drop");
        return aniClip;
    }

    public string Throw(float strength)
    {
        string aniClip = "none";

        //check animation status
        //remove child
        if (itemInhand != null)
        {
            aniClip = "ThrowRock";
            itemInhand = null;
            UpdatePlayerData();
        }

        return aniClip;
    }

    private string GetDropAniName(string tagName)
    {
        string aniName = "Idle";

        if (tagName == "Rock")
        {
            aniName = "PutDownRock";
        }

        if (tagName == "Wood")
        {
            aniName = "PutDownWood";
        }

        if (tagName == "Chop")
        {
            aniName = "PutDownChop";
        }

        if (tagName == "Bucket")
        {
            aniName = "PutDownBucket";
        }

        Debug.Log("Drop" + tagName + aniName);

        return aniName;
    }
    //look at targetItem
    private void FaceTarget(GameObject target)
    {
        Vector3 dirToItem = target.transform.position - this.transform.position;
        float fDotD = Vector3.Dot(this.transform.forward, dirToItem);

        if (fDotD < 0.3f)
        {
            this.transform.forward = Vector3.Slerp(this.transform.forward, dirToItem, 0.8f);
        }

        float dist = dirToItem.magnitude;
    }

    //set item to HoldingPos
    private void HoldItem(GameObject targetItem)
    {
        if(targetItem == null)
        {
            return;
        }

        targetItem.transform.position = holdingPos.position;
        targetItem.transform.SetParent(holdingPos);
        Rigidbody targetRG = targetItem.GetComponent<Rigidbody>();
        targetRG.isKinematic = true;
    }

    private void RemoveItem(GameObject targetItem)
    {
        if (targetItem = null)
        {
            return;
        }

        targetItem.transform.parent = null;
        Rigidbody targetRG = targetItem.GetComponent<Rigidbody>();
        targetRG.isKinematic = false;
    }
}

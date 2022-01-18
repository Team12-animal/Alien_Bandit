using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerData data;
    public Camera cam;
    private string aniClip;

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
    private GameObject itemInhand;
    private GameObject animalCatched;

    void Start()
    {
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
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
    }

    private void UpdatePlayerData()
    {
        data.item = itemInhand;
        data.animal = animalCatched;
    }

    public string MoveAndRotate(float transAmt, float rotAmt)
    {
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

    //check what to pick
    private GameObject OnTriggerStay(Collider other)
    {
        targetItem = other.gameObject;
        return targetItem;
    }
    
    public string Pick()
    {
        //update what item in hand
        if (targetItem != null)
        {
            FaceTarget(targetItem);

            string tagName = targetItem.gameObject.tag;

            if (tagName == "WorkingTable")
            {
                //Wait for ??
                //UseTable();
            }
            else
            {
                itemInhand = targetItem;
            }

            aniClip = GetUseAniName(tagName);
            UpdatePlayerData();
            targetItem = null;
        }
        else
        {
            Debug.Log("unable to use");
            aniClip = "none";
        }

        return aniClip;
    }

    private string GetUseAniName(string tagName)
    {
        string aniName = "none";

        if (tagName == "WorkingTable")
        {
            aniName = "UsingTable";
        }

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

    public string ChopTree()
    {
        GameObject tree;

        if (targetItem.tag != "Tree")
        {
            return "none";
        }
        else
        {
            tree = targetItem;
        }

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
            log.SetActive(false);
            log.transform.position = spawnPos + (-spawnDir) * 4.0f;
        }

        aniClip = "Chopping";
        return aniClip;
    }

    public string UseBucket()
    {
        return aniClip;
    }

    public string Drop()
    {
        return aniClip;
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
    }
}

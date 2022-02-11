using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerData data;
    public Camera cam;

    private Rigidbody rb;
    public Vector3 velocity;

    public bool raydect;

    //movement
    [HideInInspector]
    public float maxSpeed;
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
    [SerializeField]
    private Transform holdingPos;


    void Start()
    {
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        data = GetComponent<PlayerData>();
        InitPlayerData(data);

        rVec = cam.transform.right;
        fVec = GenNewBaseForward();

        //holdingPos = FindChildT("HoldingPos");
        holdingPos = this.transform.Find("HoldingPos").transform;
        rb = this.gameObject.GetComponent<Rigidbody>();
        velocity = rb.velocity;
    }

    private void Update()
    {
        maxSpeed = data.realSpeed;
        maxRotate = data.realRotate;
    }

    private void InitPlayerData(PlayerData data)
    {
        maxSpeed = data.realSpeed;
        maxRotate = data.realRotate;
        setDashTime = data.setDashTime;
        dashSpeed = data.dashSpeed;
        lerpAmt = data.lerpAmt;
    }

    private void UpdatePlayerData()
    {
        data.item = itemInhand;
        data.animal = animalCatched;
    }

    //private Transform FindChildT(string cName)
    //{
    //    Transform trans = this.gameObject.transform;
    //    Transform childT = trans.Find(cName);

    //    return childT;
    //}

    public string MoveAndRotate(float transAmt, float rotAmt,Camera camera)
    {
        camera = CatchCurrentMainCamera();

        string aniClip;

        Vector3 dir = (rVec * rotAmt) + (fVec * transAmt);

        this.transform.forward = Vector3.Slerp(this.transform.forward, dir * maxRotate * Time.deltaTime, lerpAmt);

        float moveDist = dir.magnitude;
        Vector3 moveAmt = this.transform.forward * moveDist * maxSpeed;

        //修正人物碰撞抖動問題
        Vector3 from = this.transform.position;
        from.y += 0.5f;
        Vector3 to = from + moveAmt;
        to.z += 0.8f;

        Debug.DrawLine(from, to, Color.black);

        RaycastHit hit;
        if (Physics.Linecast(from, to, out hit, 1 << 8))
        {
            moveAmt = hit.point - from;
            moveAmt.y += 0.5f;
            moveAmt.z -= 0.5f;

            Debug.Log("Fix collider");
        }

        //修正人物站立高度
        Vector3 toGround = from + (-this.transform.up * 10f);

        RaycastHit ground;
        if (Physics.Linecast(from, toGround, out ground, 1 << 7))
        {
            moveAmt.y = ground.point.y;
        }

        this.transform.position += moveAmt * Time.deltaTime;

        //Get Animator Name
        if (data.item != null)
        {
            aniClip = GetMoveAniName(data.item);
        }
        else
        {
            aniClip = "Run";
        }

        return aniClip;
    }

    private Camera CatchCurrentMainCamera()
    {
        Camera camera = Camera.main;
        rVec = camera.transform.right;
        Vector3 tempV = camera.transform.forward;
        tempV.y = 0;
        tempV.Normalize();
        fVec = tempV;
        return camera;
    }

    //將人物移動對齊鏡頭Y軸
    public Vector3 GenNewBaseForward()
    {
        Vector3 tempV = cam.transform.forward;
        tempV.y = 0;
        tempV.Normalize();
        return tempV;
    }

    //根據玩家持有物品取得移動動畫
    private string GetMoveAniName(GameObject item)
    {
        string aniName = "none";
        string tagName = item.gameObject.tag;

        if (tagName == "RockModel")
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

    //衝刺
    public float Dash(float dashTime)
    {
        Debug.Log("dash");

        //修正人物碰撞抖動問題
        Vector3 from = this.transform.position;
        from.y += 0.5f;
        Vector3 moveAmt = this.transform.forward * dashSpeed;
        Vector3 to = from + moveAmt;
        to.z += 0.8f;

        Debug.DrawLine(from, to, Color.black);

        RaycastHit hit;
        if (Physics.Linecast(from, to, out hit, 1 << 8))
        {
            moveAmt = hit.point - from;
            moveAmt.y += 0.5f;
            moveAmt.z -= 0.5f;

            raydect = true;

            Debug.Log("Fix collider dash");
        }
        else
        {
            raydect = false;
        }

        //修正人物站立高度
        Vector3 toGround = from + (-this.transform.up * 10f);

        RaycastHit ground;
        if (Physics.Linecast(from, toGround, out ground, 1 << 7))
        {
            moveAmt.y = ground.point.y;
        }


        this.transform.position += moveAmt * Time.deltaTime;
        

        dashTime -= Time.deltaTime;
        return dashTime;
    }

    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(this.transform.position, this.transform.position + this.transform.forward * 2);
    }

    //目標道具
    public GameObject targetItem;
    //在trigger內的道具
    public GameObject triggerItem;

    //check what to pick
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Rock")
        {
            triggerItem = other.GetComponent<RockTrigger>().targetRock;
        }
        else if(other.tag == "Tree" || other.tag == "Wood" || other.tag == "Chop" || other.tag == "Bucket")
        {
            triggerItem = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        triggerItem = null;
    }

    //拿取物品
    public string Take()
    {
        targetItem = triggerItem;

        string tagName = targetItem.gameObject.tag;
        string aniClip;

        //不能拿取
        if (tagName == "WorkingTable" || tagName == "Tree" || targetItem.tag == "Rock")
        {
            return "none";
        }

        //拿取道具
        if (targetItem != null)
        {
            FaceTarget(targetItem);
            HoldItem(targetItem);

            aniClip = GetTakeAniName(tagName);
            UpdatePlayerData();
            targetItem = null;

            Debug.Log("pick end");
        }
        else
        {
            Debug.Log("unable to use");
            return "none";
        }

        return aniClip;
    }

    /// <summary>
    /// 取得撿取動畫
    /// </summary>
    /// <param 目標道具tag="tagName"></param>
    /// <returns></returns>
    private string GetTakeAniName(string tagName)
    {
        string aniName = "none";

        if (tagName == "RockModel")
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

    //使用斧頭
    public string UseChop()
    {
        string aniClip = "none";

        //如果有樹就砍 沒樹就把斧頭丟掉
        if (data.inTree == true)
        {
            aniClip = ChopTree();
        }
        else
        {
            aniClip = Drop();
        }

        return aniClip;
    }

    //砍樹
    private string ChopTree()
    {
        string aniName;
        
        aniName = "Chopping";
        UpdatePlayerData();
        targetItem = null;

        return aniName;
    }

    //砍樹動畫結束後 產生樹樁和樹幹(給animation event)
    public void AnimaEventSpawnStumpAndLog()
    {
        GameObject tree;
        tree = targetItem;

        Vector3 spawnPos;
        Vector3 treePos = tree.transform.position;
        Vector3 spawnDir = -tree.transform.up;
        RaycastHit hit;

        //hide tree
        tree.SetActive(false);

        //spawn stump and log on treePos
        if (Physics.Raycast(treePos, spawnDir, out hit, Mathf.Infinity, 1 << 7))
        {
            spawnPos = hit.point;

            //change tree prefeb to stump
            var stumpPrefab = Resources.Load<GameObject>("TreeStump");
            GameObject stump = GameObject.Instantiate(stumpPrefab) as GameObject;
            stump.SetActive(true);
            stump.transform.position = spawnPos;

            //spawn log
            var logPrefab = Resources.Load<GameObject>("Log");
            GameObject log = GameObject.Instantiate(logPrefab) as GameObject;
            log.SetActive(true);
            log.transform.position = spawnPos + (-spawnDir) * 1.0f;
        }
    }


    ////holding bucket > ctrl press1: get water; press2: pourwater; press3: drop
    //bool bucketFilled = false;
    //public string UseBucket()
    //{
    //    if (triggerItem != null)
    //    {
    //        targetItem = triggerItem;
    //    }

    //    string aniClip;

    //    if (bucketFilled == false)
    //    {
    //        if (targetItem != null && targetItem.tag == "Water")
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
        targetItem = null;
        Debug.Log("Drop");
        return aniClip;
    }

    public string Throw(float strength)
    {
        string aniClip = "none";

        //check animation status
        //remove child
        if (itemInhand != null && itemInhand.tag == "RockModel")
        {
            aniClip = "ThrowRock";
        }
        targetItem = null;
        return aniClip;
    }

    private string GetDropAniName(string tagName)
    {
        string aniName = "none";

        if (tagName == "RockModel")
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
        if (targetItem == null || targetItem.tag == "WorkingTable" || targetItem.tag == "Tree")
        {
            return;
        }

        if(targetItem.tag == "RockModel")
        {
            
        }

        int childAmt = targetItem.transform.childCount;
        if(childAmt > 0)
        {
            foreach(Transform child in targetItem.transform)
            {
                (child.gameObject.GetComponent(typeof(Collider)) as Collider).enabled = false;
            }
        }

        targetItem.transform.position = holdingPos.position;
        targetItem.transform.forward = holdingPos.forward;
        targetItem.transform.SetParent(holdingPos);
        Rigidbody targetRG = targetItem.GetComponent<Rigidbody>();
        targetRG.isKinematic = true;
        itemInhand = targetItem;
    }
     
    private void RemoveItem()
    {
        if (data.item = null)
        {
            return;
        }

        holdingPos.DetachChildren();
        Rigidbody targetRG = itemInhand.GetComponent<Rigidbody>();
        targetRG.isKinematic = false;
        foreach (Transform child in itemInhand.transform)
        {
            (child.gameObject.GetComponent(typeof(Collider)) as Collider).enabled = true;
        }
        itemInhand = null;
        UpdatePlayerData();
    }

    private void ThrowAway()
    {
        if (data.item = null)
        {
            return;
        }

        holdingPos.DetachChildren();
        Rigidbody targetRG = itemInhand.GetComponent<Rigidbody>();
        targetRG.isKinematic = false;
        foreach (Transform child in itemInhand.transform)
        {
            (child.gameObject.GetComponent(typeof(Collider)) as Collider).enabled = true;
        }

        //給item初始力
        targetRG.AddForce(this.transform.forward * 4.0f, ForceMode.Impulse);

        itemInhand = null;
        UpdatePlayerData();
    }
}

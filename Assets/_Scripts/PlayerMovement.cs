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

    //throw
    private InputController input;

    //use working table
    public GameObject workingTable;
    private CraftingManager tableCM;

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

        input = this.gameObject.GetComponent<InputController>();
    }

    public void FindWorkTable()
    {
        if (workingTable == null)
        {
            workingTable = GameObject.Find("BenchTable");
            tableCM = workingTable.GetComponent<CraftingManager>();
        }
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
    }

    //private Transform FindChildT(string cName)
    //{
    //    Transform trans = this.gameObject.transform;
    //    Transform childT = trans.Find(cName);

    //    return childT;
    //}

    public string MoveAndRotate(float transAmt, float rotAmt, Camera camera)
    {
        camera = CatchCurrentMainCamera();

        string aniClip;

        Vector3 dir = (rVec * rotAmt) + (fVec * transAmt);

        this.transform.forward = Vector3.Slerp(this.transform.forward, dir * maxRotate * Time.deltaTime, lerpAmt);

        float moveDist = dir.magnitude;
        Vector3 moveAmt = this.transform.forward * moveDist * maxSpeed;

        //?????H???I?????????D
        Vector3 from = this.transform.position;
        from.y += 0.5f;
        //Vector3 to = from + moveAmt;
        //to.z += 0.38f;

        Vector3 to = from + this.transform.forward * 2.0f;
        to.z += 0.38f;

        Debug.DrawLine(from, to, Color.black);

        RaycastHit hit;
        if (Physics.Linecast(from, to, out hit, 1 << 8))
        {
            //moveAmt = hit.point - from;
            moveAmt = new Vector3(0, 0, 0);
            //moveAmt.y += 0.5f;
            //moveAmt.z -= 0.5f;
        }

        //?????H??????????
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


    //?N?H?????????????YY?b
    public Vector3 GenNewBaseForward()
    {
        Vector3 tempV = cam.transform.forward;
        tempV.y = 0;
        tempV.Normalize();
        return tempV;
    }

    //???????a???????~???o???????e
    private string GetMoveAniName(GameObject item)
    {
        string aniName = "none";
        string tagName = item.gameObject.tag;

        if (tagName == "RockModel")
        {
            aniName = "HoldRockWalk";
        }

        if (tagName == "Box")
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

        if (tagName == "Rope")
        {
            aniName = "HoldWoodWalk";
        }

        return aniName;
    }

    //????
    public float Dash(float dashTime)
    {
        Debug.Log("dash");

        //?????H???I?????????D
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
        }
        else
        {
            raydect = false;
        }

        //?????H??????????
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

    //?????D??
    public GameObject targetItem;
    //?btrigger?????D??
    public GameObject triggerItem;

    //check what to pick
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Rock")
        {
            triggerItem = other.GetComponent<RockTrigger>().targetRock;
        }
        else if (other.tag == "Wood" || other.tag == "Chop" || other.tag == "Bucket" || other.tag == "Box" || other.tag == "WorkingTable" || other.tag == "Rope")
        {
            triggerItem = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        triggerItem = null;
    }

    //???????~
    public string Take()
    {
        if(triggerItem == null)
        {
            return "none";
        }

        string tagName;

        targetItem = triggerItem;
        tagName = targetItem.gameObject.tag;

        string aniClip;

        //????????
        if (tagName == "WorkingTable" || tagName == "Tree" || targetItem.tag == "Rock")
        {
            return "none";
        }

        //?????D??
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
    /// ???o???????e
    /// </summary>
    /// <param ?????D??tag="tagName"></param>
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

        if (tagName == "Box")
        {
            aniName = "PickUpRock";
        }

        if (tagName == "Rope")
        {
            aniName = "PickWood";
        }

        return aniName;
    }

    //???????Y
    public string UseChop()
    {
        string aniClip = "none";

        //?p?G?????N?? ?S???N?????Y????
        if (data.inTree == true)
        {
            aniClip = ChopTree(data.tree);
        }
        else
        {
            aniClip = Drop();
        }

        return aniClip;
    }

    //????
    private string ChopTree(GameObject tree)
    {
        string aniName;
        
        aniName = "Chopping";
        UpdatePlayerData();
        targetItem = null;

        return aniName;
    }

    int hitTime = 0;

    //???????e?????? ?????????M???F(??animation event)
    public void AnimaEventSpawnStumpAndLog()
    {
        if (hitTime < 2)
        {
            hitTime += 1;
        }
        else
        {
            GameObject tree = data.tree;
            Vector3 spawnPos;
            Vector3 treePos = tree.transform.position;
            treePos.y += 10.0f;
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
                Vector3 temp = spawnPos + (-spawnDir) * 4.0f;
                temp.x += 1.0f;
                log.transform.position = temp;

                Debug.Log("SpawnStump success");
            }

            hitTime = 0;
            data.tree = null;
            data.inTree = false;
        }

        Debug.Log("SpawnStump" + hitTime);
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
        if (itemInhand != null && (itemInhand.tag == "RockModel" || itemInhand.tag == "Box"))
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

        if (tagName == "Box")
        {
            aniName = "PutDownRock";
        }

        if(tagName == "Rope")
        {
            aniName = "PutDownWood";
        }

        Debug.Log("Drop" + tagName + aniName);

        return aniName;
    }

    public string UseBench()
    {
        FindWorkTable();

        if(targetItem != null && targetItem.tag == "WorkingTable")
        {
            tableCM.CraftingItem();
            return "UsingTable";
        }
        else
        {
            return "none";
        }
    }

    //look at targetItem
    private void FaceTarget(GameObject target)
    {
        Vector3 temp;
        Vector3 dirToItem = target.transform.position - this.transform.position;
        dirToItem.y = this.transform.position.y;
        
        float fDotD = Vector3.Dot(this.transform.forward, dirToItem);

        if (fDotD < 0.5f)
        {
            temp = Vector3.Slerp(this.transform.forward, dirToItem, 1.0f);
            temp.y = this.transform.forward.y;
            this.transform.forward = temp;
        }

        float dist = dirToItem.magnitude;

        if(dist < 0.2f)
        {
            this.transform.position += -this.transform.forward * 0.2f;
        }
    }

    //set item to HoldingPos
    private void HoldItem(GameObject targetItem)
    {
        if (targetItem == null || targetItem.tag == "WorkingTable" || targetItem.tag == "Tree")
        {
            return;
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
        if (data.item == null)
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

        if(itemInhand.tag == "Box")
        {
            itemInhand.GetComponent<BoxController>().beUsing = false;
        }

        itemInhand = null;
        UpdatePlayerData();
    }

    float inputf = 0.0f;
    float force = 100.0f;

    private void ThrowAway()
    {
        if(data.item == null)
        {
            return;
        }

        holdingPos.DetachChildren();
        Rigidbody targetRG = itemInhand.GetComponent<Rigidbody>();
        targetRG.isKinematic = false;

        inputf = input.pressTimeSaver / 0.2f;
        force *= inputf;

        if (force > 6000.0f)
        {
            force = 6000.0f;
        }

        if (force <= 1000.0f)
        {
            force = 1000.0f;
        }

        //??item???l?O
        targetRG.AddForce(this.transform.forward * force, ForceMode.Impulse);

        foreach (Transform child in itemInhand.transform)
        {
            (child.gameObject.GetComponent(typeof(Collider)) as Collider).enabled = true;
        }

        if (itemInhand.tag == "Box")
        {
            itemInhand.GetComponent<BoxController>().beUsing = false;
        }

        itemInhand = null;
        UpdatePlayerData();

        Debug.Log("ThrowAway" + force);
    }
}

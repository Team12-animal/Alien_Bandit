using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fox_BehaviourTree : MonoBehaviour
{
    //init data
    public FoxAIData data;
    [SerializeField]
    public GameObject target;
    public GameObject birthPos;
    private int status;
    private float alertDist;

    public string txtName;
    public string nodeName;

    //target
    private BoxController boxC;
    private RopeController ropeC;
    private BagController bagC;

    //rock
    public bool hitten = false;

    //real speed and rot
    private float movingForce;
    private float turningForce;
    private float speed;
    private float rot;
    private Vector3 currentF;

    //players on the field
    private GameObject p1;
    private GameObject p2;
    private GameObject p3;
    private GameObject p4;
    private AnimatorController pAC;

    [SerializeField] public List<GameObject> players;
    [SerializeField]
    public List<GameObject> effectPlayers;
    public GameObject nearestPlayer;
    public bool holdingRockOrBox;

    //go home
    public float homeArriveDist;

    //seek ended or not
    public bool arriveTarget = false;

    //mission complete or not
    public bool missionComplete = false;

    //player Action
    public bool pAttact = false;

    //fox animator
    private FoxAnimatorController fAC;

    //AStar
    public bool aStarPerfoming = false;
    int currentPathPt = -1;

    //target UI
    public bool targetLocking = true;

    // Start is called before the first frame update
    void Start()
    {
        //AStar
        WPTerrain wpt = new WPTerrain();
        wpt.Init(txtName, nodeName);

        AStar aStar = new AStar();
        aStar.Init(wpt);

        //astar
        aStarPerfoming = AStar.instance.PerformAStar(this.transform.position, data.target.transform.position);
        currentPathPt = 0;

        PlayerInit();
    }

    private void OnEnable()
    {
        Debug.Log("fox init onenable");
        DataInit();

        fAC = this.GetComponent<FoxAnimatorController>();

        if (data.target != null)
        {
            if (data.target.tag == "Box")
            {
                boxC = data.target.GetComponent<BoxController>();
            }

            if (data.target.tag == "Bag")
            {
                bagC = data.target.GetComponent<BagController>();
            }

            if (data.target.tag == "Rope")
            {
                ropeC = data.target.GetComponent<RopeController>();
            }
        }
    }

    private void DataInit()
    {
        target = data.target;
        birthPos = data.birthPos;
        data.m_Go = this.gameObject;
        status = (int)data.status;
        alertDist = data.m_fRadius;

        data.arriveDist = data.oriArriveDist;

        missionComplete = false;
        arriveTarget = false;
        arriveHomeArea = false;
        pAttact = false;
        targetLocking = true;
    }

    private void PlayerInit()
    {
        players = new List<GameObject>();

        p1 = GameObject.Find("Player0" + 1);
        if (p1 != null)
        {
            players.Add(p1);
        }

        p2 = GameObject.Find("Player0" + 2);
        if (p2 != null)
        {
            players.Add(p2);
        }

        p3 = GameObject.Find("Player0" + 3);
        if (p3 != null)
        {
            players.Add(p3);
        }

        p4 = GameObject.Find("Player0" + 4);
        if (p4 != null)
        {
            players.Add(p4);
        }
    }

    // Update is called once per frame
    void Update()
    {
        FindEffectPlayer();
        CheckStatusAndUpdate();

        if (status == (int)FoxAIData.FoxStatus.Home)
        {
            target = data.birthPos;
            data.target = target;
            data.arriveDist = homeArriveDist;

            if (target != null && target.activeSelf == true)
            {
                data.m_vTarget = target.transform.position;
            }

            targetLocking = false;
        }

        if (status == (int)FoxAIData.FoxStatus.Attacked)
        {
            Hurt();
        }
        else if (!fAC.AllowToMove())
        {
            return;
        }

        
        if (UpdateTargetPosition() == true)
        {
            aStarPerfoming = AStar.instance.PerformAStar(this.transform.position, target.transform.position);
            currentPathPt = 0;
        }

        SetY();

        switch (status)
        {
            case (int)FoxAIData.FoxStatus.Safe:
                BreakItem();
                break;

            case (int)FoxAIData.FoxStatus.Alert:
                Alert(effectPlayers);
                break;

            case (int)FoxAIData.FoxStatus.AvoidAttack:
                AvoidAttack();
                break;

            case (int)FoxAIData.FoxStatus.Home:
                GoHome();
                break;

        }
        DataUpdate();
    }

    //update aidata script
    private void DataUpdate()
    {
        data.UpdateStatus(status);
    }

    Vector3 oriTPos;

    private bool UpdateTargetPosition()
    {
        bool targetPosChange;
        if (target != null)
        {
            if( oriTPos != target.transform.position)
            {
                data.m_vTarget = target.transform.position;
                oriTPos = target.transform.position;
                targetPosChange = true;
            }
            else
            {
                targetPosChange = false;
            }
        }
        else
        {
            return false;
        }

        return targetPosChange;
    }

    private void CheckStatusAndUpdate()
    {
        if (hitten == true)
        {
            status = (int)FoxAIData.FoxStatus.Attacked;
            data.UpdateStatus(status);
            return;
        }

        if (fAC.BreakingOrNot() == true)
        {
            status = (int)FoxAIData.FoxStatus.Safe;
            data.UpdateStatus(status);
            return;
        }

        if (effectPlayers.Count == 0)
        {
            if (!missionComplete)
            {
                status = (int)FoxAIData.FoxStatus.Safe;
            }

            if (missionComplete || UnableToAccessOrNot() == true)
            {
                status = (int)FoxAIData.FoxStatus.Home;
            }
        }

        if (effectPlayers.Count > 0)
        {
            if (pAC.ThorowingOrNot() == false)
            {
                status = (int)FoxAIData.FoxStatus.Alert;
            }
            else
            {
                status = (int)FoxAIData.FoxStatus.AvoidAttack;
            }
        }

        data.UpdateStatus(status);
    }

    private bool UnableToAccessOrNot()
    {
        bool result;
        result = target == null || target.activeSelf == false ||
                 (boxC != null && boxC.beUsing == true) ||
                 (ropeC != null && ropeC.beUsing == true) ||
                 (bagC != null && bagC.beUsing == true);

        if (target != null && target.transform.position.y < -0.5f)
        {
            return true;
        }

        return result;
    }


    GameObject tempPlayer;
    float nearestDist = 10000.0f;
    float tempDist;
    GameObject oriEffectPlayer;
    private void FindEffectPlayer()
    {
        //save original effectPlayer
        if (nearestPlayer != null)
        {
            oriEffectPlayer = nearestPlayer;
        }

        //reset compareData;
        nearestDist = 10000.0f;
        tempPlayer = null;
        effectPlayers.Clear();

        //find the nearest player
        foreach (GameObject p in players)
        {
            tempDist = (p.transform.position - this.transform.position).magnitude;

            if (tempDist <= alertDist)
            {
                effectPlayers.Add(p);

                if (tempDist <= nearestDist)
                {
                    tempPlayer = p;
                    nearestDist = tempDist;
                }
            }
        }

        nearestPlayer = tempPlayer;

        if (nearestPlayer != null && nearestPlayer != oriEffectPlayer)
        {
            pAC = nearestPlayer.GetComponent<AnimatorController>();
        }
    }

    #region fox behaviour tree
    private void BreakItem()
    {
        if (arriveTarget == false)
        {

            if (aStarPerfoming)
            {
                List<Vector3> path = AStar.instance.GetPath();
                int final = path.Count - 1;

                for (int i = final; i >= currentPathPt; i--)
                {
                    Vector3 sPos = path[i];
                    Vector3 cPos = this.transform.position;

                    if (Physics.Linecast(cPos, sPos, 1 << 8 | 1 << 15))
                    {
                        continue;
                    }

                    currentPathPt = i;
                    data.SetTarget(sPos);
                    break;
                }

                Debug.Log("Doing AStar");

                arriveTarget = SteeringBehavior.Seek(data);
            }
            else if (SteeringBehavior.CollisionAvoid(data) == false)
            {
                arriveTarget = SteeringBehavior.Seek(data);
            }

            SteeringBehavior.Move(data);
            fAC.ChangeAndPlayAnimation(fAC.runTrigger, data.m_fTempTurnForce * 5, data.m_Speed * 10);
        }
        else
        {
            fAC.ChangeAndPlayAnimation(fAC.breaking, 0, 0);
        }
    }

    private void Alert(List<GameObject> players)
    {
        AlertPlayer(players);
    }

    private void AlertPlayer(List<GameObject> players)
    {
        if (players.Count < 1)
        {
            return;
        }

        if (players.Count > 0)
        {
            AvoidPlayerAndRun(players);
            fAC.ChangeAndPlayAnimation(fAC.trotTrigger, data.m_fTempTurnForce * 5, data.m_Speed * 10);
        }
    }

    int left = 0;
    int right = 1;
    bool avoided = false;

    private void AvoidAttack()
    {
        if (nearestPlayer == null)
        {
            return;
        }

        avoided = fAC.AvoidAttactEnd();

        if (pAC.ThorowingOrNot() == true && avoided == false)
        {
            Vector3 pPos = nearestPlayer.transform.position;
            Vector3 pF = nearestPlayer.transform.forward;
            Vector3 foxPos = this.transform.position;

            Vector3 dir = foxPos - pPos;
            float pFDotDir = Vector3.Dot(pF, dir);

            if (pFDotDir > 0.7f) //fox in effectPlayer attackable area
            {
                int randomDir = Random.Range(0, 1);
                Vector3 targetDir;

                if (randomDir == right) //escape to right side
                {
                    targetDir = nearestPlayer.transform.right;
                    targetDir.y = foxPos.y; //move on x-z
                }
                else
                {
                    targetDir = -nearestPlayer.transform.right;
                    targetDir.y = foxPos.y;
                }

                Vector3 newDir = targetDir + this.transform.forward;
                this.transform.forward = Vector3.Slerp(this.transform.forward, newDir, 0.8f);
                this.transform.position += this.transform.forward * data.jumpSpeed;
            }

            fAC.ChangeAndPlayAnimation(fAC.jumpTrigger, 0, 0);
        }
        else
        {
            SteeringBehavior.Move(data);
            fAC.ChangeAndPlayAnimation(fAC.runTrigger, data.m_fTempTurnForce * 5, data.m_Speed * 10);
        }
    }

    private void AvoidPlayerAndRun(List<GameObject> players)
    {
        if (SteeringBehavior.PlayerAvoid(data, players) == false && SteeringBehavior.CollisionAvoid(data) == false)
        {
            SteeringBehavior.Seek(data);
        }

        SteeringBehavior.Move(data);
    }

    private void Hurt()
    {
        fAC.ChangeAndPlayAnimation(fAC.attacked, 0, 0);
    }

    public bool arriveHomeArea = false;
    private void GoHome()
    {
        if (arriveHomeArea == false)
        {
            MoveToBirthPos();
            fAC.ChangeAndPlayAnimation(fAC.runTrigger, data.m_fTempTurnForce * 5, data.m_Speed * 10);
        }
        else
        {
            fAC.ChangeAndPlayAnimation(fAC.homeTrigger, 0, 1);
        }
    }

    private void MoveToBirthPos()
    {
            if (aStarPerfoming)
            {
                List<Vector3> path = AStar.instance.GetPath();
                int final = path.Count - 1;

                for (int i = final; i >= currentPathPt; i--)
                {
                    Vector3 sPos = path[i];
                    Vector3 cPos = this.transform.position;

                    if (Physics.Linecast(cPos, sPos, 1 << 8 | 1 << 15))
                    {
                    Debug.Log("astar linecast bith");
                        continue;
                    }

                    currentPathPt = i;
                    data.SetTarget(sPos);
                    break;
                }

                Debug.Log("Doing AStar");

                arriveHomeArea = SteeringBehavior.Seek(data);
            }
            else if (SteeringBehavior.CollisionAvoid(data) == false)
            {
                arriveHomeArea = SteeringBehavior.Seek(data);
            }

            SteeringBehavior.Move(data);
            fAC.ChangeAndPlayAnimation(fAC.runTrigger, data.m_fTempTurnForce * 5, data.m_Speed * 10);
    }

    #endregion

    #region other behaviour
    public void AnimaEventBreakTarget()
    {
        if (target.tag == "box")
        {
            GameObject animalCatched = target.GetComponent<BoxController>().targetAnimal;

            if (animalCatched != null)
            {
                animalCatched.transform.parent = null;
            }
        }

        target.SetActive(false);
        GameObject.Destroy(target);
        target = null;
        missionComplete = true;
        targetLocking = false;
    }

    #endregion

    private void SetY()
    {
        Vector3 newPos;
        newPos = this.transform.position;

        Vector3 from = this.transform.position;
        from.y += 5.0f;
        from.z += 0.5f;
        Vector3 dir = -this.transform.up;

        RaycastHit hit;
        if (Physics.Raycast(from, dir, out hit, Mathf.Infinity, 1 << 7))
        {
            newPos.y = hit.point.y;
            this.transform.position = newPos;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(this.transform.position, data.m_fRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(this.transform.position, this.transform.position + this.transform.forward * data.m_fProbeLength);

        Vector3 from = this.transform.position;
        from.y += 5.0f;
        from.z += 1.0f;
        Vector3 dir = -this.transform.up;
        Gizmos.color = Color.black;
        Gizmos.DrawLine(from, from + (dir * 5));

        if (aStarPerfoming)
        {
            List<Vector3> path = AStar.instance.GetPath();
            Gizmos.color = Color.blue;
            int iCount = path.Count - 1;
            int i;
            for (i = 0; i < iCount; i++)
            {
                Vector3 sPos = path[i];
                sPos.y += 1.0f;
                Vector3 ePos = path[i + 1];
                ePos.y += 1.0f;
                Gizmos.DrawLine(sPos, ePos);
            }
        }

    }

    public void AniEventTurnhittenToFalse()
    {
        if (hitten == true)
        {
            hitten = false;
            missionComplete = true;
        }
    }

    private void SetTarget()
    {
        if (missionComplete)
        {
            target = data.birthPos;
            data.m_vTarget = target.transform.position;
        }
    }

    public Vector3 WarningUIDisplay()
    {
        Vector3 newPos;

        if (targetLocking == true)
        {
            newPos = target.transform.position;
            newPos.y += 2.5f;
        }
        else
        {
            newPos = new Vector3(1000000.0f, 1000000.0f, 1000000.0f);
        }

        return newPos;
    }
}

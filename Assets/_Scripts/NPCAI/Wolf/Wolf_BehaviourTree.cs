using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf_BehaviourTree : MonoBehaviour
{
    //data
    public WolfAIData data;

    [SerializeField]
    public List<GameObject> preys;
    public GameObject target;
    private int status;
    private GameObject homePos;
    private float alertDist;

    public GameObject catchedTarget;

    //player
    List<GameObject> players;
    GameObject p1;
    GameObject p2;
    GameObject p3;
    GameObject p4;

    GameObject nearestPlayer;
    List<GameObject> effectPlayers;

    //bools
    public bool missionComplete;
    public bool arriveTarget;
    public bool arriveHome;
    public bool targetSwitched; //ever switch target or not
    public bool hitten; //hit by rock or box
    public bool jumping;

    //AStar
    public string txtName;
    public string nodeTag;
    public bool aStarPerfoming = false;
    int currentPathPt = -1;

    //animator
    private WolfAnimatorController wAC;
    public List<GameObject> jumpPs;
    
    //warning UI
    public bool targetLocking = true;

    private void Start()
    {
        //AStar
        WPTerrain wpt = new WPTerrain();
        wpt.Init(txtName, nodeTag);

        AStar aStar = new AStar();
        aStar.Init(wpt);

        InitPlayer();

        jumpPs = data.jumpPs;
    }

    private void InitPlayer()
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

        effectPlayers = new List<GameObject>();
        Debug.Log($"wolf player init {players.Count}");
    }

    private void OnEnable()
    {
        wAC = this.GetComponent<WolfAnimatorController>();

        DataInit();
        Debug.Log($"wolf data init enable preysAmt {preys.Count}");
       
        target = SetTarget();
        data.target = target;
    }

    private void DataInit()
    {
        status = data.status;
        preys = data.preys;
        homePos = data.homePos;
        alertDist = data.alertDist;

        missionComplete = false;
        arriveTarget = false;
        arriveHome = false;
        targetSwitched = false;
        hitten = false;
        catchedTarget = null;
    }

    private GameObject SetTarget()
    {
        int amt = preys.Count;

        if(amt <= 0)
        {
            return null;
        }

        int index = Random.Range(0, amt - 1);

        Debug.Log($"wolf set target amt {amt} index{index}");

        if (preys[index].activeSelf == true && TargetAccessable(preys[index]))
        {
            return preys[index];
        }
        else
        {
            if (index + 1 < amt)
            {
                if (preys[index + 1].activeSelf == true && TargetAccessable(preys[index + 1]))
                {
                    return preys[index + 1];
                }
            }
            else if (index - 1 >= 0)
            {
                if (preys[index - 1].activeSelf == true && TargetAccessable(preys[index - 1]))
                {
                    return preys[index - 1];
                }
            }
        }

        return null;
    }

    private bool TargetAccessable(GameObject target)
    {
        return target.transform.position.y <= 2.0f;
    }

    private void Update()
    {
        FindEffectPlayer();

        if (target != homePos)
        {
            CheckAndSetTarget();
        }

        CheckStatusAndUpdate();

        if (status == (int)WolfAIData.WolfStatus.Attacked)
        {
            wAC.ChangeAndPlayAnimation(wAC.attacked, 0, 0);
        }
        
        if (TargetPositionUpdate() == true)
        {
            aStarPerfoming = AStar.instance.PerformAStar(this.transform.position, target.transform.position);
            currentPathPt = 0;
        }

        SetY();

        switch (status)
        {
            case (int)WolfAIData.WolfStatus.Safe:
                CatchTarget();
                break;

            case (int)WolfAIData.WolfStatus.Alert:
                AvoidPlayer();
                break;

            case (int)WolfAIData.WolfStatus.GoHome:
                GoHome();
                break;
        }

        data.UpdateStatus(status);
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
    }
    
    private void CheckAndSetTarget()
    {
        if (missionComplete)
        {
            target = homePos;
        }
        else if (targetSwitched == false)
        {
            if (TargetAccessable(target) == false)
            {
                GameObject newTarget = SetTarget();

                if (newTarget != target)
                {
                    target = newTarget;
                    targetSwitched = true;
                }
                else
                {
                    target = homePos;
                    missionComplete = true;
                }
            }
        }
        else
        {
            target = homePos;
            missionComplete = true;
        }

        data.target = target;
    }

    private void CheckStatusAndUpdate()
    {
        if (hitten && !missionComplete)
        {
            status = (int)WolfAIData.WolfStatus.Attacked;
            data.UpdateStatus(status);
            return;
        }

        if (wAC.BreakingOrNot() == true)
        {
            status = (int)WolfAIData.WolfStatus.Safe;
            data.UpdateStatus(status);
            return;
        }

        if (effectPlayers.Count == 0)
        {
            if (missionComplete)
            {
                status = (int)WolfAIData.WolfStatus.GoHome;
            }
            else
            {
                status = (int)WolfAIData.WolfStatus.Safe;
            }
        }
        else
        {
            status = (int)WolfAIData.WolfStatus.Alert;
        }

        data.UpdateStatus(status);
    }

    Vector3 oriPos;
    private bool TargetPositionUpdate()
    {
        bool targetPosChanged;

        if (target != null)
        {
            if (oriPos != target.transform.position)
            {
                data.m_vTarget = target.transform.position;
                oriPos = target.transform.position;
                targetPosChanged = true;
            }
            else
            {
                targetPosChanged = false;
            }
        }
        else
        {
            Debug.LogError("wolf target null");
            targetPosChanged = false;
        }
        
        return targetPosChanged;
    }

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
            newPos.y = hit.point.y + 0.38f;
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

        if (JumpOrNot() == true)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(nearestP.transform.position, 4.0f);
        }
    }

    #region wolf behaviour tree

    private void CatchTarget()
    {
        if (arriveTarget == false)
        {
            jumping = JumpOrNot();
            if (jumping == true)
            {
                arriveTarget = SteeringBehavior.Seek(data);
            }
            else if (aStarPerfoming)
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

                arriveTarget = SteeringBehavior.Seek(data);
            }
            else if (SteeringBehavior.CollisionAvoid(data) == false)
            {
                arriveTarget = SteeringBehavior.Seek(data);
            }

            SteeringBehavior.Move(data);

            if (jumping == true)
            {
                wAC.ChangeAndPlayAnimation(wAC.jumpTrigger, data.m_fTempTurnForce * 8, data.m_Speed * 8);
            }
            else
            {
                wAC.ChangeAndPlayAnimation(wAC.runTrigger, data.m_fTempTurnForce * 8, data.m_Speed * 8);
            }
        }
        else
        {
            wAC.ChangeAndPlayAnimation(wAC.catchT, 0, 0);
        }

        if (wAC.BreakingOrNot() == false && arriveTarget == true && target != homePos)
        {
            if (catchedTarget == null)
            {
                Vector3 dir = target.transform.position - this.transform.position;
                dir.y = this.transform.position.y;
                this.transform.forward = dir;
            }
            wAC.ChangeAndPlayAnimation(wAC.catchT, 0, 0);
        }
    }

    private void AvoidPlayer()
    {
        if (effectPlayers.Count <= 0)
        {
            return;
        }
        else
        {
            if (SteeringBehavior.PlayerAvoid(data, effectPlayers) == false && SteeringBehavior.CollisionAvoid(data) == false)
            {
                SteeringBehavior.Seek(data);
            }

            SteeringBehavior.Move(data);

            Debug.Log($"wac {wAC == null}");

            wAC.ChangeAndPlayAnimation(wAC.runTrigger, data.m_fTempTurnForce * 8, data.m_Speed * 8);
        }
    }

    private void GoHome()
    {
        if (arriveHome == false)
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

                arriveHome = SteeringBehavior.Seek(data);
            }
            else if (SteeringBehavior.CollisionAvoid(data) == false)
            {
                arriveHome = SteeringBehavior.Seek(data);
            }

            SteeringBehavior.Move(data);
            wAC.ChangeAndPlayAnimation(wAC.runTrigger, data.m_fTempTurnForce * 8, data.m_Speed * 8);
        }
        else
        {
            mouth.transform.DetachChildren();
            GameObject.Destroy(catchedTarget);
            catchedTarget = null;
            data.catchedTarget = null;
            this.gameObject.SetActive(false);
        }
    }

    public GameObject nearestP;
    private bool JumpOrNot()
    {
        float dist = 4.0f;
        nearestP = null;

        for (int i = 0; i < jumpPs.Count; i++)
        {
            float temp = (jumpPs[i].transform.position - this.transform.position).magnitude;
            if (temp < dist)
            {
                nearestP = jumpPs[i];
                dist = temp;
            }
        }

        if (nearestP != null)
        {
            Vector3 toP = nearestP.transform.position - this.transform.position;
            float dotP = Vector3.Dot(this.transform.forward, toP);

            if (dotP > 0.7f || dotP < -0.7f)
            {
                return true;
            }
        }

        return false;
    }

    #endregion

    #region ANIMA EVENT
    
    public GameObject mouth;

    private void AnimaEventBreakBox()
    {
        GameObject box = target.transform.parent.transform.parent.transform.parent.gameObject;

        if (box != null)
        {
            target.transform.parent = null;
            box.SetActive(false);
            GameObject.Destroy(box);

            Debug.Log($"box destroyed {box.gameObject.name}");
        }
    }

    private void AnimaEventCatchTarget()
    {
        Vector3 mouthPos = mouth.transform.position;
        mouthPos.y -= 1.0f;
        target.GetComponent<RabbitAI>().m_Data.isBited = true;
        target.transform.position = mouthPos;
        target.transform.right = mouth.transform.up;
        target.transform.parent = mouth.transform;
        (target.GetComponent(typeof(Collider)) as Collider).enabled = false;
        catchedTarget = target;
        data.catchedTarget = target;
        missionComplete = true;
    }

    private void AnimaEventAttacked()
    {
        mouth.transform.DetachChildren();
        (catchedTarget.GetComponent(typeof(Collider)) as Collider).enabled = true;
        catchedTarget = null;
        data.catchedTarget = null;
        missionComplete = true;
    }
    #endregion

    //UI display for warnUIDisplayer
    public Vector3 WarningUIDisplay()
    {
        targetLocking = target != null && target != homePos;

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
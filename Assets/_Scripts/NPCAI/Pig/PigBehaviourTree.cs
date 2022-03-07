using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigBehaviourTree : MonoBehaviour
{
    public PigAIData data;
    public GameObject target;
    private int status;

    GameObject[] players;
    public GameObject nearestPlayer;
    private Rigidbody pRB;
    [HideInInspector]
    public GameObject bumpEndPos;

    //bools
    public bool arriveHome;
    public bool bumpedP;

    //catched by box
    public bool beCatched;
    public GameObject catcher;
    public GameObject mesh;

    //Astar
    AStar astar;
    public string txtName;
    public string nodeTag;
    public bool aStarPerforming = false;
    int currentPathPt = -1;

    //bump player
    public float runDist; //dist from target(behind player) to player
    public GameObject bumpEffect;
    ParticleSystem bumpSystem;


    //animator
    private PigAnimatorController pAC;

    //crown
    public GameObject crown;

    //audio
    private AudioSource audioSource;
    public AudioClip[] clips;

    private void Awake()
    {
        //AStar
        WPTerrain wpt = new WPTerrain();
        wpt.Init(txtName, nodeTag);

        astar = new AStar();
        astar.Init(wpt);

        InitPlayer();

        bumpSystem = bumpEffect.GetComponent<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
    }

    private void InitPlayer()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
    }

    private void OnEnable()
    {
        pAC = this.GetComponent<PigAnimatorController>();
        DataInit();

        target = data.homePos;

        aStarPerforming = astar.PerformAStar(this.transform.position, data.homePos.transform.position);
        currentPathPt = 0;

        PlayPigAudio(0);
    }

    public void PlayPigAudio(int i)
    {
        audioSource.clip = clips[i];
        audioSource.Play();
    }

    private void DataInit()
    {
        crown.SetActive(true);

        target = data.homePos;
        status = data.status;

        arriveHome = false;
        bumpedP = false;
        nearestPlayer = null;
        audioPlayed = false;
    }

    private void Update()
    {
        if (nearestPlayer == null)
        {
            FindNearestPlayer();
        }

        CheckAndUpdateStatus();
        SetAndUpdateTargetPos();

        if (targetPosChange)
        {
            aStarPerforming = astar.PerformAStar(this.transform.position, target.transform.position);
            currentPathPt = 0;

            Debug.Log($"pig targetChange targetPosChange{targetPosChange} target{target.name} astar{aStarPerforming}");
        }

        SetY();

        switch (status)
        {
            case (int)PigAIData.PigStatus.Safe:
                crown.SetActive(true);
                WalkToHome();
                break;

            case (int)PigAIData.PigStatus.Alert:
                crown.SetActive(true);
                BumpPlayer();
                break;

            case (int)PigAIData.PigStatus.Flee:
                crown.SetActive(true);
                RunToHome();
                break;

            case (int)PigAIData.PigStatus.Catched:
                crown.SetActive(false);
                StayStill();
                break;
        }
    }

    GameObject tempPlayer;
    float nearestDist = 10000.0f;
    float tempDist;
    private void FindNearestPlayer()
    {
        //reset compareData;
        nearestDist = 10000.0f;
        tempPlayer = null;

        //find the nearest player
        foreach (GameObject p in players)
        {
            tempDist = (p.transform.position - this.transform.position).magnitude;

            if (tempDist <= data.m_fRadius)
            {
                if (tempDist <= nearestDist)
                {
                    tempPlayer = p;
                    nearestDist = tempDist;
                }
            }
        }

        nearestPlayer = tempPlayer;

        if (nearestPlayer != null)
        {
            pRB = nearestPlayer.GetComponent<Rigidbody>();
        }
    }

    private void CheckAndUpdateStatus()
    {
        if (beCatched == true && catcher != null)
        {
            status = (int)PigAIData.PigStatus.Catched;
        }
        else if (nearestPlayer != null && bumpedP == false)
        {
            status = (int)PigAIData.PigStatus.Alert;
        }
        else if (bumpedP == true || (beCatched == true && catcher == null))
        {
            status = (int)PigAIData.PigStatus.Flee;
        }
        else
        {
            status = (int)PigAIData.PigStatus.Safe;
        }

        data.UpdateStatus(status);
    }

    bool targetPosChange;
    Vector3 oriTarget;
    private void SetAndUpdateTargetPos()
    {
        oriTarget = target.transform.position;
        
        if (status == (int)PigAIData.PigStatus.Alert)
        {
            if (target != bumpEndPos)
            {
                bumpEndPos.transform.position = SetBumpTarget();
                target = bumpEndPos;
                data.m_vTarget = target.transform.position;
            }
        }
        else
        {
            target = data.homePos;
            data.m_vTarget = target.transform.position;
        }

        if (target.transform.position != oriTarget)
        {
            targetPosChange = true;
        }
        else
        {
            targetPosChange = false;
        }
    }

    private Vector3 SetBumpTarget()
    {
        Vector3 result;
        Vector3 dir = nearestPlayer.transform.position - this.transform.position;
        
        if (!(Physics.Linecast(this.transform.position, nearestPlayer.transform.position, 1 << 8 | 1 << 15)))
        {
            dir.Normalize();
            result = this.transform.position + dir * runDist;
        }
        else
        {
            result = this.transform.position + this.transform.forward * 1.0f;
        }

        return result;
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
            newPos.y = hit.point.y;
            this.transform.position = newPos;
        }
    }

    #region behaviour
    private void WalkToHome()
    {
        if (arriveHome == false)
        {
            arriveHome = ArriveTargetOrNot();
        }

        if (arriveHome == false)
        {
            if (aStarPerforming)
            {
                List<Vector3> path = astar.GetPath();
                int final = path.Count - 1;

                string s = "";
                foreach (Vector3 p in path)
                {
                    s += p;
                }

                for (int i = final; i >= currentPathPt; i--)
                {
                    Vector3 sPos = path[i];
                    Vector3 cPos = this.transform.position;

                    RaycastHit hit;
                    if (Physics.Linecast(cPos, sPos, out hit, 1 << 8 | 1 << 15))
                    {
                        Debug.Log($"pig linecast {hit.transform.gameObject}");
                        continue;
                    }

                    currentPathPt = i;

                    data.SetTarget(sPos);

                    Debug.Log($"pig doing astar path{s} sPos{sPos}");
                    break;
                }
                SteeringBehavior.Seek(data);
            }
            else if (SteeringBehavior.CollisionAvoid(data) == false)
            {
                SteeringBehavior.Seek(data);
            }

            SteeringBehavior.Move(data);
            pAC.ChangeAndPlayAnimation(pAC.walkTrigger, data.m_fTempTurnForce * 30, data.m_Speed * 30);
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }


    public float bumpForce;
    public float bumpUpForce;
    bool audioPlayed = false;
    private void BumpPlayer()
    {
        if (bumpedP == false)
        {
            if (ArriveTargetOrNot() == true)
            {
                bumpedP = true;
            }
            else if (Physics.Linecast(this.transform.position, target.transform.position, 1 << 8 | 1 << 15))
            {
                bumpedP = true;
            }
        }

        if (bumpedP == false)
        {
            Vector3 dir = nearestPlayer.transform.position - this.transform.position;

            if(dir.magnitude <= 3.0f)
            {
                if(audioPlayed == false)
                {
                    audioSource.loop = false;
                    PlayPigAudio(1);
                    audioPlayed = true;
                }
                bumpSystem.Play();
                pRB.AddExplosionForce(bumpForce, this.transform.position, 5.0f, bumpUpForce, ForceMode.Impulse);
            }

            if (SteeringBehavior.CollisionAvoid(data) == false)
            {
                SteeringBehavior.Seek(data);
            }

            SteeringBehavior.Move(data);
            pAC.ChangeAndPlayAnimation(pAC.runTrigger, data.m_fTempTurnForce * 30, data.m_Speed * 30);
        }
    }

    private void RunToHome()
    {
        if (arriveHome == false)
        {
            arriveHome = ArriveTargetOrNot();
        }

        if (arriveHome == false)
        {
            if (aStarPerforming)
            {
                List<Vector3> path = astar.GetPath();
                int final = path.Count - 1;
                
                string s = "";
                foreach (Vector3 p in path)
                {
                    s += p;
                }

                for (int i = final; i >= currentPathPt; i--)
                {
                    Vector3 sPos = path[i];
                    Vector3 cPos = this.transform.position;
                    RaycastHit hit;
                    if (Physics.Linecast(cPos, sPos, out hit, 1 << 8 | 1 << 15))
                    {
                        Debug.Log($"pig linecast {hit.transform.gameObject}");
                        continue;
                    }

                    currentPathPt = i;

                    data.SetTarget(sPos);
                    Debug.Log($"pig doing astar path{s} sPos{sPos}");
                    break;
                }
                SteeringBehavior.Seek(data);
            }
            else if (SteeringBehavior.CollisionAvoid(data) == false)
            {
                SteeringBehavior.Seek(data);
            }

            SteeringBehavior.Move(data);
            pAC.ChangeAndPlayAnimation(pAC.runTrigger, data.m_fTempTurnForce * 30, data.m_Speed * 30);
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }

    public float rotateAmt;
    private void StayStill()
    {
        Collider c = this.GetComponent(typeof(Collider)) as Collider;
        c.enabled = false;

        if (catcher.tag == "Box")
        {
            this.transform.localPosition = new Vector3(0.2f, 0, 0f);
            //gameObject.transform.Rotate(new Vector3(0, rotateAmt, 0));
            pAC.ChangeAndPlayAnimation(pAC.shake, 0, 0);
        }

        if (catcher.tag == "Bag")
        {
            this.transform.localPosition = new Vector3(0, 0, 0);
            mesh.SetActive(false);
            pAC.ChangeAndPlayAnimation(pAC.shake, 0, 0);
        }
    }

    #endregion
    private bool ArriveTargetOrNot()
    {
        float dist = (this.transform.position - target.transform.position).magnitude;

        if (dist <= data.arriveDist)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //for box or bag
    public void SetCatchedStatus(GameObject boxOrBag)
    {
        beCatched = true;
        catcher = boxOrBag;
    }
    private void OnDrawGizmos()
    {
        if (this.gameObject.activeSelf == true)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(data.birthPos.transform.position, 1.1f);

            Gizmos.color = Color.gray;
            Gizmos.DrawSphere(data.homePos.transform.position, 1.1f);

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
        }
      
        if (aStarPerforming)
        {
            List<Vector3> path = astar.GetPath();
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
}

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
    public bool attackEnd = false;

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
    AStar aStar;

    //target UI
    public bool targetLocking = true;

    //destroy target effect
    public GameObject destroyEffect;
    private ParticleSystem destroySystem;

    // Start is called before the first frame update
    void Start()
    {
        //AStar
        WPTerrain wpt = new WPTerrain();
        wpt.Init(txtName, nodeName);

        aStar = new AStar();
        aStar.Init(wpt);

        PlayerInit();

        destroySystem = destroyEffect.GetComponent<ParticleSystem>();

        levelControl = GameObject.Find("LevelControl").GetComponent<LevelControl>();
    }

    private void OnEnable()
    {
        Debug.Log("fox init onenable");
        DataInit();

        fAC = this.GetComponent<FoxAnimatorController>();

        if (data.target != null)
        {
            PlayFoxAudio(1);
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

        missionComplete = false;
        arriveTarget = false;
        arriveHomeArea = false;
        pAttact = false;
        targetLocking = true;
        aStarPerfoming = true;
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
        if (!(target != null && (target.transform.position - this.transform.position).magnitude < data.m_fRadius + 3.0f))
        {
            FindEffectPlayer();
        }
        else
        {
            Debug.Log("fox ignore");
            effectPlayers.Clear();
        }

        CheckStatusAndUpdate();
        
        if (target == null || status == (int)FoxAIData.FoxStatus.Home)
        {
            target = data.birthPos;
            data.target = target;

            if (target != null && target.activeSelf == true)
            {
                data.m_vTarget = target.transform.position;
            }

            targetLocking = false;
        }

        if (UpdateTargetPosition() == true || aStarPerfoming == false)
        {
            Debug.Log("fox astar start");
            aStarPerfoming = aStar.PerformAStar(this.transform.position, target.transform.position);
            currentPathPt = 0;
        }

        if (aStarPerfoming == false)
        {
            target = data.birthPos;
            missionComplete = true;
            status = (int)FoxAIData.FoxStatus.Home;
            targetLocking = false;
        }

        if (status == (int)FoxAIData.FoxStatus.Attacked)
        {
            targetLocking = false;
            Hurt();
        }
        else if (!fAC.AllowToMove())
        {
            return;
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


        Debug.Log($"target pos change {targetPosChange}");
        return targetPosChange;
    }

    private void CheckStatusAndUpdate()
    {
        if (fAC.BreakingOrNot() == true && UnableToAccessOrNot())
        {
            missionComplete = true;
            status = (int)FoxAIData.FoxStatus.Home;
            data.UpdateStatus(status);
            return;
        }

        if (hitten == true && !attackEnd)
        {
            status = (int)FoxAIData.FoxStatus.Attacked;
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
                missionComplete = true;
                status = (int)FoxAIData.FoxStatus.Home;
            }
        }

        if (effectPlayers.Count > 0)
        {
            status = (int)FoxAIData.FoxStatus.Alert;
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
            arriveTarget = ArriveTargetOrNot();
        }

        if (arriveTarget == false)
        {

            if (aStarPerfoming)
            {
                List<Vector3> path = aStar.GetPath();
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
                        Debug.Log($"fox linecast {hit.transform.gameObject}");
                        continue;
                    }

                    currentPathPt = i;

                    Debug.Log($"Doing AStar path{s} spos{sPos}");
                    data.SetTarget(sPos);
                    break;
                }
                SteeringBehavior.Seek(data);
            }
            else if (SteeringBehavior.CollisionAvoid(data) == false)
            {
                SteeringBehavior.Seek(data);
            }

            SteeringBehavior.Move(data);
            fAC.ChangeAndPlayAnimation(fAC.runTrigger, data.m_fTempTurnForce * 5, data.m_Speed * 10);
        }
        else
        {
            if (target.activeSelf == true && target != birthPos)
            {
                destroyEffect.transform.position = target.transform.position;
                destroyEffect.SetActive(true);
                destroySystem.Play();
                
            }
            else
            {
                destroySystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                destroyEffect.SetActive(false);
            }

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
            arriveHomeArea = ArriveTargetOrNot();
        }
     
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
            List<Vector3> path = aStar.GetPath();
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
                    Debug.Log($"fox linecast  {hit.transform.gameObject}");
                    continue;
                }

                currentPathPt = i;
                data.SetTarget(sPos);
                Debug.Log($"doing astar home path{s} sPos{sPos}");
                break;
            }

            SteeringBehavior.Seek(data);
        }
        else if (SteeringBehavior.CollisionAvoid(data) == false)
        {
            SteeringBehavior.Seek(data);
        }

        SteeringBehavior.Move(data);
        fAC.ChangeAndPlayAnimation(fAC.runTrigger, data.m_fTempTurnForce * 5, data.m_Speed * 10);
    }

    #endregion

    #region other behaviour

    public void AnimaEventLockTargetPos()
    {
        int childAmt = target.transform.childCount;
        if (childAmt > 0)
        {
            foreach (Transform child in target.transform)
            {
                (child.gameObject.GetComponent(typeof(Collider)) as Collider).enabled = false;
            }
        }
        if (target.GetComponent<Rigidbody>() != null)
        {
            target.GetComponent<Rigidbody>().isKinematic = true;
        }
        Vector3 temp = this.transform.position + this.transform.forward * data.arriveDist;
        temp.y = target.transform.position.y;
        target.transform.position = temp;
    }
    public void AnimaEventBreakTarget()
    {
        if (target == birthPos)
        {
            return;
        }

        if (target.tag == "Box")
        {
            GameObject animalCatched = target.GetComponent<BoxController>().targetAnimal;

            Debug.Log($"fox check catched animal {animalCatched != null}");

            if (animalCatched != null)
            {
                animalCatched.transform.SetParent(null);
                animalCatched.transform.position = target.transform.position;

                if (animalCatched.tag == "Rabbit")
                {
                    animalCatched.GetComponent<RabbitAI>().m_Data.isCatched = false;
                    animalCatched.GetComponent<RabbitAI>().m_Data.isTargeted = false;
                }

                if (animalCatched.tag == "Raccoon")
                {
                    animalCatched.GetComponent<RaccoonAI>().m_Data.isCatched = false;
                    animalCatched.GetComponent<RaccoonAI>().m_Data.isTargeted = false;
                }
            }
        }

        if (target.tag == "Bag")
        {
            GameObject animalCatched = target.GetComponent<BagController>().targetAnimal;

            if (animalCatched != null)
            {
                animalCatched.transform.parent = null;
                if (animalCatched.tag == "Raccoon")
                {
                    animalCatched.GetComponent<RaccoonAI>().m_Data.isCatched = false;
                }
            }
        }

        
        target.SetActive(false);
        GameObject.Destroy(target);
        target = null;
        missionComplete = true;
        targetLocking = false;
        MinusScore();
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
            List<Vector3> path = aStar.GetPath();
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

    private void SetTarget()
    {
        if (missionComplete)
        {
            target = data.birthPos;
            data.m_vTarget = target.transform.position;
        }
    }

    private bool music = true;
    public AudioSource audioSource;
    public AudioClip[] clip;
    public Vector3 WarningUIDisplay()
    {
        Vector3 newPos;

        if (targetLocking == true)
        {
            newPos = target.transform.position;
            newPos.y += 2.5f;
            if (music)
            {
                audioSource.clip = clip[0];
                InvokeRepeating("PlayAudio",0,1f);
                music = false;
            }
        }
        else
        {
            newPos = new Vector3(1000000.0f, 1000000.0f, 1000000.0f);
            music = true;
            CancelInvoke("PlayAudio");
        }

        return newPos;
    }

    public void PlayAudio()
    {
        audioSource.Play();
    }

    public void PlayFoxAudio(int i)
    {
        audioSource.clip = clip[i];
        audioSource.Play();
    }
    private bool ArriveTargetOrNot()
    {
        float dist = (this.transform.position - target.transform.position).magnitude;

        if (dist <= data.arriveDist)
        {
            Debug.Log($"arrivetarget true + dist {dist} arrive dist {data.arriveDist}");
            return true;
        }
        else
        {
            Debug.Log($"arrivetarget false + dist {dist} arrive dist {data.arriveDist}");
            return false;
        }
    }

    public void AnimaEventAttckEnd()
    {
        if (!missionComplete)
        {
            missionComplete = true;
        }

        attackEnd = true;
    }

    public LevelControl levelControl;
    private void MinusScore()
    {
        levelControl.MinusScorePos(this.transform.position);
        levelControl.GenTotalScore(levelControl.fox);
    }
}

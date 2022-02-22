using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fox_BehaviourTree : MonoBehaviour
{
    //init data
    public FoxAIData data;
    [SerializeField]
    public GameObject target;
    public GameObject birthPos;
    private int status;
    private float alertDist;

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

    private float maxSpeed;
    private float maxRot;
    
    //seek ended or not
    private bool arriveTarget = false;

    //mission complete or not
    private bool missionComplete = false;

    //player Action
    public bool pAttact = false;

    //fox animator
    private FoxAnimatorController fAC;

    // Start is called before the first frame update
    void Start()
    {
        DataInit();
        PlayerInit();

        fAC = this.GetComponent<FoxAnimatorController>();
    }

    private void DataInit()
    {
        data.target = target;
        data.birthPos = birthPos;
        data.m_Go = this.gameObject;
        status = (int)data.status;
        alertDist = data.m_fRadius;
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
        if (!fAC.AllowToMove())
        {
            return;
        }

        FindEffectPlayer();

        if (!missionComplete)
        {
            UpdateTargetPosition();
        }

        CheckStatusAndUpdate();

        switch (status)
        {
            case (int)FoxAIData.FoxStatus.Safe:
                BreakItem();
                break;

            case (int)FoxAIData.FoxStatus.Alert:
                Alert(effectPlayers);
                break;

            case (int)FoxAIData.FoxStatus.Attacked:
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
        data.status = status;
    }

    private void UpdateTargetPosition()
    {
        if (target != null)
        {
            data.m_vTarget = target.transform.position;
        }
    }

    private void CheckStatusAndUpdate()
    {
        if(effectPlayers.Count == 0)
        {
            if(!missionComplete)
            {
                status = (int)FoxAIData.FoxStatus.Safe;
            }

            if (missionComplete)
            {
                status = (int)FoxAIData.FoxStatus.Home;
            }
        }

        if(effectPlayers.Count > 0)
        {
            if(pAC.ThorowingOrNot() == false)
            {
                status = (int)FoxAIData.FoxStatus.Alert;
            }
            else
            {
                status = (int)FoxAIData.FoxStatus.Attacked;
            }
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
        if(nearestPlayer != null)
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

        if(nearestPlayer != null && nearestPlayer != oriEffectPlayer)
        {
            pAC = nearestPlayer.GetComponent<AnimatorController>();
        }
    }

    #region fox behaviour tree
    private void BreakItem()
    {
        if(arriveTarget == false)
        {
            if (SteeringBehavior.CollisionAvoid(data) == false)
            {
                arriveTarget = SteeringBehavior.Seek(data);
            }

            SteeringBehavior.Move(data);
            fAC.ChangeAndPlayAnimation(fAC.runTrigger, data.m_fTempTurnForce, data.m_fMoveForce);
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

        //if (players.Count == 1 && nearestPlayer != null) // fox walk around player
        //{
        //    Vector3 pPos = nearestPlayer.transform.position;
        //    Vector3 pF = nearestPlayer.transform.forward;
        //    Vector3 foxPos = this.transform.position;

        //    Vector3 dir = foxPos - pPos;
        //    float pFDotDir = Vector3.Dot(pF, dir);

        //    if (pFDotDir > -0.3f) //fox in front of effectPlayer
        //    {
        //        RotateAround(nearestPlayer);
        //    }
        //    else
        //    {
        //        BreakItem();
        //    }
        //}

        if (players.Count > 0)
        {
            AvoidPlayerAndRun(players);
            fAC.ChangeAndPlayAnimation(fAC.trotTrigger, data.m_fTempTurnForce, data.m_fMoveForce);
        }
    }

    int left = 0;
    int right = 1;
    bool avoided = false;

    private void AvoidAttack()
    {
        if(nearestPlayer == null)
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
            fAC.ChangeAndPlayAnimation(fAC.runTrigger, data.m_fTempTurnForce, data.m_fMoveForce);
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

    private bool AvoidCollision(List<GameObject> players)
    {
        Debug.Log("npc avoid collision");
        Vector3 foxPos = this.transform.position;
        Vector3 foxF = this.transform.forward;
        Vector3 vec;
        float finalDotDist;
        float finalProjDist;
        Vector3 finalVec = Vector3.forward;
        GameObject final = null;
        float dist = 0.0f;
        float dotf = 0.0f;
        float finalDot = 0.0f;
        int playerAmt = players.Count;

        float minDist = 10000.0f;
        for (int i = 0; i < playerAmt; i++)
        {
            vec = players[i].transform.position - foxPos;
            vec.y = 0.0f;
            dist = vec.magnitude;

            if(dist > data.m_fProbeLength + 2.0f)
            {
                continue;
            }

            vec.Normalize();
            dotf = Vector3.Dot(vec, foxF);

            if (dotf < 0)
            {
                continue;
            }
            else if (finalDot > 1.0f)
            {
                dotf = 1.0f;
            }

            float projDist = dist * dotf;
            float dotDist = Mathf.Sqrt(dist * dist - projDist * projDist);
            if (dotDist > 2.0f + data.m_fRadius)
            {
                continue;
            }

            if (dist < minDist)
            {
                minDist = dist;
                finalDotDist = dotDist;
                finalProjDist = projDist;
                finalVec = vec;
                final = players[i];
                finalDot = dotf;
            }
        }

        if (final != null)
        {
            Vector3 cross = Vector3.Cross(foxF, finalVec);
            float turnMag = Mathf.Sqrt(1.0f - finalDot * finalDot);

            if (cross.y > 0.0f)
            {
                turnMag = -turnMag;
            }

            data.m_fTempTurnForce = turnMag;
            float TotalLen = data.m_fProbeLength + 2.0f;
            float ratio = minDist / TotalLen;

            if(ratio > 1.0f)
            {
                ratio = 1.0f;
            }

            ratio = 1.0f - ratio;
            data.m_fMoveForce = -ratio;
            data.m_bCol = true;
            data.m_bMove = false;
            return true;
        }

        data.m_bCol = false;
        return false;
    }

    private void MoveForward()
    {
        this.transform.position += this.transform.forward * data.m_fMaxSpeed;
    }

    bool arriveHome = false;
    private void GoHome()
    {
        target = data.birthPos;
        data.m_vTarget = target.transform.position;

        MoveToBirthPos();
        fAC.ChangeAndPlayAnimation(fAC.runTrigger, data.m_fTempTurnForce, data.m_fMoveForce);

        if (arriveHome)
        {
            this.gameObject.SetActive(false);
            GameObject.Destroy(this.gameObject);
        }
    }

    private void MoveToBirthPos()
    {
        if (SteeringBehavior.CollisionAvoid(data) == false)
        {
            arriveHome = SteeringBehavior.Seek(data);
        }

        SteeringBehavior.Move(data);
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
        missionComplete = true;

        Debug.Log("npc break target" + missionComplete);
    }

    public float angularSpeed;
    float radius = 0.0f;
    float angle;

    private void RotateAround(GameObject target)
    {
        if(radius == 0.0f)
        {
            radius = (this.transform.position - target.transform.position).magnitude;
        }

        angle += (angularSpeed * Time.deltaTime) % 360;
        float posX = radius * Mathf.Sin(angle * Mathf.Deg2Rad);
        float posZ = radius * Mathf.Cos(angle * Mathf.Deg2Rad);

        this.transform.position = new Vector3(posX, 0, posZ) + target.transform.position;
    }

    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(this.transform.position, data.m_fRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(this.transform.position, this.transform.position + this.transform.forward * data.m_fProbeLength);
    }
}

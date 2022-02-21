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
    private AnimatorController ac;

    [SerializeField] public List<GameObject> players;
    [SerializeField]
    public GameObject effectPlayer;
    public bool holdingRockOrBox;

    private float maxSpeed;
    private float maxRot;
    
    //seek ended or not
    public bool arriveTarget = false;

    //mission complete or not
    public bool missionComplete = false;

    //player Action
    public bool pAttact = false;

    // Start is called before the first frame update
    void Start()
    {
        DataInit();
        PlayerInit();
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
        Debug.Log("npc update");
        FindEffectPlayer();
        if (!missionComplete)
        {
            UpdateTargetPosition();
        }

        CheckStatusAndUpdate();

        switch (status)
        {
            case (int)FoxAIData.FoxStatus.Safe:
                Debug.Log("npc safe");
                BreakItem();
                break;

            case (int)FoxAIData.FoxStatus.Alert:
                Alert(effectPlayer);
                break;

            case (int)FoxAIData.FoxStatus.Attacked:

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
        if(effectPlayer == null)
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

        if(effectPlayer != null)
        {
            if(ac.ThorowingOrNot() == false)
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

    GameObject nearestPlayer;
    float nearestDis = 10000.0f;
    float tempDist;
    GameObject oriEffectPlayer;
    private void FindEffectPlayer()
    {
        //save original effectPlayer
        if(effectPlayer != null)
        {
            oriEffectPlayer = effectPlayer;
        }

        //reset compareData;
        nearestDis = 10000.0f;
        nearestPlayer = null;

        //find the nearest player
        foreach (GameObject p in players)
        {
            tempDist = (p.transform.position - this.transform.position).magnitude;

            if (tempDist <= alertDist && tempDist <= nearestDis)
            {
                nearestPlayer = p;
                nearestDis = tempDist;
            }
        }

        effectPlayer = nearestPlayer;

        if(effectPlayer != null && effectPlayer != oriEffectPlayer)
        {
            ac = effectPlayer.GetComponent<AnimatorController>();
        }
    }

    #region fox behaviour tree
    private void BreakItem()
    {
        Debug.Log("npc break box");
        if(arriveTarget == false)
        {
            Debug.Log("npc break box2");
            if (SteeringBehavior.CollisionAvoid(data) == false)
            {
                Debug.Log("npc break box3");
                arriveTarget = SteeringBehavior.Seek(data);
            }

            SteeringBehavior.Move(data);
        }
        else
        {
            Debug.Log("npc arrive");
            BreakTarget(target);
            missionComplete = true;
        }
    }

    private void Alert(GameObject player)
    {
        AlertPlayer(effectPlayer);
    }

    private void AlertPlayer(GameObject player)
    {
        if (effectPlayer == null)
        {
            return;
        }

        Vector3 pPos = effectPlayer.transform.position;
        Vector3 pF = effectPlayer.transform.forward;
        Vector3 foxPos = this.transform.position;

        Vector3 dir = foxPos - pPos;
        float pFDotDir = Vector3.Dot(pF, dir);

        if (pFDotDir > -0.3f) //fox in front of effectPlayer
        {
            RotateAround(effectPlayer);
        }
        else
        {
            BreakItem();
        }
    }

    int left = 0;
    int right = 1;

    private void AvoidAttack()
    {
        if(effectPlayer == null)
        {
            return;
        }

        Vector3 pPos = effectPlayer.transform.position;
        Vector3 pF = effectPlayer.transform.forward;
        Vector3 foxPos = this.transform.position;

        Vector3 dir = foxPos - pPos;
        float pFDotDir = Vector3.Dot(pF, dir);

        if(pFDotDir > 0.7f) //fox in effectPlayer attackable area
        {
            int randomDir = Random.Range(0, 1);
            Vector3 targetDir;

            if(randomDir == right) //escape to right side
            {
                targetDir = effectPlayer.transform.right;
                targetDir.y = foxPos.y; //move on x-z
            }
            else
            {
                targetDir = -effectPlayer.transform.right;
                targetDir.y = foxPos.y;
            }

            Vector3 newDir = targetDir + this.transform.forward;
            this.transform.forward = Vector3.Slerp(this.transform.forward, newDir, 0.8f);
            this.transform.position += this.transform.forward * data.jumpSpeed;
        }
        else
        {
            this.transform.position += this.transform.forward * data.m_fMaxSpeed;
        }
    }

    private void GoHome()
    {
        target = data.birthPos;
        data.m_vTarget = target.transform.position;

        bool arriveHome = SteeringBehavior.Seek(data);

        if (arriveHome)
        {
            this.gameObject.SetActive(false);
            GameObject.Destroy(this.gameObject);
        }
    }

    #endregion

    #region other behaviour

    private void BreakTarget(GameObject target)
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

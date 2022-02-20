using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fox_BehaviourTree : MonoBehaviour
{
    //init data
    public FoxAIData data;
    [SerializeField]
    public GameObject target;
    private int status;
    private float alertDist;

    //real speed and rot
    public float movingForce;
    public float turningForce;
    public float speed;
    public float rot;
    private Vector3 currentF;

    //players on the field
    private GameObject p1;
    private GameObject p2;
    private GameObject p3;
    private GameObject p4;

    [SerializeField] public List<GameObject> players;
    [SerializeField]
    public GameObject effectPlayer;
    public bool holdingRockOrBox;

    public float maxSpeed;
    public float maxRot;
    //seek ended or not
    public bool arriveTarget = false;

    public enum FoxStatus
    {
        Safe,
        Alert,
        Attacked,
        Home
    }

    // Start is called before the first frame update
    void Start()
    {
        DataInit();
        PlayerInit();
    }

    private void DataInit()
    {
        data.target = target;
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
        UpdateTargetPosition();

        switch (status)
        {
            case (int)FoxStatus.Safe:
                Debug.Log("npc safe");
                BreakItem();
                break;

            case (int)FoxStatus.Alert:
                Alert(effectPlayer);
                break;

            case (int)FoxStatus.Attacked:

                break;

            case (int)FoxStatus.Home:
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

    GameObject nearestPlayer;
    float nearestDis = 10000.0f;
    float tempDist;
    private void FindEffectPlayer()
    {
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
        AlertSafeToggle();

        if (effectPlayer != null)
        {
            GameObject item = effectPlayer.GetComponent<PlayerData>().item;
            if (item != null && (item.tag == "RockModel" || item.tag == "Box"))
            {
                holdingRockOrBox = true;
            }
            else
            {
                holdingRockOrBox = false;
            }
        }
    }

    private void AlertSafeToggle()
    {
        if (effectPlayer == null)
        {
            status = (int)FoxStatus.Safe;
        }

        if (effectPlayer != null && status != (int)FoxStatus.Alert)
        {
            status = (int)FoxStatus.Alert;
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
            status = (int)FoxStatus.Home;
        }
    }

    private void Alert(GameObject player)
    {
        AlertPlayer(effectPlayer);
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
        target = data.BirthPos;
        data.m_vTarget = target.transform.position;
        bool arriveHome = SteeringBehavior.Seek(data);

        if (arriveHome)
        {
            this.gameObject.SetActive(false);
        }
    }

    #endregion

    #region steering behaviour
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

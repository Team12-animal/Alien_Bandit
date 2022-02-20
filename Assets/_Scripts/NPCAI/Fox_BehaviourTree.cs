using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fox_BehaviourTree : MonoBehaviour
{
    [SerializeField]
    private GameObject target;
    private Fox_AIData data;
    private int status;
    private float maxSpeed;
    private float maxRot;
    private float movingForce;
    private float turningForce;

    private float alertDist;
    private float probeLength;

    //real speed and rot
    private float speed;
    private float rot;

    //players on the field
    private GameObject p1;
    private GameObject p2;
    private GameObject p3;
    private GameObject p4;

    [SerializeField] public List<GameObject> players;
    [SerializeField]
    public GameObject effectPlayer;
    public bool holdingRockOrBox;

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
        data = this.gameObject.GetComponent<Fox_AIData>();
        DataInit();
        PlayerInit();
    }

    private void DataInit()
    {
        target = data.dirTarget;
        status = (int)data.status;
        maxSpeed = data.realSpeed;
        maxRot = data.realRot;
        alertDist = data.alertDist;
        probeLength = data.probeLength;
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

        switch (status)
        {
            case (int)FoxStatus.Safe:
                BreakBoxAndStealRabbit(target);
                break;

            case (int)FoxStatus.Alert:
                Alert(effectPlayer);
                break;

            case (int)FoxStatus.Attacked:
                break;

            case (int)FoxStatus.Home:
                break;

        }

        DataUpdate();
    }

    //update aidata script
    private void DataUpdate()
    {
        data.status = status;
    }


    GameObject comparedPlayer;
    float comparedDist = 10000.0f;

    private void FindEffectPlayer()
    {
        //reset compareData;
        comparedDist = 10000.0f;
        comparedPlayer = null;

        foreach (GameObject p in players)
        {
            float tempDist = (p.transform.position - this.transform.position).magnitude;

            if (tempDist < alertDist && tempDist < comparedDist)
            {
                comparedPlayer = p;
                comparedDist = tempDist;
            }
        }

        Debug.Log("npc find p" + comparedPlayer.name);
        if(comparedPlayer == null)
        {
            effectPlayer = null;
        }
        else
        {
            effectPlayer = comparedPlayer;
        }

        CheckAndChangeStatus();


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

    private void CheckAndChangeStatus()
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

    private void BreakBoxAndStealRabbit(GameObject target)
    {
        if(arriveTarget == false)
        {
            if (AvoidCollision() == false)
            {
                arriveTarget = Seek(target);
            }

            Move();
        }
        else
        {
            Debug.Log("npc arrive");
            BreakTarget(target);
            StealRabbit(target);
            status = (int)FoxStatus.Home;
        }
    }

    private void Alert(GameObject player)
    {

    }

    #endregion

    #region steering behaviour

    private void Move()
    {
        Vector3 npcPos = this.transform.position;
        Vector3 vf = this.transform.forward;
        Vector3 vr = this.transform.right;

        //maximum and minimum of turningforce
        if (turningForce > maxRot)
        {
            turningForce = maxRot;
        }
        else if (turningForce < -maxRot)
        {
            turningForce = -maxRot;
        }

        //get new forward
        vf += vr * turningForce;
        vf.Normalize();
        this.transform.forward = vf;

        //get new speed
        speed += movingForce * Time.deltaTime;

        //maximum and minmum of speed
        if (speed < 0.1f)
        {
            speed = 0.1f;
        }
        else if (speed > maxSpeed)
        {
            speed = maxSpeed;
        }
    }

    private bool Seek(GameObject target)
    {
        Vector3 npcPos = this.transform.position;
        Vector3 dir = target.transform.position - npcPos;
        dir.y = 0.0f;
        float dist = dir.magnitude;

        //arrive target
        if (dist < 0.2f)
        {
            speed = 0.0f;
            rot = 0.0f;
            return true;
        }

        dir.Normalize();

        //fox's moving and turning force
        Vector3 vf = this.transform.forward;
        Vector3 vr = this.transform.right;

        //get forward force
        float dotF = Vector3.Dot(vf, dir);


        if (dotF > 0.96f)
        {
            dotF = 1.0f;
            this.transform.forward = dir;
            turningForce = 0.0f;
        }
        else
        {
            if (dotF < -1.0f)
            {
                dotF = -1.0f;
            }

            //get turning force
            float dotR = Vector3.Dot(vr, dir);

            if (dotF < 0.0f) //target is behind NPC
            {
                if (dotR > 0.0f) // target is on the right side
                {
                    dotR = 1.0f; //turn fully right
                }
                else
                {
                    dotR = -1.0f; // turn fully left
                }
            }

            //turning less and less
            if (dist < 3.0f)
            {
                dotR *= (dist / 3.0f + 1.0f);
            }

            turningForce = dotR;
        }

        //slow done
        if(dist < 3.0f)
        {
            if(speed > 0.1f)
            {
                movingForce = -(1.0f - dist / 0.3f) * 5.0f;
            }
            else
            {
                movingForce = dotF * 100.0f;
            }
        }
        else
        {
            movingForce = 100.0f;
        }

        return false;
    }

    private bool AvoidCollision()
    {
        return false;
    }

    #endregion

    #region other behaviour

    private void BreakTarget(GameObject target)
    {
        
    }

    private void StealRabbit(GameObject target)
    {

    }

    #endregion
}

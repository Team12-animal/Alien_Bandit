using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WolfController : MonoBehaviour
{
    [Header("GameObjects(Targets) in Scene")]
    GameObject restPlace;
    GameObject finalPlace;
    GameObject[] rabbits;
    bool catched;
    bool rested;
    bool finaled;
    float restTime = 0.0f;

    [Header("Self condition")]
    [SerializeField] Transform mousePosition;

    Animator animator;
    Rigidbody rigidbody;

    [Header("Using Others Scripts and Sources")]
    public AIData_WOLF aiData;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        restPlace = GameObject.Find("Rest");
        finalPlace = GameObject.Find("Final");
        catched = false;
        rested = false;
        finaled = false;
    }

    private void Update()
    {
        Vector3 temp = aiData.target.transform.position - transform.position;
        if (!catched && animator.GetCurrentAnimatorStateInfo(0).IsName("Wolfr_Run_Forward") && (temp.magnitude) <= 3.6f)
        {
            animator.Play("Wolfr_Attack_JumpForward");
        }
        else if (!catched && animator.GetCurrentAnimatorStateInfo(0).IsName("Wolfr_Run_Forward") && (temp.magnitude) >= 3.0f)
        {
            SteeringBehavoirTest.Seek(aiData, aiData.target);
            SteeringBehavoirTest.Move(aiData);
        }
        else if (catched && !rested)
        {
            aiData.target = restPlace;
            SteeringBehavoirTest.Seek(aiData, aiData.target);
            SteeringBehavoirTest.Move(aiData);
            if (temp.magnitude <= 3.0f)
            {
                rested = true;
                animator.Play("Wolfr_Sneak_Idle 0");
                aiData.target = finalPlace;
            }
        }
        else if(rested && restTime>= 6.0f && (temp.magnitude) >= 6.0f && !finaled)
        {
            animator.Play("Wolfr_Run_Forward 0 0");
            SteeringBehavoirTest.Seek(aiData, aiData.target);
            SteeringBehavoirTest.Move(aiData);
            return;
        }
        else if (rested && restTime >= 6.0f && (temp.magnitude) <= 3.0f)
        {
            animator.applyRootMotion = false;
            animator.Play("Wolfr_Jump_InPlace");
            finaled = true;
            return;
        }

        if (rested)
        {
            restTime += Time.deltaTime;
        }

        //if(transform.position.y >= 30.74f || transform.position.y <= -30.74f)
        //{
        //    Destroy(gameObject);
        //}
    }

    #region AnimationEvents
    public void ApplyRootMotion()
    {
        animator.applyRootMotion = true;
    }

    public void CancelRooMotion()
    {
        animator.applyRootMotion = false;
    }

    public void AddForceToJump()
    {
        Vector3 go = transform.forward * 480.0f + transform.up * 540.0f;
        rigidbody.AddForce(go, ForceMode.Impulse);
    }

    public void AddForceToFinalJump()
    {
        Vector3 go = transform.forward * 600.0f + transform.up * 2700.0f;
        rigidbody.AddForce(go, ForceMode.Impulse);
    }

    public void FindRabbits()
    {
        rabbits = GameObject.FindGameObjectsWithTag("Rabbit");
        aiData.target = rabbits[0];
    }

    public void FindPlayers()
    {

    }

    public void CatchRabbit()
    {
        rabbits[0].transform.SetParent(mousePosition, false);
        rabbits[0].transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
        rabbits[0].GetComponent<SphereCollider>().enabled = false;
        rabbits[0].GetComponent<RabbitAI>().enabled = false;
        rabbits[0].GetComponent<NavMeshAgent>().enabled = false;
        catched = true;
    }

    public void SetSetCreatedToFalse()
    {
       GameObject.Find("WolfSetting").GetComponent<WolfTrigger>().created = false;
    }


    #endregion
    //void OnAnimatorMove()
    //{
    //    Animator animator = GetComponent<Animator>();

    //    if (animator.GetCurrentAnimatorStateInfo(0).IsName("Wolfr_Sneak_Forward 0"))
    //    {
    //        animator.applyRootMotion = true;
    //    }
    //}
}

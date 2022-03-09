using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class BoxController : MonoBehaviour
{
    public bool firstCreated = false;
    public bool beUsing;
    public GameObject user;
    public GameObject targetAnimal;
    private Collider ac;
    private GameObject contentSpot;
    public bool animalCatched = false;
    private Rigidbody rb;
    public bool touchingGround = false;
    public bool physicStart = false;
    private void Awake()
    {
        contentSpot = this.transform.Find("Box").Find("ContentSpot").gameObject;
        rb = this.gameObject.GetComponent<Rigidbody>();
    }

    private void Start()
    {
        firstCreated = true;
    }

    private void FixedUpdate()
    {
        if (touchingGround == true && rb.velocity.magnitude > 0.2f)
        {
            physicStart = true;
        }

        if (physicStart == true && beUsing == true)
        {
            TurnBeUsingToFalse();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (beUsing == true && (other.gameObject.tag == "Rabbit" || other.gameObject.tag == "Pig") && targetAnimal == null)
        {
            if (other.gameObject.tag == "Rabbit")
            {
                if (other.gameObject.GetComponent<RabbitAI>().m_Data.isBited == true)
                {
                    Debug.Log($"ISBITED");
                    return;
                }
            }

            float dist = (other.transform.position - this.transform.position).magnitude;
            if (dist <= 3.5f)
            {
                targetAnimal = other.gameObject;
                targetAnimal.transform.parent = contentSpot.gameObject.transform;
                targetAnimal.transform.localEulerAngles = contentSpot.transform.eulerAngles;

                if (targetAnimal.tag == "Rabbit")
                {
                    RabbitAIData data = targetAnimal.GetComponent<RabbitAI>().m_Data;

                    data.isTargeted = true;
                    data.isCatched = true;
                }

                if (targetAnimal.tag == "Pig")
                {
                    targetAnimal.GetComponent<PigBehaviourTree>().SetCatchedStatus(this.gameObject);
                }
                
                animalCatched = true;
            }
        }
    }

    private void TurnBeUsingToFalse()
    {
        if (rb.velocity == new Vector3(0.0f, 0.0f, 0.0f))
        {
            beUsing = false;
            physicStart = false;
        }
    }
}

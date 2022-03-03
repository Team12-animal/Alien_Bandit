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

    private void Awake()
    {
        contentSpot = this.transform.Find("Box").Find("ContentSpot").gameObject;
        rb = this.gameObject.GetComponent<Rigidbody>();
    }

    private void Start()
    {
        firstCreated = true;
    }

    private void Update()
    {
        if (beUsing == true)
        {
            TurnBeUsingToFalse();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("box trigger" + other.gameObject.name);
        if (beUsing == true && other.gameObject.tag == "Rabbit" && targetAnimal == null)
        {
            targetAnimal = other.gameObject;
            targetAnimal.GetComponent<RabbitAI>().m_Data.isCatched = true;
            targetAnimal.transform.parent = contentSpot.gameObject.transform;
            targetAnimal.transform.localPosition = Vector3.zero;
            animalCatched = true;
        }
    }

    private void TurnBeUsingToFalse()
    {
        if (rb.velocity == new Vector3(0.0f, 0.0f, 0.0f) && touchingGround)
        {
            beUsing = false;
        }
    }
}
